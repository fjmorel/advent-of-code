using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File
	.ReadAllLines("input.txt")[0]
	.Split(',')
	.Select((x, i) => (num: int.Parse(x), i))
	.ToList();

var timer = Stopwatch.StartNew();
WriteLine($"{FindNth(2020)} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{FindNth(30_000_000)} :: {timer.Elapsed}");
timer.Stop();

int FindNth(int limit)
{
	var dict = list.SkipLast(1).ToDictionary(x => x.num, x => x.i + 1);
	var last = list.Last().num;
	for (var i = list.Count(); i < limit; i++)
	{
		var pos = dict.GetValueOrDefault(last, i);
		dict[last] = i;
		last = i - pos;
	}
	return last;
}