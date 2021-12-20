namespace Tests2021;

public class WorkInProgressTests
{
    private static readonly SolutionTester _runner = new(typeof(Solution01).Assembly);

    private const string _day = "21";
    private const long _example1 = 0000;
    private const long _example2 = 0000;

    private const long _input1 = 0000;
    private const long _input2 = 0000;

    [Fact]
    public Task DailyExample1() => _runner.RunPart1(_day, "examples", _example1);

    [Fact]
    public Task DailyExample2() => _runner.RunPart2(_day, "examples", _example2);

    [Fact]
    public Task DailyInput1() => _runner.RunPart1(_day, "inputs", _input1);

    [Fact]
    public Task DailyInput2() => _runner.RunPart2(_day, "inputs", _input2);

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
