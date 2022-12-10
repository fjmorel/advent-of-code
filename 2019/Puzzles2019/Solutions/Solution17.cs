namespace Puzzles2019.Solutions;

public record Solution17(long[] _opCodes) : ISolution<Solution17>
{
    public static Solution17 Init(string[] lines) => new(lines[0].ParseCsv<long>());

    public async ValueTask<long> GetPart1()
    {
        var cameraChannel = Channel.CreateUnbounded<long>(new()
        {
            SingleReader = true,
            SingleWriter = true,
        });
        var robotChannel = Channel.CreateUnbounded<long>(new()
        {
            SingleReader = true,
            SingleWriter = true,
        });
        var computer = new IntCodeComputer(_opCodes);
        var compute = computer.Run(robotChannel.Reader, cameraChannel.Writer);

        await compute;

        var view = await MakeView(cameraChannel.Reader);


        var intersections = view.Keys
            .Where(x => view[x] == '#' && x.GetOrthogonal().All(y => view.GetValueOrDefault(y, '.') != '.'))
            .Select<Point, long>(pt => pt.x * pt.y)
            .Sum();

        return intersections;
    }

    private async Task<Dictionary<Point, char>> MakeView(ChannelReader<long> reader)
    {
        var grid = new Dictionary<Point, char>();
        int x = 0, y = 0;
        await foreach (var symbol in reader.ReadAllAsync())
        {
            switch (symbol)
            {
                case '\n':
                    y++;
                    x = 0;
                    break;
                default:
                    grid[new(x, y)] = (char)symbol;
                    x++;
                    break;
            }
        }

        // grid.Keys.Print(pt => grid[pt]);

        return grid;
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }
}
