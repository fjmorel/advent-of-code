using System.Reflection;

namespace TestsShared;

// use Assert.Equal<object> to force xunit to output the entire string for easy comparison

public record SolutionTester(Assembly _assembly)
{
    public async Task Day(string day, string folder, string part1, string part2)
    {
        var job = GetJob(day, folder);
        var actual1 = await job.GetPart1String();
        Assert.Equal<object>(part1, actual1);
        var actual2 = await job.GetPart2String();
        Assert.Equal<object>(part2, actual2);
    }

    public Task Day(string day, string folder, long part1, long part2)
        => Day(day, folder, part1.ToString(), part2.ToString());

    public async Task Part1(string day, string folder, string solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution, await job.GetPart1String());
    }

    public Task Part1(string day, string folder, long solution)
        => Part1(day, folder, solution.ToString());

    public async Task Part2(string day, string folder, string solution)
    {
        var job = GetJob(day, folder);
        Assert.Equal<object>(solution, await job.GetPart2String());
    }

    public Task Part2(string day, string folder, long solution)
        => Part2(day, folder, solution.ToString());

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
