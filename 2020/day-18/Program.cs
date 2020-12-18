using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return list.Sum(x => CalcLine(x, NoPrecedence));
}

long Part2()
{
	return list.Sum(x => CalcLine(x, AddFirst));
}

long CalcLine(string line, Func<string, long> calculator)
{
	var open = line.LastIndexOf('(');
	if (open > -1)
	{
		var close = line.IndexOf(')', open + 1);
		return CalcLine(line[0..open] + calculator(line[(open + 1)..close]) + line[(close + 1)..], calculator);
	}
	return calculator(line);
}

long NoPrecedence(string line)
{
	var items = line.Split(' ').ToList();
	var num = long.Parse(items[0]);
	for (var i = 2; i < items.Count; i += 2)
	{
		var newNum = long.Parse(items[i]);
		num = items[i - 1] == "*" ? num * newNum : num + newNum;
	}
	return num;
}

long AddFirst(string line) => line.Split(" * ")
	.Select(sub => sub.Split(" + ").Sum(long.Parse))
	.Aggregate(1L, (acc, value) => acc * value);
