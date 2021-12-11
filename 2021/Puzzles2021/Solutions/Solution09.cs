namespace Puzzles2021.Solutions;

public class Solution09 : ISolution
{
    private readonly Dictionary<Point, int> _points;

    public Solution09(string[] lines)
    {
        _points = new Dictionary<Point, int>();
        for (var y = 0; y < lines.Length; y++)
        {
            var nums = lines[y].AsSpan();
            for (var x = 0; x < nums.Length; x++)
            {
                _points[new(x, y)] = int.Parse(nums[x..(x + 1)]);
            }
        }
    }

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
