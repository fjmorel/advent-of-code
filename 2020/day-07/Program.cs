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
	var firstMatch = Regex.Match(line, @"(\w+ \w+) bags contain (.+)");
	var color = firstMatch.Groups[1].Value;

	var contents = firstMatch.Groups[2].Value;
	var extract = Regex.Matches(contents, @"(?<none>no other bags)|((?<num>[0-9]+) (?<color>\w+ \w+)) bag");
	var rules = new Dictionary<string, int>();
	if (!extract.Any(x => x.Groups["none"].Success))
	{
		foreach (Match match in extract)
		{
			var color2 = match.Groups["color"].Value;
			var num = match.Groups["num"].Value;
			rules[color2] = int.Parse(num);
		}
	}
	return (color, rules);
}
