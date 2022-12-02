namespace Puzzles2019.Solutions;

public record Solution03(
    List<Point> firstPositions,
    List<Point> secondPositions,
    HashSet<Point> intersections
) : ISolution<Solution03>
{
    public static Solution03 Init(string[] lines)
    {
        var wires = lines.Select(ParseInstructions).ToArray();
        var firstPositions = GetPositions(wires[0]);
        var secondPositions = GetPositions(wires[1]);
        var intersections = firstPositions.Intersect(secondPositions).ToHashSet();
        return new Solution03(firstPositions, secondPositions, intersections);
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

    private static List<Point> GetPositions(List<Instruction> instructions)
    {
        return instructions.Aggregate(new List<Point>() { default }, (list, instruction) =>
        {
            var previous = list[^1];
            for (var i = 1; i <= instruction.magnitude; i++)
                list.Add(Move(previous, instruction with { magnitude = i }));
            return list;
        });
    }

    private static List<Instruction> ParseInstructions(string line)
    {
        return line
            .Split(',')
            .Select(x => new Instruction(x[0], int.Parse(x.AsSpan()[1..])))
            .ToList();
    }

    private static Point Move(Point start, Instruction move)
    {
        return move.dir switch
        {
            'L' => start with { x = start.x - move.magnitude },
            'R' => start with { x = start.x + move.magnitude },
            'D' => start with { y = start.y - move.magnitude },
            'U' => start with { y = start.y + move.magnitude },
            _ => throw new ArgumentException($"Unexpected direction: [{move.dir}]"),
        };
    }

    private readonly record struct Instruction(char dir, int magnitude);
}
