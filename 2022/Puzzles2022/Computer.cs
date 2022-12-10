namespace Puzzles2022;

public record Computer
{
    private const int SCREEN_WIDTH = 40;
    private const int SCREEN_HEIGHT = 6;
    private readonly List<Instruction> _instructions;

    public Computer(string[] lines)
    {
        _instructions = lines.Select(x => ParseLine(x)).ToList();
    }

    public static Channel<long> CreateInputChannel() => Channel.CreateUnbounded<long>(new() { SingleReader = true, SingleWriter = true });
    public static Channel<State> CreateScreenChannel() => Channel.CreateUnbounded<State>(new() { SingleReader = true, SingleWriter = true });

    private static ChannelReader<long> GetCompletedInputReader()
    {
        var inputChannel = CreateInputChannel();
        inputChannel.Writer.Complete();
        return inputChannel.Reader;
    }

    private static Instruction ParseLine(ReadOnlySpan<char> line)
    {
        var op = Enum.Parse<Op>(line[0..4]);
        return op switch
        {
            Op.noop => new Instruction(op, 0),
            Op.addx => new Instruction(op, int.Parse(line[5..])),
            _ => throw new UnreachableException(),
        };
    }

    public async ValueTask Run(ChannelReader<long>? input, ChannelWriter<State> output)
    {
        input ??= GetCompletedInputReader();

        // Write initial value to push output of other ops to *after* first cycle
        var state = new State(1);
        await output.WriteAsync(state);

        foreach (var instruction in _instructions)
        {
            state = instruction.op switch
            {
                Op.noop => await Noop(output, state),
                Op.addx => await AddX(output, state, instruction.value),
                _ => throw new UnreachableException(),
            };
        }

        output.Complete();
    }

    /// <summary>
    /// 1 cycle, no changes
    /// </summary>
    private static async ValueTask<State> Noop(ChannelWriter<State> output, State state)
    {
        await output.WriteAsync(state);
        return state;
    }

    /// <summary>
    /// 2 cycles, add value to Register X
    /// </summary>
    private static async ValueTask<State> AddX(ChannelWriter<State> output, State state, long value)
    {
        await output.WriteAsync(state);
        state = state with { X = state.X + value };
        await output.WriteAsync(state);
        return state;
    }

    public async IAsyncEnumerable<string> PrintScreen(ChannelReader<State> registers)
    {
        var output = new StringBuilder(SCREEN_WIDTH * SCREEN_HEIGHT + SCREEN_HEIGHT * 2);
        while (await registers.WaitToReadAsync())
        {
            for (var y = 0; y < SCREEN_HEIGHT; y++)
            {
                for (var x = 0; x < SCREEN_WIDTH; x++)
                {
                    var value = (await registers.ReadAsync()).X;
                    if (value == x + 1 || value == x - 1 || value == x)
                        output.Append('⬜');
                    else
                        output.Append('⬛');
                }

                output.AppendLine();
            }

            yield return output.ToString();
            await registers.ReadAsync();
        }
    }

    public readonly record struct State(long X);

    private record struct Instruction(Op op, int value);

    private enum Op
    {
        noop,
        addx,
    }
}
