/* A class to serialize and deserialize messages.*/

using System.Diagnostics.Tracing;
using System.Text.Json;
using Sprache;

public class MessageFactory
{
    public MessageFactory()
    {
        ServiceLocator.Register<MessageFactory>(this);
    }

    public string ToJson(Message message)
    {
        return JsonSerializer.Serialize(message, message.GetType());
    }

    public Message? FromJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
        {
            return null; // or throw
        }
        
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
                Message.MessageType.InitializeGame => JsonSerializer.Deserialize<InitializeGame>(json),
                _ => throw new ParseException($"Unknown message type: {type}"),
            };
        }
        throw new ParseException("Could not parse a message.");
    }
}
