using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Neuralm.Services.Common.Mapping;
using Neuralm.Services.MessageQueue.Application.Configurations;
using Neuralm.Services.MessageQueue.Mapping;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Threading.Tasks;

namespace Neuralm.Services.MessageQueue.NeuralmMQ
{
    /// <summary>
    /// Represents the <see cref="Startup"/> class.
    /// </summary>
    public class Startup
    {
        private IGenericServiceProvider _genericServiceProvider;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly CancellationTokenSource _cancellationTokenSourceTimed;

        /// <summary>
        /// Prepares a new instance of the <see cref="Startup"/> class.
        /// </summary>
        /// <param name="cancellationTokenSource">The cancellation token source.</param>
        /// <param name="timeOutInSeconds">The time out in seconds.</param>
        public Startup(CancellationTokenSource cancellationTokenSource, uint timeOutInSeconds)
        {
            _cancellationTokenSource = cancellationTokenSource;
            _cancellationTokenSourceTimed = new CancellationTokenSource(TimeSpan.FromSeconds(timeOutInSeconds));
        }

        /// <summary>
        /// Initializes the <see cref="Startup"/> instance.
        /// </summary>
        /// <param name="configuration">The configuration.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="ArgumentNullException">Thrown when the configuration is null.</exception>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        public Task InitializeAsync(IConfiguration configuration, CancellationToken cancellationToken)
        {
            if (configuration == null)
                throw new ArgumentNullException(nameof(configuration));

            _genericServiceProvider = new ServiceCollection()
                .AddConfigurations(configuration)
                .Configure<MessageQueueConfiguration>(configuration.GetSection("MessageQueue").Bind)
                .AddApplicationServices()
                .BuildServiceProvider()
                .ToGenericServiceProvider();

            // Initializes further tasks like creating the server certificate, verifying database, etc..

            List<Task> tasks = new List<Task>
            {
                //Task.Run(() => MapMessagesToServices(_cancellationTokenSourceTimed.Token), cancellationToken),
                Task.Run(() => CreateServerCertificate(_cancellationTokenSourceTimed.Token), cancellationToken)
            };

            return Task.WhenAll(tasks).ContinueWith(task =>
            {
                if (task.IsCanceled)
                    _cancellationTokenSource.Cancel();
            }, cancellationToken);
        }

        /// <summary>
        /// Gets the generic service provider.
        /// </summary>
        /// <exception cref="InitializationException">If <see cref="_genericServiceProvider"/> is <c>null</c>.</exception>
        /// <returns>Returns the generic service provider.</returns>
        public IGenericServiceProvider GetGenericServiceProvider()
        {
            return _genericServiceProvider ?? throw new InitializationException("GenericServiceProvider is unset. Call InitializeAsync() first.");
        }

        /// <summary>
        /// Creates the server certificate.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns an awaitable <see cref="Task"/>.</returns>
        private Task CreateServerCertificate(CancellationToken cancellationToken)
        {
            MessageQueueConfiguration configuration = _genericServiceProvider.GetService<IOptions<MessageQueueConfiguration>>().Value;

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                X509Store computerCaStore = new X509Store(StoreName.My, StoreLocation.LocalMachine);
                try
                {
                    computerCaStore.Open(OpenFlags.ReadOnly);
                    X509Certificate2Collection certificatesInStore = computerCaStore.Certificates.Find(X509FindType.FindBySubjectName, configuration.Host, true);
                    if (certificatesInStore.Count == 0)
                        throw new EmptyCertificateCollectionException($"No certificate was found with the given subject name: {configuration.Host}");

                    if (certificatesInStore.Count > 1)
                    {
                        foreach (X509Certificate2 cert in certificatesInStore)
                        {
                            DisplayCertificate(cert);
                        }
                        throw new ArgumentOutOfRangeException(nameof(certificatesInStore), "More than one certificate was found!");
                    }

                    X509Certificate2 certificate = certificatesInStore[0];
                    DisplayCertificate(certificate);
                    configuration.Certificate = certificate;
                    cancellationToken.ThrowIfCancellationRequested();
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{nameof(CreateServerCertificate)}: {e}");
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        Console.WriteLine($"Please check if you have a valid MessageQueueConfiguration.Host name, also known as the Common Name (CN) or Fully Qualified Domain Name (FQDN)!\n\t{e.Message}");
                        _cancellationTokenSource.Cancel();
                    }

                    Console.WriteLine("CreateServerCertificate is cancelled.");
                    return Task.FromCanceled(cancellationToken);
                }
                finally
                {
                    computerCaStore.Close();
                }
            }
            else
            {
                Console.WriteLine($"{new NotImplementedException("For non-Windows operating systems the certificate code is yet to be implemented.")}");
                Console.WriteLine("CreateServerCertificate is cancelled.");
                _cancellationTokenSource.Cancel();
                return Task.FromCanceled(cancellationToken);
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Displays the given certificate to the console.
        /// </summary>
        /// <param name="cert">The certificate.</param>
        private static void DisplayCertificate(X509Certificate2 cert)
        {
            Console.WriteLine("------------Certificate------------");
            Console.WriteLine($"Common Name: {cert.SubjectName.Name?.Replace("CN=", "")}");
            Console.WriteLine($"Issuer: {cert.Issuer}");
            Console.WriteLine($"Expiration date: {cert.GetExpirationDateString()}");
            Console.WriteLine($"Effective date: {cert.GetEffectiveDateString()}");
            Console.WriteLine("-----------------------------------");
        }
    }
}