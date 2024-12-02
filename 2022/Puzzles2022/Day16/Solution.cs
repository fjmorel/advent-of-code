using System.Collections;

namespace Puzzles2022.Day16;

public partial record Solution(Solution.Valve[] _valves) : ISolution<Solution>
{
    public static Solution Init(string[] lines)
    {
        var parsed = lines.ToList(line =>
        {
            var match = LineRegex.Match(line);
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

    public async ValueTask<long> GetPart1() =>
        GetPossibilities(new(0, 0, 30, new BitArray(_valves.Length)))
            .Max(x => x.totalFlow);

    public async ValueTask<long> GetPart2()
    {
        var one = GetPossibilities(new(0, 0, 26, new BitArray(_valves.Length)))
                // todo: technically works but why? find a smarter way to narrow down the problem space
                .Where(x => AtLeastXTrue(x.valves, _valves.Length / 2 - 2) && x.minutesLeft > 2)
            //.ToList()
            ;
        var both = one.SelectMany(x => GetPossibilities(x with { valveIndex = 0, minutesLeft = 26 }))
            //.ToList()
            ;

        return both.Max(x => x.totalFlow);
    }

    private IEnumerable<State> GetPossibilities(State state)
    {
        yield return state;

        for (var valveIndex = 1; valveIndex < state.valves.Length; valveIndex++)
        {
            if (state.valves[valveIndex])
                continue;
            var newTimeLeft = state.minutesLeft - _valves[state.valveIndex].distances[valveIndex] - 1;
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

    public static bool AtLeastXTrue(BitArray array, int minimum)
    {
        for (var i = 0; i < array.Length; i++)
            if (array[i])
                minimum--;
        return minimum <= 0;
    }

    public readonly record struct Valve(int flowRate, int[] distances);

    public readonly record struct State(long totalFlow, int valveIndex, int minutesLeft, BitArray valves);

    [GeneratedRegex("""Valve ([A-Z][A-Z]) has flow rate=([0-9]+); tunnels? leads? to valves? ([A-Z, ]+)""")]
    private static partial Regex LineRegex { get; }
}
