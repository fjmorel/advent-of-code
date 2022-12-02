namespace Puzzles2019.Solutions;

public record Solution05(long[] _opCodes) : ISolution<Solution05>
{
    public static Solution05 Init(string[] lines) => new(lines[0].ParseCsvLongs());

    public ValueTask<long> GetPart1() => Compute(1);

    public ValueTask<long> GetPart2() => Compute(5);

    private async ValueTask<long> Compute(long input)
    {
        var intCode = new IntCodeComputer(_opCodes);

        var inputs = Channel.CreateUnbounded<long>();
        await inputs.Writer.WriteAsync(input);

        var outputs = Channel.CreateUnbounded<long>();

        await intCode.Run(inputs.Reader, outputs.Writer);

        var last = 0L;
        await foreach (var value in outputs.Reader.ReadAllAsync())
            last = value;
        return last;
    }
}
