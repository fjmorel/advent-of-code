namespace Puzzles2019.Solutions;

public class Solution09 : ISolution
{

    private readonly long[] _opCodes;
	public Solution09(string[] lines)
	{
        _opCodes = lines[0].ParseCsvLongs();
	}

	public async ValueTask<long> GetPart1()
	{
        var channel = Channel.CreateUnbounded<long>();
        var writer = channel.Writer;
        var reader = channel.Reader;
        await writer.WriteAsync(1);
        await new Computer(_opCodes).Run(reader, writer);
        return await reader.ReadAsync();
	}

	public async ValueTask<long> GetPart2()
	{
        var channel = Channel.CreateUnbounded<long>();
        var writer = channel.Writer;
        var reader = channel.Reader;
        await writer.WriteAsync(2);
        await new Computer(_opCodes).Run(reader, writer);
        return await reader.ReadAsync();
	}

}
