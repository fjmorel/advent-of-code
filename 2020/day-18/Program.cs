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
	return list.Sum(CalcLine);
}

long Part2()
{
	return list.Sum(CalcLine2);
}

long CalcLine(string line)
{
	var withIndex = line.Select((ch, i) => (ch, i)).ToArray();
	var opens = withIndex.Where(x => x.ch == '(').Select(x => x.i).Reverse();
	var closes = withIndex.Where(x => x.ch == ')').Select(x => x.i).ToList();
	var groups = new List<Range>();
	foreach (var open in opens)
	{
		var close = closes.Where(x => x > open).First();
		groups.Add(new Range(open, close));
		closes.Remove(close);
	}
	if (groups.Any())
	{
		var range = groups.OrderBy(x => x.End.Value - x.Start.Value).First();
		return CalcLine(line.Substring(0, range.Start.Value) + NoPrecedence(line[(range.Start.Value + 1)..range.End]) + line.Substring(range.End.Value + 1));
	}
	return NoPrecedence(line);
}

long NoPrecedence(string line)
{
	var items = line.Split(' ').ToList();
	var num = long.Parse(items[0]);
	for (var i = 2; i < items.Count; i += 2)
	{
		var op = items[i - 1];
		var newNum = long.Parse(items[i]);
		num = op == "*" ? num * newNum : num + newNum;
	}
	return num;
}

long CalcLine2(string line)
{
	var withIndex = line.Select((ch, i) => (ch, i)).ToArray();
	var opens = withIndex.Where(x => x.ch == '(').Select(x => x.i).Reverse();
	var closes = withIndex.Where(x => x.ch == ')').Select(x => x.i).ToList();
	var groups = new List<Range>();
	foreach (var open in opens)
	{
		var close = closes.Where(x => x > open).First();
		groups.Add(new Range(open, close));
		closes.Remove(close);
	}
	if (groups.Any())
	{
		var range = groups.OrderBy(x => x.End.Value - x.Start.Value).First();
		return CalcLine2(line.Substring(0, range.Start.Value) + AddFirst(line[(range.Start.Value + 1)..range.End]) + line.Substring(range.End.Value + 1));
	}
	return AddFirst(line);
}

long AddFirst(string line) => line.Split(" * ").Select(AddPieces).Aggregate(1L, (acc, value) => acc * value);
long AddPieces(string line) => line.Split(' ').Where(x => x != "+").Sum(long.Parse);