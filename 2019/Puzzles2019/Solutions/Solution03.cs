namespace Puzzles2019.Solutions;

public class Solution03 : ISolution
{
    private readonly List<Point> firstPositions;
    private readonly List<Point> secondPositions;
    private readonly HashSet<Point> intersections;

    public Solution03(string[] lines)
	{
        var wires = lines.Select(ParseInstructions).ToArray();
        firstPositions = GetPositions(wires[0]);
        secondPositions = GetPositions(wires[1]);
        intersections = firstPositions.Intersect(secondPositions).ToHashSet();
	}

	public async ValueTask<long> GetPart1()
	{
        return intersections.Skip(1).Min(point => Math.Abs(point.x) + Math.Abs(point.y));
	}

	public async ValueTask<long> GetPart2()
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
}

