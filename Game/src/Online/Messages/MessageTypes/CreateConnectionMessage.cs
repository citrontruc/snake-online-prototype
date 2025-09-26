/* A message that the server sends to all the clients in order to start the game.
It contains the information for the board of the game as well as the id the player is supposed to have.*/

public class CreateConnectionGame : Message
{
    public string SessionName;

    public CreateConnectionGame(string sessionName, int playerId)
    {
        _thisMessageType = MessageType.Update;
        SessionName = sessionName;
        PlayerId = playerId;
    }
}
