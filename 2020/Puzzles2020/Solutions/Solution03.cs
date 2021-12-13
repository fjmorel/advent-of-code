namespace Puzzles2020.Solutions;

public class Solution03 : ISolution
{
    private readonly string[] _lines;
    private readonly int _width;

    public Solution03(string[] lines)
    {
        _lines = lines;
        _width = lines[0].Length;
    }

    public async ValueTask<long> GetPart1() => HowManyHits(3, 1);

    public async ValueTask<long> GetPart2()
    {
        return new[]
        {
            HowManyHits(1, 1),
            HowManyHits(3, 1),
            HowManyHits(5, 1),
            HowManyHits(7, 1),
            HowManyHits(1, 2),
        }.Aggregate((long)1, (acc, x) => acc * x);
    }

    long HowManyHits(int right, int down)
    {
        var treesHit = 0;
        for (var i = 0; (i * down) < _lines.Length; i++)
        {
            if (_lines[i * down][(i * right) % _width] == '#')
                treesHit++;
        }

        return treesHit;
    }
}
