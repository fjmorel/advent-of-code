namespace PuzzlesTemplate.Solutions;

public record Solution12(string[] _lines) : ISolution<Solution12>
{
    public static Solution12 Init(string[] lines)
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
