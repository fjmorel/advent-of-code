using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

var rules = System.IO.File.ReadAllLines("input.txt").Select(ParseLine).ToDictionary(x => x.color, x => x.contents);

Console.WriteLine(FindColorsContainingColor("shiny gold", new()).Count());
Console.WriteLine(CountContents("shiny gold") - 1);

HashSet<string> FindColorsContainingColor(string color, HashSet<string> matches)
{
	var newMatches = rules.Where(x => x.Value.Any(x => x.Key == color)).Select(x => x.Key).Except(matches).ToHashSet();
	matches.UnionWith(newMatches);
	foreach (var match in newMatches)
		FindColorsContainingColor(match, matches);

	return matches;
}

int CountContents(string color) => 1 + rules[color].Sum(sub => sub.Value * CountContents(sub.Key));

(string color, Dictionary<string, int> contents) ParseLine(string line)
{
	var main = Regex.Match(line, @"(?<main>\w+ \w+) bags contain (?<contents>.+)");
	var primary = main.Groups["main"].Value;

	var contents = Regex.Matches(main.Groups["contents"].Value, @"(?<num>[0-9]+) (?<color>\w+ \w+)");
	var nums = contents.Select(x => int.Parse(x.Groups["num"].Value));
	var colors = contents.Select(x => x.Groups["color"].Value);
	return (primary, colors.Zip(nums).ToDictionary(x => x.First, x => x.Second));
}
