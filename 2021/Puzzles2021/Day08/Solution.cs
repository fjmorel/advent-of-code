namespace Puzzles2021.Day08;

public record Solution(
    List<Solution.Line> _lines,
    HashSet<char> _sharedFives,
    HashSet<char> _sharedSixes
) : ISolution<Solution>
{
    private const string ALL = "abcdefg";

    private static readonly string[] _real =
    {
        "abcefg",// 0, 6 segments
        "cf",// 1, 2 segments
        "acdeg",// 2, 5 segments
        "acdfg",// 3, 5 segments
        "bcdf",// 4, 4 segments
        "abdfg",// 5, 5 segments
        "abdefg",// 6, 6 segments
        "acf",// 7, 3 segments
        "abcdefg",// 8, 7 segments
        "abcdfg",// 9, 6 segments
    };


    public static Solution Init(string[] lines)
    {
        var parsed = lines.Select(ParseLine).ToList();
        var sharedFives = GetSharedLetters(_real, 5);
        var sharedSixes = GetSharedLetters(_real, 6);
        return new(parsed, sharedFives, sharedSixes);
    }

    public async ValueTask<long> GetPart1() => _lines.Sum(x => x.GetUniqueCounts());

    public async ValueTask<long> GetPart2() => _lines.Sum(x => x.GetValue(_real, _sharedFives, _sharedSixes));

    private static Line ParseLine(string line)
    {
        var matches = Regex.Matches(line, "([a-g]+)");
        var patterns = matches.Take(10).Select(x => x.Value).ToArray();
        var digits = matches.Skip(10).Select(x => x.Value).ToArray();
        return new(patterns, digits);
    }

    private static HashSet<char> GetSharedLetters(string[] patterns, int segmentCount)
        => patterns.Where(x => x.Length == segmentCount).Aggregate(ALL.ToHashSet(), (a, b) => a.Intersect(b).ToHashSet());

    public readonly record struct Line(string[] patterns, string[] digits)
    {
        public long GetUniqueCounts() => digits.Count(digit => digit.Length is 2 or 4 or 3 or 7);

        public long GetValue(string[] real, HashSet<char> realSharedFives, HashSet<char> realSharedSixes)
        {
            // Setup what every letter can be
            var possible = new Dictionary<char, HashSet<char>>();
            foreach (var letter in ALL)
                possible[letter] = ALL.ToHashSet();

            // Easy filtering for patterns of unique length
            foreach (var jumbled in patterns.Where(x => x.Length is 2 or 4 or 3 or 7))
            {
                var actual = real.First(x => x.Length == jumbled.Length);
                foreach (var letter in jumbled)
                    possible[letter].IntersectWith(actual);
            }

            // Filter the segments shared by all 5-segments
            foreach (var letter in GetSharedLetters(patterns, 5))
                possible[letter].IntersectWith(realSharedFives);

            // Filter the segments shared by all 6-segments
            foreach (var letter in GetSharedLetters(patterns, 6))
                possible[letter].IntersectWith(realSharedSixes);

            // Now solve each letter one by one
            var lookup = new Dictionary<char, char>();
            while (possible.Any())
            {
                var pair = possible.First(x => x.Value.Count == 1);
                var realLetter = pair.Value.First();
                lookup[pair.Key] = realLetter;
                possible.Remove(pair.Key);
                foreach (var set in possible.Values)
                    set.Remove(realLetter);
            }

            var realSegments = digits.Select(digit => digit.Select(segment => lookup[segment]).ToArray()).ToArray();
            foreach (var digit in realSegments)
                Array.Sort(digit);
            var realDigits = realSegments.Select(x => Array.IndexOf(real, new string(x))).ToArray();
            var number = realDigits[0] * 1000 + realDigits[1] * 100 + realDigits[2] * 10 + realDigits[3];
            return number;
        }
    }
}
