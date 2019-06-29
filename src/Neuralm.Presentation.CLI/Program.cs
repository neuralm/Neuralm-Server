using System;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Neuralm.Application.Interfaces;
using Neuralm.Application.Messages.Requests;
using Neuralm.Application.Messages.Responses;
using Neuralm.Mapping;
using Neuralm.Utilities;

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
            Console.WriteLine("Press any key to continue..");
            Console.ReadKey();
        }

        private async Task RunAsync(CancellationToken token)
        {
            IConfiguration configuration = ConfigurationLoader.GetConfiguration("appSettings");

            Startup startup = new Startup();
            Console.WriteLine("Initializing...");
            await startup.InitializeAsync(configuration);
            Console.WriteLine("Finished initializing!\n");

            _genericServiceProvider = startup.GetServiceProvider();
            IUserService userService = _genericServiceProvider.GetService<IUserService>();

            RegisterRequest registerRequest = new RegisterRequest(Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "Name");
            Console.WriteLine($"RegisterRequest:\n  Id: {registerRequest.Id}\n  CredentialTypeCode: {registerRequest.CredentialTypeCode}\n  Username: {registerRequest.Username}\n  Password: {registerRequest.Password}\n  DateTime: {registerRequest.DateTime.ToString("dd-MM-yyyy HH:mm:ss.fffff", CultureInfo.InvariantCulture)}\n");
            RegisterResponse registerResponse = await userService.RegisterAsync(registerRequest);
            Console.WriteLine($"RegisterResponse:\n  Id: {registerResponse.Id}\n  RequestId: {registerResponse.RequestId}\n  Error: {registerResponse.Error}\n  DateTime: {registerResponse.DateTime.ToString("dd-MM-yyyy HH:mm:ss.fffff", CultureInfo.InvariantCulture)}\n  Success: {registerResponse.Success}\n");

            AuthenticateRequest authenticateRequest = new AuthenticateRequest(registerRequest.Username, registerRequest.Password, "Name");
            Console.WriteLine($"AuthenticateRequest:\n  Id: {authenticateRequest.Id}\n  CredentialTypeCode: {authenticateRequest.CredentialTypeCode}\n  Username: {authenticateRequest.Username}\n  Password: {authenticateRequest.Password}\n  DateTime: {authenticateRequest.DateTime.ToString("dd-MM-yyyy HH:mm:ss.fffff", CultureInfo.InvariantCulture)}\n");
            AuthenticateResponse authenticateResponse = await userService.AuthenticateAsync(authenticateRequest);
            Console.WriteLine($"AuthenticateResponse:\n  Id: {authenticateResponse.Id}\n  RequestId: {authenticateResponse.RequestId}\n  AccessToken: {authenticateResponse.AccessToken.Substring(0, 16)}...\n  Error: {authenticateResponse.Error}\n  DateTime: {authenticateResponse.DateTime.ToString("dd-MM-yyyy HH:mm:ss.fffff", CultureInfo.InvariantCulture)}\n  Success: {authenticateResponse.Success}\n");
        }
    }
}
