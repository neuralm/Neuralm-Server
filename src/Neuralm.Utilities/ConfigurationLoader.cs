﻿using System.IO;
using Microsoft.Extensions.Configuration;

namespace Neuralm.Utilities
{
    /// <summary>
    /// Represents the <see cref="ConfigurationLoader"/> class.
    /// </summary>
    public static class ConfigurationLoader
    {
        private static IConfiguration _currentConfiguration;

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        /// <param name="configuration">The configuration file name.</param>
        /// <returns>Returns the <see cref="IConfiguration"/>.</returns>
        public static IConfiguration GetConfiguration(string configuration)
        {
            if (_currentConfiguration != null)
                return _currentConfiguration;
            string basePath = Directory.GetCurrentDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile($"{configuration}.json", optional: false, reloadOnChange: false);
            return _currentConfiguration = builder.Build();
        }
    }
}
