namespace TestsTemplate;

public class SolutionTests
{
    private static readonly SolutionTester _runner = new(typeof(Solution01).Assembly);

    [Theory]
    [InlineData("01", 0000, 0000)]
    [InlineData("02", 0000, 0000)]
    [InlineData("03", 0000, 0000)]
    [InlineData("04", 0000, 0000)]
    [InlineData("05", 0000, 0000)]
    [InlineData("06", 0000, 0000)]
    [InlineData("07", 0000, 0000)]
    [InlineData("08", 0000, 0000)]
    [InlineData("09", 0000, 0000)]
    [InlineData("10", 0000, 0000)]
    [InlineData("11", 0000, 0000)]
    [InlineData("12", 0000, 0000)]
    [InlineData("13", 0000, 0000)]
    [InlineData("14", 0000, 0000)]
    [InlineData("15", 0000, 0000)]
    [InlineData("16", 0000, 0000)]
    [InlineData("17", 0000, 0000)]
    [InlineData("18", 0000, 0000)]
    [InlineData("19", 0000, 0000)]
    [InlineData("20", 0000, 0000)]
    [InlineData("21", 0000, 0000)]
    [InlineData("22", 0000, 0000)]
    [InlineData("23", 0000, 0000)]
    [InlineData("24", 0000, 0000)]
    [InlineData("25", 0000, 0000)]
    public async Task RunExample(string day, long part1, long part2)
    {
        await _runner.RunDay(day, "examples", part1, part2);
    }

    [Theory]
    [InlineData("01", 0000, 0000)]
    [InlineData("02", 0000, 0000)]
    [InlineData("03", 0000, 0000)]
    [InlineData("04", 0000, 0000)]
    [InlineData("05", 0000, 0000)]
    [InlineData("06", 0000, 0000)]
    [InlineData("07", 0000, 0000)]
    [InlineData("08", 0000, 0000)]
    [InlineData("09", 0000, 0000)]
    [InlineData("10", 0000, 0000)]
    [InlineData("11", 0000, 0000)]
    [InlineData("12", 0000, 0000)]
    [InlineData("13", 0000, 0000)]
    [InlineData("14", 0000, 0000)]
    [InlineData("15", 0000, 0000)]
    [InlineData("16", 0000, 0000)]
    [InlineData("17", 0000, 0000)]
    [InlineData("18", 0000, 0000)]
    [InlineData("19", 0000, 0000)]
    [InlineData("20", 0000, 0000)]
    [InlineData("21", 0000, 0000)]
    [InlineData("22", 0000, 0000)]
    [InlineData("23", 0000, 0000)]
    [InlineData("24", 0000, 0000)]
    [InlineData("25", 0000, 0000)]
    public async Task RunInput(string day, long part1, long part2)
    {
        await _runner.RunDay(day, "inputs", part1, part2);
    }

}
