namespace Puzzles2021.Solutions;

public class Solution05 : ISolution
{
	private readonly List<Vent> _vents;

	public Solution05(string[] lines)
	{
		_vents = lines.Select(x => new Vent(x)).ToList();
	}

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
			var groups = Regex.Match(line, "([0-9]+),([0-9]+) -> ([0-9]+),([0-9]+)").Groups;
			this.start = new Point(int.Parse(groups[1].ValueSpan), int.Parse(groups[2].ValueSpan));
			this.end = new Point(int.Parse(groups[3].ValueSpan), int.Parse(groups[4].ValueSpan));
		}

		public bool IsStraight => start.x == end.x || start.y == end.y;

		public IEnumerable<Point> GetPoints()
		{
			var steps = Math.Max(Math.Abs(start.x - end.x), Math.Abs(start.y - end.y)) + 1;
			var xStep = start.x == end.x ? 0 : end.x > start.x ? 1 : -1;
			var yStep = start.y == end.y ? 0 : end.y > start.y ? 1 : -1;
			for (var i = 0; i < steps; i++)
			{
				yield return new(start.x + i * xStep, start.y + i * yStep);
			}
		}
	}
	public readonly record struct Point(int x, int y);
}

