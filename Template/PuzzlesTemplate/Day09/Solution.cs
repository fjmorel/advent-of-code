namespace PuzzlesTemplate.Day09;

public record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        return new Solution(lines);
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
