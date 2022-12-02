namespace Puzzles2022.Solutions;

public record Solution04(string[] _lines) : ISolution<Solution04>
{
    public static Solution04 Init(string[] lines)
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
