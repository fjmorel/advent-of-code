using Spectre.Console;

namespace Puzzles2019.Solutions;

public class Solution11 : ISolution
{
    private readonly long[] _opCodes;

    public Solution11(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
    }

    public async ValueTask<long> GetPart1()
    {
        var grid = await RunPaintJob(0);
        return grid.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        var grid = await RunPaintJob(1);

        var minX = grid.Keys.MinBy(p => p.x).x;
        var maxX = grid.Keys.MaxBy(p => p.x).x;

        var minY = grid.Keys.MinBy(p => p.y).y;
        var maxY = grid.Keys.MaxBy(p => p.y).y;

        // still not sure why this axis was inverted
        AnsiConsole.WriteLine();
        for (var y = maxY; y >= minY; y--)
        {
            for (var x = minX; x <= maxX; x++)
            {
                var color = grid.GetValueOrDefault(new Point(x, y));
                AnsiConsole.Write(color == 1 ? '#' : ' ');
            }

            AnsiConsole.WriteLine();
        }
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();
        AnsiConsole.WriteLine();

        return -1;
    }

    private async Task<Dictionary<Point, long>> RunPaintJob(long startColor)
    {
        var sensorChannel = Channel.CreateUnbounded<long>();
        var botChannel = Channel.CreateUnbounded<long>();

        var computer = new IntCodeComputer(_opCodes);
        var compute = computer.Run(sensorChannel.Reader, botChannel.Writer);
        var paint = Paint(botChannel.Reader, sensorChannel.Writer, startColor);

        await Task.WhenAll(compute, paint);
        sensorChannel.Writer.TryComplete();

        return await paint;
    }

    private async Task<Dictionary<Point, long>> Paint(ChannelReader<long> reader, ChannelWriter<long> writer, long startColor)
    {
        var grid = new Dictionary<Point, long>();

        var current = default(Point);
        var direction = Direction.Up;

        await writer.WriteAsync(startColor);
        while (await reader.WaitToReadAsync())
        {
            var newColor = await reader.ReadAsync();
            grid[current] = newColor;

            var turn = await reader.ReadAsync();
            direction = (direction, turn) switch
            {
                (Direction.Up, 1) => Direction.Right,
                (Direction.Right, 1) => Direction.Down,
                (Direction.Down, 1) => Direction.Left,
                (Direction.Left, 1) => Direction.Up,

                (Direction.Up, 0) => Direction.Left,
                (Direction.Left, 0) => Direction.Down,
                (Direction.Down, 0) => Direction.Right,
                (Direction.Right, 0) => Direction.Up,
                _ => throw new NotSupportedException("Unexpected data"),
            };
            current = direction switch
            {
                Direction.Up => current with { y = current.y + 1 },
                Direction.Down => current with { y = current.y - 1 },

                Direction.Right => current with { x = current.x + 1 },
                Direction.Left => current with { x = current.x - 1 },
                _ => throw new NotSupportedException("Unexpected data"),
            };
            await writer.WriteAsync(grid.GetValueOrDefault(current, startColor));
        }

        return grid;
    }

    public enum Direction
    {
        Up,
        Right,
        Down,
        Left,
    }
}
