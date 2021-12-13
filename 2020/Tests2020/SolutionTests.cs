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
    [InlineData("07", 4, 32)]
    [InlineData("08", 5, 8)]
    //[InlineData("09", 0000, 0000)]
    [InlineData("10", 220, 19208)]
    [InlineData("11", 37, 26)]
    [InlineData("12", 25, 286)]
    [InlineData("13", 295, 1068781)]
    [InlineData("14", 165, 404)]
    [InlineData("15", 436, 175594)]
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
    [InlineData("07", 372, 8015)]
    [InlineData("08", 1782, 797)]
    [InlineData("09", 776203571, 104800569)]
    [InlineData("10", 1856, 2314037239808)]
    [InlineData("11", 2265, 2045)]
    [InlineData("12", 2297, 89984)]
    [InlineData("13", 205, 803025030761664)]
    [InlineData("14", 13727901897109, 5579916171823)]
    [InlineData("15", 211, 2159626)]
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
