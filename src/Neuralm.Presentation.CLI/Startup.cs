using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Mapping;
using Neuralm.Persistence.Contexts;
using Console = System.Console;

namespace Neuralm.Presentation.CLI
{
    /// <summary>
    /// Represents the <see cref="Startup"/> class.
    /// </summary>
    internal class Startup
    {
        private IGenericServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;

        public Startup(CancellationTokenSource cancellationTokenSource)
        {
            _cancellationTokenSource = cancellationTokenSource;
        }

        /// <summary>
        /// Initializes the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        internal Task InitializeAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _serviceProvider = new ServiceCollection()
                .AddConfigurations(configuration)
                .AddApplicationServices()
                .AddJwtBearerBasedAuthentication()
                .BuildServiceProvider()
                .ToGenericServiceProvider();

            List<Task> tasks = new List<Task>
            {
                Task.Run(() => VerifyDatabaseConnection(cancellationToken), cancellationToken),
                Task.Run(() => MapMessagesToServices(cancellationToken), cancellationToken),
                Task.Run(() => CreateServerCertificate(cancellationToken), cancellationToken)
            };
            return Task.WhenAll(tasks);
        }

        /// <summary>
        /// Gets the generic service provider.
        /// </summary>
        /// <exception cref="InitializationException">If <see cref="_serviceProvider"/> is <c>null</c>.</exception>
        /// <returns>Returns the generic service provider.</returns>
        internal IGenericServiceProvider GetGenericServiceProvider()
        {
            return _serviceProvider ?? throw new InitializationException("GenericServiceProvider is unset. Call InitializeAsync() first.");
        }

        /// <summary>
        /// Verifies the database connection.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private async Task VerifyDatabaseConnection(CancellationToken cancellationToken)
        {
            try
            {
                Console.WriteLine("Starting database verification..");
                NeuralmDbContext neuralmDbContext = _serviceProvider.GetService<IFactory<NeuralmDbContext>>().Create();
                RelationalDatabaseCreator relationalDatabaseCreator =
                    neuralmDbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                bool? exists = relationalDatabaseCreator?.Exists();
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"Database {(exists.Value ? "" : "does not ")}exists.");
                if (!exists.Value)
                {
                    relationalDatabaseCreator?.Create();
                    Console.WriteLine("Database created.");
                }

                IEnumerable<string> pendingMigrations = neuralmDbContext.Database.GetPendingMigrations().ToList();
                bool hasPendingMigrations = pendingMigrations.Any();
                Console.WriteLine($"Database {(hasPendingMigrations ? "has" : "does not have")} pending migrations.");
                if (hasPendingMigrations)
                {
                    Console.WriteLine(
                        "Would you like to clear the database using EnsureDeleted. And, afterwards reapply all migrations? y/n");
                    ConsoleKeyInfo keyInfo = await WaitForReadKey(cancellationToken);
                    cancellationToken.ThrowIfCancellationRequested();
                    if (keyInfo.KeyChar == 'y')
                    {
                        neuralmDbContext.Database.EnsureDeleted();
                        cancellationToken.ThrowIfCancellationRequested();
                        foreach (string migration in neuralmDbContext.Database.GetMigrations())
                        {
                            Console.WriteLine($"\tMigration: {migration}");
                        }
                    }
                    else
                    {
                        foreach (string pendingMigration in pendingMigrations)
                        {
                            Console.WriteLine($"\tPending migration: {pendingMigration}");
                        }
                    }

                    neuralmDbContext.Database.Migrate();
                    cancellationToken.ThrowIfCancellationRequested();
                    Console.WriteLine("Applied the migrations to the database.");
                }

                Console.WriteLine("Finished database verification!");
            }
            catch (Exception e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{nameof(VerifyDatabaseConnection)}: {e.Message}");
                    _cancellationTokenSource.Cancel();
                }
                Console.WriteLine("VerifyDatabaseConnection is cancelled.");
            }
        }

        /// <summary>
        /// Maps messages to services.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private Task MapMessagesToServices(CancellationToken cancellationToken)
        {
            try
            {
                // NOTE: Create the singleton once to map the services.
                _ = _serviceProvider.GetService<MessageToServiceMapper>();
                cancellationToken.ThrowIfCancellationRequested();
            }
            catch (Exception e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"{nameof(MapMessagesToServices)}: {e.Message}");
                    _cancellationTokenSource.Cancel();
                }
                Console.WriteLine("MapMessagesToServices is cancelled.");
                return Task.FromCanceled(cancellationToken);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Creates the server certificate.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private Task CreateServerCertificate(CancellationToken cancellationToken)
        {
            string certificateName = _serviceProvider.GetService<IOptions<ServerConfiguration>>().Value.CertificateName;
            ServerConfiguration configuration = _serviceProvider.GetService<IOptions<ServerConfiguration>>().Value;

            try
            {
                X509Certificate certificate = X509Certificate2.CreateFromCertFile(certificateName);
                cancellationToken.ThrowIfCancellationRequested();
                configuration.Certificate = certificate;
            }
            catch (Exception e)
            {
                if (!cancellationToken.IsCancellationRequested)
                {
                    Console.WriteLine($"Please check if you have a valid CertificateName!\n\t{e.Message}");
                    _cancellationTokenSource.Cancel();
                }

                Console.WriteLine("CreateServerCertificate is cancelled.");
                return Task.FromCanceled(cancellationToken);
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// Waits for the <see cref="Console.ReadKey()"/> method to return on an available key.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/> with type parameter <see cref="ConsoleKeyInfo"/>.</returns>
        private static async Task<ConsoleKeyInfo> WaitForReadKey(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (Console.KeyAvailable)
                {
                    return Console.ReadKey(false);
                }
                await Task.Delay(50, cancellationToken);
            }
            return new ConsoleKeyInfo((char)0, 0, false, false, false);
        }
    }
}
