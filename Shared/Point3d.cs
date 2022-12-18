namespace Shared;

/// <summary>
/// Basic coordinates
/// </summary>
public readonly record struct Point3d(int x, int y, int z)
{
    public Point3d(ReadOnlySpan<char> x, ReadOnlySpan<char> y, ReadOnlySpan<char> z) : this(int.Parse(x), int.Parse(y), int.Parse(z)) { }
    public Point3d(Capture x, Capture y, Capture z) : this(x.ValueSpan, y.ValueSpan, z.ValueSpan) { }
    public Point3d(int[] values) : this(values[0], values[1], values[2]) { }

    /// <summary>
    /// Get all adjacent points (diagonal and orthogonal)
    /// </summary>
    public IEnumerable<Point3d> GetAllAdjacent() => GetOrthogonal().Concat(GetDiagonal());

    /// <summary>
    /// Get orthogonally-adjacent points (move on one axis only)
    /// </summary>
    public IEnumerable<Point3d> GetOrthogonal()
    {
        yield return this with { x = x + 1 };
        yield return this with { x = x - 1 };
        yield return this with { y = y + 1 };
        yield return this with { y = y - 1 };
        yield return this with { z = z + 1 };
        yield return this with { z = z - 1 };
    }

    /// <summary>
    /// Get diagonally-adjacent points (move on 2 or 3 axes)
    /// </summary>
    public IEnumerable<Point3d> GetDiagonal()
    {
        // x/y
        yield return this with { x = x - 1, y = y - 1 };
        yield return this with { x = x - 1, y = y + 1 };
        yield return this with { x = x + 1, y = y - 1 };
        yield return this with { x = x + 1, y = y + 1 };

        // y/z
        yield return this with { y = y - 1, z = z - 1 };
        yield return this with { y = y - 1, z = z + 1 };
        yield return this with { y = y + 1, z = z - 1 };
        yield return this with { y = y + 1, z = z + 1 };

        // x/z
        yield return this with { x = x - 1, z = z - 1 };
        yield return this with { x = x - 1, z = z + 1 };
        yield return this with { x = x + 1, z = z - 1 };
        yield return this with { x = x + 1, z = z + 1 };

        // x/y/z
        yield return this - new Point3d(-1, -1, -1);
        yield return this - new Point3d(-1, -1, 1);
        yield return this - new Point3d(-1, 1, -1);
        yield return this - new Point3d(-1, 1, 1);
        yield return this - new Point3d(1, -1, -1);
        yield return this - new Point3d(1, -1, 1);
        yield return this - new Point3d(1, 1, -1);
        yield return this - new Point3d(1, 1, 1);
    }

    public static Point3d operator +(Point3d a) => a;
    public static Point3d operator -(Point3d a) => new(-a.x, -a.y, -a.z);
    public static Point3d operator +(Point3d a, Point3d b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Point3d operator -(Point3d a, Point3d b) => a + (-b);

    public static Point3d Apply(Point3d a, Point3d b, Func<int, int, int> func) => new Point3d(func(a.x, b.x), func(a.y, b.y), func(a.z, b.z));
    public static bool Match(Point3d a, Point3d b, Func<int, int, bool> func) => func(a.x, b.x) && func(a.y, b.y) && func(a.z, b.z);

    public long GetMagnitude() => long.Abs(x) + long.Abs(y) + long.Abs(z);

    public bool IsWithinInclusive(Point3d min, Point3d max) => x >= min.x && x <= max.x && y >= min.y && y <= max.y && z >= min.z && z <= max.z;
}
