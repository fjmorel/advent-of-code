using System.Reflection;

namespace TestsShared;

public record SolutionTester(Assembly _assembly)
{
    public async Task RunDay(string day, string folder, string part1, string part2)
    {
        var job = GetJob(day, folder);
        var actual1 = await job.GetPart1String();
        Assert.Equal<object>(part1, actual1);
        var actual2 = await job.GetPart2String();
        Assert.Equal<object>(part2, actual2);
    }

    public Task RunDay(string day, string folder, long part1, long part2)
        => RunDay(day, folder, part1.ToString(), part2.ToString());

    public async Task RunPart1(string day, string folder, string solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution, await job.GetPart1String());
    }

    public Task RunPart1(string day, string folder, long solution)
        => RunPart1(day, folder, solution.ToString());

    public async Task RunPart2(string day, string folder, string solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution, await job.GetPart2String());
    }

    public Task RunPart2(string day, string folder, long solution)
        => RunPart2(day, folder, solution.ToString());

    private ISolution GetJob(string day, string folder)
    {
        var runner = new Runner(_assembly);
        Assert.True(runner.TryGetData(folder, day, out var lines));
        Assert.NotNull(lines);
        Assert.True(runner.TryGetSolution(day, lines, out var job));
        Assert.NotNull(job);
        return job;
    }
}
