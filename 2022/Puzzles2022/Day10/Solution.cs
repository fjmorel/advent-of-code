namespace Puzzles2022.Day10;

public record Solution(List<Solution.Instruction> _lines) : ISolution<Solution>
{
    private const int WIDTH = 40;
    private const int HEIGHT = 6;
    private readonly List<long> _x = new() { 1 };

    public static Solution Init(string[] lines)
    {
        return new Solution(lines.Select(x => ParseLine(x)).ToList());
    }

    public static Instruction ParseLine(ReadOnlySpan<char> line)
    {
        var op = Enum.Parse<Op>(line[0..4]);
        return op switch
        {
            Op.noop => new Instruction(op, 0),
            Op.addx => new Instruction(op, int.Parse(line[5..])),
        };
    }

    public async ValueTask<long> GetPart1()
    {
        long currentX = 1;
        _x.Add(currentX);
        foreach (var instruction in _lines)
        {
            _x.Add(currentX);
            if (instruction.op == Op.noop)
                continue;
            else if (instruction.op == Op.addx)
            {
                currentX += instruction.value;
                _x.Add(currentX);
            }
        }

        var value = 0L;
        for (var i = 20; i <= 220; i += 40)
            value += _x[i] * i;
        return value;
    }

    public async ValueTask<string> GetPart2String()
    {
        if (_x.Count == 1)
            await GetPart1();
        var output = new StringBuilder(WIDTH * HEIGHT + HEIGHT * 2);
        for (var y = 0; y < HEIGHT; y++)
        {
            for (var x = 0; x < WIDTH; x++)
            {
                var cycle = x + (y * WIDTH) + 1;
                var value = _x[cycle];
                if (value == x + 1 || value == x - 1 || value == x)
                    output.Append('#');
                else
                    output.Append('.');
            }

            output.AppendLine();
        }

        return output.ToString();
    }
    //
    // public static long GetValueAtCycle(Dictionary<long, long> register, long cycle)
    // {
    //     while (cycle > 0)
    //     {
    //         if (register.TryGetValue(cycle, out long value))
    //             return value;
    //         cycle--;
    //     }
    //
    //     return 1;
    // }

    public record struct Instruction(Op op, int value);

    public enum Op
    {
        noop,
        addx,
    }
}
