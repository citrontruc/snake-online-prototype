/* A class to serialize and deserialize messages.*/

using System.Diagnostics.Tracing;
using System.Text.Json;

public static class MessageFactory
{
    static MessageFactory() { }

    public static string ToJson(Message message)
    {
        return JsonSerializer.Serialize(message, message.GetType());
    }

    public static Message? FromJson(string json)
    {
        JsonDocument doc = JsonDocument.Parse(json);
        // Check message type to find the right deserialize method
        string? type = doc.RootElement.GetProperty("MessageType").GetString();
        if (type is null)
        {
            throw new EventSourceException("No type was found in the message.");
        }

        bool parseSuccess = Enum.TryParse(type, out Message.MessageType messageType);
        if (parseSuccess)
        {
            return messageType switch
            {
                Message.MessageType.Update => JsonSerializer.Deserialize<UpdateMessage>(json),
                Message.MessageType.Disconnect => JsonSerializer.Deserialize<UpdateMessage>(json),
                _ => throw new TypeAccessException($"Unknown message type: {type}"),
            };
        }
        throw new TypeAccessException($"Unknown message type: {type}");
    }
}
