namespace Tests2022;

public class SolutionTests
{
    private static readonly SolutionTester _tester = new(typeof(Puzzles2022.Day01.Solution).Assembly);

    [Theory]
    [InlineData("01", 24000, 45000)]
    [InlineData("02", 15, 12)]
    [InlineData("03", 157, 70)]
    [InlineData("04", 2, 4)]
    [InlineData("06", 7, 19)]
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
    public Task RunExample(string day, long part1, long part2) => _tester.Day(day, "examples", part1, part2);

    [Theory]
    [InlineData("01", 69693, 200945)]
    [InlineData("02", 10718, 14652)]
    [InlineData("03", 7742, 2276)]
    [InlineData("04", 644, 926)]
    [InlineData("06", 1100, 2421)]
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
    public Task RunInput(string day, long part1, long part2) => _tester.Day(day, "inputs", part1, part2);

    // [Trait("Slow", "true")]
    // [Theory]
    // [InlineData("inputs", "01", 0000, 0000)]
    // public Task RunSlowDays(string folder, string day, long part1, long part2) => _tester.Day(day, folder, part1, part2);
    //
    public static TheoryData<string, string, string, string> RunStrings_Data => new()
    {
        { "examples", "05", "CMZ", "MCD" },
        { "inputs", "05", "QNHWJVJZW", "BPCZJLFJW" },
    };

    [Theory, MemberData(nameof(RunStrings_Data))]
    public Task RunStrings(string folder, string day, string part1, string part2) => _tester.Day(day, folder, part1, part2);
}
