namespace Tests;

public class WorkInProgressTests
{
    [Fact]
    public async Task DailyExample()
    {
        await Helpers.RunDay("08", "examples", 0, 0);
    }

    [Fact]
    public async Task DailyInput()
    {
        await Helpers.RunDay("08", "inputs", 0, 0);
    }
}
