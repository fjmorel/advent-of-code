namespace Puzzles2019.Solutions;

public class Solution15 : ISolution
{
    private readonly long[] _opCodes;

    public Solution15(string[] lines)
    {
        _opCodes = lines[0].ParseCsvLongs();
    }

    public async ValueTask<long> GetPart1()
    {
        var reportChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true,
        });
        var movementChannel = Channel.CreateBounded<long>(new BoundedChannelOptions(1)
        {
            FullMode = BoundedChannelFullMode.Wait,
            SingleReader = true,
            SingleWriter = true,
        });
        var compute = new IntCodeComputer(_opCodes).Run(movementChannel.Reader, reportChannel.Writer);
        var findSystem = FindSystem(reportChannel.Reader, movementChannel.Writer);
        await Task.WhenAll(compute, findSystem);
        return 0;
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }

    private async Task FindSystem(ChannelReader<long> reports, ChannelWriter<long> movements)
    {
        long steps = 0;
        var area = new Dictionary<Point, PointState>();
        var location = new Point(0, 0);
        area[location] = PointState.Clear;

        var previouslyWall = false;
        var nextMove = Direction.East;
        await movements.WriteAsync((long)nextMove);
        while (await reports.WaitToReadAsync())
        {
            var report = (PointState)await reports.ReadAsync();
            steps++;
            var expectedLocation = Move(location, nextMove);
            area[expectedLocation] = report;
            if (report == PointState.Wall)
            {
                (nextMove, previouslyWall) = (nextMove, previouslyWall) switch
                {
                    // Turn right by default
                    (Direction.North, false) => (Direction.East, true),
                    (Direction.East, false) => (Direction.South, true),
                    (Direction.South, false) => (Direction.West, true),
                    (Direction.West, false) => (Direction.North, true),
                    // If we hit a wall after turning right, turn back around to go left
                    (Direction.North, true) => (Direction.West, true),
                    (Direction.East, true) => (Direction.North, true),
                    (Direction.South, true) => (Direction.East, true),
                    (Direction.West, true) => (Direction.South, true),
                    // (Direction.North, true) => (Direction.South, false),
                    // (Direction.East, true) => (Direction.West, false),
                    // (Direction.South, true) => (Direction.North, false),
                    // (Direction.West, true) => (Direction.East, false),

                    _ => throw new NotSupportedException("Unexpected value"),
                };
            }
            else
            {
                location = expectedLocation;
                previouslyWall = false;
            }

            if (report == PointState.System)
            {
                movements.TryComplete();
                break;
            }

            await movements.WriteAsync((long)nextMove);
        }
    }

    public static Point Move(Point pt, Direction direction) => direction switch
    {
        Direction.North => pt with { y = pt.y - 1 },
        Direction.South => pt with { y = pt.y + 1 },
        Direction.West => pt with { x = pt.x - 1 },
        Direction.East => pt with { x = pt.x + 1 },
        _ => throw new NotSupportedException("Unexpected value"),
    };

    public enum PointState
    {
        Wall = 0,
        Clear = 1,
        System = 2,
    }

    public enum Direction
    {
        North = 1,
        South = 2,
        West = 3,
        East = 4,
    }
}
