namespace Puzzles2021.Day14;

public record Solution(string _template, Dictionary<(char before, char after), char> _rules) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var template = lines[0];
        var rules = lines.Skip(2).ToDictionary(x => (x[0], x[1]), x => x[^1]);
        return new(template, rules);
    }

    public async ValueTask<long> GetPart1() => Run(10);

    public async ValueTask<long> GetPart2() => Run(40);

    private long Run(int iterations)
    {
        var start = _rules.ToDictionary(x => x.Key, _ => 0L);
        for (var i = 1; i < _template.Length; i++)
            start[(_template[i - 1], _template[i])]++;

        var counts = Enumerable
            .Range(1, iterations)
            .Aggregate(start, (previous, _) =>
                previous.Aggregate(_rules.ToDictionary(x => x.Key, _ => 0L), (next, kv) =>
                {
                    var ((before, after), count) = kv;
                    var middle = _rules[(before, after)];
                    next[(before, middle)] += count;
                    next[(middle, after)] += count;
                    return next;
                }))
            .SelectMany(kv => new (char letter, long count)[] { (kv.Key.before, kv.Value), (kv.Key.after, kv.Value) })
            .GroupBy(x => x.letter).ToDictionary(x => x.Key, x => x.Sum(y => y.count) / 2);

        counts[_template[0]] += 1;
        counts[_template[^1]] += 1;

        return counts.Values.Max() - counts.Values.Min();
    }
}
