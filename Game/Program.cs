/* Main program to launch all our code. */

public class Game
{
    public static void Main()
    {
        DotNetVariables dotnetVariables = new();
        Console.WriteLine("Type l to launch and j to join.");
        string userInput = new("");
        while (userInput != "l" && userInput != "j")
        {
            userInput = Console.ReadLine() ?? "";
        }
        if (userInput == "l")
        {
            SnakeServer snakeServer = new();
            snakeServer.LaunchServer();
            for (int i = 0; i < 2; i++)
            {
                snakeServer.ConnectToClient();
            }
            snakeServer.CreateClientThreads();
        }
        else
        {
            SnakeClient.JoinServer();
        }
    }
}
