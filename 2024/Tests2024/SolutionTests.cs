using Puzzles2024.Day01;

namespace Tests2024;

public class SolutionTests
{
    private static readonly SolutionTester _tester = new(typeof(Solution).Assembly);

    [Theory]
    [InlineData("01", "11", "31")]
    [InlineData("02", "0", "0")]
    [InlineData("03", "0", "0")]
    [InlineData("04", "0", "0")]
    [InlineData("05", "0", "0")]
    [InlineData("06", "0", "0")]
    [InlineData("07", "0", "0")]
    [InlineData("08", "0", "0")]
    [InlineData("09", "0", "0")]
    [InlineData("10", "0", "0")]
    [InlineData("11", "0", "0")]
    [InlineData("12", "0", "0")]
    [InlineData("13", "0", "0")]
    [InlineData("14", "0", "0")]
    [InlineData("15", "0", "0")]
    [InlineData("16", "0", "0")]
    [InlineData("17", "0", "0")]
    [InlineData("18", "0", "0")]
    [InlineData("19", "0", "0")]
    [InlineData("20", "0", "0")]
    [InlineData("21", "0", "0")]
    [InlineData("22", "0", "0")]
    [InlineData("23", "0", "0")]
    [InlineData("24", "0", "0")]
    [InlineData("25", "0", "0")]
    public Task RunExample(string day, string part1, string part2) => _tester.Day(day, "examples", part1, part2);

    [Theory]
    [InlineData("01", "2264607", "19457120")]
    [InlineData("02", "0", "0")]
    [InlineData("03", "0", "0")]
    [InlineData("04", "0", "0")]
    [InlineData("05", "0", "0")]
    [InlineData("06", "0", "0")]
    [InlineData("07", "0", "0")]
    [InlineData("08", "0", "0")]
    [InlineData("09", "0", "0")]
    [InlineData("10", "0", "0")]
    [InlineData("11", "0", "0")]
    [InlineData("12", "0", "0")]
    [InlineData("13", "0", "0")]
    [InlineData("14", "0", "0")]
    [InlineData("15", "0", "0")]
    [InlineData("16", "0", "0")]
    [InlineData("17", "0", "0")]
    [InlineData("18", "0", "0")]
    [InlineData("19", "0", "0")]
    [InlineData("20", "0", "0")]
    [InlineData("21", "0", "0")]
    [InlineData("22", "0", "0")]
    [InlineData("23", "0", "0")]
    [InlineData("24", "0", "0")]
    [InlineData("25", "0", "0")]
    public Task RunInput(string day, string part1, string part2) => _tester.Day(day, "inputs", part1, part2);

    // [Trait("Category", "Slow")]
    // [Theory]
    // [InlineData("inputs", "01", "0", "0")]
    // public Task RunSlowDays(string folder, string day, string part1, string part2) => _tester.Day(day, folder, part1, part2);
}
