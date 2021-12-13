using Shared;
using System.Reflection;

namespace TestsShared;

public record SolutionRunner(Assembly _assembly)
{
    public async Task RunDay(string day, string folder, long part1, long part2)
    {
        var job = GetJob(day, folder);
        var actual1 = await job.GetPart1();
        Assert.Equal(part1, actual1);
        var actual2 = await job.GetPart2();
        Assert.Equal(part2, actual2);
    }

    public async Task RunPart1(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart1());
    }

    public async Task RunPart2(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal(solution, await job.GetPart2());
    }

    private ISolution GetJob(string day, string folder)
    {
        var runner = new Runner(_assembly);
        Assert.True(runner.TryGetData(folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(runner.TryGetSolution(day, lines!, out var job));
        Assert.NotNull(job);
        return job!;
    }
}
