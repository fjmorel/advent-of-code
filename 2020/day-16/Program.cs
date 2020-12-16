using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var timer = Stopwatch.StartNew();

var rules = new List<Rule>();

var i = 0;
while (!string.IsNullOrEmpty(list[i]))
{
	var pieces = list[i].Split(": ");
	var nums = new HashSet<int>();
	var ranges = pieces[1].Split(" or ").Select(x => x.Split('-').Select(int.Parse).ToList());
	foreach (var range in ranges)
		nums.UnionWith(Enumerable.Range(range[0], range[1] - range[0] + 1));
	rules.Add(new Rule(pieces[0], nums));
	i++;
}
i += 2;// skip empty and "your ticket:"
var myTicket = list[i].Split(',').Select(int.Parse).ToList();

var invalid = new List<int>();
var valid = new List<List<int>>();
i += 3;// skip my ticket, empty, and "nearby tickets:"
foreach (var line in list.Skip(i))
{
	var nums = line.Split(',').Select(int.Parse).ToList();
	var isValid = true;
	foreach (var num in nums)
	{
		if (!rules.Any(x => x.allowed.Contains(num)))
		{
			invalid.Add(num);
			isValid = false;
			break;
		}
	}
	if (isValid)
		valid.Add(nums);
}

WriteLine($"{invalid.Sum()} :: {timer.Elapsed}");
timer.Restart();

var fieldsByIndex = new Dictionary<int, string>();
var unmatched = new List<Rule>(rules);
while (unmatched.Any())
{
	var copy = new List<Rule>(unmatched);
	unmatched.Clear();
	foreach (var rule in copy)
	{
		var unknown = Enumerable.Range(0, valid[0].Count).Where(x => !fieldsByIndex.ContainsKey(x)).ToList();
		var matches = unknown.Where(tIndex => valid.All(x => rule.allowed.Contains(x[tIndex]))).ToList();
		if (matches.Count() == 1)
			fieldsByIndex[matches[0]] = rule.name;
		else
			unmatched.Add(rule);
	}
}

var part2 = fieldsByIndex
	.Where(x => x.Value.StartsWith("departure ", StringComparison.Ordinal))
	.Select(x => myTicket[x.Key])
	.Aggregate(1L, (acc, value) => acc * value);

WriteLine($"{part2} :: {timer.Elapsed}");
timer.Stop();

record Rule(string name, HashSet<int> allowed);
