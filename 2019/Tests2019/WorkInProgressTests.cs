namespace Tests2019;

[Trait("Category", "InProgress")]
public class WorkInProgressTests
{
    private static readonly SolutionTester _runner = new(typeof(Puzzles2019.Solutions.Solution01).Assembly);

    private const string _day = "19";
    private const string _example1 = "";
    private const string _example2 = "";

    private const string _input1 = "";
    private const string _input2 = "";

    [Fact]
    public Task DailyExample1() => _runner.Part1(_day, "examples", _example1);

    [Fact]
    public Task DailyExample2() => _runner.Part2(_day, "examples", _example2);

    [Fact]
    public Task DailyInput1() => _runner.Part1(_day, "inputs", _input1);

    [Fact]
    public Task DailyInput2() => _runner.Part2(_day, "inputs", _input2);
}
