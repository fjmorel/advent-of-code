public interface ISolution
{
    Task<long> GetPart1();
    Task<long> GetPart2();
}

public readonly record struct Point(int x, int y)
{
    public Point[] GetAdjacent() => new[]
    {
        this with { x = x + 1 },
        this with { x = x - 1 },
        this with { y = y + 1 },
        this with { y = y - 1 },
    };
}
