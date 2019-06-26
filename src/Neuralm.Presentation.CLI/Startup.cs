using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Neuralm.Mapping;

namespace Neuralm.Presentation.CLI
{
    internal class Startup
    {
        private IGenericServiceProvider _serviceProvider;

        internal Task InitializeAsync(IConfiguration configuration)
        {
            _serviceProvider = new ServiceCollection()
                .AddConfigurations(configuration)
                .AddApplicationServices()
                .BuildServiceProvider()
                .ToGenericServiceProvider();

            return Task.CompletedTask;
        }

        internal IGenericServiceProvider GetServiceProvider()
        {
            return _serviceProvider;
        }
    }
}
