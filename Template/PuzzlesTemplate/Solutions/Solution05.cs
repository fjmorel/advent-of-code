namespace PuzzlesTemplate.Solutions;

public record Solution05(string[] _lines) : ISolution<Solution05>
{
    public static Solution05 Init(string[] lines)
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
