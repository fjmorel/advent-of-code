namespace Shared;

/// <summary>
/// Basic coordinates
/// </summary>
public readonly record struct Point(int x, int y)
{
    public Point(ReadOnlySpan<char> x, ReadOnlySpan<char> y) : this(int.Parse(x), int.Parse(y)) { }
    public Point(Capture x, Capture y) : this(x.ValueSpan, y.ValueSpan) { }

    /// <summary>
    /// Get all adjacent points (diagonal and orthogonal)
    /// </summary>
    public IEnumerable<Point> GetAllAdjacent() => GetOrthogonal().Concat(GetDiagonal());

    /// <summary>
    /// Get points that are orthogonally-adjacent
    /// </summary>
    public IEnumerable<Point> GetOrthogonal()
    {
        yield return this with { x = x + 1 };
        yield return this with { x = x - 1 };
        yield return this with { y = y + 1 };
        yield return this with { y = y - 1 };
    }

    /// <summary>
    /// Get points that are diagonally-adjacent
    /// </summary>
    public IEnumerable<Point> GetDiagonal()
    {
        yield return new Point(x + 1, y + 1);
        yield return new Point(x + 1, y - 1);
        yield return new Point(x - 1, y - 1);
        yield return new Point(x - 1, y + 1);
    }

    public static Point operator +(Point a) => a;
    public static Point operator -(Point a) => new(-a.x, -a.y);
    public static Point operator +(Point a, Point b) => new(a.x + b.x, a.y + b.y);

    public static Point operator -(Point a, Point b) => a + (-b);

    // todo: switch to overload by passing in Origin
    public bool IsWithin(Point size) => x >= 0 && x < size.x && y >= 0 && y < size.y;
    public bool IsWithinInclusive(Point min, Point max) => x >= min.x && x <= max.x && y >= min.y && y <= max.y;

    /// <summary>
    /// Assuming the 2 points are in a horizontal or vertical line,
    /// return a points between them (inclusive)
    /// </summary>
    public IEnumerable<Point> FlatLineTo(Point destination)
    {
        var self = this;
        if (x == destination.x)
        {
            if (y >= destination.y)
                return Enumerable.Range(destination.y, y - destination.y + 1).Select(newY => self with { y = newY });

            return Enumerable.Range(y, destination.y - y + 1).Select(newY => self with { y = newY });
        }

        if (y == destination.y)
        {
            if (x >= destination.x)
                return Enumerable.Range(destination.x, x - destination.x + 1).Select(newX => self with { x = newX });

            return Enumerable.Range(x, destination.x - x + 1).Select(newX => self with { x = newX });
        }

        throw new UnreachableException("You did something wrong. This is not a horizontal or vertical line.");
    }
}
