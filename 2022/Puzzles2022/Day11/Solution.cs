namespace Puzzles2022.Day11;

public partial record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1() => Run(20, 3);

    public async ValueTask<long> GetPart2() => Run(10_000, 1);

    public long Run(int numberOfIterations, long worryReduction)
    {
        var monkeys = _lines.Chunk(7).Select(Monkey.Parse).ToList();
        var leastCommonMultiple = monkeys.Aggregate(1L, (lcm, monkey) => lcm * monkey.divisor);
        for (var i = 0; i < numberOfIterations; i++)
        {
            foreach (var monkey in monkeys)
            {
                monkey.TakeTurn(monkeys, worryReduction, leastCommonMultiple);
            }
        }

        var highest = monkeys.OrderByDescending(x => x.Inspected).Take(2).Select(x => x.Inspected).ToList();
        return highest[0] * highest[1];
    }

    public record Monkey(Queue<long> items, Operation operation, long divisor, int ifTrue, int ifFalse)
    {
        public long Inspected { get; private set; }

        public void TakeTurn(List<Monkey> allMonkeys, long worryReduction, long leastCommonMultiple)
        {
            Inspected += items.Count;
            while (items.TryDequeue(out var item))
            {
                var newValue = operation.Run(item) / worryReduction % leastCommonMultiple;
                var test = newValue % divisor == 0;
                var index = test ? ifTrue : ifFalse;
                allMonkeys[index].items.Enqueue(newValue);
            }
        }

        public static Monkey Parse(string[] lines)
        {
            var items = new Queue<long>(lines[1][18..].ParseCsv<long>());
            var op = Operation.Parse(lines[2]);
            var divisor = int.Parse(lines[3].AsSpan()[21..]);
            var ifTrue = lines[4][^1] - '0';
            var ifFalse = lines[5][^1] - '0';

            return new(items, op, divisor, ifTrue, ifFalse);
        }
    }

    public readonly partial record struct Operation(Operation.Op op, long number)
    {
        public long Run(long item) => op switch
        {
            Op.Square => item * item,
            Op.Add => item + number,
            Op.Multiply => item * number,
            _ => throw new UnreachableException(),
        };

        public static Operation Parse(string s)
        {
            var groups = OperationRegex.Match(s).Groups;
            if (groups[2].ValueSpan is "old")
                return new(Op.Square, 0);

            var enumValue = groups[1].ValueSpan switch
            {
                "*" => Op.Multiply,
                "+" => Op.Add,
                _ => throw new UnreachableException(),
            };
            var num = long.Parse(groups[2].ValueSpan);
            return new(enumValue, num);
        }

        [GeneratedRegex("= old ([+*]) ([a-z0-9]+)")]
        private static partial Regex OperationRegex { get; }

        public enum Op
        {
            Add,
            Multiply,
            Square,
        }
    }
}
