namespace Puzzles2021.Solutions;

public class Solution20 : ISolution
{
    private readonly Dictionary<Point, bool> _image;
    private readonly bool[] _algorithm;

    public Solution20(string[] lines)
    {
        _algorithm = lines[0].Select(x => x == '#').ToArray();
        _image = lines.Skip(2).SelectMany((line, y) => line.Select((ch, x) => (ch, x, y))).ToDictionary(tuple => new Point(tuple.x, tuple.y), tuple => tuple.ch == '#');
    }

    public async ValueTask<long> GetPart1() => GetLitPixelsAfter(2);

    public async ValueTask<long> GetPart2() => GetLitPixelsAfter(50);

    public long GetLitPixelsAfter(int iterations)
        => Enumerable.Range(1, iterations)
            .Aggregate(new Data(false, _image), (data, _) => Enhance(data))
            .Defined.LongCount(x => x.Value);

    public Data Enhance(Data data)
    {
        var (infiniteLit, original) = data;
        var newImage = new Dictionary<Point, bool>();
        const int expansion = 2;

        var minX = original.Min(pair => pair.Key.x);
        var minY = original.Min(pair => pair.Key.y);
        var maxX = original.Max(pair => pair.Key.x);
        var maxY = original.Max(pair => pair.Key.y);

        for (var y = minY - expansion; y < maxY + expansion; y++)
        {
            for (var x = minX - expansion; x < maxX + expansion; x++)
            {
                var pt = new Point(x, y);
                var pixels = new bool[]
                {
                    original.GetValueOrDefault(pt + new Point(-1, -1), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(0, -1), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(1, -1), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(-1, 0), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(0, 0), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(1, 0), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(-1, 1), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(0, 1), infiniteLit),
                    original.GetValueOrDefault(pt + new Point(1, 1), infiniteLit),
                };

                var index = Convert.ToInt32(string.Join("", pixels.Select(p => p ? '1' : '0')), 2);
                newImage[pt] = _algorithm[index];
            }
        }

        return new(_algorithm[infiniteLit ? ^1 : 0], newImage);
    }

    public record Data(bool LitInfinite, Dictionary<Point, bool> Defined);
}
