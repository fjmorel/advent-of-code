using Shared;
using System.Reflection;

namespace TestsShared;

public record SolutionRunner(Assembly _assembly)
{
    public async Task RunDay(string day, string folder, long part1, long part2)
    {
        var job = GetJob(day, folder);
        var actual1 = await job.GetPart1String();
        Assert.Equal<object>(part1.ToString(), actual1);
        var actual2 = await job.GetPart2String();
        Assert.Equal<object>(part2.ToString(), actual2);
    }

    public async Task RunPart1(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution.ToString(), await job.GetPart1String());
    }

    public async Task RunPart2(string day, string folder, long solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution.ToString(), await job.GetPart2String());
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
