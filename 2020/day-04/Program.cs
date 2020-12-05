using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

var validLoose = 0;
var validStrict = 0;

HashSet<string> validEyes = new() { "amb", "blu", "brn", "gry", "grn", "hzl", "oth", "amb" };

Dictionary<string, string> current = new();
foreach (var line in System.IO.File.ReadLines("input.txt"))
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

Console.WriteLine(validLoose);
Console.WriteLine(validStrict);

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
	var hgt = info["hgt"];
	var hcl = info["hcl"];
	var ecl = info["ecl"];
	var pid = info["pid"];

	var strict = byr >= 1920 && byr <= 2002 &&
		iyr >= 2010 && iyr <= 2020 &&
		eyr >= 2020 && eyr <= 2030 &&
		ValidHeight(hgt) &&
		Regex.IsMatch(hcl, "^#([0-9a-f]{6,6})$") &&
		validEyes.Contains(ecl) &&
		Regex.IsMatch(pid, "^[0-9]{9,9}$");
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
