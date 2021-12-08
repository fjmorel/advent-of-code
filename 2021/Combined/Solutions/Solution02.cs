namespace Combined.Solutions;

public class Solution02 : ISolution
{
    private readonly List<Step> _data;

    public Solution02(string[] lines)
    {
        _data = lines.Select(x =>
        {
            var split = x.Split(' ');
            return new Step(split[0], int.Parse(split[1]));
        }).ToList();
    }

    public async Task<long> GetPart1()
    {
        var location = _data.Aggregate(default(Point), (point, step) => point.Move(step));
        return location.x * location.y;
    }

    public async Task<long> GetPart2()
    {
        var location = _data.Aggregate(default(PointWithAim), (point, step) => point.Move(step));
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
}

