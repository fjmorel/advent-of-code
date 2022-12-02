namespace PuzzlesTemplate.Solutions;

public record Solution17(string[] _lines) : ISolution<Solution17>
{
    public static Solution17 Init(string[] lines)
    {
        return new(lines);
    }

	public async ValueTask<long> GetPart1()
	{
		return 0;
	}

	public async ValueTask<long> GetPart2()
	{
		return 0;
	}

}
