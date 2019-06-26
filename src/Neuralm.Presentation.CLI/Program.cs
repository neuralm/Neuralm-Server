using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Mapping;

namespace Neuralm.Presentation.CLI
{
    public class Program
    {
        private IGenericServiceProvider _genericServiceProvider;

        public static async Task Main(string[] args)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            _ = new Program().RunAsync(cancellationTokenSource.Token);

            while (Console.ReadKey().Key != ConsoleKey.Q)
            {
                Console.WriteLine();
                Console.WriteLine("Press Q to shut down the server.");
            }
            cancellationTokenSource.Cancel();
            Console.WriteLine();
            Console.WriteLine("Server has shut down");
        }

        private async Task RunAsync(CancellationToken token)
        {
            IConfiguration configuration = BuildConfiguration();

            Startup startup = new Startup();
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration);
            Console.WriteLine("Finished initializing!");

            _genericServiceProvider = startup.GetServiceProvider();
            IUserService userService = _genericServiceProvider.GetService<IUserService>();

            RegisterRequest registerRequest = new RegisterRequest()
            {
                Id = Guid.NewGuid(),
                CredentialTypeCode = "Name",
                Username = Guid.NewGuid().ToString(),
                Password = Guid.NewGuid().ToString()
            };
            Console.WriteLine($"RegisterRequest:\n\tId: {registerRequest.Id}\n\tCredentialTypeCode: {registerRequest.CredentialTypeCode}\n\tUsername: {registerRequest.Username}\n\tPassword: {registerRequest.Password}");
            RegisterResponse registerResponse = await userService.RegisterAsync(registerRequest);
            Console.WriteLine($"RegisterResponse:\n\tId: {registerResponse.Id}\n\tRequestId: {registerResponse.RequestId}\n\tError: {registerResponse.Error}\n\tSuccess: {registerResponse.Success}");

            AuthenticateRequest authenticateRequest = new AuthenticateRequest()
            {
                Id = Guid.NewGuid(),
                CredentialTypeCode = "Name",
                Username = registerRequest.Username,
                Password = registerRequest.Password
            };
            Console.WriteLine($"AuthenticateRequest:\n\tId: {authenticateRequest.Id}\n\tCredentialTypeCode: {authenticateRequest.CredentialTypeCode}\n\tUsername: {authenticateRequest.Username}\n\tPassword: {authenticateRequest.Password}");
            AuthenticateResponse authenticateResponse = await userService.AuthenticateAsync(authenticateRequest);
            Console.WriteLine($"AuthenticateResponse:\n\tId: {authenticateResponse.Id}\n\tRequestId: {authenticateResponse.RequestId}\n\tAccessToken:{authenticateResponse.AccessToken}\n\tError: {authenticateResponse.Error}\n\tSuccess: {authenticateResponse.Success}");
        }

        private static IConfiguration BuildConfiguration()
        {
            string basePath = Directory.GetCurrentDirectory();
            IConfigurationBuilder builder = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appSettings.json", optional: false, reloadOnChange: true);
            return builder.Build();
        }
    }
}
