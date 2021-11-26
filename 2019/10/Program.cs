var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var grid = list.Select(line => line.Select(x => x == '#').ToArray()).ToArray();
var height = grid.Length;
var width = grid[0].Length;

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var slopes = GetSlopes().ToHashSet();
	var asteroids = GetAsteroids().ToHashSet();
	var visibleAtEachAsteroid = new Dictionary<Point, HashSet<Point>>();

	foreach (var station in asteroids)
	{
		var detected = new HashSet<Point>();
		foreach (var slope in slopes)
		{
			var pt = station + slope;
			while (pt.WithinGrid(width, height))
			{
				if (asteroids.Contains(pt))
				{
					detected.Add(pt);
					break;
				}
				pt += slope;
			}
		}
		visibleAtEachAsteroid[station] = detected;
	}

	var best = visibleAtEachAsteroid.MaxBy(x => x.Value.Count);
	return best.Value.Count;
}

long Part2()
{
	return -1;
}

IEnumerable<Point> GetAsteroids()
{
	for (var x = 0; x < width; x++)
	{
		for (var y = 0; y < height; y++)
		{
			if (grid[y][x])
				yield return new(x, y);

		}
	}
}

IEnumerable<Point> GetSlopes()
{
	yield return new(0, 1);
	yield return new(0, -1);
	yield return new(1, 0);
	yield return new(-1, 0);

	yield return new(1, 1);
	yield return new(1, -1);
	yield return new(-1, 1);
	yield return new(-1, -1);

	for (var x = 1; x < width; x++)
	{
		for (var y = 1; y < height; y++)
		{
			if (x < 2 && y < 2)
			{
				yield return new(x, y);
				yield return new(x, -y);
				yield return new(-x, y);
				yield return new(-x, -y);
			}
			else
			{
				var multiple = false;
				for (var test = Math.Max(x, y); test > 1; test--)
				{
					if (x % test == 0 && y % test == 0)
					{
						multiple = true;
						break;
					}
				}
				if (!multiple)
				{
					yield return new(x, y);
					yield return new(x, -y);
					yield return new(-x, y);
					yield return new(-x, -y);
				}
			}
		}
	}
}

readonly record struct Point(int x, int y)
{
	public static Point operator +(Point a, Point b) => new(a.x + b.x, a.y + b.y);
	public bool WithinGrid(int width, int height) => this.x >= 0 && this.x < width && this.y >= 0 && this.y < height;
}