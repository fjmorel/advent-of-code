namespace Puzzles2022.Day23;

public record Solution(ImmutableHashSet<Point> _start) : ISolution<Solution>
{
    public static readonly ImmutableList<Point> _directions = ImmutableList.Create<Point>(new(0, -1), new(0, 1), new(-1, 0), new(1, 0));

    public static readonly Dictionary<Point, Point[]> _neighbors = new()
    {
        [new(0, -1)] = new Point[] { new(-1, -1), new(0, -1), new(1, -1) },
        [new(0, 1)] = new Point[] { new(-1, 1), new(0, 1), new(1, 1) },
        [new(-1, 0)] = new Point[] { new(-1, -1), new(-1, 0), new(-1, 1) },
        [new(1, 0)] = new Point[] { new(1, -1), new(1, 0), new(1, 1) },
    };

    public static Solution Init(string[] lines) => new(lines.ToPointSet(x => x == '#').ToImmutableHashSet());

    public async ValueTask<long> GetPart1()
    {
        var directions = _directions.ToList();
        var elves = _start.ToHashSet();
        for (var i = 0; i < 10; i++)
            Iterate(elves, directions);

        var minX = elves.Min(e => e.x);
        var minY = elves.Min(e => e.y);
        var maxX = elves.Max(e => e.x);
        var maxY = elves.Max(e => e.y);
        var unoccupied = 0L;
        for (var x = minX; x <= maxX; x++)
        for (var y = minY; y <= maxY; y++)
            if (!elves.Contains(new(x, y)))
                unoccupied++;

        return unoccupied;
    }

    public async ValueTask<long> GetPart2()
    {
        var directions = _directions.ToList();
        var elves = _start.ToHashSet();
        var rounds = 1;
        while (Iterate(elves, directions))
            rounds++;

        return rounds;
    }

    public static bool Iterate(HashSet<Point> elves, List<Point> directions)
    {
        var movesTo = new ConcurrentDictionary<Point, int>();
        var proposed = new ConcurrentBag<(Point from, Point to)>();
        Parallel.ForEach(elves, elf =>
        {
            if (elf.GetAllAdjacent().All(x => !elves.Contains(x)))
                return;

            foreach (var direction in directions)
            {
                if (_neighbors[direction].All(x => !elves.Contains(x + elf)))
                {
                    var newPlace = elf + direction;
                    movesTo.AddOrUpdate(newPlace, 1, (pt, current) => current + 1);
                    proposed.Add((elf, newPlace));
                    break;
                }
            }
        });

        var moved = false;
        foreach (var move in proposed)
        {
            if (movesTo[move.to] == 1)
            {
                moved = true;
                elves.Remove(move.from);
                elves.Add(move.to);
            }
        }

        var firstDirection = directions[0];
        directions.RemoveAt(0);
        directions.Add(firstDirection);

        return moved;
    }
}
