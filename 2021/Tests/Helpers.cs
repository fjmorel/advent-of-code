namespace Tests;

internal class Helpers
{
    public static async Task RunDay(string day, string folder, long part1, long part2)
    {
        Assert.True(Utilities.TryGetData(folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(Utilities.TryGetSolution(day, lines!, out var job));
        Assert.NotNull(job);

        Assert.Equal(part1, await job!.GetPart1());
        Assert.Equal(part2, await job.GetPart2());
    }
}
