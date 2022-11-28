namespace Shared;

/// <summary>
/// Basic coordinates
/// </summary>
public readonly record struct Point3d(int x, int y, int z)
{
    public Point3d(ReadOnlySpan<char> x, ReadOnlySpan<char> y, ReadOnlySpan<char> z) : this(int.Parse(x), int.Parse(y), int.Parse(z)) { }

    // /// <summary>
    // /// Get all adjacent Point3ds (diagonal and orthogonal)
    // /// </summary>
    // public IEnumerable<Point3d> GetAllAdjacent() => GetOrthogonal().Concat(GetDiagonal());
    //
    // /// <summary>
    // /// Get Point3ds that are orthogonally-adjacent
    // /// </summary>
    // public IEnumerable<Point3d> GetOrthogonal()
    // {
    //     yield return this with { x = x + 1 };
    //     yield return this with { x = x - 1 };
    //     yield return this with { y = y + 1 };
    //     yield return this with { y = y - 1 };
    // }
    //
    // /// <summary>
    // /// Get Point3ds that are diagonally-adjacent
    // /// </summary>
    // public IEnumerable<Point3d> GetDiagonal()
    // {
    //     yield return this with { x = x + 1, y = y + 1 };
    //     yield return this with { x = x + 1, y = y - 1 };
    //     yield return this with { x = x - 1, y = y - 1 };
    //     yield return this with { x = x - 1, y = y + 1 };
    // }

    public static Point3d operator +(Point3d a) => a;
    public static Point3d operator -(Point3d a) => new(-a.x, -a.y, -a.z);
    public static Point3d operator +(Point3d a, Point3d b) => new(a.x + b.x, a.y + b.y, a.z + b.z);

    public static Point3d operator -(Point3d a, Point3d b) => a + (-b);

    public static Point3d Apply(Point3d a, Point3d b, Func<int, int, int> func) => new Point3d(func(a.x, b.x), func(a.y, b.y), func(a.z, b.z));
    public static bool Match(Point3d a, Point3d b, Func<int, int, bool> func) => func(a.x, b.x) && func(a.y, b.y) && func(a.z, b.z);

    public long GetMagnitude() => long.Abs(x) + long.Abs(y) + long.Abs(z);

    public bool IsWithinInclusive(Point3d min, Point3d max) => x >= min.x && x <= max.x && y >= min.y && y <= max.y && z >= min.z && z <= max.z;
}
