namespace Puzzles2019.Solutions;

public class Solution13 : ISolution
{
    private readonly long[] _opCodes;

    public Solution13(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
    }

    public async ValueTask<long> GetPart1()
    {
        var screenChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(3)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true,
        });
        var inputChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = true,
        });
        var screen = new Dictionary<Point, TileState>();
        var computer = new IntCodeComputer(_opCodes);
        var compute = computer.Run(inputChannel.Reader, screenChannel.Writer);
        var paint = Paint(screen, screenChannel.Reader, inputChannel.Writer);

        await Task.WhenAll(compute, paint);

        return screen.Values.Count(x => x == TileState.Block);
    }

    public async ValueTask<long> GetPart2()
    {
        _opCodes[0] = 2;// "play for free"
        var screenChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(3)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true,
        });
        var inputChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.DropOldest,
            SingleReader = true,
            SingleWriter = true,
        });

        var screen = new Dictionary<Point, TileState>();
        var computer = new IntCodeComputer(_opCodes);
        var compute = computer.Run(inputChannel.Reader, screenChannel.Writer);
        var paint = Paint(screen, screenChannel.Reader, inputChannel.Writer);

        await Task.WhenAll(compute, paint);
        inputChannel.Writer.TryComplete();

        return await paint;
    }

    private async Task<long> Paint(Dictionary<Point, TileState> screen, ChannelReader<long> output, ChannelWriter<long> input)
    {
        bool enableOutput = true;
        try
        {
            AnsiConsole.Clear();
        }
        catch (Exception)
        {
            enableOutput = false;
            AnsiConsole.WriteLine("Console manipulation has been disabled due to redirection");
        }

        long score = 0, paddleX = 0, paddleY = 0;
        await input.WriteAsync(0);
        while (await output.WaitToReadAsync())
        {
            var x = await output.ReadAsync();
            var y = await output.ReadAsync();
            var state = await output.ReadAsync();

            if (x == -1 && y == 0)
            {
                score = state;
                if (enableOutput)
                {
                    AnsiConsole.Cursor.SetPosition(0, 0);
                    AnsiConsole.Write("Score: " + score);
                }
            }
            else
            {
                var tile = (TileState)state;
                if (tile == TileState.HorizontalPaddle)
                {
                    paddleX = x;
                    paddleY = y;
                }

                if (tile == TileState.Ball)
                {
                    var num = 0;
                    if (x > paddleX)
                        num = 1;
                    else if (x < paddleX)
                        num = -1;
                    await input.WriteAsync(num);
                }

                screen[new(x, y)] = tile;

                if (enableOutput)
                {
                    AnsiConsole.Cursor.SetPosition((int)x, (int)y + 1);
                    var ch = tile switch
                    {
                        TileState.Empty => " ",
                        TileState.Ball => "O",
                        TileState.Block => "=",
                        TileState.HorizontalPaddle => "=",
                        TileState.Wall => "*",
                        _ => throw new NotSupportedException("Unexpected tile state"),
                    };
                    AnsiConsole.Write(ch);
                }
            }
        }

        if (enableOutput)
            AnsiConsole.Cursor.SetPosition(0, (int)paddleY + 3);
        return score;
    }
    public readonly record struct Point(long x, long y);

    public enum TileState
    {
        /// <summary>Nothing</summary>
        Empty = 0,

        /// <summary>Indestructible</summary>
        Wall = 1,

        /// <summary>Can be broken by the ball</summary>
        Block = 2,

        /// <summary>Indestructible</summary>
        HorizontalPaddle = 3,

        /// <summary>Moves diagonally and bounces off objects</summary>
        Ball = 4,
    }
}
