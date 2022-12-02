namespace Puzzles2021.Solutions;

public record Solution13(HashSet<Point> _points, List<Solution13.Fold> _folds) : ISolution<Solution13>
{
    public static Solution13 Init(string[] lines)
    {
        var points = new HashSet<Point>();
        var folds = new List<Fold>();
        foreach (var line in lines)
        {
            var coordMatch = Regex.Match(line, "([0-9]+),([0-9]+)");
            if (coordMatch.Success)
                points.Add(new(int.Parse(coordMatch.Groups[1].Value), int.Parse(coordMatch.Groups[2].Value)));
            else
            {
                var foldMatch = Regex.Match(line, "fold along ([xy])=([0-9]+)");
                if (foldMatch.Success)
                {
                    folds.Add(new(foldMatch.Groups[1].ValueSpan[0], int.Parse(foldMatch.Groups[2].ValueSpan)));
                }
            }
        }

        return new(points, folds);
    }

    public async ValueTask<long> GetPart1() => FoldPaper(_points, _folds[0]).Count;

    public async ValueTask<string> GetPart2String()
    {
        var dots = _folds.Aggregate(_points, FoldPaper);
        return dots.ToString(pt => dots.Contains(pt) ? '█' : ' ');
    }

    private HashSet<Point> FoldPaper(HashSet<Point> dots, Fold fold)
    {
        return (fold.axis switch
        {
            'x' => dots.Select(dot => dot.x < fold.index ? dot : dot with { x = fold.index - (dot.x - fold.index) }),
            'y' => dots.Select(dot => dot.y < fold.index ? dot : dot with { y = fold.index - (dot.y - fold.index) }),
            _ => throw new ArgumentException("Unexpected axis: " + fold.axis),
        }).ToHashSet();
    }

    public readonly record struct Fold(char axis, int index);
}
