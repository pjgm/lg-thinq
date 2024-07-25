using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace ThinQ.CLI.Extensions;

public static class CertificateExtensions
{
    public static X509Certificate2 CopyWithPrivateKey(this X509Certificate2 certificate, AsymmetricKeyParameter privateKey)
    {
        RSA rsa;
        if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
        {
            rsa = DotNetUtilities.ToRSA(privateKey as RsaPrivateCrtKeyParameters);
        }
        else
        {
            RSAParameters parameters = DotNetUtilities.ToRSAParameters(privateKey as RsaPrivateCrtKeyParameters);
            rsa = RSA.Create();
            rsa.ImportParameters(parameters);
        }

        return RSACertificateExtensions.CopyWithPrivateKey(certificate, rsa);
    }
}
