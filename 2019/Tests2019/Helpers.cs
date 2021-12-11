using Puzzles2019.Solutions;
using System.Reflection;

namespace Tests2019;

internal class Helpers
{
    private static readonly Assembly _assembly = typeof(Solution01).Assembly;

    public static async Task RunDay(string day, string folder, long part1, long part2)
    {
        var job = GetJob(day, folder);
        var actual1 = await job.GetPart1();
        Assert.Equal(part1, actual1);
        var actual2 = await job.GetPart2();
        Assert.Equal(part2, actual2);
    }

    public static async Task RunPart1(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart1());
    }

    public static async Task RunPart2(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart2());
    }

    private static ISolution GetJob(string day, string folder)
    {
        Assert.True(RunnerUtilities.TryGetData(_assembly, folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(RunnerUtilities.TryGetSolution(_assembly, day, lines!, out var job));
        Assert.NotNull(job);
        return job!;
    }
}
