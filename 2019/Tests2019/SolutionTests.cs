namespace Tests2019;

public class SolutionTests
{

    [Theory]
    [InlineData("01", 3364035, 5043167)]
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
    [InlineData("12", 0, 0)]
    [InlineData("13", 0, 0)]
    [InlineData("14", 0, 0)]
    [InlineData("15", 0, 0)]
    [InlineData("16", 0, 0)]
    [InlineData("17", 0, 0)]
    [InlineData("18", 0, 0)]
    [InlineData("19", 0, 0)]
    [InlineData("20", 0, 0)]
    [InlineData("21", 0, 0)]
    [InlineData("22", 0, 0)]
    [InlineData("23", 0, 0)]
    [InlineData("24", 0, 0)]
    [InlineData("25", 0, 0)]
    public async Task RunExample(string day, long part1, long part2)
    {
        await Helpers.RunDay(day, "examples", part1, part2);
    }

    [Theory]
    [InlineData("01", 3364035, 5043167)]
    [InlineData("02", 3101878, 8444)]
    [InlineData("03", 375, 14746)]
    [InlineData("04", 1729, 1172)]
    [InlineData("05", 16225258, 2808771)]
    [InlineData("06", 147223, 340)]
    [InlineData("07", 422858, 14897241)]
    [InlineData("08", 1703, -1)]// HCGFE
    [InlineData("09", 3780860499, 33343)]
    [InlineData("10", 326, 1623)]
    [InlineData("11", 2021, 0)]// LBJHEKLH
    [InlineData("12", 10028, 314610635824376)]
    [InlineData("13", 369, 19210)]
    [InlineData("14", 532506, 2595245)]
    [InlineData("15", 0, 0)]
    [InlineData("16", 40921727, 89950138)]
    [InlineData("17", 0, 0)]
    [InlineData("18", 0, 0)]
    [InlineData("19", 0, 0)]
    [InlineData("20", 0, 0)]
    [InlineData("21", 0, 0)]
    [InlineData("22", 0, 0)]
    [InlineData("23", 0, 0)]
    [InlineData("24", 0, 0)]
    [InlineData("25", 0, 0)]
    public async Task RunInput(string day, long part1, long part2)
    {
        await Helpers.RunDay(day, "inputs", part1, part2);
    }

}
