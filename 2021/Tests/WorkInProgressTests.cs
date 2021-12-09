namespace Tests;

public class WorkInProgressTests
{
	private const string _day = "09";

	[Fact]
	public async Task DailyExample()
	{
		await Helpers.RunDay(_day, "examples", 0, 0);
	}

	[Fact]
	public async Task DailyInput()
	{
		await Helpers.RunDay(_day, "inputs", 0, 0);
	}
}
