namespace Puzzles2022.Day18;

public record Solution(HashSet<Point3d> _lava) : ISolution<Solution>
{
    private readonly Point3d _min = new(_lava.Min(pt => pt.x), _lava.Min(pt => pt.y), _lava.Min(pt => pt.z));
    private readonly Point3d _max = new(_lava.Max(pt => pt.x), _lava.Max(pt => pt.y), _lava.Max(pt => pt.z));

    public static Solution Init(string[] lines) => new(lines.Select(x => new Point3d(x.ParseCsv<int>())).ToHashSet());

    public async ValueTask<long> GetPart1() => _lava.Select(x => x.GetOrthogonal().Except(_lava).Count()).Sum();

    public async ValueTask<long> GetPart2()
    {
        var inner = new HashSet<Point3d>();
        var water = new HashSet<Point3d>();
        for (var x = _min.x; x <= _max.x; x++)
        for (var y = _min.y; y <= _max.y; y++)
        for (var z = _min.z; z <= _max.z; z++)
        {
            var pt = new Point3d(x, y, z);
            if (_lava.Contains(pt)) { }
            else if (x == _min.x || x == _max.x || y == _min.y || y == _max.y || z == _min.z || z == _max.z)
                water.Add(pt);
            else if (pt.GetOrthogonal().Intersect(water).Any())
                water.Add(pt);
            else
                inner.Add(pt);
        }

        while (water.Any())
        {
            inner.RemoveWhere(x => water.Contains(x));
            water = water.SelectMany(x => x.GetOrthogonal()).Intersect(inner).ToHashSet();
        }

        return _lava.Select(x => x.GetOrthogonal().Except(_lava).Except(inner).Count()).Sum();
    }
}
