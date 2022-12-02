namespace Puzzles2022.Solutions;

public record Solution13(string[] _lines) : ISolution<Solution13>
{
    public static Solution13 Init(string[] lines)
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
