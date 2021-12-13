namespace Puzzles2020.Solutions;

public class Solution13 : ISolution
{
    private readonly string[] _lines;

    public Solution13(string[] lines)
    {
        _lines = lines;
    }

    public async ValueTask<long> GetPart1()
    {
        var earliest = int.Parse(_lines[0]);
        var firstBus = _lines[1]
            .Split(',')
            .Where(x => x != "x")
            .Select(x => int.Parse(x))
            .Select(x => (id: x, delay: (earliest / x + 1) * x))
            .OrderBy(x => x.delay)
            .First();
        return (firstBus.id * (firstBus.delay - earliest));
    }

    public async ValueTask<long> GetPart2()
    {
        var desc = _lines[1]
            .Split(',')
            .Select((id, i) => (id, i))
            .Where(x => x.id != "x")
            .Select(x => (id: long.Parse(x.id), index: x.i))
            .OrderByDescending(x => x.id)
            .ToList();
        var biggest = desc.First();
        long ts = biggest.id - biggest.index;
        long factor = 1;
        foreach (var x in desc)
        {
            while ((ts + x.index) % x.id != 0)
                ts += factor;
            factor *= x.id;
        }

        return ts;
    }
}
