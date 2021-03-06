﻿using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using Neuralm.Services.Common.Application;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Domain;
using Neuralm.Services.Common.Infrastructure.Services;
using Neuralm.Services.Common.Messages;
using Neuralm.Services.Common.Messages.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Security.Claims;

namespace Neuralm.Services.Common.Mapping
{
    /// <summary>
    /// Represents the <see cref="StartupExtensions"/> class.
    /// </summary>
    public static class StartupExtensions
    {
        // ReSharper disable once ConditionIsAlwaysTrueOrFalse
        /// <summary>
        /// Gets a value whether the application is running in Debug mode.
        /// </summary>
        public static bool IsDebug
        {
            get
            {
                bool isDebug = false;
#if DEBUG
                isDebug = true;
#endif
                return isDebug;
            }
        }

        /// <summary>
        /// Adds the AutoMapper mappings from the provided assembly.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="assembly">The assembly.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddAutoMapper(this IServiceCollection serviceCollection, Assembly assembly)
        {
            serviceCollection.AddAutoMapper(new[] { assembly, Assembly.GetAssembly(typeof(StartupExtensions)) });
            return serviceCollection;
        }

        /// <summary>
        /// Adds and binds configurations into the <see cref="serviceCollection"/>.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="configuration">The configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddConfigurations(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.AddOptions();
            serviceCollection.Configure<DbConfiguration>(configuration.GetSection("Database").Bind);
            serviceCollection.Configure<JwtConfiguration>(configuration.GetSection("Jwt").Bind);
            serviceCollection.Configure<RegistryServiceConfiguration>(configuration.GetSection("RegistryService").Bind);
            return serviceCollection;
        }

        /// <summary>
        /// Adds authentication using Jwt bearers.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="jwtConfiguration">The jwt configuration.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection serviceCollection, JwtConfiguration jwtConfiguration)
        {
            // NOTE: Temporarily added for debugging.
            IdentityModelEventSource.ShowPII = true;
            serviceCollection.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(jwtConfiguration.SecretBytes),
                    ValidIssuer = jwtConfiguration.Issuer,
                    ValidAudience = jwtConfiguration.Audience,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true
                };
            });

            return serviceCollection;
        }

        /// <summary>
        /// Adds Jwt authentication with cors.
        /// </summary>
        /// <param name="app">The application builder.</param>
        /// <returns>Returns the application builder.</returns>
        public static IApplicationBuilder UseJwtAuthenticationWithCors(this IApplicationBuilder app)
        {
            app.UseCors(x => x.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseAuthentication();
            return app;
        }

        /// <summary>
        /// Adds the registry service for the given service name.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <param name="serviceName">The service name.</param>
        /// <returns>Returns the service collection to chain further upon.</returns>
        public static IServiceCollection AddRegistryService(this IServiceCollection serviceCollection, string serviceName)
        {
            serviceCollection.AddHttpClient<IRegistryService, RegistryService>((provider, client) =>
            {
                IAccessTokenService accessTokenService = provider.GetService<IAccessTokenService>();
                RegistryServiceConfiguration registryServiceConfiguration = provider.GetService<IOptions<RegistryServiceConfiguration>>().Value;
                List<Claim> claims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, serviceName),
                    new Claim(ClaimTypes.Role, "Service")
                };
                client.DefaultRequestHeaders.Add("Authorization", $"Bearer {accessTokenService.GenerateAccessToken(claims)}");
                client.BaseAddress = new Uri($"http://{registryServiceConfiguration.Host}:{registryServiceConfiguration.Port}");
            });
            return serviceCollection;
        }

        /// <summary>
        /// Registers the service with the registry service.
        /// </summary>
        /// <param name="app">The application builder interface.</param>
        /// <param name="configuration">The configuration interface.</param>
        /// <param name="serviceName">The service name.</param>
        /// <returns>Returns <see cref="IApplicationBuilder"/> to chain further upon.</returns>
        public static IApplicationBuilder RegisterService(this IApplicationBuilder app, IConfiguration configuration, string serviceName)
        {
            ServiceConfiguration serviceConfiguration = configuration.GetSection("Service").Get<ServiceConfiguration>();
            app.ApplicationServices.GetService<IStartupService>().RegisterServiceAsync(serviceName,
                serviceConfiguration.Host, serviceConfiguration.Port);
            return app;
        }

        /// <summary>
        /// Adds the health service to the service collection.
        /// </summary>
        /// <param name="serviceCollection">The service collection.</param>
        /// <returns>Returns <see cref="IApplicationBuilder"/> to chain further upon.</returns>
        public static IServiceCollection AddHealthChecks<TDbContext>(this IServiceCollection serviceCollection) where TDbContext : DbContext
        {
            serviceCollection.AddHealthChecks()
                .AddDbContextCheck<TDbContext>("Database");
            return serviceCollection;
        }

        /// <summary>
        /// Maps the controller and health check end points.
        /// </summary>
        /// <param name="app">The application builder. </param>
        /// <param name="serviceName">The service name. </param>
        /// <returns>Returns <see cref="IApplicationBuilder"/> to chain further upon.</returns>
        public static IApplicationBuilder MapControllerAndHealthCheckEndPoints(this IApplicationBuilder app, string serviceName)
        {
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHealthChecks($"{serviceName}/health", new HealthCheckOptions()
                {
                    ResponseWriter = async (context, result) =>
                    {
                        IMessageSerializer messageSerializer = app.ApplicationServices.GetService<IMessageSerializer>();
                        IMapper mapper = app.ApplicationServices.GetService<IMapper>();
                        Guid requestId = Guid.Empty;
                        ServiceHealthCheckResponse response;
                        if (context.Request.Headers.ContainsKey("Request-Id") && Guid.TryParse(context.Request.Headers["Request-Id"].FirstOrDefault(), out requestId))
                        {
                            ServiceHealthReport report = result.ToServiceHealthReport();
                            response = new ServiceHealthCheckResponse(requestId, mapper.Map<ServiceHealthReportDto>(report), "", true);
                        }
                        else
                        {
                            response = new ServiceHealthCheckResponse(requestId, null, "Please provide a 'Request-Id' header with request id as valid guid.", false);
                        }

                        string json = messageSerializer.SerializeToString(response);
                        context.Response.ContentType = $"application/{messageSerializer.SerializerType}";
                        await context.Response.WriteAsync(json);
                    }
                });
            });
            return app;
        }

        /// <summary>
        /// Converts an <see cref="IServiceProvider"/> to an <see cref="IGenericServiceProvider"/>.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        /// <returns>Returns a <see cref="IGenericServiceProvider"/> implementation.</returns>
        public static IGenericServiceProvider ToGenericServiceProvider(this ServiceProvider serviceProvider)
        {
            return new GenericServiceProvider(serviceProvider);
        }
    }
}
