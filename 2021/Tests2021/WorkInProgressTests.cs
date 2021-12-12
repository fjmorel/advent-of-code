namespace Tests2021;

public class WorkInProgressTests
{
    private const string _day = "12";
    private const long _example1 = 00000;
    private const long _example2 = 00000;

    private const long _input1 = 00000;
    private const long _input2 = 00000;

    [Fact]
    public Task DailyExample1() => Helpers.RunPart1(_day, "examples", _example1);

    [Fact]
    public Task DailyExample2() => Helpers.RunPart2(_day, "examples", _example2);

    [Fact]
    public Task DailyInput1() => Helpers.RunPart1(_day, "inputs", _input1);

    [Fact]
    public Task DailyInput2() => Helpers.RunPart2(_day, "inputs", _input2);
}
