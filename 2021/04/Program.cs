var timer = Stopwatch.StartNew();
var lines = File.ReadAllLines("input.txt");
// lines = File.ReadAllLines("example.txt");
var numbers = lines[0].Split(',').Select(x => int.Parse(x)).ToArray();
var cards = lines.Skip(1).Chunk(6).Select(sixLines => sixLines.Skip(1).SelectMany(line => line.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries).Select(x => int.Parse(x))).ToArray()).ToList();

Console.WriteLine($"_ :: {timer.Elapsed}");// setup time
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var lowest = numbers.Length + 1;
	int[]? winningCard = null;
	foreach (var card in cards)
	{
		var count = CountNeededToWin(card);
		if (count < lowest)
		{
			lowest = count;
			winningCard = card;
		}
	}

	return GetScore(winningCard, lowest);
}

long Part2()
{
	var highest = 0;
	int[]? winningCard = null;
	foreach (var card in cards)
	{
		var count = CountNeededToWin(card);
		if (count > highest)
		{
			highest = count;
			winningCard = card;
		}
	}

	return GetScore(winningCard, highest);
}

long GetScore(int[] card, int needed)
{
	var list = card.ToList();
	var marked = new List<int>();
	for (var i = 0; i < needed; i++)
	{
		if (list.Remove(numbers[i]))
			marked.Add(numbers[i]);
	}
	return list.Sum() * numbers[needed - 1];
}

int CountNeededToWin(int[] card)
{
	var currentSet = new HashSet<int>(numbers.Length);
	for (var i = 0; i < numbers.Length; i++)
	{
		currentSet.Add(numbers[i]);
		if (HasWinningLine(card, currentSet))
			return i + 1;
	}
	return numbers.Length + 1;
}

bool HasWinningLine(int[] card, HashSet<int> numbers)
{
	for (var i = 0; i < 5; i++)
	{
		// Each horizontal line
		for (var j = 0; j < 5; j++)
		{
			var index = (i * 5) + j;
			if (!numbers.Contains(card[index]))
				break;
			if (j == 4)
				return true;
		}
		// Each vertical line
		for (var j = 0; j < 5; j++)
		{
			var index = (j * 5) + i;
			if (!numbers.Contains(card[index]))
				break;
			if (j == 4)
				return true;
		}
	}

	return false;
}