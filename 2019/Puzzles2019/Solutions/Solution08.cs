using Spectre.Console;

namespace Puzzles2019.Solutions;

public class Solution08 : ISolution
{
    private const int layerSize = 25 * 6;
    private readonly List<int[]> layers;

    public Solution08(string[] lines)
    {
        var values = lines[0].ParseDigits();
        layers = values.Chunk(layerSize).ToList();
    }

    public async ValueTask<long> GetPart1()
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

    public async ValueTask<long> GetPart2()
    {
        AnsiConsole.WriteLine();
        // set up transparent image
        var image = Enumerable.Range(1, 6).Select(_ => Enumerable.Range(1, 25).Select(_ => 2).ToArray()).ToArray();
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
        AnsiConsole.Background = Color.White;
        foreach (var row in image)
        {
            foreach (var pixel in row)
            {
                var color = pixel switch
                {
                    0 => Color.Black,
                    1 => Color.White,
                    _ => throw new ArgumentException($"Unexpected pixel value: [{pixel}]"),
                };
                AnsiConsole.Foreground = color;
                AnsiConsole.Write(block);
            }

            AnsiConsole.WriteLine();
        }

        AnsiConsole.ResetColors();
        return -1;
    }
}
