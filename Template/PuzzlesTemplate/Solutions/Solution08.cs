namespace PuzzlesTemplate.Solutions;

public record Solution08(string[] _lines) : ISolution<Solution08>
{
    public static Solution08 Init(string[] lines)
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
