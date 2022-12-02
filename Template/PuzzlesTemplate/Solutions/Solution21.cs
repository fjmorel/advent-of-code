namespace PuzzlesTemplate.Solutions;

public record Solution21(string[] _lines) : ISolution<Solution21>
{
    public static Solution21 Init(string[] lines)
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
