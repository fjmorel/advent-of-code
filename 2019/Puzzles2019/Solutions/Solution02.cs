namespace Puzzles2019.Solutions;

public record Solution02(long[] _opCodes) : ISolution<Solution02>
{
    private readonly Channel<long> _channel = Channel.CreateUnbounded<long>();

    public static Solution02 Init(string[] lines) => new(lines[0].ParseCsv<long>());

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
