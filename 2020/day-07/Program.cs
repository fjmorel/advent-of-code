using System;
using System.Collections.Generic;
using System.Linq;

Dictionary<string, BagColor> rules = new();

var data = System.IO.File.ReadAllLines("input.txt").Select(x => x.Split(" contain "));
foreach (var group in data)
{
	var primary = group[0].Substring(0, group[0].IndexOf(" bags"));
	Console.WriteLine(primary);
	var contents = group[1] switch
	{
		"no other bags." => new List<BagColor>(),
		string s => ParseContents(s),
	};
	rules[primary] = new BagColor(primary, 0, contents);
	Console.WriteLine(rules[primary]);
}

var searched = new HashSet<string>();
var matches = new HashSet<string>();

FindColorsContainingColor("shiny gold");
Console.WriteLine(matches.Count());
Console.WriteLine(CalculateContents("shiny gold") - 1);

void FindColorsContainingColor(string color)
{
	foreach (var result in rules.Where(x => x.Value.contents.Any(x => x.color == color)).Select(x => x.Key))
	{
		matches.Add(result);
		if (!searched.Contains(result))
		{
			FindColorsContainingColor(result);
		}
	}
}

int CalculateContents(string color)
{
	var rule = rules[color];
	var sum = 1;
	foreach (var sub in rule.contents)
	{
		sum += sub.count * CalculateContents(sub.color);
	}
	return sum;
}

List<BagColor> ParseContents(string contents)
{
	var list = new List<BagColor>();
	var pieces = contents.Split(", ");
	foreach (var bag in pieces)
	{
		var color = bag.Replace(" bags", "").Replace(" bag", "").Replace(".", "");
		var num = int.Parse(color.Substring(0, color.IndexOf(' ')));
		color = color.Substring(color.IndexOf(' ') + 1);
		var subrule = new BagColor(color, num, new List<BagColor>());
		Console.WriteLine(subrule);
		list.Add(subrule);
	}

	return list;
}

record BagColor(string color, int count, List<BagColor> contents);
