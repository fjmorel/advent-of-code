namespace Puzzles2022.Day06;

public record Solution(string _line) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines[0]);

    public async ValueTask<long> GetPart1() => GetMarker(4);

    public async ValueTask<long> GetPart2() => GetMarker(14);

    private long GetMarker(int uniqueCount)
    {
        for (var i = uniqueCount; i < _line.Length; i++)
        {
            if (_line[(i - uniqueCount)..i].Distinct().Count() == uniqueCount)
                return i;
        }

        return 0;
    }
}
