using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

ConcurrentDictionary<string, Bag> rules = new();
var data = System.IO.File.ReadAllLines("input.txt").Select(x => x.Split(" bags contain "));
Parallel.ForEach(data, pieces =>
{
	rules[pieces[0]] = new Bag(pieces[0], ParseContents(pieces[1]));
});

Console.WriteLine(FindColorsContainingColor("shiny gold", new()).Count());
Console.WriteLine(CountContents("shiny gold", true));

HashSet<string> FindColorsContainingColor(string color, HashSet<string> matches)
{
	foreach (var result in rules.Where(x => x.Value.contents.Any(x => x.color == color)).Select(x => x.Key).Except(matches))
	{
		matches.Add(result);
		matches = FindColorsContainingColor(result, matches);
	}
	return matches;
}

int CountContents(string color, bool initial)
{
	return (initial ? 0 : 1) + rules[color].contents.Sum(sub => sub.count * CountContents(sub.color, false));
}

List<Content> ParseContents(string contents)
{
	var list = new List<Content>();
	if (contents != "no other bags.")
	{
		foreach (var piece in contents.Split(", "))
		{
			var num = int.Parse(piece.Substring(0, piece.IndexOf(' ')));
			var color = piece.Replace(" bags", "").Replace(" bag", "").Replace(".", "").Substring(piece.IndexOf(' ') + 1);
			list.Add(new Content(color, num));
		}
	}
	return list;
}

record Bag(string color, List<Content> contents);
record Content(string color, int count);
