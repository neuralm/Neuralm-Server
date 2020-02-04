using System;
using System.Net;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

namespace Neuralm.Services.MessageQueue.Tests
{
    /// <summary>
    /// Represents the <see cref="X509Certificate2Builder"/> class.
    /// Used to create a self-signed certificate for testing purposes.
    /// </summary>
    public class X509Certificate2Builder
    {
        /// <summary>
        /// Builds a self-signed server certificate using the given certificate name.
        /// </summary>
        /// <param name="certificateName">The certificate name.</param>
        /// <returns>Returns the certificate for the given certificate name.</returns>
        public static X509Certificate2 BuildSelfSignedServerCertificate(string certificateName)
        {
            SubjectAlternativeNameBuilder sanBuilder = new SubjectAlternativeNameBuilder();
            sanBuilder.AddIpAddress(IPAddress.Loopback);
            sanBuilder.AddIpAddress(IPAddress.IPv6Loopback);
            sanBuilder.AddDnsName("localhost");
            sanBuilder.AddDnsName(Environment.MachineName);

            X500DistinguishedName distinguishedName = new X500DistinguishedName($"CN={certificateName}");

            using RSA rsa = RSA.Create(2048);
            CertificateRequest request = new CertificateRequest(distinguishedName, rsa, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
            request.CertificateExtensions.Add(new X509KeyUsageExtension(X509KeyUsageFlags.NonRepudiation | X509KeyUsageFlags.DigitalSignature, false));
            request.CertificateExtensions.Add(new X509EnhancedKeyUsageExtension(new OidCollection { new Oid("1.3.6.1.5.5.7.3.1") }, false));
            request.CertificateExtensions.Add(sanBuilder.Build());
            
            X509Certificate2 certificate = request.CreateSelfSigned(new DateTimeOffset(DateTime.UtcNow.AddDays(-1)), new DateTimeOffset(DateTime.UtcNow.AddDays(3650)));
            certificate.FriendlyName = certificateName;

            return new X509Certificate2(certificate.Export(X509ContentType.Pfx, ""), "", X509KeyStorageFlags.MachineKeySet);
        }
    }
}