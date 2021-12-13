namespace Tests2020;

public class SolutionTests
{
    private static readonly SolutionRunner _runner = new(typeof(Puzzles2020.Solutions.Solution01).Assembly);

    [Theory]
    [InlineData("01", 514579, 241861950)]
    [InlineData("02", 2, 1)]
    [InlineData("03", 7, 336)]
    [InlineData("04", 2, 2)]
    //[InlineData("05", 0000, 0000)]
    [InlineData("06", 11, 6)]
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
    //[InlineData("25", 0000, 0000)]
    public async Task RunExample(string day, long part1, long part2)
    {
        await _runner.RunDay(day, "examples", part1, part2);
    }

    [Theory]
    [InlineData("01", 870331, 283025088)]
    [InlineData("02", 560, 303)]
    [InlineData("03", 223, 3517401300)]
    [InlineData("04", 202, 137)]
    [InlineData("05", 842, 617)]
    [InlineData("06", 6748, 3445)]
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
    [InlineData("25", 9714832, 0000)]
    public async Task RunInput(string day, long part1, long part2)
    {
        await _runner.RunDay(day, "inputs", part1, part2);
    }

}
