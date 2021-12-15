namespace Puzzles2021.Solutions;

public class Solution15 : ISolution
{
    private readonly int[][] _nums;
    private readonly int _width;
    private readonly int _height;

    public Solution15(string[] lines)
    {
        _nums = lines.Select(x => x.ParseDigits()).ToArray();

        _width = _nums[0].Length;
        _height = _nums.Length;
    }

    public async ValueTask<long> GetPart1()
    {
        var grid = new Dictionary<Point, Node>();
        AddNodes(grid, 0, 0);

        return Dijkstra(grid, new(_width - 1, _height - 1));
    }

    public async ValueTask<long> GetPart2()
    {
        var grid = new ConcurrentDictionary<Point, Node>();
        var offsets = Enumerable.Range(0, 5).SelectMany(x => Enumerable.Range(0, 5).Select(y => (x, y)));
        foreach (var (x, y) in offsets)
            AddNodes(grid, x, y);

        return Dijkstra(grid, new(_width * 5 - 1, _height * 5 - 1));
    }

    private void AddNodes(IDictionary<Point, Node> grid, int xOffSet, int yOffset)
    {
        for (var y = 0; y < _height; y++)
        {
            for (var x = 0; x < _width; x++)
            {
                var risk = _nums[y][x];
                var pt = new Point(x + _width * xOffSet, y + _height * yOffset);
                var adjusted = risk + xOffSet + yOffset;
                if (adjusted > 9)
                    adjusted -= 9;
                grid[pt] = new(pt, adjusted);
            }
        }
    }

    public record Node(Point Point, long Risk)
    {
        public long Total { get; set; } = long.MaxValue;
        public bool Visited { get; set; } = false;
    }

    // https://en.wikipedia.org/wiki/Dijkstra's_algorithm
    private static long Dijkstra(IReadOnlyDictionary<Point, Node> graph, Point target)
    {
        var queue = new PriorityQueue<Node, long>();

        // Set up origin
        var origin = new Point(0, 0);
        graph[origin].Total = 0;
        queue.Enqueue(graph[origin], graph[origin].Total);

        while (queue.Count > 0)
        {
            var current = queue.Dequeue();
            if (current.Visited)
                continue;

            current.Visited = true;

            if (current.Point == target)
                return graph[target].Total;

            foreach (var adj in current.Point.GetOrthogonal())
            {
                if (!graph.TryGetValue(adj, out var neighbor) || neighbor!.Visited)
                    continue;

                neighbor.Total = Math.Min(current.Total + neighbor.Risk, neighbor.Total);
                queue.Enqueue(neighbor, neighbor.Total);
            }
        }

        return graph[target].Total;
    }
}
