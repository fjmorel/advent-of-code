using Spectre.Console;

namespace Puzzles2021.Solutions;

public class Solution13 : ISolution
{
    private readonly HashSet<Point> _points;
    private readonly List<Fold> _folds;

    public Solution13(string[] lines)
    {
        _points = new HashSet<Point>();
        _folds = new List<Fold>();
        foreach (var line in lines)
        {
            var coordMatch = Regex.Match(line, "([0-9]+),([0-9]+)");
            if (coordMatch.Success)
                _points.Add(new(int.Parse(coordMatch.Groups[1].Value), int.Parse(coordMatch.Groups[2].Value)));
            else
            {
                var foldMatch = Regex.Match(line, "fold along ([xy])=([0-9]+)");
                if (foldMatch.Success)
                {
                    _folds.Add(new(foldMatch.Groups[1].ValueSpan[0], int.Parse(foldMatch.Groups[2].ValueSpan)));
                }
            }
        }
    }

    public async ValueTask<long> GetPart1() => FoldPaper(_points, _folds[0]).Count;

    public async ValueTask<long> GetPart2()
    {
        var dots = _folds.Aggregate(_points, FoldPaper);
        dots.Print(pt => dots.Contains(pt) ? '#' : ' ');
        return -1;
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

    private readonly record struct Fold(char axis, int index);
}
