using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Application.Interfaces;
using Neuralm.Mapping;
using Neuralm.Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;
using static Neuralm.Utilities.ConsoleUtility;
using Console = System.Console;
using Microsoft.OpenApi.Models;

namespace Neuralm.Presentation.CLI
{
    /// <summary>
    /// Represents the <see cref="Startup"/> class.
    /// </summary>
    internal class Startup
    {
        private IGenericServiceProvider _serviceProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationTokenSource _cancellationTokenSourceTimed;

        /// <summary>
        /// Initializes an instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <param name="timeoutInSeconds">The time out in seconds.</param>
        public Startup(CancellationTokenSource cancellationTokenSource, uint timeoutInSeconds)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _cancellationTokenSourceTimed = new CancellationTokenSource(TimeSpan.FromSeconds(timeoutInSeconds));
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
                Task.Run(() => VerifyDatabaseConnection(_cancellationTokenSourceTimed.Token), cancellationToken),
                Task.Run(() => MapMessagesToServices(_cancellationTokenSourceTimed.Token), cancellationToken),
                Task.Run(() => CreateServerCertificate(_cancellationTokenSourceTimed.Token), cancellationToken)
            };

            return Task.WhenAll(tasks).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _cancellationTokenSource.Cancel();
            }, cancellationToken);
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
                if (neuralmDbContext.Database.IsInMemory())
                {
                    Console.WriteLine("InMemory database provider found.");
                    neuralmDbContext.Database.EnsureCreated();
                    Console.WriteLine("Ensured that the database is created.");
                    return;
                }

                RelationalDatabaseCreator relationalDatabaseCreator = neuralmDbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
                if (relationalDatabaseCreator is null)
                    throw new TargetException("The DbProvider is not relational database provider.");

                bool exists = relationalDatabaseCreator.Exists();
                cancellationToken.ThrowIfCancellationRequested();
                Console.WriteLine($"Database {(exists ? "" : "does not ")}exists.");
                if (!exists)
                {
                    relationalDatabaseCreator.Create();
                    Console.WriteLine("Database created.");
                }

                IEnumerable<string> pendingMigrations = neuralmDbContext.Database.GetPendingMigrations().ToList();
                bool hasPendingMigrations = pendingMigrations.Any();
                Console.WriteLine($"Database {(hasPendingMigrations ? "has" : "does not have")} pending migrations.");
                if (hasPendingMigrations)
                {
                    Console.WriteLine("Would you like to clear the database using EnsureDeleted. And, afterwards reapply all migrations? y/n");
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
            ServerConfiguration configuration = _serviceProvider.GetService<IOptions<ServerConfiguration>>().Value;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                X509Store computerCaStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                try
                {
                    computerCaStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certificatesInStore = computerCaStore.Certificates.Find(X509FindType.FindBySubjectName, configuration.Host, true);
                    if (certificatesInStore.Count == 0)
                        throw new EmptyCertificateCollectionException($"No certificate was found with the given subject name: {configuration.Host}");

                    if (certificatesInStore.Count > 1)
                    {
                        foreach (X509Certificate2 cert in certificatesInStore)
                        {
                            DisplayCertificate(cert);
                        }
                        throw new ArgumentOutOfRangeException(nameof(certificatesInStore), "More than one certificate was found!");
                    }

                    X509Certificate2 certificate = certificatesInStore[0];
                    DisplayCertificate(certificate);
                    configuration.Certificate = certificate;
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception e)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine($"Please check if you have a valid ServerConfiguration.Host name, also known as the Common Name (CN) or Fully Qualified Domain Name (FQDN)!\n\t{e.Message}");
                        _cancellationTokenSource.Cancel();
                    }

                    Console.WriteLine("CreateServerCertificate is cancelled.");
                    return Task.FromCanceled(cancellationToken);
                }
                finally
                {
                    computerCaStore.Close();
                }
            }
            else
            {
                Console.WriteLine($"{new NotImplementedException("For non-Windows operating systems the certificate code is yet to be implemented.")}");
                Console.WriteLine("CreateServerCertificate is cancelled.");
                _cancellationTokenSource.Cancel();
                return Task.FromCanceled(cancellationToken);
            }
            
            return Task.CompletedTask;
        }

        /// <summary>
        /// Displays the given certificate to the console.
        /// </summary>
        /// <param name="cert">The certificate.</param>
        private static void DisplayCertificate(X509Certificate2 cert)
        {
            Console.WriteLine("------------Certificate------------");
            Console.WriteLine($"Common Name: {cert.SubjectName.Name?.Replace("CN=", "")}");
            Console.WriteLine($"Issuer: {cert.Issuer}");
            Console.WriteLine($"Expiration date: {cert.GetExpirationDateString()}");
            Console.WriteLine($"Effective date: {cert.GetEffectiveDateString()}");
            Console.WriteLine("-----------------------------------");
        }
    }
}
