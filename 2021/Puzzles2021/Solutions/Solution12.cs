namespace Puzzles2021.Solutions;

public class Solution12 : ISolution
{
    private readonly Dictionary<string, List<string>> _connections;

    public Solution12(string[] lines)
    {
        _connections = new();
        foreach (var line in lines)
        {
            var pair = line.Split('-');
            _connections.TryAdd(pair[0], new());
            _connections.TryAdd(pair[1], new());
            // don't let  anyone go back to the start
            if (pair[1] != "start")
                _connections[pair[0]].Add(pair[1]);
            if (pair[0] != "start")
                _connections[pair[1]].Add(pair[0]);
        }
    }

    public async ValueTask<long> GetPart1()
    {
        var finished = new List<Route>();
        var queue = new Queue<Route>();
        queue.Enqueue(new Route());
        while (queue.Any())
        {
            var state = queue.Dequeue();
            state.Visited.Add(state.Current);
            var newRoutes = _connections[state.Current]
                .Where(cave => !state.Visited.Contains(cave) || cave.ToUpper() == cave)
                .Select(cave => state with { Current = cave, Visited = new(state.Visited) });
            foreach (var route in newRoutes)
            {
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
            state.Visited.Add(state.Current);
            foreach (var route in FindRoutes2(state))
            {
                if (route.Current == "end")
                    finished.Add(route);
                else
                    queue.Enqueue(route);
            }
        }

        return finished.Count;
    }

    public IEnumerable<Route> FindRoutes2(Route state)
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
        public HashSet<string> Visited { get; init; } = new();
        public bool Revisited { get; set; }
    }
}
