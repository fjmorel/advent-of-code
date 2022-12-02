namespace PuzzlesTemplate.Solutions;

public record Solution03(string[] _lines) : ISolution<Solution03>
{
    public static Solution03 Init(string[] lines)
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

