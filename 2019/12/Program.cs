var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

var initialPositions = list.Select(line =>
{
	var nums = Regex.Matches(line, "(-?[0-9]+)+").Select(x => long.Parse(x.ValueSpan)).ToList();
	return new Coordinates(nums[0], nums[1], nums[2]);
}).ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	// Setup moons in first position with 0 velocity
	var moons = initialPositions.Select(x => new Moon() { Position = x }).ToArray();

	var linked = new MoonWithInfluencingBodies[moons.Length];
	for (var i = 0; i < moons.Length; i++)
	{

		var related = new List<Moon>(moons);
		related.Remove(moons[i]);
		linked[i] = new(moons[i], related.ToArray());
	}

	for (var i = 1; i <= 1_000; i++)
	{
		MoveOneStep(linked);
	}

	return moons.Sum(x => x.GetEnergy());
}

long Part2()
{
	// Setup moons in first position with 0 velocity
	var moons = initialPositions.Select(x => new Moon() { Position = x }).ToArray();

	var linked = new MoonWithInfluencingBodies[moons.Length];
	for (var i = 0; i < moons.Length; i++)
	{

		var related = new List<Moon>(moons);
		related.Remove(moons[i]);
		linked[i] = new(moons[i], related.ToArray());
	}

	// 4_686_774_924
	for (var i = 1; i <= 1_000_000; i++)
	{
		MoveOneStep(linked);
	}

	return moons.Sum(x => x.GetEnergy());
}

void MoveOneStep(MoonWithInfluencingBodies[] linked)
{
	// adjust velocities based on other bodies
	foreach (var (moon, bodies) in linked)
	{
		long x = 0, y = 0, z = 0;
		foreach (var body in bodies)
		{
			if (body.Position.x > moon.Position.x)
				x++;
			else if (body.Position.x < moon.Position.x)
				x--;

			if (body.Position.y > moon.Position.y)
				y++;
			else if (body.Position.y < moon.Position.y)
				y--;

			if (body.Position.z > moon.Position.z)
				z++;
			else if (body.Position.z < moon.Position.z)
				z--;
		}
		moon.Velocity += new Coordinates(x, y, z);
	}
	// adjust positions based on velocity
	foreach (var (moon, _) in linked)
	{
		moon.Position += moon.Velocity;
	}
}

readonly record struct Coordinates(long x, long y, long z)
{
	public static Coordinates operator +(Coordinates a, Coordinates b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
	public long GetEnergy() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
}

// not a struct so we can modify its coordinates while still keeping references in MoonWithInfluencingBodies
record Moon
{
	public Coordinates Position { get; set; }
	public Coordinates Velocity { get; set; }
	public long GetEnergy() => Position.GetEnergy() * Velocity.GetEnergy();
}

record MoonWithInfluencingBodies(Moon moon, Moon[] bodies);
