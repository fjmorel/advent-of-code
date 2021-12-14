namespace Puzzles2020.Solutions;

public class Solution17 : ISolution
{
    private readonly string[] list;

    public Solution17(string[] lines)
    {
        list = lines;
    }

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

    record Cube(int x, int y, int z)
    {
        public IEnumerable<Cube> GetAdjacent(bool includeItself = false)
        {
            foreach (var i in Enumerable.Range(-1, 3))
            foreach (var j in Enumerable.Range(-1, 3))
            foreach (var k in Enumerable.Range(-1, 3))
                if (!(i == 0 && j == 0 && k == 0 && !includeItself))
                    yield return new Cube(this.x + i, this.y + j, this.z + k);
        }
    }

    record Hypercube(int w, int x, int y, int z)
    {
        public IEnumerable<Hypercube> GetAdjacent(bool includeItself = false)
        {
            foreach (var h in Enumerable.Range(-1, 3))
            foreach (var i in Enumerable.Range(-1, 3))
            foreach (var j in Enumerable.Range(-1, 3))
            foreach (var k in Enumerable.Range(-1, 3))
                if (!(h == 0 && i == 0 && j == 0 && k == 0 && !includeItself))
                    yield return new Hypercube(this.w + h, this.x + i, this.y + j, this.z + k);
        }
    }
}
