using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

var nums = System.IO.File.ReadLines("example.txt").Select(x => int.Parse(x)).OrderBy(x => x).ToArray();
var max = nums.Max();
var minNeeded = max / 3;
var device = max + 3;

WriteLine(Part1(nums, device));

var numValid = 0;
Parallel.ForEach(Enumerable.Range(minNeeded, nums.Length), select =>
{
	var checks = nums.Select(x => false).ToArray();
	CheckCombos(ref nums, select, 0, 0, ref checks);
});
// foreach (var set in combs)
// {
// 	if (IsValid(set, device))
// 		numValid++;
// }
WriteLine(numValid);

void CheckCombos(ref int[] list, int request, int s, int currLen, ref bool[] check)
{
	if (currLen > request)
		return;

	if (currLen == request)
	{
		var previous = 0;
		for (int i = 0; i < list.Length; i++)
		{
			var cur = list[i];
			if (cur > previous + 3)
				break;

			if (check[i] == true)
				previous = cur;
		}
		if (device <= previous + 3)
			Interlocked.Increment(ref numValid);
		return;
	}

	if (s == list.Length)
		return;

	check[s] = true;
	CheckCombos(ref list, request, s + 1, currLen + 1, ref check);
	//recursively call Combi() with incremented value of ‘currLen’ and ‘s’.
	check[s] = false;
	CheckCombos(ref list, request, s + 1, currLen, ref check);
	// recursively call Combi() with only incremented value of ‘s’.
}

static int Part1(IEnumerable<int> items, int device)
{
	var list = items.Prepend(0).Append(device).OrderBy(x => x).ToList();
	var diffs = new List<int>();
	for (var i = 1; i < list.Count; i++)
	{
		diffs.Add(list[i] - list[i - 1]);
	}

	var dict = diffs.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
	return dict[1] * dict[3];
}
