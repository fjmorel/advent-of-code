using Range = Shared.Point;

namespace Puzzles2022.Day04;

public partial record Solution(List<(Range, Range)> _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var groups = lines.Select(line =>
        {
            var groups = GetLineRegex().Match(line).Groups;
            return (new Range(groups[1], groups[2]), new Range(groups[3], groups[4]));
        }).ToList();
        return new Solution(groups);
    }

    public async ValueTask<long> GetPart1() => _lines.Count(x => Contains(x.Item1, x.Item2) || Contains(x.Item2, x.Item1));
    public async ValueTask<long> GetPart2() => _lines.Count(x => Overlaps(x.Item1, x.Item2) || Overlaps(x.Item2, x.Item1));

    private static bool Contains(Range a, Range b) => a.x >= b.x && a.y <= b.y;
    private static bool Overlaps(Range a, Range b) => (a.x >= b.x && a.x <= b.y) || (a.y <= b.y && a.y >= b.x);

    [GeneratedRegex("([0-9]+)-([0-9]+),([0-9]+)-([0-9]+)")]
    private static partial Regex GetLineRegex();
}
