namespace Puzzles2019.Solutions;

public class Solution02 : ISolution
{
    private readonly long[] _opCodes;
    private readonly Channel<long> _channel;

    public Solution02(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
        _channel = Channel.CreateUnbounded<long>();
    }

    public async ValueTask<long> GetPart1()
    {
        _opCodes[1] = 12;// noun
        _opCodes[2] = 2;// verb
        var memory = await new IntCodeComputer(_opCodes).Run(_channel.Reader, _channel.Writer);
        return memory[0];
    }

    public async ValueTask<long> GetPart2()
    {
        for (var i = 0; i < 100; i++)
        {
            for (var j = 0; j < 100; j++)
            {
                _opCodes[1] = i;// noun
                _opCodes[2] = j;// verb
                var memory = await new IntCodeComputer(_opCodes).Run(_channel.Reader, _channel.Writer);
                if (memory[0] == 19690720)
                    return 100 * i + j;
            }
        }

        return -1;
    }
}
