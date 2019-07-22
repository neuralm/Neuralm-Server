using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Configuration;
using Neuralm.Application.Interfaces;
using Neuralm.Mapping;
using Neuralm.Persistence.Contexts;

namespace Neuralm.Presentation.CLI
{
    /// <summary>
    /// Represents the <see cref="Startup"/> class.
    /// </summary>
    internal class Startup
    {
        private IGenericServiceProvider _serviceProvider;

        /// <summary>
        /// Initializes the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <exception cref="ArgumentNullException">If configuration is null.</exception>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        internal Task InitializeAsync(IConfiguration configuration)
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
                Task.Run(VerifyDatabaseConnection),
                Task.Run(MapMessagesToServices)
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
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private Task VerifyDatabaseConnection()
        {
            Console.WriteLine("Starting database verification..");
            NeuralmDbContext neuralmDbContext = _serviceProvider.GetService<IFactory<NeuralmDbContext>>().Create();
            RelationalDatabaseCreator relationalDatabaseCreator = neuralmDbContext.Database.GetService<IDatabaseCreator>() as RelationalDatabaseCreator;
            bool? exists = relationalDatabaseCreator?.Exists();
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
                Console.WriteLine("Would you like to clear the database using EnsureDeleted. And, afterwards reapply all migrations? y/n");
                ConsoleKeyInfo key = Console.ReadKey(false);
                if (key.KeyChar == 'y')
                {
                    neuralmDbContext.Database.EnsureDeleted();
                    
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
                Console.WriteLine("Applied the migrations to the database.");
            }
            Console.WriteLine("Finished database verification!");
            return Task.CompletedTask;
        }

        /// <summary>
        /// Maps messages to services.
        /// </summary>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private Task MapMessagesToServices()
        {
            // NOTE: Create the singleton once to map the services.
            _ = _serviceProvider.GetService<MessageToServiceMapper>();
            return Task.CompletedTask;
        }
    }
}
