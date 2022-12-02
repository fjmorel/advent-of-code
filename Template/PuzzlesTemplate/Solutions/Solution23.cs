namespace PuzzlesTemplate.Solutions;

public record Solution23(string[] _lines) : ISolution<Solution23>
{
    public static Solution23 Init(string[] lines)
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
