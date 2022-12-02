namespace Puzzles2019.Solutions;

public record Solution06(Dictionary<string, string> keyOrbitsValue) : ISolution<Solution06>
{
    private const string CENTER = "COM";

    public static Solution06 Init(string[] lines)
    {
        var keyOrbitsValue = lines.Aggregate(new Dictionary<string, string>(), (dict, line) =>
        {
            var paren = line.IndexOf(')');
            dict[line[(paren + 1)..]] = line[0..(paren)];
            return dict;
        });
        return new(keyOrbitsValue);
    }

    public async ValueTask<long> GetPart1()
    {
        return keyOrbitsValue.Keys.Select(key =>
        {
            long chain = 1;
            var center = keyOrbitsValue[key];
            while (center != CENTER)
            {
                center = keyOrbitsValue[center];
                chain++;
            }

            return chain;
        }).Sum();
    }

    public async ValueTask<long> GetPart2()
    {
        var you = GetChain("YOU").ToList();
        var santa = GetChain("SAN").ToList();
        var common = you.First(x => santa.Contains(x));

        return you.IndexOf(common) + santa.IndexOf(common);
    }

    private IEnumerable<string> GetChain(string start)
    {
        while (start != CENTER)
        {
            start = keyOrbitsValue[start];
            yield return start;
        }
    }
}
