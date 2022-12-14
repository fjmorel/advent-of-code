namespace Tests2022;

[Trait("Category", "InProgress")]
public class WorkInProgressTests
{
    private static readonly SolutionTester _tester = new(typeof(Puzzles2022.Day01.Solution).Assembly);

    private const string _day = "14";

    private const string _example1 = "";
    private const string _example2 = "";

    private const string _input1 = "";
    private const string _input2 = "";

    [Fact]
    public Task Example1() => _tester.Part1(_day, "examples", _example1);

    [Fact]
    public Task Example2() => _tester.Part2(_day, "examples", _example2);

    [Fact]
    public Task Input1() => _tester.Part1(_day, "inputs", _input1);

    [Fact]
    public Task Input2() => _tester.Part2(_day, "inputs", _input2);
}
