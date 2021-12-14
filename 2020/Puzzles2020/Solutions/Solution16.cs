namespace Puzzles2020.Solutions;

public class Solution16 : ISolution
{
    private readonly List<Rule> rules;
    private readonly List<int> allInvalid;
    private readonly List<int> myTicket;
    private readonly List<List<int>> tickets;

    public Solution16(string[] lines)
    {
        rules = new List<Rule>();
        var i = 0;
        for (; !string.IsNullOrEmpty(lines[i]); i++)
        {
            var pieces = lines[i].Split(": ");
            var ranges = pieces[1].Split(" or ").Select(x => x.Split('-').Select(int.Parse).ToList()).ToList();
            rules.Add(new Rule(pieces[0], ranges[0][0], ranges[0][1], ranges[1][0], ranges[1][1]));
        }

        // skip empty and "your ticket:"
        myTicket = lines[i + 2].Split(',').Select(int.Parse).ToList();

        allInvalid = new List<int>();
        tickets = new List<List<int>>();
        // skip my ticket, empty, and "nearby tickets:"
        foreach (var line in lines.Skip(i + 5))
        {
            var nums = line.Split(',').Select(int.Parse).ToList();
            var invalidNums = nums.Where(num => !rules.Any(x => x.Matches(num))).ToList();
            if (invalidNums.Any())
                allInvalid.AddRange(invalidNums);
            else
                tickets.Add(nums);
        }
    }

    public async ValueTask<long> GetPart1()
    {
        return allInvalid.Sum();
    }

    public async ValueTask<long> GetPart2()
    {
        var matched = new Dictionary<int, string>();
        var unmatched = new List<Rule>(rules);
        while (unmatched.Any())
        {
            var remaining = new List<Rule>(unmatched);
            unmatched.Clear();
            foreach (var rule in remaining)
            {
                var matches = Enumerable.Range(0, tickets[0].Count)
                    .Where(x => !matched.ContainsKey(x) && tickets.All(ticket => rule.Matches(ticket[x])))
                    .ToList();
                if (matches.Count == 1)
                    matched[matches[0]] = rule.name;
                else
                    unmatched.Add(rule);
            }
        }

        return matched
            .Where(x => x.Value.StartsWith("departure ", StringComparison.Ordinal))
            .Aggregate(1L, (acc, x) => acc * myTicket[x.Key]);
    }

    private record Rule(string name, int aMin, int aMax, int bMin, int bMax)
    {
        public bool Matches(int num) => (num >= aMin && num <= aMax) || (num >= bMin && num <= bMax);
    }
}
