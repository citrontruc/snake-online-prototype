/* Standard message type and all the different types of messages that can be sent.*/

public abstract class Message
{
    public enum MessageType
    {
        Update,
        ConnectionCreate,
        InitializeGame,
        Disconnect
    }
    protected MessageType _thisMessageType;
    public int PlayerId;
}
