namespace Puzzles2021.Solutions;

public class Solution12 : ISolution
{
    private readonly Dictionary<string, List<string>> _connections;

    public Solution12(string[] lines)
    {
        _connections = new();
        foreach (var line in lines)
        {
            var split = line.Split('-');
            if (!_connections.ContainsKey(split[0]))
                _connections[split[0]] = new();
            // don't let  anyone go back to the start
            if (split[1] != "start")
                _connections[split[0]].Add(split[1]);

            if (!_connections.ContainsKey(split[1]))
                _connections[split[1]] = new();
            // don't let  anyone go back to the start
            if (split[0] != "start")
                _connections[split[1]].Add(split[0]);
        }

        _connections["end"] = new();
    }

    public async ValueTask<long> GetPart1()
    {
        var finished = new List<Route>();
        var queue = new Queue<Route>();
        queue.Enqueue(new Route());
        while (queue.Any())
        {
            var state = queue.Dequeue();
            var newRoutes = _connections[state.Current]
                .Where(cave => !state.Visited.Contains(cave) || cave.ToUpper() == cave)
                .Select(cave => state with { Current = cave, Visited = new(state.Visited) });
            foreach (var route in newRoutes)
            {
                route.Visited.Add(route.Current);
                if (route.Current == "end")
                    finished.Add(route);
                else
                    queue.Enqueue(route);
            }
        }

        return finished.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        var finished = new List<Route>();
        var queue = new Queue<Route>();
        queue.Enqueue(new Route());
        while (queue.Any())
        {
            var state = queue.Dequeue();
            foreach (var path in FindPaths2(state))
            {
                path.Visited.Add(path.Current);
                if (path.Current == "end")
                    finished.Add(path);
                else
                    queue.Enqueue(path);
            }
        }

        return finished.Count;
    }

    public IEnumerable<Route> FindPaths2(Route state)
    {
        foreach (var next in _connections[state.Current])
        {
            var route = state with { Current = next, Visited = new(state.Visited) };
            var canVisit = !state.Visited.Contains(next) || next.ToUpper() == next;
            if (!canVisit && !state.Revisited)
            {
                canVisit = true;
                route.Revisited = true;
            }

            if (canVisit)
                yield return route;
        }
    }

    public record Route
    {
        public string Current { get; init; } = "start";
        public HashSet<string> Visited { get; init; } = new() { "start" };
        public bool Revisited { get; set; }
    }
}
