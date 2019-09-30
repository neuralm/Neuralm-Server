using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Application;
using Neuralm.Services.Common.Configurations;
using System;

namespace Neuralm.Services.Common.Persistence.EFCore.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
    /// </summary>
    /// <typeparam name="TContext">The database context type.</typeparam>
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private DbContextOptionsBuilder<TContext> _dbContextOptionsBuilder;
        private readonly DbConfiguration _dbConfiguration;
        private readonly ILoggerFactory _loggerFactory;

        /// <summary>
        /// Initializes an instance of the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        /// <param name="loggerFactory">The logger factory.</param>
        protected DesignTimeDbContextFactoryBase(IOptions<DbConfiguration> dbConfigurationOptions, ILoggerFactory loggerFactory)
        {
            _dbConfiguration = dbConfigurationOptions == null
                ? GetDbConfiguration()
                : dbConfigurationOptions.Value;
            _loggerFactory = loggerFactory;
        }

        /// <summary>
        /// Creates a new DbContext instance using options.
        /// </summary>
        /// <param name="dbContextOptions">The options.</param>
        /// <returns>Returns a new DbContext instance.</returns>
        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> dbContextOptions);

        /// <summary>
        /// Creates a new DbContext instance using string arguments.
        /// </summary>
        /// <param name="args">The arguments.</param>
        /// <returns>Returns a new DbContext instance.</returns>
        public virtual TContext CreateDbContext(string[] args)
        {
            if (_dbContextOptionsBuilder == null)
                SetDbContextOptionsBuilder(_dbConfiguration.ConnectionString);
            return CreateNewInstance(_dbContextOptionsBuilder.Options);
        }

        private void SetDbContextOptionsBuilder(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Connection string '{connectionString}' is null or empty.", nameof(connectionString));

            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();

            // Adds lazy loading.
            if (_dbConfiguration.UseLazyLoading)
                optionsBuilder.UseLazyLoadingProxies();

            // Switches to the correct DbProvider.
            switch (_dbConfiguration.DbProvider.ToLower())
            {
                case "mssql":
                    optionsBuilder.UseSqlServer(connectionString);
                    break;
                case "mysql":
                    optionsBuilder.UseMySql(connectionString);
                    break;
                default:
                    optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
                    break;
            }

            // Adds logging.
            if (_loggerFactory != null)
            {
                optionsBuilder.UseLoggerFactory(_loggerFactory);
                optionsBuilder.EnableSensitiveDataLogging();
            }

            _dbContextOptionsBuilder = optionsBuilder;
        }

        private static DbConfiguration GetDbConfiguration()
        {
            DbConfiguration dbConfiguration = new DbConfiguration();
            ConfigurationLoader.GetConfiguration("appsettings").GetSection("Database").Bind(dbConfiguration);
            return dbConfiguration;
        }
    }
}
