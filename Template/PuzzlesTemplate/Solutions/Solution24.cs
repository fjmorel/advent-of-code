namespace PuzzlesTemplate.Solutions;

public record Solution24(string[] _lines) : ISolution<Solution24>
{
    public static Solution24 Init(string[] lines)
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
