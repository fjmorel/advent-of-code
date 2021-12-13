namespace Tests2021;

public class SolutionTests
{
    private static readonly SolutionRunner _runner = new(typeof(Solution01).Assembly);

    [Theory]
    [InlineData("01", 7L, 5L)]
    [InlineData("02", 150, 900)]
    [InlineData("03", 198, 230)]
    [InlineData("04", 4512, 1924)]
    [InlineData("05", 5, 12)]
    [InlineData("06", 5934, 26984457539)]
    [InlineData("07", 37, 168)]
    [InlineData("08", 26, 61229)]
    [InlineData("09", 15, 1134)]
    [InlineData("10", 26397, 288957)]
    [InlineData("11", 1656, 195)]
    [InlineData("12", 19, 103)]
    [InlineData("13", 17, -1)]
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
    [InlineData("01", 1374L, 1418L)]
    [InlineData("02", 1561344, 1848454425)]
    [InlineData("03", 2743844, 6677951)]
    [InlineData("04", 39902, 26936)]
    [InlineData("05", 5084, 17882)]
    [InlineData("06", 383160, 1721148811504)]
    [InlineData("07", 329389, 86397080)]
    [InlineData("08", 512, 1091165)]
    [InlineData("09", 452, 1263735)]
    [InlineData("10", 299793, 3654963618)]
    [InlineData("11", 1665, 235)]
    [InlineData("12", 5874, 153592)]
    [InlineData("13", 720, -1)]// Console output: AHPRPAUZ
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
