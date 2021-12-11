namespace Combined.Solutions;

public class Solution11 : ISolution
{
    private readonly Dictionary<Point, long> _original = new();

    public Solution11(string[] lines)
    {
        for (var y = 0; y < lines.Length; y++)
        {
            var nums = lines[y].ParseDigits();
            for (var x = 0; x < nums.Length; x++)
            {
                _original[new(x, y)] = nums[x];
            }
        }
    }

    public async Task<long> GetPart1()
    {
        var grid = new Dictionary<Point, long>(_original);
        return Enumerable.Range(1, 100).Aggregate(0L, (flashes, _) => flashes + Iterate(grid));
    }

    public async Task<long> GetPart2()
    {
        var grid = new Dictionary<Point, long>(_original);
        return Enumerable.Range(1, int.MaxValue).First(i => Iterate(grid) == grid.Count);
    }

    long Iterate(Dictionary<Point, long> grid)
    {
        foreach (var pt in grid.Keys)
            grid[pt]++;

        var flashes = Flash(grid);

        foreach (var pt in grid.Keys)
            grid[pt] = grid[pt] > 9 ? 0 : grid[pt];

        return flashes;
    }

    long Flash(Dictionary<Point, long> grid)
    {
        var toFlash = new Queue<Point>();
        var flashed = new HashSet<Point>();

        foreach (var pt in grid.Where(x => x.Value > 9).Select(x => x.Key))
            toFlash.Enqueue(pt);

        while (toFlash.Any())
        {
            var pt = toFlash.Dequeue();
            if (flashed.Contains(pt))
                continue;

            foreach (var adj in pt.GetAllAdjacent().Where(grid.ContainsKey))
            {
                grid[adj]++;
                if (grid[adj] > 9)
                    toFlash.Enqueue(adj);
            }

            flashed.Add(pt);
        }

        return flashed.Count;
    }
}
