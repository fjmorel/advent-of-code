namespace Puzzles2021.Day05;

public partial record Solution(List<Solution.Vent> _vents) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.Select(x => new Vent(x)).ToList());

    public async ValueTask<long> GetPart1() => _vents.Where(v => v.IsStraight).SelectMany(v => v.GetPoints()).Aggregate(new Dictionary<Point, int>(), MapPoint).Count(x => x.Value > 1);

    public async ValueTask<long> GetPart2() => _vents.SelectMany(v => v.GetPoints()).Aggregate(new Dictionary<Point, int>(), MapPoint).Count(x => x.Value > 1);

    private Dictionary<Point, int> MapPoint(Dictionary<Point, int> map, Point pt)
    {
        map[pt] = map.GetValueOrDefault(pt, 0) + 1;
        return map;
    }

    public readonly record struct Vent(Point start, Point end)
    {
        public Vent(string line) : this(default, default)
        {
            var groups = GetLineRegex().Match(line).Groups;
            start = new Point(groups[1], groups[2]);
            end = new Point(groups[3], groups[4]);
        }

        public bool IsStraight => start.x == end.x || start.y == end.y;

        public IEnumerable<Point> GetPoints()
        {
            var steps = int.Max(int.Abs(start.x - end.x), int.Abs(start.y - end.y)) + 1;
            var xStep = start.x == end.x ? 0 : end.x > start.x ? 1 : -1;
            var yStep = start.y == end.y ? 0 : end.y > start.y ? 1 : -1;
            for (var i = 0; i < steps; i++)
            {
                yield return new(start.x + i * xStep, start.y + i * yStep);
            }
        }
    }

    [GeneratedRegex("([0-9]+),([0-9]+) -> ([0-9]+),([0-9]+)")]
    private static partial Regex GetLineRegex();
}
