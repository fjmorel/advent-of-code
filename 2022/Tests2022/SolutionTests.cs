namespace Tests2022;

public class SolutionTests
{
    private static readonly SolutionTester _tester = new(typeof(Puzzles2022.Day01.Solution).Assembly);

    [Theory]
    [InlineData("01", "24000", "45000")]
    [InlineData("02", "15", "12")]
    [InlineData("03", "157", "70")]
    [InlineData("04", "2", "4")]
    [InlineData("05", "CMZ", "MCD")]
    [InlineData("06", "7", "19")]
    [InlineData("07", "95437", "24933642")]
    [InlineData("08", "21", "4")]
    [InlineData("09", "88", "36")]
    [InlineData("10", "13140", """
        ⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛⬜⬜⬛⬛
        ⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛⬛⬛⬜⬜⬜⬛
        ⬜⬜⬜⬜⬛⬛⬛⬛⬜⬜⬜⬜⬛⬛⬛⬛⬜⬜⬜⬜⬛⬛⬛⬛⬜⬜⬜⬜⬛⬛⬛⬛⬜⬜⬜⬜⬛⬛⬛⬛
        ⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛
        ⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜
        ⬜⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛⬛⬛⬜⬜⬜⬜⬜⬜⬜⬛⬛⬛⬛⬛

        """
    )]
    [InlineData("11", "10605", "2713310158")]
    [InlineData("12", "31", "29")]
    [InlineData("13", "13", "140")]
    [InlineData("14", "24", "93")]
    // [InlineData("15", "26", "56000011")] (have to change code to make example work instead of input)
    [InlineData("16", "1651", "1707")]
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
    [InlineData("01", "69693", "200945")]
    [InlineData("02", "10718", "14652")]
    [InlineData("03", "7742", "2276")]
    [InlineData("04", "644", "926")]
    [InlineData("05", "QNHWJVJZW", "BPCZJLFJW")]
    [InlineData("06", "1100", "2421")]
    [InlineData("07", "1334506", "7421137")]
    [InlineData("08", "1859", "332640")]
    [InlineData("09", "6026", "2273")]
    [InlineData("10", "15260", "PGHFGLUG")]
    [InlineData("11", "78678", "15333249714")]
    [InlineData("12", "490", "488")]
    [InlineData("13", "5825", "24477")]
    [InlineData("14", "843", "27625")]
    [InlineData("15", "4560025", "12480406634249")]
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

    [Trait("Category", "Slow")]
    [Theory]
    [InlineData("inputs", "16", "1720", "0")]
    public Task RunSlowDays(string folder, string day, string part1, string part2) => _tester.Day(day, folder, part1, part2);

}
