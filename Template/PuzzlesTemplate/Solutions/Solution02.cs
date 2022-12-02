namespace PuzzlesTemplate.Solutions;

public record Solution02(string[] _lines) : ISolution<Solution02>
{
    public static Solution02 Init(string[] lines)
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

