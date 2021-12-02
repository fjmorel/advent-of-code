var timer = Stopwatch.StartNew();
var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var instructions = list.Select(x =>
{
	var split = x.Split(' ');
	return new Step(split[0], int.Parse(split[1]));
}).ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

int Part1()
{
	var location = instructions.Aggregate(default(Point), (point, step) => point.Move(step));
	return location.x * location.y;
}

int Part2()
{
	var location = instructions.Aggregate(default(PointWithAim), (point, step) => point.Move(step));
	return location.x * location.y;
}

public readonly record struct Point(int x, int y)
{
	public Point Move(Step step) => step.direction switch
	{
		"down" => this with { y = this.y + step.magnitude },
		"up" => this with { y = this.y - step.magnitude },
		"forward" => this with { x = this.x + step.magnitude },
		_ => throw new ArgumentException("Unexpected direction: " + step.direction),
	};
}
public readonly record struct PointWithAim(int x, int y, int aim)
{
	public PointWithAim Move(Step step) => step.direction switch
	{
		"down" => this with { aim = this.aim + step.magnitude },
		"up" => this with { aim = this.aim - step.magnitude },
		"forward" => this with { x = this.x + step.magnitude, y = this.y + step.magnitude * this.aim },
		_ => throw new ArgumentException("Unexpected direction: " + step.direction),
	};
}
public readonly record struct Step(string direction, int magnitude);
