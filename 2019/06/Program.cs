var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");

const string CENTER = "COM";

var keyOrbitsValue = list.Aggregate(new Dictionary<string, string>(), (dict, line) =>
{
	var paren = line.IndexOf(')');
	dict[line[(paren + 1)..]] = line[0..(paren)];
	return dict;
});

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	return keyOrbitsValue.Keys.Select(key =>
	{
		long chain = 1;
		var center = keyOrbitsValue[key];
		while (center != CENTER)
		{
			center = keyOrbitsValue[center];
			chain++;
		}
		return chain;
	}).Sum();
}

long Part2()
{
	var you = GetChain("YOU").ToList();
	var santa = GetChain("SAN").ToList();
	var common = you.First(x => santa.Contains(x));

	return you.IndexOf(common) + santa.IndexOf(common);
}

IEnumerable<string> GetChain(string start)
{
	while (start != CENTER)
	{
		start = keyOrbitsValue[start];
		yield return start;
	}
}