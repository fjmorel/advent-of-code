using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static System.Console;

#nullable enable

var list = System.IO.File.ReadAllLines("input.txt");
var i = 0;
var rules = new Dictionary<int, Rule>();
for (; list[i] != ""; i++)
{
	var pieces = list[i].Split(": ");
	var index = int.Parse(pieces[0]);
	if (pieces[1][0] == '"')
		rules[index] = new Rule(pieces[1][1], null);
	else
		rules[index] = new Rule(null, pieces[1].Split(" | ").Select(x => x.Split(" ").Select(int.Parse).ToList()).ToList());
}
i++;

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	//var potentials = GeneratePossibilities(rules, rules[0]);
	return list.Skip(i)
		.Select(line => (line.Length, result: IsValid(rules, line, rules[0])))
		.Count(x => x.result.valid && x.result.handled == x.Length);
	// if (potentials.Contains(line) || potentials.Any(x => x.StartsWith(line)))
	// 	valid++;
}

long Part2()
{
	// 8: 42 | 42 8
	// 11: 42 31 | 42 11 31
	rules[8] = new Rule(null, new() { new() { 42 }, new() { 42, 8 } });
	rules[11] = new Rule(null, new() { new() { 42, 31 }, new() { 42, 11, 31 } });

	var potential42s = GeneratePossibilities(rules, rules[42]);
	var potential31s = GeneratePossibilities(rules, rules[31]);

	var valid = 0;
	foreach (var line in list.Skip(i))
	{
		int num42 = 0, num31 = 0;
		var sub = line.AsSpan();
		while (!sub.IsEmpty)
		{
			var found = false;
			foreach (var fortyTwo in potential42s)
			{
				if (sub.StartsWith(fortyTwo))
				{
					sub = sub[fortyTwo.Length..];
					found = true;
					num42++;
					break;
				}
			}
			if (!found)
				foreach (var thirtyOne in potential31s)
				{
					if (sub.EndsWith(thirtyOne))
					{
						sub = sub[0..^thirtyOne.Length];
						found = true;
						num31++;
						break;
					}
				}
			if (!found)
				break;
		}
		var isValid = sub.IsEmpty && num42 > num31 && num31 > 0;
		if (isValid)
			valid++;
	}
	return valid;
}

(bool valid, int handled) IsValid(Dictionary<int, Rule> rules, ReadOnlySpan<char> substring, Rule rule)
{
	if (substring.IsEmpty)
		return (true, 0);

	if (rule.letter.HasValue)
		return (rule.letter == substring[0], 1);

	foreach (var alternative in rule.options ?? Enumerable.Empty<List<int>>())
	{
		var isValid = true;
		var counted = 0;
		foreach (var subrule in alternative.Select(x => rules[x]))
		{
			var (valid, handled) = IsValid(rules, substring[counted..], subrule);
			if (!valid)
			{
				isValid = false;
				break;
			}
			counted += handled;
		}
		if (isValid)
		{
			return (true, counted);
		}
	}
	return (false, 0);
}

HashSet<string> GeneratePossibilities(Dictionary<int, Rule> rules, Rule rule)
{
	if (rule.letter.HasValue)
		return new() { rule.letter.Value.ToString() };

	var list = new HashSet<string>() { };
	foreach (var alternate in rule.options ?? Enumerable.Empty<List<int>>())
	{
		var sublist = new HashSet<string>() { "" };
		foreach (var subrule in alternate.Select(x => rules[x]))
		{
			var subtrees = GeneratePossibilities(rules, subrule);
			sublist = sublist.SelectMany(x => subtrees.Select(y => x + y)).ToHashSet();
		}
		list.UnionWith(sublist);
	}
	return list;
}

record Rule(char? letter, List<List<int>>? options);
