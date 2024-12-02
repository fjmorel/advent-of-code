namespace Puzzles2022.Day05;

public partial record Solution(List<List<char>> _stacks, List<Solution.Command> _commands) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var layers = new Stack<string>();
        var commands = new List<Command>();
        foreach (var line in lines)
        {
            var match = CommandRegex.Match(line);
            if (match.Success)
            {
                commands.Add(new(int.Parse(match.Groups[1].ValueSpan), int.Parse(match.Groups[2].ValueSpan), int.Parse(match.Groups[3].ValueSpan)));
            }
            else
            {
                layers.Push(line);
            }
        }

        layers.Pop();// empty line
        var numberOfStacks = layers.Pop().Split("  ").Length;
        var stacks = Enumerable.Range(1, numberOfStacks + 1).Select(x => new List<char>()).ToList();
        while (layers.Any())
        {
            var layer = layers.Pop();
            for (var i = 1; i <= numberOfStacks; i++)
            {
                var ch = layer[i * 4 - 3];
                if (char.IsLetter(ch))
                    stacks[i].Add(ch);
            }
        }

        return new Solution(stacks, commands);
    }

    public async ValueTask<string> GetPart1String()
    {
        var stacks = GetOriginalStacks();
        foreach (var (count, from, to) in _commands)
            for (var i = 1; i <= count; i++)
                stacks[to].Push(stacks[from].Pop());

        return new string(stacks.Skip(1).Select(x => x.Pop()).ToArray());
    }

    public async ValueTask<string> GetPart2String()
    {
        var stacks = GetOriginalStacks();
        foreach (var (count, from, to) in _commands)
        {
            var values = new Stack<char>();
            for (var i = 1; i <= count; i++)
                values.Push(stacks[from].Pop());

            while (values.Any())
                stacks[to].Push(values.Pop());
        }

        return new string(stacks.Skip(1).Select(x => x.Pop()).ToArray());
    }

    public List<Stack<char>> GetOriginalStacks() =>
        _stacks.Select(stack => new Stack<char>(stack)).ToList();

    public record struct Command(int count, int from, int to);

    [GeneratedRegex("move ([0-9]+) from ([0-9]+) to ([0-9]+)")]
    public static partial Regex CommandRegex { get; }
}
