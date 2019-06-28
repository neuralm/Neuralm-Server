using System.IO;
using Microsoft.Extensions.Configuration;

namespace Neuralm.Utilities
{
    public static class ConfigurationLoader
    {
        private static IConfiguration _currentConfiguration;

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
