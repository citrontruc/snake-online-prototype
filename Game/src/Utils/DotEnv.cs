using DotNetEnv;

public class DotNetVariables
{
    public int ServerPort => int.Parse(Environment.GetEnvironmentVariable("SERVER_PORT") ?? "5000");
    public string ServerIP => Environment.GetEnvironmentVariable("SERVER_IP") ?? "127.0.0.1";
    public string? SessionName => Environment.GetEnvironmentVariable("SESSION_NAME");

    public DotNetVariables()
    {
        ServiceLocator.Register<DotNetVariables>(this);
        Env.Load();
    }
}
