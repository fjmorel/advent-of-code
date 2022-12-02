namespace PuzzlesTemplate.Solutions;

public record Solution18(string[] _lines) : ISolution<Solution18>
{
    public static Solution18 Init(string[] lines)
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
