namespace Combined.Solutions;

public class Solution10 : ISolution
{
    private readonly HashSet<char> OPEN = new() { '(', '[', '{', '<' };
    private readonly string[] _lines;

    public Solution10(string[] lines)
    {
        _lines = lines;
    }

    public async Task<long> GetPart1()
    {
        var corrupt = new Dictionary<char, long>()
        {
            [')'] = 0,
            [']'] = 0,
            ['}'] = 0,
            ['>'] = 0,
        };
        foreach (var line in _lines)
        {
            var (result, _) = CheckLine(line);
            if (result is char symbol)
                corrupt[symbol]++;
        }

        return corrupt[')'] * 3 + corrupt[']'] * 57 + corrupt['}'] * 1197 + corrupt['>'] * 25137;
    }

    public async Task<long> GetPart2()
    {
        var scores = new List<long>();
        foreach (var line in _lines)
        {
            var (_, result) = CheckLine(line);
            if (result.Any())
                scores.Add(result.Aggregate(0L, (score, symbol) => score * 5 + CloseToScore(symbol)));
        }

        scores.Sort();
        return scores[scores.Count / 2];
    }

    private (char? corruptSymbol, char[] neededToClose) CheckLine(string line)
    {
        var open = new Stack<char>();
        foreach (var symbol in line)
        {
            if (OPEN.Contains(symbol))
                open.Push(symbol);
            else
            {
                if (!open.TryPeek(out var match))
                    throw new InvalidOperationException("Don't know what to do here");

                if (symbol == OpenToClose(match))
                    open.Pop();
                else
                {
                    return (symbol, Array.Empty<char>());
                }
            }
        }

        return (null, open.Select(OpenToClose).ToArray());
    }

    private static char OpenToClose(char open) => open switch
    {
        '[' => ']',
        '{' => '}',
        '<' => '>',
        '(' => ')',
    };

    private static long CloseToScore(char close) => close switch
    {
        ')' => 1,
        ']' => 2,
        '}' => 3,
        '>' => 4,
    };
}
