namespace Puzzles2021.Solutions;

public record Solution09(Dictionary<Point, int> _points) : ISolution<Solution09>
{
    public static Solution09 Init(string[] lines) => new(lines.ToDigitGrid());

    public async ValueTask<long> GetPart1() => GetLowestPoints().Select(pt => _points[pt] + 1).Sum();

    public async ValueTask<long> GetPart2()
    {
        var sizes = new List<long>();
        foreach (var low in GetLowestPoints())
        {
            var basin = new HashSet<Point>();
            var toCheck = new Queue<Point>();
            toCheck.Enqueue(low);
            while (toCheck.Any())
            {
                var pt = toCheck.Dequeue();
                basin.Add(pt);
                foreach (var adj in pt.GetOrthogonal().Where(x => _points.GetValueOrDefault(x, 9) != 9 && !basin.Contains(x)))
                    toCheck.Enqueue(adj);
            }

            sizes.Add(basin.Count);
        }

        return sizes.OrderByDescending(x => x).Take(3).Aggregate(1L, (a, b) => a * b);
    }

    private IEnumerable<Point> GetLowestPoints()
        => _points.Where(kv => kv.Key.GetOrthogonal().All(x => _points.GetValueOrDefault(x, 9) > kv.Value)).Select(x => x.Key);
}
