namespace Puzzles2020.Solutions;

public class Solution12 : ISolution
{
    private readonly List<(char, int)> read;

    public Solution12(string[] lines)
    {
        read = lines.Select(x => (x[0], int.Parse(x.Substring(1)))).ToList();
    }

    public async ValueTask<long> GetPart1()
    {
        var vector = read.Aggregate(new Vector(Direction.E, 0, 0), (vector, line) => vector.Move(line.Item1, line.Item2));
        return Math.Abs(vector.X) + Math.Abs(vector.Y);
    }

    public async ValueTask<long> GetPart2()
    {
        var waypoint = read.Aggregate(new WaypointNav(0, 0, 10, 1), (waypoint, line) => waypoint.Move(line.Item1, line.Item2));
        return Math.Abs(waypoint.ShipX) + Math.Abs(waypoint.ShipY);

    }

    enum Direction
    {
        E = 0,
        N = 90,
        W = 180,
        S = 270,
    }

    record Vector(Direction Direction, int X, int Y)
    {
        public Vector Turn(int adj) => this with { Direction = (Direction)(((int)this.Direction + adj) % 360) };

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

    record WaypointNav(int ShipX, int ShipY, int WaypointX, int WaypointY)
    {
        public WaypointNav Turn(int adj)
            => adj == 0 ? this : (this with { WaypointX = -WaypointY, WaypointY = WaypointX }).Turn(adj - 90);

        public WaypointNav Move(char dir, int adj) => dir switch
        {
            'N' => this with { WaypointY = WaypointY + adj },
            'S' => this with { WaypointY = WaypointY - adj },

            'E' => this with { WaypointX = WaypointX + adj },
            'W' => this with { WaypointX = WaypointX - adj },

            'L' => this.Turn(adj),
            'R' => this.Turn(360 - adj),

            'F' => this with { ShipX = ShipX + adj * WaypointX, ShipY = ShipY + adj * WaypointY },
            _ => throw new NotSupportedException(),
        };
    }
}
