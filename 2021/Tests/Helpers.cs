namespace Tests;

internal class Helpers
{
    public static async ValueTask RunDay(string day, string folder, long part1, long part2)
    {
        var job = GetJob(day, folder);
        Assert.Equal(part1, await job.GetPart1());
        Assert.Equal(part2, await job.GetPart2());
    }

    public static async ValueTask RunPart1(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart1());
    }

    public static async ValueTask RunPart2(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart2());
    }

    private static ISolution GetJob(string day, string folder)
    {
        Assert.True(Utilities.TryGetData(folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(Utilities.TryGetSolution(day, lines!, out var job));
        Assert.NotNull(job);
        return job!;
    }
}
