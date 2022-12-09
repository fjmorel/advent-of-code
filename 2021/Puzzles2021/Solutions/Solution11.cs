namespace Puzzles2021.Solutions;

public record Solution11(Dictionary<Point, int> _original) : ISolution<Solution11>
{
    public static Solution11 Init(string[] lines) => new(lines.ToDigitGrid());

    public async ValueTask<long> GetPart1()
    {
        var grid = new Dictionary<Point, int>(_original);
        return Enumerable.Range(1, 100).Aggregate(0L, (flashes, _) => flashes + Iterate(grid));
    }

    public async ValueTask<long> GetPart2()
    {
        var grid = new Dictionary<Point, int>(_original);
        return Enumerable.Range(1, int.MaxValue).First(i => Iterate(grid) == grid.Count);
    }

    private static long Iterate(Dictionary<Point, int> grid)
    {
        foreach (var pt in grid.Keys)
            grid[pt]++;

        var flashes = Flash(grid);

        foreach (var pt in grid.Keys)
            grid[pt] = grid[pt] > 9 ? 0 : grid[pt];

        return flashes;
    }

    private static long Flash(Dictionary<Point, int> grid)
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
