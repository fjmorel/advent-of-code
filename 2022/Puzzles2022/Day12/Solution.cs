namespace Puzzles2022.Day12;

public record Solution(Dictionary<Point, int> _grid, Point _start, Point _end) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var grid = lines.ToGrid(x => x switch
        {
            'E' => 25,
            'S' => 0,
            _ => x - 'a',
        });
        Point start = new(), end = new();
        for (var y = 0; y < lines.Length; y++)
        for (var x = 0; x < lines[y].Length; x++)
        {
            if (lines[y][x] == 'S')
                start = new(x, y);
            if (lines[y][x] == 'E')
                end = new(x, y);
        }

        return new Solution(grid, start, end);
    }

    public async ValueTask<long> GetPart1()
    {
        var graph = _grid.ToDictionary(x => x.Key, x => new Node(x.Value));
        return Dijkstra(graph, _start, _end);
    }

    // todo: consider going in reverse to try to get from Z to any a for 1 iteration through grid
    public async ValueTask<long> GetPart2()
    {
        var aaaaaaa = _grid.Where(x => x.Value == 0).Select(x => x.Key).ToList();
        var shortest = long.MaxValue;
        foreach (var a in aaaaaaa)
        {
            var graph = _grid.ToDictionary(x => x.Key, x => new Node(x.Value));
            shortest = Math.Min(shortest, Dijkstra(graph, a, _end));
        }

        return shortest;
    }

    public record Node(int Height)
    {
        public long Total { get; set; } = long.MaxValue;
        public bool Visited { get; set; }
    }

    // https://en.wikipedia.org/wiki/Dijkstra's_algorithm
    private static long Dijkstra(IReadOnlyDictionary<Point, Node> graph, Point origin, Point target)
    {
        var queue = new ConcurrentQueue<Point>();

        // Set up origin
        graph[origin].Total = 0;
        queue.Enqueue(origin);

        while (queue.TryDequeue(out var point))
        {
            var node = graph[point];
            if (node.Visited)
                continue;
            node.Visited = true;

            foreach (var adjacent in point.GetOrthogonal())
            {
                if (!graph.TryGetValue(adjacent, out var otherNode) || otherNode.Height > node.Height + 1)
                    continue;

                otherNode.Total = long.Min(node.Total + 1, otherNode.Total);
                if (!otherNode.Visited)
                    queue.Enqueue(adjacent);
            }
        }

        return graph[target].Total;
    }
}
