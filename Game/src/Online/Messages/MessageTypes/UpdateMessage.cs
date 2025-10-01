/* A message to pass a playerInput and update the game.
Every client sends one of them to the server, the server sends multiple of them to the clients.*/

public class UpdateMessage : Message
{
    public CellCoordinates SnakeDirection { get; set; }

    public UpdateMessage(CellCoordinates snakeDirection, int playerId)
    {
        _thisMessageType = MessageType.Update;
        SnakeDirection = snakeDirection;
        PlayerId = playerId;
    }
}
