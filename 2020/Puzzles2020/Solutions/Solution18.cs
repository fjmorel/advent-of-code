namespace Puzzles2020.Solutions;

public record Solution18(string[] list) : ISolution<Solution18>
{
    public static Solution18 Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1()
    {
        return list.Sum(x => CalcLine(x, NoPrecedence));
    }

    public async ValueTask<long> GetPart2()
    {
        return list.Sum(x => CalcLine(x, AddFirst));
    }

    private long CalcLine(string line, Func<string, long> calculator)
    {
        var open = line.LastIndexOf('(');
        if (open > -1)
        {
            var close = line.IndexOf(')', open + 1);
            return CalcLine(line[0..open] + calculator(line[(open + 1)..close]) + line[(close + 1)..], calculator);
        }

        return calculator(line);
    }

    private static long NoPrecedence(string line)
    {
        var items = line.Split(' ').ToList();
        var num = long.Parse(items[0]);
        for (var i = 2; i < items.Count; i += 2)
        {
            var newNum = long.Parse(items[i]);
            num = items[i - 1] == "*" ? num * newNum : num + newNum;
        }

        return num;
    }

    private static long AddFirst(string line) => line.Split(" * ")
        .Select(sub => sub.Split(" + ").Sum(long.Parse))
        .Aggregate(1L, (acc, value) => acc * value);
}
