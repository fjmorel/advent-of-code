namespace PuzzlesTemplate.Solutions;

public record Solution01(string[] _lines) : ISolution<Solution01>
{
    public static Solution01 Init(string[] lines)
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
