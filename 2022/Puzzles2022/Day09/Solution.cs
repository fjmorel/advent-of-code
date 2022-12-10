namespace Puzzles2022.Day09;

public record Solution(List<(char direction, int magnitude)> _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.Select(x => (x[0], int.Parse(x.AsSpan()[2..]))).ToList());

    public async ValueTask<long> GetPart1() => MoveKnots(2);

    public async ValueTask<long> GetPart2() => MoveKnots(10);

    public long MoveKnots(int numOfKnots)
    {
        var knots = new Point[numOfKnots];
        var visited = new HashSet<Point>();
        foreach (var (direction, magnitude) in _lines)
        {
            for (var i = 1; i <= magnitude; i++)
            {
                knots[0] = Move(knots[0], direction);
                for (var k = 1; k < knots.Length; k++)
                {
                    knots[k] = Catchup(knots[k - 1], knots[k]);
                    if (k == numOfKnots - 1)
                        visited.Add(knots[k]);
                }
            }
        }

        return visited.Count;
    }

    public static Point Catchup(Point head, Point tail)
    {
        if (head == tail || head.GetAllAdjacent().Contains(tail))
            return tail;

        // Move orthogonally to keep up
        if (tail == head with { x = head.x - 2 })
            return tail with { x = head.x - 1 };

        if (tail == head with { x = head.x + 2 })
            return tail with { x = head.x + 1 };

        if (tail == head with { y = head.y - 2 })
            return tail with { y = head.y - 1 };

        if (tail == head with { y = head.y + 2 })
            return tail with { y = head.y + 1 };

        // Move diagonally to keep up
        var xMove = head.x - tail.x;
        xMove /= Math.Max(1, Math.Abs(xMove));
        var yMove = head.y - tail.y;
        yMove /= Math.Max(1, Math.Abs(yMove));
        return new(tail.x + xMove, tail.y + yMove);
    }

    public static Point Move(Point point, char direction) => direction switch
    {
        'R' => point with { x = point.x + 1 },
        'L' => point with { x = point.x - 1 },
        'U' => point with { y = point.y + 1 },
        'D' => point with { y = point.y - 1 },
        _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null),
    };
}
