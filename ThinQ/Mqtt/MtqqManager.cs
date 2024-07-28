using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using Org.BouncyCastle.Crypto;
using ThinQ.Extensions;
using ThinQ.HttpClients;
using ThinQ.Models;

namespace ThinQ.Mqtt;

public class MqttManager(ThinQClient thinQClient, IMqttOutputWriter writer, string clientId)
{
    private readonly IMqttClient _mqttClient = new MqttFactory().CreateMqttClient();

    public async Task Connect()
    {
        var route = await thinQClient.GetRoute();

        await thinQClient.RegisterDevice();

        var csr = X509CertificateHelper.CreateCsr(out AsymmetricCipherKeyPair keyPair);

        var certResponse = await thinQClient.RegisterIotCertificate(csr);

        using var certificate = certResponse.result.CertificatePemCertificate;
        var cert = certificate.CopyWithPrivateKey(keyPair.Private);

        var brokerUri = new Uri(route.result.mqttServer);

        var options = new MqttClientOptions
        {
            ChannelOptions = new MqttClientTcpOptions
            {
                Server = brokerUri.Host,
                Port = brokerUri.Port,
                TlsOptions = new MqttClientTlsOptions
                {
                    UseTls = true,
                    AllowUntrustedCertificates = true,
                    ClientCertificatesProvider = new CertificateProvider(cert),
                    CertificateValidationHandler = (c) => true,
                    SslProtocol = SslProtocols.None
                }
            },
            ClientId = clientId
        };

        _mqttClient.DisconnectedAsync += OnDisconnectAsync;
        _mqttClient.ConnectedAsync += OnConnectedAsync;
        _mqttClient.ApplicationMessageReceivedAsync += OnMessageReceivedAsync;

        await _mqttClient.ConnectAsync(options, CancellationToken.None);
        await SubscribeToAllTopics(certResponse.result.subscriptions);
    }

    private Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

        writer.WriteMessageReceived(payload);

        return Task.CompletedTask;
    }

    private Task OnConnectedAsync(MqttClientConnectedEventArgs args)
    {
        writer.WriteOnConnect();
        return Task.CompletedTask;
    }

    private Task OnDisconnectAsync(MqttClientDisconnectedEventArgs args)
    {
        writer.WriteOnDisconnect();
        return Task.CompletedTask;
    }

    private async Task SubscribeToAllTopics(IEnumerable<string> subscriptions)
    {
        foreach (var topic in subscriptions)
        {
            writer.WriteSubscribeToTopic(topic);
            await _mqttClient.SubscribeAsync(new MqttTopicFilter { Topic = topic });
        }
    }
}


internal class CertificateProvider : IMqttClientCertificatesProvider
{
    private X509CertificateCollection _certificates;
    public CertificateProvider(X509Certificate x509Certificate)
    {
        _certificates = new X509CertificateCollection() { x509Certificate };
    }

    public X509CertificateCollection GetCertificates()
    {
        return _certificates;
    }
}
