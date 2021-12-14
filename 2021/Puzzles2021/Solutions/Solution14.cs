namespace Puzzles2021.Solutions;

public class Solution14 : ISolution
{
    private readonly HashSet<char> _letters;
    private readonly Dictionary<(char, char), char> _rules = new();
    private readonly string _template;

    public Solution14(string[] lines)
    {
        _template = lines[0];
        foreach (var line in lines.Skip(2))
            _rules[(line[0], line[1])] = line[^1];

        _letters = _rules.SelectMany(x => new char[] { x.Key.Item1, x.Key.Item2, x.Value }).ToHashSet();
    }

    public async ValueTask<long> GetPart1() => Run(10);

    public async ValueTask<long> GetPart2() => Run(40);

    private long Run(int iterations)
    {
        var start = GetEmpty();
        for (var i = 1; i < _template.Length; i++)
            start[(_template[i - 1], _template[i])]++;

        var final = Enumerable.Range(1, iterations).Aggregate(start, Insert);
        var totals = GetCounts(final);
        return totals.Max() - totals.Min();
    }

    private Dictionary<(char, char), long> GetEmpty() => _rules.ToDictionary(x => x.Key, _ => 0L);

    private IReadOnlyCollection<long> GetCounts(Dictionary<(char, char), long> pairs)
    {
        var result = _letters.ToDictionary(x => x, _ => 0L);

        foreach (var ((before, after), value) in pairs)
        {
            result[before] += value;
            result[after] += value;
        }

        foreach (var letter in _letters)
            result[letter] /= 2;
        result[_template[0]] += 1;
        result[_template[^1]] += 1;

        return result.Values;
    }

    private Dictionary<(char, char), long> Insert(Dictionary<(char, char), long> previous, int _)
    {
        var next = GetEmpty();
        foreach (var ((before, after), value) in previous)
        {
            var middle = _rules[(before, after)];
            next[(before, middle)] += value;
            next[(middle, after)] += value;
        }

        return next;
    }
}
