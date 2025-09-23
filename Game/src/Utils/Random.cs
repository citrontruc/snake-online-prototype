/* A class to have global randomness */

public static class RandomGlobal
{
    static readonly Random _rnd = new(42);

    public static int Next(int upperBound)
    {
        return _rnd.Next(upperBound);
    }
}
