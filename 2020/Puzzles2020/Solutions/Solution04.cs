namespace Puzzles2020.Solutions;

public class Solution04 : ISolution
{
    private long validLoose = 0;
    private long validStrict = 0;
    private readonly HashSet<string> validEyes = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth", "amb" };

    public Solution04(string[] lines)
    {
        Dictionary<string, string> current = new();
        foreach (var line in lines)
        {
            if (string.IsNullOrEmpty(line) && current.Any())
            {
                CheckPassport(current);
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

        CheckPassport(current);
    }

    public async ValueTask<long> GetPart1() => validLoose;

    public async ValueTask<long> GetPart2() => validStrict;

    void CheckPassport(Dictionary<string, string> info)
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

        var strict = byr >= 1920 && byr <= 2002 &&
                     iyr >= 2010 && iyr <= 2020 &&
                     eyr >= 2020 && eyr <= 2030 &&
                     ValidHeight(info["hgt"]) &&
                     Regex.IsMatch(info["hcl"], "^#([0-9a-f]{6,6})$") &&
                     validEyes.Contains(info["ecl"]) &&
                     Regex.IsMatch(info["pid"], "^[0-9]{9,9}$");
        if (strict)
            validStrict++;
    }

    bool ValidHeight(string value)
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
}