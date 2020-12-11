using System.Collections.Generic;
using System.Linq;
using static System.Console;

var nums = System.IO.File.ReadLines("input.txt").Select(x => int.Parse(x)).OrderBy(x => x).ToArray();
var device = nums.Last() + 3;

WriteLine(Part1(nums, device));
WriteLine(GetCombinationTotal(nums));

static long GetCombinationTotal(IEnumerable<int> nums) => GroupBy3(nums)
		.Select(GetValidCombosForGroup)
		.Aggregate((long)1, (acc, value) => acc * value);

static int GetValidCombosForGroup(List<int> group)
{
	var max = group.Last();
	var min = group.First();
	var middle = group.Skip(1).SkipLast(1).ToArray();
	var numValid = 1;
	for (var select = 0; select < middle.Length; select++)
	{
		var checks = middle.Select(x => false).ToArray();
		CheckCombos(ref middle, select, 0, 0, ref checks, min, max, ref numValid);
	}
	return numValid;
}

static void CheckCombos(ref int[] list, int request, int s, int currLen, ref bool[] check, int min, int max, ref int numValid)
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

static List<List<int>> GroupBy3(IEnumerable<int> items)
{
	var list = items.Prepend(0).ToList();// Make sure to include 0 in first group
	var groups = new List<List<int>>();

	var cur = new List<int>() { list.First() };
	for (var i = 1; i < list.Count(); i++)
	{
		if (list[i] - list[i - 1] == 3)
		{
			groups.Add(cur);
			cur = new List<int>();
		}
		cur.Add(list[i]);
	}
	groups.Add(cur);

	return groups;
}