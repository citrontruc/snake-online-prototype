public class HostLobby : Menu
{
    private SnakeServer _snakeServer => ServiceLocator.Get<SnakeServer>();
    int playerNumber = 0;

    public HostLobby()
        : base("Waiting for other players...")
    {
        ServiceLocator.Register<HostLobby>(this);
    }

    public override void Load()
    {
        playerNumber = 0;
        LaunchServer();
        _snakeServer.CreateClientThreads();
    }

    public override void Unload() { }

    public void LaunchServer()
    {
        int playerNumber = 0;
        bool validPlayerNumber = false;
        while (!validPlayerNumber)
        {
            Console.WriteLine("Choose number of players in the game: ");
            string? consoleResult = Console.ReadLine();
            bool validInt = int.TryParse(consoleResult, out playerNumber);
            if (validInt && playerNumber > 1 && playerNumber <= 4)
            {
                validPlayerNumber = true;
            }
            else
            {
                Console.WriteLine("Game can only have between 2 and 4 players.");
            }
        }
        _snakeServer.LaunchServer();
        for (int i = 1; i < playerNumber; i++)
        {
            _snakeServer.ConnectToClient();
        }
    }
}
