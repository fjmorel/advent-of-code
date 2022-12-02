namespace PuzzlesTemplate.Solutions;

public record Solution20(string[] _lines) : ISolution<Solution20>
{
    public static Solution20 Init(string[] lines)
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
