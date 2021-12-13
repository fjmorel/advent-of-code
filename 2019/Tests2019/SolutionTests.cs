namespace Tests2019;

public class SolutionTests
{
    private static readonly SolutionRunner _runner = new(typeof(Solution01).Assembly);

    [Theory]
    [InlineData("01", 33583, 50346)]
    //[InlineData("02", 0, 0)]// Intcode puzzle
    [InlineData("03", 135, 410)]
    //[InlineData("04", 0, 0)]// No example
    //[InlineData("05", 0, 0)]// Intcode puzzle
    [InlineData("06", 54, 4)]// Not 42 due to added lines from part 2 example
    //[InlineData("07", 0, 0)]// Intcode puzzle
    //[InlineData("08", 0, 0)]// No example (returns console output)
    //[InlineData("09", 0, 0)]// Intcode puzzle
    [InlineData("10", 210, 802)]
    //[InlineData("11", 0, 0)]// Intcode puzzle (returns console output)
    [InlineData("12", 14645, 4686774924)]
    //[InlineData("13", 0, 0)]// Intcode puzzle
    [InlineData("14", 2210736, 460664)]
    //[InlineData("15", 0, 0)]// Intcode puzzle
    [InlineData("16", 24465799, 84462026)]// Example data from part 2
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
        await _runner.RunDay(day, "examples", part1, part2);
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
    [InlineData("11", 2021, -1)]// LBJHEKLH
    [InlineData("12", 10028, 314610635824376)]
    [InlineData("13", 369, 19210)]
    [InlineData("14", 532506, 2595245)]
    //[InlineData("15", 0, 0)]// not done
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
        await _runner.RunDay(day, "inputs", part1, part2);
    }
}
