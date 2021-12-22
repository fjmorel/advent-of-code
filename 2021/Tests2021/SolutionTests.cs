namespace Tests2021;

public class SolutionTests
{
    private static readonly SolutionTester _runner = new(typeof(Solution01).Assembly);

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
    [InlineData("14", 1588, 2188189693529)]
    [InlineData("15", 40, 315)]
    [InlineData("16", 6, 2021)]
    [InlineData("17", 45, 112)]
    [InlineData("18", 4140, 3993)]
    [InlineData("20", 35, 3351)]
    [InlineData("21", 739785, 444_356_092_776_315)]
    [InlineData("22", 474140, 2758514936282235)]
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
    [InlineData("14", 3143, 4110215602456)]
    [InlineData("15", 456, 2831)]
    [InlineData("16", 991, 1264485568252)]
    [InlineData("17", 6903, 2351)]
    [InlineData("18", 3305, 4563)]
    [InlineData("20", 5044, 18074)]
    [InlineData("21", 518418, 116741133558209)]
    [InlineData("22", 647076, 1233304599156793)]
    [InlineData("23", 0000, 0000)]
    [InlineData("24", 0000, 0000)]
    [InlineData("25", 0000, 0000)]
    public async Task RunInput(string day, long part1, long part2)
    {
        await _runner.RunDay(day, "inputs", part1, part2);
    }

    [Theory]
    [InlineData("examples", "19", 79, 3621)]
    [InlineData("inputs", "19", 398, 10965)]
    public async Task RunSlowDays(string folder, string day, long part1, long part2)
    {
        await _runner.RunDay(day, folder, part1, part2);
    }

    public static object[][] RunStrings_Data => new[]
    {
        new object[]
        {
            "examples",
            "13",
            "17",
            string.Join(Environment.NewLine, new string[]
            {
                "",
                "█████",
                "█   █",
                "█   █",
                "█   █",
                "█████",
                "",
            }),
        },
        new object[]
        {
            "inputs",
            "13",
            "720",
            string.Join(Environment.NewLine, new string[]
            {
                "",
                " ██  █  █ ███  ███  ███   ██  █  █ ████",
                "█  █ █  █ █  █ █  █ █  █ █  █ █  █    █",
                "█  █ ████ █  █ █  █ █  █ █  █ █  █   █ ",
                "████ █  █ ███  ███  ███  ████ █  █  █  ",
                "█  █ █  █ █    █ █  █    █  █ █  █ █   ",
                "█  █ █  █ █    █  █ █    █  █  ██  ████",
                "",
            }),
        },
    };

    [Theory, MemberData(nameof(RunStrings_Data))]
    public async Task RunStrings(string folder, string day, string part1, string part2)
    {
        await _runner.RunDay(day, folder, part1, part2);
    }
}
