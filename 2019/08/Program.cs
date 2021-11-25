var list = File.ReadAllLines("input.txt");
// list = File.ReadAllLines("example.txt");
var values = list[0].Select(x => int.Parse(new string(x, 1))).ToList();
var layerSize = 25 * 6;
var layers = values.Chunk(layerSize).ToList();

var timer = Stopwatch.StartNew();
Console.WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
Console.WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var fewestZeros = layerSize;
	var layerWithFewestZeros = Array.Empty<int>();
	foreach (var layer in layers)
	{
		var count = layer.Count(x => x == 0);
		if (count < fewestZeros)
		{
			fewestZeros = count;
			layerWithFewestZeros = layer;
		}
	}
	var ones = layerWithFewestZeros.Count(x => x == 1);
	var twos = layerWithFewestZeros.Count(x => x == 2);
	return ones * twos;
}

long Part2()
{
	// set up transparent image
	var image = Enumerable.Range(1, 6).Select(x => Enumerable.Range(1, 25).Select(y => 2).ToArray()).ToArray();
	foreach (var layer in layers)
	{
		var rows = layer.Chunk(25).ToList();
		for (var i = 0; i < rows.Count; i++)
		{
			var row = rows[i];
			for (var j = 0; j < row.Length; j++)
			{
				if (image[i][j] == 2)
				{
					image[i][j] = row[j];
				}
			}
		}
	}

	const char block = '\u2588';
	Console.BackgroundColor = ConsoleColor.White;
	foreach (var row in image)
	{
		foreach (var pixel in row)
		{
			var color = pixel switch
			{
				0 => ConsoleColor.Black,
				1 => ConsoleColor.White,
			};
			Console.ForegroundColor = color;
			Console.Write(block);
		}
		Console.WriteLine();
	}
	Console.ResetColor();
	return -1;
}