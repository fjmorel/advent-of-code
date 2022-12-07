namespace Puzzles2022.Day07;

public record Solution(Dictionary<string, long> _totalPerDirectory) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var current = "";
        var sizePerDirectory = new Dictionary<string, long>();
        foreach (var line in lines)
        {
            if (line.StartsWith("$ cd"))
                current = line switch
                {
                    "$ cd .." => current[0..(current.LastIndexOf('/'))],
                    "$ cd /" => "/",
                    _ => current + '/' + line[5..],
                };
            else if (line.StartsWith("$ ls"))
                sizePerDirectory[current] = 0;
            else if (line.StartsWith("dir "))
                continue;
            else
                sizePerDirectory[current] += long.Parse(line.AsSpan()[0..line.IndexOf(' ')]);
        }

        return new Solution(GetTotalSizes(sizePerDirectory));
    }

    public async ValueTask<long> GetPart1() => _totalPerDirectory.Values.Where(x => x <= 100_000).Sum();

    public async ValueTask<long> GetPart2()
    {
        var free = 70_000_000 - _totalPerDirectory["/"];
        var needToFree = 30_000_000 - free;
        return _totalPerDirectory.Where(x => x.Value >= needToFree).MinBy(x => x.Value).Value;
    }

    private static Dictionary<string, long> GetTotalSizes(Dictionary<string, long> sizes) =>
        sizes.ToDictionary(x => x.Key, kv => sizes.Keys.Where(k => k.StartsWith(kv.Key)).Sum(key => sizes[key]));
}
