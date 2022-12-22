using OneOf;

namespace Puzzles2022.Day22;

public record Solution(Dictionary<Point, bool> _jungle, char[] _instructions) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var jungle = new Dictionary<Point, bool>();
        for (var y = 0; y < lines.Length - 1; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] is '#' or '.')
                jungle[new(x + 1, y + 1)] = lines[y][x] == '#';
        }

        return new Solution(jungle, lines[^1].ToCharArray());
    }

    public async ValueTask<long> GetPart1()
    {
        int maxX = _jungle.Keys.Max(x => x.x), maxY = _jungle.Keys.Max(x => x.y);
        var current = _jungle.Keys.Where(pt => pt.y == 1).MinBy(pt => pt.x);
        var points = _jungle.Keys.ToHashSet();
        var direction = new Point(1, 0);
        // var visited = new List<Point>() { current };

        foreach (var move in GetNextInstruction())
        {
            if (move.IsT1)
                direction = Turn(direction, move.AsT1);
            else
            {
                var magnitude = move.AsT0;
                while (magnitude > 0)
                {
                    var next = current + direction;
                    // wrap around as needed
                    if (!points.Contains(next))
                        next = Wrap2d(current, direction, points, maxX, maxY);

                    // if wall, do nothing
                    if (_jungle[next])
                        break;

                    // otherwise keep moving
                    current = next;
                    // visited.Add(next);
                    magnitude--;
                }
            }
        }

        return 1000 * current.y + 4 * current.x + GetFacingScore(direction);
    }

    private static Point Wrap2d(Point current, Point direction, HashSet<Point> points, int maxX, int maxY)
    {
        var next = direction switch
        {
            (1, 0) => current with { x = 1 },
            (-1, 0) => current with { x = maxX },
            (0, 1) => current with { y = 1 },
            (0, -1) => current with { y = maxY },
            _ => throw new UnreachableException(),
        };
        while (!points.Contains(next))
            next += direction;
        return next;
    }

    public async ValueTask<long> GetPart2()
    {
        int maxX = _jungle.Keys.Max(x => x.x), maxY = _jungle.Keys.Max(x => x.y);
        var current = _jungle.Keys.Where(pt => pt.y == 1).MinBy(pt => pt.x);
        var points = _jungle.Keys.ToHashSet();
        var direction = new Point(1, 0);
        // var visited = new List<Point>() { current };

        foreach (var move in GetNextInstruction())
        {
            if (move.IsT1)
                direction = Turn(direction, move.AsT1);
            else
            {
                var magnitude = move.AsT0;
                while (magnitude > 0)
                {
                    var newDirection = direction;
                    var next = current + direction;
                    // wrap around as needed
                    if (!points.Contains(next))
                        (next, newDirection) = Wrap3d(current, direction, points, maxX, maxY);

                    // if wall, do nothing
                    if (_jungle[next])
                        break;

                    // otherwise keep moving
                    current = next;
                    direction = newDirection;
                    // visited.Add(next);
                    magnitude--;
                }
            }
        }

        return 1000 * current.y + 4 * current.x + GetFacingScore(direction);
    }

    private static (Point next, Point newDirection) Wrap3d(Point current, Point direction, HashSet<Point> points, int maxX, int maxY)
    {
        // todo: refactor to handle cube turning on wrap
        var next = direction switch
        {
            (1, 0) => current with { x = 1 },
            (-1, 0) => current with { x = maxX },
            (0, 1) => current with { y = 1 },
            (0, -1) => current with { y = maxY },
            _ => throw new UnreachableException(),
        };
        while (!points.Contains(next))
            next += direction;
        return (next, direction);
    }

    public static Point Turn(Point currentDirection, char rotate)
    {
        if (rotate == 'L')
        {
            var turns = new LinkedList<Point>();
            turns.AddLast(new Point(1, 0));
            turns.AddLast(new Point(0, -1));
            turns.AddLast(new Point(-1, 0));
            turns.AddLast(new Point(0, 1));

            var node = turns.Find(currentDirection);
            return (node!.Next ?? turns.First!).Value;
        }
        else// right
        {
            var turns = new LinkedList<Point>();
            turns.AddLast(new Point(1, 0));
            turns.AddLast(new Point(0, 1));
            turns.AddLast(new Point(-1, 0));
            turns.AddLast(new Point(0, -1));

            var node = turns.Find(currentDirection);
            return (node!.Next ?? turns.First!).Value;
        }
    }

    public IEnumerable<OneOf<int, char>> GetNextInstruction()
    {
        var index = 0;
        while (index < _instructions.Length)
        {
            if (_instructions[index] is 'R' or 'L')
            {
                yield return _instructions[index];
                index++;
            }
            else
            {
                var endIndex = index + 1;
                while (endIndex < _instructions.Length && char.IsAsciiDigit(_instructions[endIndex]))
                    endIndex++;
                yield return int.Parse(_instructions.AsSpan()[index..endIndex]);
                index = endIndex;
            }
        }
    }

    public static int GetFacingScore(Point direction) => direction switch
    {
        (1, 0) => 0,
        (0, 1) => 1,
        (-1, 0) => 2,
        (0, -1) => 3,
        _ => throw new UnreachableException(),
    };
}
