namespace Combined.Solutions;

public class Solution10 : ISolution
{
    private static readonly char[] OPEN = "([{<".ToCharArray();
    private static readonly char[] CLOSE = ")]}>".ToCharArray();
    private readonly string[] _lines;

    public Solution10(string[] lines)
    {
        _lines = lines;
    }

    public async Task<long> GetPart1()
    {
        var corrupt = new Dictionary<char, long>();
        foreach (var symbol in CLOSE)
            corrupt[symbol] = 0;
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
                scores.Add(result.Aggregate(0L, (score, symbol) => score * 5 + Array.IndexOf(CLOSE, symbol) + 1));
        }

        scores.Sort();
        return scores[scores.Count / 2];
    }

    private static (char? corruptSymbol, char[] neededToClose) CheckLine(string line)
    {
        var stack = new Stack<char>();
        foreach (var symbol in line)
        {
            if (OPEN.Contains(symbol))
                stack.Push(symbol);
            else
            {
                var open = stack.Peek();
                if (symbol == OpenToClose(open))
                    stack.Pop();
                else
                    return (symbol, Array.Empty<char>());
            }
        }

        return (null, stack.Select(OpenToClose).ToArray());
    }

    private static char OpenToClose(char open) => CLOSE[Array.IndexOf(OPEN, open)];
}
