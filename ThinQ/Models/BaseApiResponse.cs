namespace ThinQ.Models;

public class BaseApiResponse
{
    public required string MessageId { get; set; }
    public required DateTime Timestamp { get; set; }
}