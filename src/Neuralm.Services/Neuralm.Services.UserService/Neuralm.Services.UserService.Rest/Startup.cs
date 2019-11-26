using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Neuralm.Services.Common.Application.Interfaces;
using Neuralm.Services.Common.Configurations;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.UserService.Mapping;

namespace Neuralm.Services.UserService.Rest
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddConfigurations(Configuration);
            services.AddApplicationServices();
            services.AddJwtAuthentication(Configuration.GetSection("Jwt").Get<JwtConfiguration>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseJwtAuthenticationWithCors();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
//
//            List<Claim> claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, "UserService"),
//                new Claim(ClaimTypes.Role, "Service")
//            };
//            var service = app.ApplicationServices.GetService(typeof(IAccessTokenService)) as IAccessTokenService;
//            string x = service.GenerateAccessToken(claims);
//            Console.WriteLine(x);
            app.RegisterService(Configuration, "UserService");
        }
    }
}
