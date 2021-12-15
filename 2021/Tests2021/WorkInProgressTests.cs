namespace Tests2021;

public class WorkInProgressTests
{
    private static readonly SolutionTester _runner = new(typeof(Solution01).Assembly);

    private const string _day = "15";
    private const long _example1 = 40;
    private const long _example2 = 315;

    private const long _input1 = 456;
    private const long _input2 = 2831;

    [Fact]
    public Task DailyExample1() => _runner.RunPart1(_day, "examples", _example1);

    [Fact]
    public Task DailyExample2() => _runner.RunPart2(_day, "examples", _example2);

    [Fact]
    public Task DailyInput1() => _runner.RunPart1(_day, "inputs", _input1);

    [Fact]
    public Task DailyInput2() => _runner.RunPart2(_day, "inputs", _input2);
}
