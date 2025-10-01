/* A class to serialize and deserialize messages.*/

using System.Diagnostics.Tracing;
using System.Text.Json;
using Serilog;
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
            return null;
        }

        JsonDocument doc = JsonDocument.Parse(json);
        // Check message type to find the right deserialize method
        bool foundMessage = doc.RootElement.TryGetProperty("ThisMessageType", out JsonElement type);
        if (!foundMessage)
        {
            throw new EventSourceException("No type was found in the message.");
        }

        Message.MessageType messageType = (Message.MessageType)type.GetInt32();
        return messageType switch
        {
            Message.MessageType.Update => JsonSerializer.Deserialize<UpdateMessage>(json),
            Message.MessageType.InitializeGame => JsonSerializer.Deserialize<InitializeGame>(json),
            _ => throw new ParseException($"Unknown message type: {type}"),
        };
        throw new ParseException("Could not parse a message.");
    }
}
