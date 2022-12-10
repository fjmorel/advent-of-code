namespace Puzzles2019.Solutions;

public record Solution08(List<int[]> layers) : ISolution<Solution08>
{
    private const int layerSize = 25 * 6;

    public static Solution08 Init(string[] lines)
    {
        var values = lines[0].ParseDigits();
        var layers = values.Chunk(layerSize).ToList();
        return new(layers);
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

    public async ValueTask<string> GetPart2String()
    {
        var sb = new StringBuilder();
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

        foreach (var row in image)
        {
            foreach (var pixel in row)
            {
                var symbol = pixel switch
                {
                    0 => '⬛',
                    1 => '⬜',
                    _ => throw new ArgumentException($"Unexpected pixel value: [{pixel}]"),
                };
                sb.Append(symbol);
            }

            sb.AppendLine();
        }

        return sb.ToString().ParseAsciiLetters();
    }
}
