namespace Puzzles2020.Solutions;

public record Solution17(string[] list) : ISolution<Solution17>
{
    public static Solution17 Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1()
    {
        var alive = new HashSet<Cube>();
        for (var i = 0; i < list.Length; i++)
        for (var j = 0; j < list[i].Length; j++)
            if (list[i][j] == '#')
                alive.Add(new(i - 1, j - 1, 0));

        for (var cycle = 1; cycle <= 6; cycle++)
        {
            alive = alive.SelectMany(x => x.GetAdjacent(true)).Distinct().AsParallel().Where(point =>
            {
                var adjActive = point.GetAdjacent().Count(x => alive.Contains(x));
                return adjActive == 3 || (adjActive == 2 && alive.Contains(point));
            }).ToHashSet();
        }

        return alive.Count;
    }

    public async ValueTask<long> GetPart2()
    {
        var alive = new HashSet<Hypercube>();
        for (var i = 0; i < list.Length; i++)
        for (var j = 0; j < list[i].Length; j++)
            if (list[i][j] == '#')
                alive.Add(new(0, i - 1, j - 1, 0));

        for (var cycle = 1; cycle <= 6; cycle++)
        {
            alive = alive.SelectMany(x => x.GetAdjacent(true)).Distinct().AsParallel().Where(point =>
            {
                var adjActive = point.GetAdjacent().Count(x => alive.Contains(x));
                return adjActive == 3 || (adjActive == 2 && alive.Contains(point));
            }).ToHashSet();
        }

        return alive.Count;
    }

    private record Cube(int x, int y, int z)
    {
        public IEnumerable<Cube> GetAdjacent(bool includeItself = false)
        {
            foreach (var i in Enumerable.Range(-1, 3))
            foreach (var j in Enumerable.Range(-1, 3))
            foreach (var k in Enumerable.Range(-1, 3))
                if (!(i == 0 && j == 0 && k == 0 && !includeItself))
                    yield return new Cube(x + i, y + j, z + k);
        }
    }

    private record Hypercube(int w, int x, int y, int z)
    {
        public IEnumerable<Hypercube> GetAdjacent(bool includeItself = false)
        {
            foreach (var h in Enumerable.Range(-1, 3))
            foreach (var i in Enumerable.Range(-1, 3))
            foreach (var j in Enumerable.Range(-1, 3))
            foreach (var k in Enumerable.Range(-1, 3))
                if (!(h == 0 && i == 0 && j == 0 && k == 0 && !includeItself))
                    yield return new Hypercube(w + h, x + i, y + j, z + k);
        }
    }
}
