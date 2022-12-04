namespace Puzzles2021.Solutions;

public partial record Solution17(List<int> _heights) : ISolution<Solution17>
{
    public static Solution17 Init(string[] lines)
    {
        var groups = GetParser().Match(lines[0]).Groups;
        var min = new Point(groups[1], groups[3]);
        var max = new Point(groups[2], groups[4]);
        var heights = FindMaximumHeights(min, max).ToList();
        return new(heights);
    }

    public async ValueTask<long> GetPart1() => _heights.Max();

    public async ValueTask<long> GetPart2() => _heights.Count;

    private static IEnumerable<int> FindMaximumHeights(Point min, Point max)
    {
        for (var startX = (int)Math.Sqrt(min.x); startX <= max.x; startX++)
        for (var startY = min.y; startY <= -min.y; startY++)
        {
            var velocity = new Point(startX, startY);
            var position = new Point(0, 0);
            int highest = 0;
            while (position.y > min.y && !(position.x < min.x && velocity.x == 0))
            {
                position += velocity;
                velocity += new Point(velocity.x > 0 ? -1 : 0, -1);
                highest = Math.Max(highest, position.y);
                if (position.IsWithinInclusive(min, max))
                {
                    yield return highest;
                    break;
                }
            }
        }
    }

    [GeneratedRegex("target area: x=(-?[0-9]+)..(-?[0-9]+), y=(-?[0-9]+)..(-?[0-9]+)")]
    private static partial Regex GetParser();
}
