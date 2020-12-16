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
	var ranges = pieces[1].Split(" or ").Select(x => x.Split('-').Select(y => int.Parse(y)).ToList());
	foreach (var range in ranges)
		nums.UnionWith(Enumerable.Range(range[0], range[1] - range[0] + 1));
	rules.Add(new Rule(pieces[0], nums));
	i++;
}
i += 2;
var myTicket = list[i].Split(',').Select(x => int.Parse(x)).ToList();

var invalid = new List<int>();
var valid = new List<List<int>>();
i += 3;
for (; i < list.Length; i++)
{
	var nums = list[i].Split(',').Select(x => int.Parse(x)).ToList();
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


var dict = new Dictionary<int, Rule>();
var multipleRuleIndexes = new List<int>();

for (var r = 0; r < rules.Count; r++)
{
	FindMatchForRule(r);
}

while (multipleRuleIndexes.Any())
{
	var copy = new List<int>(multipleRuleIndexes);
	multipleRuleIndexes.Clear();
	foreach (var r in copy)
		FindMatchForRule(r);
}

if (multipleRuleIndexes.Any()) throw new Exception("multiple again");

var part2 = dict
	.Where(x => x.Value.name.StartsWith("departure "))
	.Select(x => x.Key)
	.Select(x => myTicket[x])
	.Aggregate(1L, (acc, value) => acc * value);

WriteLine($"{part2} :: {timer.Elapsed}");
timer.Stop();

void FindMatchForRule(int r)
{
	var rule = rules[r];
	var matches = new List<int>();
	for (var tIndex = 0; tIndex < valid[0].Count; tIndex++)
	{
		if (dict.ContainsKey(tIndex))
			continue;
		var isMatch = valid.All(x => rule.allowed.Contains(x[tIndex]));
		if (isMatch)
			matches.Add(tIndex);
	}
	if (matches.Count == 1)
		dict[matches[0]] = rule;
	else
		multipleRuleIndexes.Add(r);
}

record Rule(string name, HashSet<int> allowed);
