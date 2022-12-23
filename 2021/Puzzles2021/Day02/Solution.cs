namespace Puzzles2021.Day02;

public record Solution(List<Solution.Step> _data) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var data = lines.Select(x =>
        {
            var split = x.Split(' ');
            return new Step(split[0], int.Parse(split[1]));
        }).ToList();
        return new(data);
    }

    public async ValueTask<long> GetPart1()
    {
        var location = _data.Aggregate(default(Point), Move);
        return location.x * location.y;
    }

    public async ValueTask<long> GetPart2()
    {
        var location = _data.Aggregate(default(PointWithAim), Move);
        return location.x * location.y;
    }

    public Point Move(Point pt, Step step) => step.direction switch
    {
        "down" => pt with { y = pt.y + step.magnitude },
        "up" => pt with { y = pt.y - step.magnitude },
        "forward" => pt with { x = pt.x + step.magnitude },
        _ => throw new ArgumentException("Unexpected direction: " + step.direction),
    };

    public PointWithAim Move(PointWithAim pt, Step step) => step.direction switch
    {
        "down" => pt with { aim = pt.aim + step.magnitude },
        "up" => pt with { aim = pt.aim - step.magnitude },
        "forward" => pt with { x = pt.x + step.magnitude, y = pt.y + step.magnitude * pt.aim },
        _ => throw new ArgumentException("Unexpected direction: " + step.direction),
    };

    public readonly record struct PointWithAim(int x, int y, int aim);

    public readonly record struct Step(string direction, int magnitude);
}
