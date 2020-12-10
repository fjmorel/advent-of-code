using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static System.Console;

var nums = System.IO.File.ReadLines("input.txt").Select(x => int.Parse(x)).OrderBy(x => x).ToArray();
var device = nums.Max() + 3;

WriteLine(Part1(nums, device));

var groups = GroupBy3(nums);

var countPerGroup = new List<int>();
foreach (var group in groups)
{
	WriteLine(string.Join(',', group.Select(x => x.ToString())));
	var max = group.Last();
	var min = group.First();
	var middle = group.Skip(1).SkipLast(1).ToArray();
	var numValid = 1;
	for (var select = 0; select < middle.Length; select++)
	{
		var checks = middle.Select(x => false).ToArray();
		CheckCombos(ref middle, select, 0, 0, ref checks, min, max, ref numValid);
	}
	WriteLine(numValid);
	countPerGroup.Add(numValid);
}

var total = countPerGroup.Aggregate((long)1, (acc, value) => acc * value);
// foreach (var set in combs)
// {
// 	if (IsValid(set, device))
// 		numValid++;
// }
WriteLine(total);

void CheckCombos(ref int[] list, int request, int s, int currLen, ref bool[] check, int min, int max, ref int numValid)
{
	if (currLen > request)
		return;

	if (currLen == request)
	{
		var previous = min;
		for (int i = 0; i < list.Length; i++)
		{
			var cur = list[i];
			if (cur > previous + 3)
				break;

			if (check[i] == true)
				previous = cur;
		}
		if (max <= previous + 3)
			numValid++;
		return;
	}

	if (s == list.Length)
		return;

	check[s] = true;
	CheckCombos(ref list, request, s + 1, currLen + 1, ref check, min, max, ref numValid);
	//recursively call Combi() with incremented value of ‘currLen’ and ‘s’.
	check[s] = false;
	CheckCombos(ref list, request, s + 1, currLen, ref check, min, max, ref numValid);
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

static List<List<int>> GroupBy3(IList<int> list)
{
	var items = list.Prepend(0).ToArray();
	var all = new List<List<int>>();

	var cur = new List<int>() { items.First() };
	for (var i = 1; i < items.Count(); i++)
	{
		if (items[i] - items[i - 1] == 3)
		{
			all.Add(cur);
			cur = new List<int>();
		}
		cur.Add(items[i]);
	}
	all.Add(cur);

	return all;
}