var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var wires = list.Select(ParseInstructions).ToArray();
var firstPositions = GetPositions(wires[0]);
var secondPositions = GetPositions(wires[1]);
var intersections = firstPositions.Intersect(secondPositions).ToHashSet();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return intersections.Skip(1).Min(point => Math.Abs(point.x) + Math.Abs(point.y));
}

long Part2()
{
	return intersections.Skip(1).Min(point =>
	{
		var first = firstPositions.IndexOf(point);
		var second = secondPositions.IndexOf(point);
		return first + second;
	});
}

List<Point> GetPositions(List<Instruction> instructions)
{
	return instructions.Aggregate(new List<Point>() { default }, (list, instruction) =>
	{
		var previous = list[^1];
		for (var i = 1; i <= instruction.magnitude; i++)
			list.Add(Move(previous, instruction with { magnitude = i }));
		return list;
	});
}

List<Instruction> ParseInstructions(string line)
{
	return line
		.Split(',')
		.Select(x => new Instruction(x[0], int.Parse(x.AsSpan()[1..])))
		.ToList();
}

Point Move(Point start, Instruction move)
{
	return move.dir switch
	{
		'L' => start with { x = start.x - move.magnitude },
		'R' => start with { x = start.x + move.magnitude },
		'D' => start with { y = start.y - move.magnitude },
		'U' => start with { y = start.y + move.magnitude },
	};
}

readonly record struct Instruction(char dir, int magnitude);
readonly record struct Point(int x, int y);
