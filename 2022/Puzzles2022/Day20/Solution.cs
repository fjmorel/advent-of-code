namespace Puzzles2022.Day20;

public record Solution(List<int> _numbers) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines.ParsePerLine<int>());

    public async ValueTask<string> GetPart1String()
    {
        var original = _numbers.Select((value, index) => new Number(value, index)).ToImmutableArray();
        var data = Decrypt(original, [..original]);
        return GetOutput(data).ToString();
    }

    public async ValueTask<string> GetPart2String()
    {
        var original = _numbers.Select((value, index) => new Number(value * (Int128)811589153, index)).ToImmutableArray();
        var data = Enumerable.Range(1, 10).Aggregate(new List<Number>(original), (data, _) => Decrypt(original, data));
        return GetOutput(data).ToString();
    }

    private static List<Number> Decrypt(ImmutableArray<Number> original, List<Number> data)
    {
        var size = original.Length;

        foreach (var number in original)
        {
            var index = data.IndexOf(number);
            var newIndex = index + (int)(number.value % (size - 1));
            while (newIndex < 0)
                newIndex += size - 1;
            newIndex %= size - 1;

            data.RemoveAt(index);
            data.Insert(newIndex, number);
        }

        return data;
    }

    private static Int128 GetOutput(List<Number> data)
    {
        var zero = data.FindIndex(x => x.value == 0);
        return GetValueAt(data, zero + 1000) + GetValueAt(data, zero + 2000) + GetValueAt(data, zero + 3000);
    }

    private static Int128 GetValueAt(List<Number> list, int index) => list[index % list.Count].value;

    private readonly record struct Number(Int128 value, int originalIndex);
}
