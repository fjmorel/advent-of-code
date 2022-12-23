namespace Puzzles2021.Day03;

public record Solution(string[] _data) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1()
	{
		var gamma = string.Join("", _data[0].Select((_, i) => GetSurpulusOnes(_data, i) >= 0 ? '1' : '0'));
		var epsilon = string.Join("", gamma.Select(x => x == '0' ? '1' : '0'));
		return ConvertNum(gamma) * ConvertNum(epsilon);
	}

	public async ValueTask<long> GetPart2()
	{
		var o2 = GetRating(_data, true);
		var co2 = GetRating(_data, false);
		return ConvertNum(o2) * ConvertNum(co2);
	}

    private string GetRating(string[] lines, bool useMost)
	{
		var i = 0;
		do
		{
			var oneCount = GetSurpulusOnes(lines, i);
			var digit = ((useMost && oneCount >= 0) || (!useMost && oneCount < 0)) ? '1' : '0';
			lines = lines.Where(x => x[i] == digit).ToArray();
		} while (lines.Length > 1 && ++i < lines[0].Length);
		return lines[0];
	}

    private long ConvertNum(string digits) => Convert.ToInt64(digits, 2);

    private long GetSurpulusOnes(string[] lines, int index) => lines.Aggregate(0, (sum, x) => x[index] == '1' ? ++sum : --sum);
}

