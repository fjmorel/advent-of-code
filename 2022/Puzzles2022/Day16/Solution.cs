using System.Collections;

namespace Puzzles2022.Day16;

public partial record Solution(Solution.Valve[] _valves) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var parsed = lines.ToList(line =>
        {
            var match = GetLineRegex().Match(line);
            var groups = match.Groups;
            var name = groups[1].Value;
            var flowRate = int.Parse(groups[2].ValueSpan);
            var leadsTo = groups[3].Value.Split(", ");

            return (name, flowRate, leadsTo);
        });
        var leads = parsed.ToDictionary(x => x.name, x => x.leadsTo);
        var relevant = parsed.Where(x => x.flowRate != 0 || x.name == "AA").OrderBy(x => x.name).ToList();

        var distances = relevant.Select((valve, index) =>
        {
            var leadsTo = leads[valve.name].ToDictionary(name => name, _ => 1);
            var step = 1;
            while (leadsTo.Count < lines.Length)
            {
                step++;
                var nextLeads = leads.Where(x => leadsTo.ContainsKey(x.Key)).SelectMany(v => v.Value).Except(leadsTo.Keys).ToList();
                foreach (var lead in nextLeads)
                    leadsTo.TryAdd(lead, step);
            }

            return leadsTo.Where(x => relevant.Any(y => y.name == x.Key)).OrderBy(x => relevant.FindIndex(y => y.name == x.Key)).Select(x => x.Value).ToArray();
        }).ToArray();
        var valves = relevant.Zip(distances).Select(x => new Valve(x.First.flowRate, x.Second)).ToArray();
        return new(valves);
    }

    public async ValueTask<long> GetPart1()
    {
        var open = new BitArray(_valves.Length, false);
        return GetPossibilities(new(0, 0, 30, open)).Max(x => x.totalFlow);
    }

    public async ValueTask<long> GetPart2()
    {
        var open = new BitArray(_valves.Length, false);
        var p1 = GetPossibilities(new(0, 0, 26, open));
        var p2 = p1.SelectMany(x => GetPossibilities(x with { location = 0, minutesLeft = 26 }));

        return p2.Max(x => x.totalFlow);
    }

    private IEnumerable<State> GetPossibilities(State state)
    {
        yield return state;

        for (var valveIndex = 1; valveIndex < state.valves.Length; valveIndex++)
        {
            if (state.valves[valveIndex])
                continue;
            var newTimeLeft = state.minutesLeft - _valves[state.location].distances[valveIndex] - 1;
            if (newTimeLeft < 0)
                continue;
            var newOpen = new BitArray(state.valves) { [valveIndex] = true };
            var totalFlow = state.totalFlow + newTimeLeft * _valves[valveIndex].flowRate;
            var newState = new State(totalFlow, valveIndex, newTimeLeft, newOpen);
            if (newTimeLeft == 0)
                yield return newState;
            foreach (var possibility in GetPossibilities(newState))
                yield return possibility;
        }
    }

    public readonly record struct Valve(int flowRate, int[] distances);

    public readonly record struct State(long totalFlow, int location, int minutesLeft, BitArray valves);

    [GeneratedRegex("""Valve ([A-Z][A-Z]) has flow rate=([0-9]+); tunnels? leads? to valves? ([A-Z, ]+)""")]
    private static partial Regex GetLineRegex();
}
