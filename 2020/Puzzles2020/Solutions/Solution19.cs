namespace Puzzles2020.Solutions;

public class Solution19 : ISolution
{
    private readonly string[] _lines;
    private readonly Dictionary<int, Rule> rules = new();
    private int i = 0;

    public Solution19(string[] lines)
    {
        _lines = lines;
        for (; lines[i] != ""; i++)
        {
            var pieces = lines[i].Split(": ");
            var index = int.Parse(pieces[0]);
            if (pieces[1][0] == '"')
                rules[index] = new Rule(pieces[1][1], null);
            else
                rules[index] = new Rule(null, pieces[1].Split(" | ").Select(x => x.Split(" ").Select(int.Parse).ToList()).ToList());
        }

        i++;
    }

    public async ValueTask<long> GetPart1()
    {
        //var potentials = GeneratePossibilities(rules, rules[0]);
        return _lines.Skip(i)
            .Select(line => (line.Length, result: IsValid(rules, line, rules[0])))
            .Count(x => x.result.valid && x.result.handled == x.Length);
        // if (potentials.Contains(line) || potentials.Any(x => x.StartsWith(line)))
        // 	valid++;
    }

    public async ValueTask<long> GetPart2()
    {
        // 8: 42 | 42 8
        // 11: 42 31 | 42 11 31
        rules[8] = new Rule(null, new() { new() { 42 }, new() { 42, 8 } });
        rules[11] = new Rule(null, new() { new() { 42, 31 }, new() { 42, 11, 31 } });

        var potential42s = GeneratePossibilities(rules, rules[42]);
        var potential31s = GeneratePossibilities(rules, rules[31]);

        var valid = 0;
        foreach (var line in _lines.Skip(i))
        {
            int num42 = 0, num31 = 0;
            var sub = line;
            // todo: figure out how to restore this
            // var sub = line.AsSpan();
            while (sub.Length != 0)
            {
                var found = false;
                foreach (var fortyTwo in potential42s)
                {
                    if (sub.StartsWith(fortyTwo))
                    {
                        sub = sub[fortyTwo.Length..];
                        found = true;
                        num42++;
                        break;
                    }
                }

                if (!found)
                    foreach (var thirtyOne in potential31s)
                    {
                        if (sub.EndsWith(thirtyOne))
                        {
                            sub = sub[0..^thirtyOne.Length];
                            found = true;
                            num31++;
                            break;
                        }
                    }

                if (!found)
                    break;
            }

            var isValid = sub.Length == 0 && num42 > num31 && num31 > 0;
            if (isValid)
                valid++;
        }

        return valid;
    }

    private static (bool valid, int handled) IsValid(IReadOnlyDictionary<int, Rule> rules, ReadOnlySpan<char> substring, Rule rule)
    {
        if (substring.IsEmpty)
            return (true, 0);

        if (rule.letter.HasValue)
            return (rule.letter == substring[0], 1);

        foreach (var alternative in rule.options ?? Enumerable.Empty<List<int>>())
        {
            var isValid = true;
            var counted = 0;
            foreach (var subRule in alternative.Select(x => rules[x]))
            {
                var (valid, handled) = IsValid(rules, substring[counted..], subRule);
                if (!valid)
                {
                    isValid = false;
                    break;
                }

                counted += handled;
            }

            if (isValid)
            {
                return (true, counted);
            }
        }

        return (false, 0);
    }

    private static HashSet<string> GeneratePossibilities(IReadOnlyDictionary<int, Rule> rules, Rule rule)
    {
        if (rule.letter.HasValue)
            return new() { rule.letter.Value.ToString() };

        var lines = new HashSet<string>();
        foreach (var alternate in rule.options ?? Enumerable.Empty<List<int>>())
        {
            var sublist = new HashSet<string>() { "" };
            foreach (var subRule in alternate.Select(x => rules[x]))
            {
                var subtrees = GeneratePossibilities(rules, subRule);
                sublist = sublist.SelectMany(x => subtrees.Select(y => x + y)).ToHashSet();
            }

            lines.UnionWith(sublist);
        }

        return lines;
    }

    private record Rule(char? letter, List<List<int>>? options);
}
