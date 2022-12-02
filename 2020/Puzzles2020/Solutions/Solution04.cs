namespace Puzzles2020.Solutions;

public partial record Solution04(long validLoose, long validStrict) : ISolution<Solution04>
{
    private static readonly HashSet<string> validEyes = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth", "amb" };

    public static Solution04 Init(string[] lines)
    {
        long validLoose = 0, validStrict = 0;
        Dictionary<string, string> current = new();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) && current.Any())
            {
                CheckPassport(current, ref validLoose, ref validStrict);
                current = new();
            }
            else
            {
                var pieces = line.Split(' ');
                foreach (var piece in pieces)
                {
                    var kv = piece.Split(':');
                    current[kv[0]] = kv[1];
                }
            }
        }

        CheckPassport(current, ref validLoose, ref validStrict);
        return new(validLoose, validStrict);
    }

    public async ValueTask<long> GetPart1() => validLoose;

    public async ValueTask<long> GetPart2() => validStrict;

    private static void CheckPassport(IReadOnlyDictionary<string, string> info, ref long validLoose, ref long validStrict)
    {
        var loose = info.ContainsKey("byr")
                    && info.ContainsKey("iyr")
                    && info.ContainsKey("eyr")
                    && info.ContainsKey("hgt")
                    && info.ContainsKey("hcl")
                    && info.ContainsKey("ecl")
                    && info.ContainsKey("pid");
        if (!loose)
            return;
        validLoose++;

        var byr = int.Parse(info["byr"]);
        var iyr = int.Parse(info["iyr"]);
        var eyr = int.Parse(info["eyr"]);

        var strict = byr is >= 1920 and <= 2002
                     && iyr is >= 2010 and <= 2020
                     && eyr is >= 2020 and <= 2030
                     && ValidHeight(info["hgt"])
                     && GetHairRegex().IsMatch(info["hcl"])
                     && validEyes.Contains(info["ecl"])
                     && GetPassportIdRegex().IsMatch(info["pid"]);
        if (strict)
            validStrict++;
    }

    private static bool ValidHeight(string value)
    {
        var unit = value[^2..];
        var num = int.Parse(value[..^2]);
        return (unit, num) switch
        {
            ("cm", >= 150 and <= 193) => true,
            ("in", >= 59 and <= 76) => true,
            _ => false,
        };
    }

    [GeneratedRegex("^#([0-9a-f]{6,6})$")]
    private static partial Regex GetHairRegex();

    [GeneratedRegex("^[0-9]{9,9}$")]
    private static partial Regex GetPassportIdRegex();
}
