namespace Puzzles2020.Solutions;

public class Solution10 : ISolution
{
    private readonly List<long> nums;
    private readonly long device;

    public Solution10(string[] lines)
    {
        nums = lines.ParseLongs();
        nums.Sort();
        device = nums.Last() + 3;
    }

    public async ValueTask<long> GetPart1()
    {
        var list = nums.Prepend(0).Append(device).OrderBy(x => x).ToList();
        var diffs = new List<long>();
        for (var i = 1; i < list.Count; i++)
        {
            diffs.Add(list[i] - list[i - 1]);
        }

        var dict = diffs.GroupBy(x => x).ToDictionary(x => x.Key, x => x.Count());
        return dict[1] * dict[3];
    }

    public async ValueTask<long> GetPart2() => GroupBy3(nums)
        .Select(GetValidCombosForGroup)
        .Aggregate((long)1, (acc, value) => acc * value);

    private static long GetValidCombosForGroup(List<long> group)
    {
        var max = group.Last();
        var min = group.First();
        var middle = group.Skip(1).SkipLast(1).ToArray();
        var numValid = 1L;
        for (var select = 0; select < middle.Length; select++)
        {
            var checks = middle.Select(_ => false).ToArray();
            CheckCombos(ref middle, select, 0, 0, ref checks, min, max, ref numValid);
        }

        return numValid;
    }

    private static void CheckCombos(ref long[] list, long request, long s, long currLen, ref bool[] check, long min, long max, ref long numValid)
    {
        if (currLen > request)
            return;

        if (currLen == request)
        {
            var previous = min;
            for (long i = 0; i < list.Length; i++)
            {
                var cur = list[i];
                if (cur > previous + 3)
                    break;

                if (check[i])
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

    private static IEnumerable<List<long>> GroupBy3(IEnumerable<long> items)
    {
        var list = items.Prepend(0).ToList();// Make sure to include 0 in first group

        var cur = new List<long>() { list.First() };
        for (var i = 1; i < list.Count; i++)
        {
            if (list[i] - list[i - 1] == 3)
            {
                yield return cur;
                cur = new List<long>();
            }

            cur.Add(list[i]);
        }

        yield return cur;
    }
}
