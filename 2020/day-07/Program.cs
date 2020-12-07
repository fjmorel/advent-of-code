using System;
using System.Collections.Generic;
using System.Linq;

Dictionary<string, Dictionary<string, int>> rules = new();
var data = System.IO.File.ReadAllLines("input.txt").Select(x => x.Split(" bags contain "));
foreach (var pieces in data)
	rules[pieces[0]] = ParseContents(pieces[1]);

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
