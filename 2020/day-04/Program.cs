using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

#nullable enable

List<Passport> passports = new();

List<string> currentPieces = new();
foreach (var line in System.IO.File.ReadLines("input.txt"))
{
	if (string.IsNullOrEmpty(line) && currentPieces.Any())
	{
		passports.Add(ParsePassport(currentPieces));
		currentPieces.Clear();
	}
	else
	{
		currentPieces.AddRange(line.Split(' '));
	}
}
passports.Add(ParsePassport(currentPieces));

Console.WriteLine(passports.Count(x => x.IsValid));
Console.WriteLine(passports.Count(x => x.IsValidStrict));

Passport ParsePassport(List<string> currentPieces)
{
	var components = currentPieces.Select(x => x.Split(':')).ToDictionary(x => x[0], x => x[1]);
	return new Passport
	{
		byr = components.ContainsKey("byr") ? int.Parse(components["byr"]) : null,
		iyr = components.ContainsKey("iyr") ? int.Parse(components["iyr"]) : null,
		eyr = components.ContainsKey("eyr") ? int.Parse(components["eyr"]) : null,
		hgt = components.ContainsKey("hgt") ? components["hgt"] : null,
		hcl = components.ContainsKey("hcl") ? components["hcl"] : null,
		ecl = components.ContainsKey("ecl") ? components["ecl"] : null,
		pid = components.ContainsKey("pid") ? components["pid"] : null,
		cid = components.ContainsKey("cid") ? components["cid"] : null,
	};
}

record Passport
{
	public int? byr { get; init; }
	public int? iyr { get; init; }
	public int? eyr { get; init; }
	public string? hgt { get; init; }
	public string? hcl { get; init; }
	public string? ecl { get; init; }
	public string? pid { get; init; }
	public string? cid { get; init; }

	public bool IsValid =>
		byr != null &&
		iyr != null &&
		eyr != null &&
		hgt != null &&
		hcl != null &&
		ecl != null &&
		pid != null;

	public bool IsValidStrict =>
		byr is int birth && birth >= 1920 && birth <= 2002 &&
		iyr is int issue && issue >= 2010 && issue <= 2020 &&
		eyr is int exp && exp >= 2020 && exp <= 2030 &&
		hgt != null && Data.ValidHeight(hgt) &&
		hcl != null && Data.ColorMatch.IsMatch(hcl) &&
		ecl != null && Data.ValidEyes.Contains(ecl) &&
		pid != null && Regex.IsMatch(pid, "^[0-9]{9,9}$");
}

public class Data
{
	public static HashSet<string> ValidEyes = new HashSet<string>()
	{
		"amb",
		"blu",
		"brn",
		"gry",
		"grn",
		"hzl",
		"oth",
		"amb",
	};

	public static Regex ColorMatch = new("^#([0-9a-f]{6,6})$");

	public static bool ValidHeight(string value)
	{
		if (value.Length < 3)
			return false;
		var unit = value.Substring(value.Length - 2);
		var num = int.Parse(value.Substring(0, value.Length - 2));
		return (unit, num) switch
		{
			("cm", >= 150 and <= 193) => true,
			("in", >= 59 and <= 76) => true,
			_ => false,
		};
	}
}