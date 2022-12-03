namespace Puzzles2022.Day03;

public record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1() => _lines.Sum(line =>
    {
        var half = line.Length / 2;
        return FindIntersectionPriority(line[..half], line[half..]);
    });

    public async ValueTask<long> GetPart2() => _lines.Chunk(3).Sum(FindIntersectionPriority);

    private static long FindIntersectionPriority(params IEnumerable<char>[] lines)
    {
        var intersection = lines.Skip(1).Aggregate(lines[0], (a, b) => a.Intersect(b)).Single();
        return char.IsUpper(intersection) ? intersection - 'A' + 27 : intersection - 'a' + 1;
    }
}
