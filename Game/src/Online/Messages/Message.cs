/* Standard message type and all the different types of messages that can be sent.*/

public abstract class Message
{
    public enum MessageType
    {
        Update,
        ConnectionCreate,
        InitializeGame,
    }

    protected MessageType _thisMessageType;
    public MessageType ThisMessageType => _thisMessageType;
    public int PlayerId { get; set; }

    public MessageType GetMessageType()
    {
        return _thisMessageType;
    }
}
