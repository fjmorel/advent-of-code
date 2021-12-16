/// <summary>
/// Basic coordinates
/// </summary>
public readonly record struct Point(int x, int y)
{
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
        yield return this with { x = x + 1, y = y + 1 };
        yield return this with { x = x + 1, y = y - 1 };
        yield return this with { x = x - 1, y = y - 1 };
        yield return this with { x = x - 1, y = y + 1 };
    }

    public static Point operator +(Point a) => a;
    public static Point operator -(Point a) => new(-a.x, -a.y);
    public static Point operator +(Point a, Point b) => new(a.x + b.x, a.y + b.y);
    public static Point operator -(Point a, Point b) => a + (-b);
    public bool IsWithin(Point size) => x >= 0 && x < size.x && y >= 0 && y < size.y;
}
