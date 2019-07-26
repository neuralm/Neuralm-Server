# Create a new selfsignedcertificate
New-SelfSignedCertificate -DnsName "SelfSignedNeuralm" -CertStoreLocation "cert:\LocalMachine\My" -FriendlyName "NeuralmCert"

# Find the certificate in the store and export a private key from it
$cert = Get-ChildItem -Path 'Cert:\localmachine\My' |
    Where-Object { $_.FriendlyName -eq "NeuralmCert" }
    Export-PfxCertificate -Cert $cert -FilePath "c:\NeuralmCert.pfx" -Password (Read-Host -AsSecureString -Prompt 'Pfx Password')

# Imprt the certificate with private key back but into CA store
Import-PfxCertificate -FilePath "c:\NeuralmCert.pfx" -CertStoreLocation "cert:\LocalMachine\Root" -Password (Read-Host -AsSecureString -Prompt 'Pfx Password (same as before)')