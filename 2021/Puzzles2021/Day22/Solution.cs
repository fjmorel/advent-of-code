namespace Puzzles2021.Day22;

public partial record Solution(List<Solution.Command> _commands) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var commands = new List<Command>();
        foreach (var line in lines)
        {
            var groups = Parser.Match(line).Groups;

            var on = groups[1].ValueSpan is "on";
            var min = new Point3d(groups[2], groups[4], groups[6]);
            var max = new Point3d(groups[3], groups[5], groups[7]);

            commands.Add(new(on, new(min, max)));
        }

        return new(commands);
    }

    public async ValueTask<long> GetPart1()
    {
        var max = new Point3d(int.MaxValue, int.MaxValue, int.MaxValue);
        var min = new Point3d(int.MinValue, int.MinValue, int.MinValue);

        // Go ahead and turn off stuff out of bounds at the end
        var commands = _commands
            .Concat([
                new(false, new(min with { x = 51 }, max)),
                new(false, new(min, max with { x = -51 })),

                new(false, new(min with { y = 51 }, max)),
                new(false, new(min, max with { y = -51 })),

                new(false, new(min with { z = 51 }, max)),
                new(false, new(min, max with { z = -51 })),
            ]);

        return GetCount(commands);
    }

    public async ValueTask<long> GetPart2() => GetCount(_commands);

    public static long GetCount(IEnumerable<Command> commands)
    {
        var sections = new Dictionary<Cube, long>();

        foreach (var command in commands)
        {
            var newCube = command.cube;

            var newSections = new Dictionary<Cube, long>();

            // Go through existing cubes, figuring out how much to subtract from current command
            // (because we're turning something off, or it's already on)
            foreach ((Cube section, long existingValue) in sections)
            {
                // Figure out overlap
                var overlapMin = Point3d.Apply(newCube.min, section.min, int.Max);
                var overlapMax = Point3d.Apply(newCube.max, section.max, int.Min);
                if (Point3d.Match(overlapMin, overlapMax, (a, b) => a <= b))
                {
                    var overlap = new Cube(overlapMin, overlapMax);
                    newSections[overlap] = newSections.GetValueOrDefault(overlap, 0) - existingValue;
                }
            }

            // Then add this cube if it's on
            if (command.on)
                newSections[newCube] = newSections.GetValueOrDefault(newCube, 0) + (command.on ? 1 : -1);

            // Update final state with current step
            foreach ((Cube section, long value) in newSections)
                sections[section] = sections.GetValueOrDefault(section, 0) + value;
        }

        return sections.Sum(a => a.Key.GetSize() * a.Value);
    }

    public record Cube(Point3d min, Point3d max)
    {
        public long GetSize() => (max.x - min.x + 1L) * (max.y - min.y + 1L) * (max.z - min.z + 1L);
    }

    public record Command(bool on, Cube cube);

    [GeneratedRegex("([a-z]+) x=([0-9\\-]+)..([0-9\\-]+),y=([0-9\\-]+)..([0-9\\-]+),z=([0-9\\-]+)..([0-9\\-]+)")]
    private static partial Regex Parser { get; }
}
