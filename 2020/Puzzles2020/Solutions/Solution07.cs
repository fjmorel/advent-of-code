namespace Puzzles2020.Solutions;

public class Solution07 : ISolution
{
    private readonly Dictionary<string, Dictionary<string, int>> _rules;

    public Solution07(string[] lines)
    {
        _rules = lines.Select(ParseLine).ToDictionary(x => x.color, x => x.contents);
    }

    public async ValueTask<long> GetPart1() => FindColorsContainingColor("shiny gold", new()).Count;

    public async ValueTask<long> GetPart2() => CountContents("shiny gold") - 1;

    private HashSet<string> FindColorsContainingColor(string color, HashSet<string> matches)
    {
        var newMatches = _rules.Where(x => x.Value.Any(y => y.Key == color)).Select(x => x.Key).Except(matches).ToHashSet();
        matches.UnionWith(newMatches);
        foreach (var match in newMatches)
            FindColorsContainingColor(match, matches);

        return matches;
    }

    private int CountContents(string color) => 1 + _rules[color].Sum(sub => sub.Value * CountContents(sub.Key));

    private static (string color, Dictionary<string, int> contents) ParseLine(string line)
    {
        var main = Regex.Match(line, @"(?<main>\w+ \w+) bags contain (?<contents>.+)");
        var primary = main.Groups["main"].Value;

        var contents = Regex.Matches(main.Groups["contents"].Value, @"(?<num>[0-9]+) (?<color>\w+ \w+)");
        var nums = contents.Select(x => int.Parse(x.Groups["num"].Value));
        var colors = contents.Select(x => x.Groups["color"].Value);
        return (primary, colors.Zip(nums).ToDictionary(x => x.First, x => x.Second));
    }
}
