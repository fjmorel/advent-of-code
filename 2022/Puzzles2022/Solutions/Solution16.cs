namespace Puzzles2022.Solutions;

public record Solution16(string[] _lines) : ISolution<Solution16>
{
    public static Solution16 Init(string[] lines)
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
