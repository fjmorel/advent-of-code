namespace Tests2021;

[Trait("Category", "InProgress")]
public class WorkInProgressTests
{
    private static readonly SolutionTester _runner = new(typeof(Puzzles2021.Solutions.Solution01).Assembly);

    private const string _day = "23";
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

    // [Theory]
    // [InlineData("D2FE28", 2021)]
    // [InlineData("C200B40A82", 3)]
    // [InlineData("04005AC33890", 54)]
    // [InlineData("880086C3E88112", 7)]
    // [InlineData("CE00C43D881120", 9)]
    // [InlineData("D8005AC2A8F0", 1)]
    // [InlineData("F600BC2D8F", 0)]
    // [InlineData("9C005AC2F8F0", 0)]
    // [InlineData("9C0141080250320F1802104A08", 1)]
    // public async Task Examples(string line, long expected)
    // {
    //     var sol = new Solution16(new string[] { line });
    //     var actual = await sol.GetPart2();
    //     Assert.Equal(expected, actual);
    // }
}
