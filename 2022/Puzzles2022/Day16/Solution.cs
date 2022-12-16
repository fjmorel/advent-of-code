using System.Collections;

namespace Puzzles2022.Day16;

public partial record Solution(int[] _sortedRates, Dictionary<int, int>[] _sortedDistances) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var totalCount = lines.Length;

        var valves = lines.ToList(line =>
        {
            var match = GetLineRegex().Match(line);
            var groups = match.Groups;
            var name = groups[1].Value;
            var flowRate = int.Parse(groups[2].ValueSpan);
            var leadsTo = groups[3].Value.Split(", ");

            return (name, flowRate, leadsTo);
        });
        var relevant = valves.Where(x => x.flowRate != 0 || x.name == "AA").OrderBy(x => x.name).ToList();

        var rates = relevant.Select(x => x.flowRate).ToArray();

        var distances = relevant.Select((valve, index) =>
        {
            var step = 1;
            var leadsTo = valve.leadsTo.ToDictionary(name => name, _ => step);
            while (leadsTo.Count < totalCount)
            {
                step++;
                var nextLeads = valves.Where(v => leadsTo.ContainsKey(v.name)).SelectMany(v => v.leadsTo).Except(leadsTo.Keys).ToList();
                foreach (var lead in nextLeads)
                    leadsTo.TryAdd(lead, step);
            }

            return leadsTo.Where(x => relevant.Any(y => y.name == x.Key)).ToDictionary(x => relevant.FindIndex(y => y.name == x.Key), x => x.Value);
        }).ToArray();

        return new(rates, distances);
    }

    public async ValueTask<long> GetPart1()
    {
        var beings = ImmutableArray.Create(new Being(0, 30));
        var open = new BitArray(_sortedRates.Length, false);
        return GetMax(open, 0, beings);
    }

    public async ValueTask<long> GetPart2()
    {
        var beings = ImmutableArray.Create(new Being(0, 26), new Being(0, 26));
        var open = new BitArray(_sortedRates.Length, false);
        return GetMax(open, 0, beings);
    }

    private int GetMax(BitArray open, int pastFlow, ImmutableArray<Being> beings)
    {
        var max = pastFlow;
        for (var beingIndex = 0; beingIndex < beings.Length; beingIndex++)
        {
            var being = beings[beingIndex];
            if (being.done)
                continue;

            for (var valveIndex = 1; valveIndex < open.Length; valveIndex++)
            {
                if (open[valveIndex])
                    continue;
                var newStep = being.timeLeft - _sortedDistances[being.valveIndex][valveIndex] - 1;
                // if this one is out of time, left the other(s) keep opening valves
                if (newStep < 0)
                {
                    var newBeings = beings.SetItem(beingIndex, being with { done = true });
                    max = int.Max(max, GetMax(open, pastFlow, newBeings));
                }
                else
                {
                    var newOpen = new BitArray(open) { [valveIndex] = true };
                    var newFlow = pastFlow + newStep * _sortedRates[valveIndex];
                    var newBeings = beings.SetItem(beingIndex, new(valveIndex, newStep));
                    max = int.Max(max, GetMax(newOpen, newFlow, newBeings));
                }
            }
        }

        return max;
    }

    public readonly record struct Being(int valveIndex, int timeLeft, bool done = false);

    [GeneratedRegex("""Valve ([A-Z][A-Z]) has flow rate=([0-9]+); tunnels? leads? to valves? ([A-Z, ]+)""")]
    private static partial Regex GetLineRegex();
}
