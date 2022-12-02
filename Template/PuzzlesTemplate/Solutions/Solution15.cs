namespace PuzzlesTemplate.Solutions;

public record Solution15(string[] _lines) : ISolution<Solution15>
{
    public static Solution15 Init(string[] lines)
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
