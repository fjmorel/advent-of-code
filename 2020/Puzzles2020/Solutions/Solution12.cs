namespace Puzzles2020.Solutions;

public record Solution12(List<(char, int)> read) : ISolution<Solution12>
{
    public static Solution12 Init(string[] lines) => new(lines.Select(x => (x[0], int.Parse(x.AsSpan()[1..]))).ToList());

    public async ValueTask<long> GetPart1()
    {
        var vector = read.Aggregate(new Vector(Direction.E, 0, 0), (vector, line) => vector.Move(line.Item1, line.Item2));
        return int.Abs(vector.X) + int.Abs(vector.Y);
    }

    public async ValueTask<long> GetPart2()
    {
        var waypoint = read.Aggregate(new WaypointNav(0, 0, 10, 1), (waypoint, line) => waypoint.Move(line.Item1, line.Item2));
        return int.Abs(waypoint.ShipX) + int.Abs(waypoint.ShipY);
    }

    private enum Direction
    {
        E = 0,
        N = 90,
        W = 180,
        S = 270,
    }

    private record Vector(Direction Direction, int X, int Y)
    {
        private Vector Turn(int adj) => this with { Direction = (Direction)(((int)this.Direction + adj) % 360) };

        public Vector Move(char dir, int adj) => dir switch
        {
            'N' => this with { Y = Y + adj },
            'S' => this with { Y = Y - adj },

            'E' => this with { X = X + adj },
            'W' => this with { X = X - adj },

            'L' => Turn(adj),
            'R' => Turn(360 - adj),

            'F' => Move(Direction.ToString()[0], adj),
            _ => throw new NotSupportedException(),
        };
    }

    private record WaypointNav(int ShipX, int ShipY, int WaypointX, int WaypointY)
    {
        private WaypointNav Turn(int adj)
            => adj == 0 ? this : (this with { WaypointX = -WaypointY, WaypointY = WaypointX }).Turn(adj - 90);

        public WaypointNav Move(char dir, int adj) => dir switch
        {
            'N' => this with { WaypointY = WaypointY + adj },
            'S' => this with { WaypointY = WaypointY - adj },

            'E' => this with { WaypointX = WaypointX + adj },
            'W' => this with { WaypointX = WaypointX - adj },

            'L' => Turn(adj),
            'R' => Turn(360 - adj),

            'F' => this with { ShipX = ShipX + adj * WaypointX, ShipY = ShipY + adj * WaypointY },
            _ => throw new NotSupportedException(),
        };
    }
}
