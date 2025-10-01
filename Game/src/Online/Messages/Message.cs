/* Standard message type and all the different types of messages that can be sent.*/

public abstract class Message
{
    public enum MessageType
    {
        Update,
        InitializeGame,
    }

    protected MessageType _thisMessageType;
    public MessageType ThisMessageType => _thisMessageType;

    /// <summary>
    /// Unused right now. Will be used to nkow who sent the message.
    /// </summary>
    public int PlayerId { get; set; }

    #region Getters and setters
    public MessageType GetMessageType()
    {
        return _thisMessageType;
    }
    #endregion
}
