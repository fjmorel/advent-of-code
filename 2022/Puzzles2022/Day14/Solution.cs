namespace Puzzles2022.Day14;

public record Solution(List<List<Point>> _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.ToList(ParseLine));

    private static List<Point> ParseLine(string line) =>
        line.Split(" -> ").Select(vertex => vertex.Split(',')).ToList(nums => new Point(nums[0], nums[1]));

    public async ValueTask<long> GetPart1()
    {
        var occupied = CreateGrid();
        var maxHeight = occupied.Max(pt => pt.y) + 1;
        return Iterate(occupied, match => match == default, match => match.y == maxHeight);
    }

    public async ValueTask<long> GetPart2()
    {
        var occupied = CreateGrid();
        var maxHeight = occupied.Max(pt => pt.y) + 2;
        return Iterate(occupied, match => match == default || match.y == maxHeight, _ => false);
    }

    public static long Iterate(HashSet<Point> occupied, Predicate<Point> breakCondition, Predicate<Point> returnCondition)
    {
        Point[] _moves = [new(0, 1), new(-1, 1), new(1, 1)];
        Point _sandStart = new(500, 0);
        var sandGrains = 0;

        while (!occupied.Contains(_sandStart))
        {
            var sandPosition = _sandStart;
            while (true)
            {
                var match = _moves.Select(x => sandPosition + x).FirstOrDefault(x => !occupied.Contains(x));
                if (breakCondition(match))
                    break;
                if (returnCondition(match))
                    return sandGrains;
                sandPosition = match;
            }

            occupied.Add(sandPosition);
            sandGrains++;
        }

        return sandGrains;
    }

    private HashSet<Point> CreateGrid()
    {
        var occupied = new HashSet<Point>();
        foreach (var line in _lines)
        {
            for (var i = 0; i < line.Count - 1; i++)
            {
                foreach (var pt in line[i].FlatLineTo(line[i + 1]))
                    occupied.Add(pt);
            }
        }

        return occupied;
    }
}
