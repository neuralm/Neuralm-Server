using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Neuralm.Application.Configurations;
using Neuralm.Utilities;

namespace Neuralm.Persistence.Infrastructure
{
    public abstract class DesignTimeDbContextFactoryBase<TContext> : IDesignTimeDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly string _uniqueDbName;
        protected readonly DbConfiguration DbConfiguration;
        private DbContextOptionsBuilder<TContext> _dbContextOptionsBuilder;

        protected DesignTimeDbContextFactoryBase(IOptions<DbConfiguration> dbConfigurationOptions)
        {
            DbConfiguration = dbConfigurationOptions == null 
                ? GetDbConfiguration() 
                : dbConfigurationOptions.Value;
            _uniqueDbName = Guid.NewGuid().ToString();
        }

        protected abstract TContext CreateNewInstance(DbContextOptions<TContext> dbContextOptions);
        public virtual TContext CreateDbContext(string[] args)
        {
            if (_dbContextOptionsBuilder == null)
                SetDbContextOptionsBuilder(DbConfiguration.ConnectionString);
            return CreateNewInstance(_dbContextOptionsBuilder.Options);
        }
        private void SetDbContextOptionsBuilder(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentException($"Connection string '{connectionString}' is null or empty.", nameof(connectionString));
            DbContextOptionsBuilder<TContext> optionsBuilder = new DbContextOptionsBuilder<TContext>();
            optionsBuilder.UseLazyLoadingProxies();
            if (connectionString.Equals("InMemoryDatabase"))
                optionsBuilder.UseInMemoryDatabase(_uniqueDbName); // NOTE: connectionString
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
