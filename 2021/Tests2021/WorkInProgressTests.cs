namespace Tests2021;

public class WorkInProgressTests
{
    private const string _day = "12";

    [Fact]
    public async ValueTask DailyExample1()
    {
        await Helpers.RunPart1(_day, "examples", 0);
    }

    [Fact]
    public async ValueTask DailyExample2()
    {
        await Helpers.RunPart2(_day, "examples", 0);
    }

    [Fact]
    public async ValueTask DailyInput1()
    {
        await Helpers.RunPart1(_day, "inputs", 0);
    }

    [Fact]
    public async ValueTask DailyInput2()
    {
        await Helpers.RunPart2(_day, "inputs", 0);
    }
}
