using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Utilities;

namespace Neuralm.Persistence.Infrastructure
{
    /// <summary>
    /// Represents the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
    /// </summary>
    /// <typeparam name="TContext"></typeparam>
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private DbContextOptionsBuilder<TContext> _dbContextOptionsBuilder;
        private readonly DbConfiguration _dbConfiguration;

        /// <summary>
        /// Initializes an instance of the <see cref="DesignTimeDbContextFactoryBase{TContext}"/> class.
        /// </summary>
        /// <param name="dbConfigurationOptions">The options.</param>
        protected DesignTimeDbContextFactoryBase(IOptions<DbConfiguration> dbConfigurationOptions)
        {
            _dbConfiguration = dbConfigurationOptions == null 
                ? GetDbConfiguration() 
                : dbConfigurationOptions.Value;
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
            optionsBuilder.UseLazyLoadingProxies();
            if (connectionString.Equals("InMemoryDatabase"))
                optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
            else
                optionsBuilder.UseSqlServer(connectionString);
            _dbContextOptionsBuilder = optionsBuilder;
        }

        private static DbConfiguration GetDbConfiguration()
        {
            DbConfiguration dbConfiguration = new DbConfiguration();
            ConfigurationLoader.GetConfiguration("appSettings").GetSection("NeuralmDb").Bind(dbConfiguration);
            return dbConfiguration;
        }
    }
}
