namespace Puzzles2022.Solutions;

public record Solution11(string[] _lines) : ISolution<Solution11>
{
    public static Solution11 Init(string[] lines)
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
