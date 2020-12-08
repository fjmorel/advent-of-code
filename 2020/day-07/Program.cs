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

(string color, Dictionary<string, int> contents) ParseLine(string line){
	var match = new Regex(@"(\w+ \w+) bags contain (.+)").Match(line);
	var color = match.Groups[1].Value;
	var contents = ParseContents(match.Groups[2].Value);
	return (color, contents);
}

Dictionary<string, int> ParseContents(string rule)
{
	Dictionary<string, int> contents = new();
	if (rule != "no other bags.")
	{
		foreach (var piece in rule.Split(", "))
		{
			var color = piece.Replace(" bags", "").Replace(" bag", "").Replace(".", "").Substring(piece.IndexOf(' ') + 1);
			contents[color] = int.Parse(piece.Substring(0, piece.IndexOf(' ')));
		}
	}
	return contents;
}
