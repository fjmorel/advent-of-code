namespace Puzzles2022.Day21;

using Monkey = OneOf.OneOf<Solution.Equation, long, bool>;

public partial record Solution(Dictionary<string, Monkey> _original) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.Aggregate(new Dictionary<string, Monkey>(), ParseLine));

    private static Dictionary<string, Monkey> ParseLine(Dictionary<string, Monkey> monkeys, string line)
    {
        var name = line[0..4];
        var numMatch = ConstantMonkeyRegex.Match(line);
        if (numMatch.Success)
            monkeys[name] = int.Parse(numMatch.Groups[1].ValueSpan);
        else
        {
            var opMatch = EquationMonkeyRegex.Match(line);
            monkeys[name] = new Equation(opMatch.Groups[2].ValueSpan[0], opMatch.Groups[1].Value, opMatch.Groups[3].Value);
        }

        return monkeys;
    }

    [GeneratedRegex(": ([0-9]+)")]
    private static partial Regex ConstantMonkeyRegex { get; }

    [GeneratedRegex(": ([a-z]+) ([/*\\-+]) ([a-z]+)")]
    private static partial Regex EquationMonkeyRegex { get; }

    public async ValueTask<long> GetPart1() => Simplify(_original["root"], new(_original)).AsT1;

    public async ValueTask<long> GetPart2()
    {
        var monkeys = new Dictionary<string, Monkey>(_original) { ["humn"] = true };
        var rootEquation = monkeys["root"].AsT0;
        var bValue = Simplify(monkeys[rootEquation.right], monkeys).AsT1;
        var aEquation = Simplify(monkeys[rootEquation.left], monkeys).AsT0!;
        return FindHumanValue(bValue, aEquation, monkeys);
    }

    public static long FindHumanValue(long y, Equation equation, Dictionary<string, Monkey> monkeys)
    {
        while (true)
        {
            var left = monkeys[equation.left];
            var right = monkeys[equation.right];
            // change y = x?b to isolate x
            if (right.TryPickT1(out var b, out _))
            {
                y = equation.operation switch
                {
                    '+' => y - b,
                    '*' => y / b,
                    '-' => y + b,
                    '/' => y * b,
                };
                if (left.TryPickT0(out var newEquation, out _))
                {
                    equation = newEquation;
                    continue;
                }
            }
            // change y = a?b to isolate x
            else if (left.TryPickT1(out var a, out _))
            {
                y = equation.operation switch
                {
                    '+' => y - a,
                    '*' => y / a,
                    '-' => a - y,
                    '/' => a / y,
                };
                if (right.TryPickT0(out var newEquation, out _))
                {
                    equation = newEquation;
                    continue;
                }
            }

            return y;
        }
    }

    public static Monkey Simplify(Monkey node, Dictionary<string, Monkey> monkeys)
    {
        // if value, already simplified. If human input, can't simplify
        if (!node.TryPickT0(out var equation, out _))
            return node;

        monkeys[equation.left] = Simplify(monkeys[equation.left], monkeys);
        monkeys[equation.right] = Simplify(monkeys[equation.right], monkeys);

        if (monkeys[equation.left].TryPickT1(out var a, out _) && monkeys[equation.right].TryPickT1(out var b, out _))
        {
            return equation.operation switch
            {
                '+' => a + b,
                '-' => a - b,
                '*' => a * b,
                '/' => a / b,
            };
        }

        return node;
    }

    public record Equation(char operation, string left, string right);
}
