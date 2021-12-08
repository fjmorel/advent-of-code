public interface ISolution
{
    Task<long> GetPart1();
    Task<long> GetPart2();
}

public readonly record struct Point(int x, int y);
