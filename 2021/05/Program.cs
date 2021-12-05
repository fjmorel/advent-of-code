var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
// lines = File.ReadAllLines("example.txt");

var vents = lines.Select(x => new Vent(x)).ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1() => vents.Where(v => v.IsStraight).Aggregate(new Dictionary<Point, int>(), MapVent).Count(x => x.Value > 1);

long Part2() => vents.Aggregate(new Dictionary<Point, int>(), MapVent).Count(x => x.Value > 1);

Dictionary<Point, int> MapVent(Dictionary<Point, int> map, Vent vent)
{
	foreach (var pt in vent.GetPoints())
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
