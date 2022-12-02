namespace PuzzlesTemplate.Solutions;

public record Solution14(string[] _lines) : ISolution<Solution14>
{
    public static Solution14 Init(string[] lines)
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
