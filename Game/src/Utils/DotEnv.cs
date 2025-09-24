using DotNetEnv;

public class DotNetVariables
{
    public int ServerPort => int.Parse(Environment.GetEnvironmentVariable("API_KEY") ?? "5000");

    public DotNetVariables()
    {
        ServiceLocator.Register<DotNetVariables>(this);
        Env.Load();
    }
}
