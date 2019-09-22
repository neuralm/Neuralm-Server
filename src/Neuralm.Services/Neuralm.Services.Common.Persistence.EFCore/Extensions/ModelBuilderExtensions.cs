using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Reflection;

namespace Neuralm.Services.Common.Persistence.EFCore.Extensions
{
    /// <summary>
    /// Represents the extensions for the <see cref="ModelBuilder"/> class.
    /// </summary>
    public static class ModelBuilderExtensions
    {
        /// <summary>
        /// Applies all configurations found in the <see cref="TDbContext"/> assembly that use the <see cref="IEntityTypeConfiguration{TEntity}"/> interface.
        /// </summary>
        /// <remarks>finds all the configurations and applies them, then discards the instances.</remarks>
        /// <param name="modelBuilder">the model builder.</param>
        public static void ApplyAllConfigurations<TDbContext>(this ModelBuilder modelBuilder) where TDbContext : DbContext
        {
            MethodInfo applyConfigurationMethodInfo = modelBuilder
                .GetType()
                .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                .First(m => m.Name.Equals("ApplyConfiguration", StringComparison.OrdinalIgnoreCase));

            _ = typeof(TDbContext).Assembly
                .GetTypes()
                .Select(t => (t, i: t.GetInterfaces().FirstOrDefault(i => i.Name.Equals(typeof(IEntityTypeConfiguration<>).Name, StringComparison.Ordinal))))
                .Where(it => it.i != null)
                .Select(it => (et: it.i.GetGenericArguments()[0], cfgObj: Activator.CreateInstance(it.t)))
                .Select(it => applyConfigurationMethodInfo.MakeGenericMethod(it.et).Invoke(modelBuilder, new[] { it.cfgObj }))
                .ToList();
        }
    }
}
