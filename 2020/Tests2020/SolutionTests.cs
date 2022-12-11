namespace Tests2020;

public class SolutionTests
{
    private static readonly SolutionTester _runner = new(typeof(Puzzles2020.Solutions.Solution01).Assembly);

    [Theory]
    [InlineData("01", "514579", "241861950")]
    [InlineData("02", "2", "1")]
    [InlineData("03", "7", "336")]
    [InlineData("04", "2", "2")]
    //[InlineData("05", "", "")]
    [InlineData("06", "11", "6")]
    [InlineData("07", "4", "32")]
    [InlineData("08", "5", "8")]
    //[InlineData("09", "", "")]
    [InlineData("10", "220", "19208")]
    [InlineData("11", "37", "26")]
    [InlineData("12", "25", "286")]
    [InlineData("13", "295", "1068781")]
    [InlineData("14", "165", "404")]
    [InlineData("15", "436", "175594")]
    [InlineData("16", "71", "1")]
    [InlineData("17", "112", "848")]
    [InlineData("18", "71", "231")]
    //[InlineData("19", "", "")]// Different examples for Parts 1 and 2
    [InlineData("20", "20899048083289", "273")]
    [InlineData("21", "5", "mxmxvkd,sqjhc,fvjkl")]
    [InlineData("22", "306", "291")]
    [InlineData("23", "67384529", "149245887792")]
    [InlineData("24", "10", "2208")]
    [InlineData("25", "14897079", "0")]
    public async Task RunExample(string day, string part1, string part2)
    {
        await _runner.Day(day, "examples", part1, part2);
    }

    [Theory]
    [InlineData("01", "870331", "283025088")]
    [InlineData("02", "560", "303")]
    [InlineData("03", "223", "3517401300")]
    [InlineData("04", "202", "137")]
    [InlineData("05", "842", "617")]
    [InlineData("06", "6748", "3445")]
    [InlineData("07", "372", "8015")]
    [InlineData("08", "1782", "797")]
    [InlineData("09", "776203571", "104800569")]
    [InlineData("10", "1856", "2314037239808")]
    [InlineData("11", "2265", "2045")]
    [InlineData("12", "2297", "89984")]
    [InlineData("13", "205", "803025030761664")]
    [InlineData("14", "13727901897109", "5579916171823")]
    [InlineData("15", "211", "2159626")]
    [InlineData("16", "28882", "1429779530273")]
    [InlineData("17", "448", "2400")]
    [InlineData("18", "131076645626", "109418509151782")]
    [InlineData("19", "299", "414")]
    [InlineData("20", "7492183537913", "2323")]
    [InlineData("21", "2573", "bjpkhx,nsnqf,snhph,zmfqpn,qrbnjtj,dbhfd,thn,sthnsg")]
    [InlineData("22", "33694", "31835")]
    [InlineData("23", "25468379", "474747880250")]
    [InlineData("24", "549", "4147")]
    [InlineData("25", "9714832", "0")]
    public async Task RunInput(string day, string part1, string part2)
    {
        await _runner.Day(day, "inputs", part1, part2);
    }
}
