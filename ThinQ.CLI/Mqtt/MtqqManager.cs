using System.Security.Authentication;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Packets;
using Org.BouncyCastle.Crypto;
using Spectre.Console;
using ThinQ.CLI.Extensions;
using ThinQ.CLI.HttpClients;
using ThinQ.CLI.Models;
using ThinQ.CLI.Services;

namespace ThinQ.CLI.Mqtt;

public class MqttManager(ThinQClient thinQClient, string clientId)
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

    private async Task OnMessageReceivedAsync(MqttApplicationMessageReceivedEventArgs args)
    {
        var payload = Encoding.UTF8.GetString(args.ApplicationMessage.Payload);

        AnsiConsole.MarkupLine($"[yellow]Received message[/]: {payload}");
    }

    private async Task OnConnectedAsync(MqttClientConnectedEventArgs args)
    {
        AnsiConsole.MarkupLine("[green]Connected[/]");
    }

    private async Task OnDisconnectAsync(MqttClientDisconnectedEventArgs args)
    {
        AnsiConsole.MarkupLine("[red]Disconnected[/]");
    }

    private async Task SubscribeToAllTopics(IEnumerable<string> subscriptions)
    {
        foreach (var topic in subscriptions)
        {
            AnsiConsole.MarkupLine($"[yellow]Subscribing to topic[/]: {topic}");
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
