using Puzzles2021.Solutions;
using System.Reflection;

namespace Tests2021;

internal class Helpers
{
    private static readonly Assembly _assembly = typeof(Solution01).Assembly;
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
        Assert.True(RunnerUtilities.TryGetData(_assembly, folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(RunnerUtilities.TryGetSolution(_assembly, day, lines!, out var job));
        Assert.NotNull(job);
        return job!;
    }
}
