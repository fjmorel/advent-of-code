namespace Puzzles2019.Solutions;

public record Solution09(long[] _opCodes) : ISolution<Solution09>
{
    public static Solution09 Init(string[] lines) => new(lines[0].ParseCsv<long>());

	public async ValueTask<long> GetPart1()
	{
        var channel = Channel.CreateUnbounded<long>();
        var writer = channel.Writer;
        var reader = channel.Reader;
        await writer.WriteAsync(1);
        await new IntCodeComputer(_opCodes).Run(reader, writer);
        return await reader.ReadAsync();
	}

	public async ValueTask<long> GetPart2()
	{
        var channel = Channel.CreateUnbounded<long>();
        var writer = channel.Writer;
        var reader = channel.Reader;
        await writer.WriteAsync(2);
        await new IntCodeComputer(_opCodes).Run(reader, writer);
        return await reader.ReadAsync();
	}

}
