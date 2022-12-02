namespace Puzzles2022.Solutions;

public record Solution02(string[] lines) : ISolution
{
    public async ValueTask<long> GetPart1() => lines.Aggregate(0L, (score, line) =>
    {
        var p1 = line[0] - 'A';
        var p2 = line[2] - 'X';
        var outcome = ((p2 - p1 + 3) % 3 + 1) % 3 * 3;
        return score + (p2 + 1) + outcome;
    });

    public async ValueTask<long> GetPart2() => lines.Aggregate(0L, (score, line) =>
    {
        var p1 = line[0] - 'A';
        var outcome = line[2] - 'X';
        var p2 = (p1 + outcome + 2) % 3 + 1;
        return score + p2 + (outcome * 3);
    });
}
