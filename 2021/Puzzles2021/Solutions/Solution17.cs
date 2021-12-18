namespace Puzzles2021.Solutions;

public class Solution17 : ISolution
{
    private readonly List<int> _heights;
    private readonly Point _max;
    private readonly Point _min;

    public Solution17(string[] lines)
    {
        var groups = Regex.Match(lines[0], "target area: x=(-?[0-9]+)..(-?[0-9]+), y=(-?[0-9]+)..(-?[0-9]+)").Groups;
        _min = new(groups[1].ValueSpan, groups[3].ValueSpan);
        _max = new(groups[2].ValueSpan, groups[4].ValueSpan);
        _heights = FindMaximumHeights().ToList();
    }

    public async ValueTask<long> GetPart1() => _heights.Max();

    public async ValueTask<long> GetPart2() => _heights.Count;

    private IEnumerable<int> FindMaximumHeights()
    {
        for (var startX = (int)Math.Sqrt(_min.x); startX <= _max.x; startX++)
        for (var startY = _min.y; startY <= -_min.y; startY++)
        {
            var velocity = new Point(startX, startY);
            var position = new Point(0, 0);
            int highest = 0;
            while (position.y > _min.y && !(position.x < _min.x && velocity.x == 0))
            {
                position += velocity;
                velocity += new Point(velocity.x > 0 ? -1 : 0, -1);
                highest = Math.Max(highest, position.y);
                if (position.IsWithinInclusive(_min, _max))
                {
                    yield return highest;
                    break;
                }
            }
        }
    }
}
