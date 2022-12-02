namespace Puzzles2020.Solutions;

public record Solution15(List<(int num, int i)> list) : ISolution<Solution15>
{
    public static Solution15 Init(string[] lines)
    {
        var list = lines[0].Split(',')
            .Select((x, i) => (num: int.Parse(x), i))
            .ToList();
        return new(list);
    }

    public async ValueTask<long> GetPart1() => FindNth(2020);

    public async ValueTask<long> GetPart2() => FindNth(30_000_000);

    private int FindNth(int limit)
    {
        var dict = list.SkipLast(1).ToDictionary(x => x.num, x => x.i + 1);
        var last = list[^1].num;
        for (var i = list.Count; i < limit; i++)
        {
            var pos = dict.GetValueOrDefault(last, i);
            dict[last] = i;
            last = i - pos;
        }

        return last;
    }
}
