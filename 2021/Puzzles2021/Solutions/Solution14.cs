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

    public async ValueTask<long> GetPart1()
    {
        var final = Enumerable.Range(1, 10).Aggregate(GetEmpty(), Insert);
        var totals = GetCounts(final);
        return totals.Max() - totals.Min();
    }

    public async ValueTask<long> GetPart2()
    {
        var final = Enumerable.Range(1, 40).Aggregate(GetEmpty(), Insert);
        var totals = GetCounts(final);
        return totals.Max() - totals.Min();
    }

    private Dictionary<(char, char), long> GetEmpty(bool initialize = true)
    {
        var counts = new Dictionary<(char, char), long>();
        foreach (var rule in _rules.Keys)
            counts[rule] = 0;

        if (initialize)
            for (var i = 1; i < _template.Length; i++)
                counts[(_template[i - 1], _template[i])]++;

        return counts;
    }

    private IReadOnlyCollection<long> GetCounts(Dictionary<(char, char), long> pairs)
    {
        var result = new Dictionary<char, long>();
        foreach (var letter in _letters)
            result[letter] = 0;

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
        var next = GetEmpty(false);
        foreach (var ((before, after), value) in previous)
        {
            var middle = _rules[(before, after)];
            next[(before, middle)] += value;
            next[(middle, after)] += value;
        }

        return next;
    }
}
