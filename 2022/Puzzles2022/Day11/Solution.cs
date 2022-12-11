namespace Puzzles2022.Day11;

public partial record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1() => Run(20, 3);

    public async ValueTask<long> GetPart2() => Run(10_000, 1);

    public long Run(int numberOfIterations, long worryReduction)
    {
        var monkeys = _lines.Chunk(7).Select(ParseMonkey).ToList();
        var leastCommonMultiple = monkeys.Aggregate(1L, (lcm, monkey) => lcm * monkey.divisor);
        for (var i = 0; i < numberOfIterations; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.Inspected += monkey.Items.Count;
                while (monkey.Items.TryDequeue(out var item))
                {
                    var newValue = Operate(item, monkey.operation) / worryReduction % leastCommonMultiple;
                    var test = newValue % monkey.divisor == 0;
                    var index = test ? monkey.ifTrue : monkey.ifFalse;
                    monkeys[index].Items.Enqueue(newValue);
                }
            }
        }

        var highest = monkeys.OrderByDescending(x => x.Inspected).Take(2).Select(x => x.Inspected).ToList();
        return highest[0] * highest[1];
    }

    public static long Operate(long item, Operation op) => op.op switch
    {
        Op.Square => item * item,
        Op.Add => item + op.number,
        Op.Multiply => item * op.number,
        _ => throw new UnreachableException(),
    };

    private static Monkey ParseMonkey(string[] lines)
    {
        var items = new Queue<long>(lines[1][18..].ParseCsv<long>());

        var op = GetOperationRegex().Match(lines[2]).Groups;
        Operation operation;
        if (op[2].ValueSpan is "old")
            operation = new(Op.Square, 0);
        else
        {
            var enumValue = op[1].ValueSpan switch
            {
                "*" => Op.Multiply,
                "+" => Op.Add,
                _ => throw new UnreachableException(),
            };
            var num = long.Parse(op[2].ValueSpan);
            operation = new(enumValue, num);
        }

        var testDivisor = int.Parse(lines[3].AsSpan()[21..]);
        var trueMonkey = lines[4][^1] - '0';
        var falseMonkey = lines[5][^1] - '0';

        return new(items, operation, testDivisor, trueMonkey, falseMonkey);
    }


    public record Monkey(Queue<long> Items, Operation operation, long divisor, int ifTrue, int ifFalse)
    {
        public long Inspected { get; set; }
    }

    public record struct Operation(Op op, long number);

    public enum Op
    {
        Add,
        Multiply,
        Square,
    }

    [GeneratedRegex("= old ([+*]) ([a-z0-9]+)")]
    private static partial Regex GetOperationRegex();
}
