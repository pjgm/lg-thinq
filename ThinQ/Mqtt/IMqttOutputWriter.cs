namespace ThinQ.Mqtt;

public interface IMqttOutputWriter
{
    void WriteMessageReceived(string message);
    void WriteOnConnect();
    void WriteOnDisconnect();
    void WriteSubscribeToTopic(string topic);
}
