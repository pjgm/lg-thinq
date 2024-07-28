using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace ThinQ.Models;

public class RegisterIoTCertificateResponse
{
    public string resultCode { get; set; }
    public CertificateResult result { get; set; }
}

public class CertificateResult
{
    public X509Certificate2 CertificatePemCertificate => new(Encoding.UTF8.GetBytes(certificatePem));
    public string certificatePem { get; set; }
    public string[] subscriptions { get; set; }
}
