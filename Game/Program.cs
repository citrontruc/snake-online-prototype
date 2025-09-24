/* Main program to launch all our code. */

public class Game
{
    public static void Main()
    {
        Console.WriteLine("Type l to launch and j to join.");
        string userInput = new("");
        while (userInput != "l" && userInput != "j")
        {
            userInput = Console.ReadLine() ?? "";
        }
        if (userInput == "l")
        {
            SnakeServer.LaunchServer();
        }
        else
        {
            SnakeClient.JoinServer();
        }        
    }
}
