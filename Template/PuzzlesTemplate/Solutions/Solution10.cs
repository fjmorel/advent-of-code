namespace PuzzlesTemplate.Solutions;

public record Solution10(string[] _lines) : ISolution<Solution10>
{
    public static Solution10 Init(string[] lines)
    {
        return new(lines);
    }

    public async ValueTask<long> GetPart1()
    {
        return 0;
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }
}
