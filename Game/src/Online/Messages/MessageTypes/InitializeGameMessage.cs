/* A message that the server sends to all the clients in order to start the game.
It contains the information for the board of the game as well as the id the player is supposed to have.*/

public class InitializeGame : Message
{
    public string test = "312";

    public InitializeGame(int playerId)
    {
        _thisMessageType = MessageType.InitializeGame;
        PlayerId = playerId;
    }
}
