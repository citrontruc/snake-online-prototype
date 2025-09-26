/* A message to pass a playerInput and update the game.
Every client sends one of them to the server, the server sends multiple of them to the clients.*/

public class UpdateMessage : Message
{
    public UserInput userInput;
    public UpdateMessage(UserInput userInput, int playerId)
    {
        _thisMessageType = MessageType.Update;
        this.userInput = userInput;
        PlayerId = playerId;
    }
}
