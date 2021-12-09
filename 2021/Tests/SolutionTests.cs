namespace Tests;

public class SolutionTests
{

    [Theory]
    [InlineData("01", 7L, 5L)]
    [InlineData("02", 150, 900)]
    [InlineData("03", 198, 230)]
    [InlineData("04", 4512, 1924)]
    [InlineData("05", 5, 12)]
    [InlineData("06", 5934, 26984457539)]
    [InlineData("07", 37, 168)]
    [InlineData("08", 26, 61229)]
    public async Task RunExample(string day, long part1, long part2)
    {
        await Helpers.RunDay(day, "examples", part1, part2);
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
    public async Task RunInput(string day, long part1, long part2)
    {
        await Helpers.RunDay(day, "inputs", part1, part2);
    }

}
