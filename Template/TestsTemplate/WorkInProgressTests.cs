namespace TestsTemplate;

public class WorkInProgressTests
{
    private static readonly SolutionTester _runner = new(typeof(PuzzlesTemplate.Solutions.Solution01).Assembly);

    private const string _day = "01";

    private const long _example1 = 00000;
    private const long _example2 = 00000;

    private const long _input1 = 00000;
    private const long _input2 = 00000;

    [Fact]
    public Task DailyExample1() => _runner.RunPart1(_day, "examples", _example1);

    [Fact]
    public Task DailyExample2() => _runner.RunPart2(_day, "examples", _example2);

    [Fact]
    public Task DailyInput1() => _runner.RunPart1(_day, "inputs", _input1);

    [Fact]
    public Task DailyInput2() => _runner.RunPart2(_day, "inputs", _input2);
}
