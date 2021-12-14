namespace Puzzles2020.Solutions;

public class Solution20 : ISolution
{
    private readonly List<Tile> tiles;
    private readonly Dictionary<int, Dictionary<Side, int>> adjacent;

    public Solution20(string[] lines)
    {
        tiles = new List<Tile>();
        adjacent = new Dictionary<int, Dictionary<Side, int>>();
        for (var i = 0; i < lines.Length; i += 12)
            tiles.Add(new(int.Parse(lines[i][5..^1]), lines[(i + 1)..(i + 11)].Select(x => x.Select(x => x == '#').ToArray()).ToArray()));
    }

    public async ValueTask<long> GetPart1()
    {
        tiles[0] = tiles[0] with { Transformed = true };
        var queue = new Queue<Tile>();
        queue.Enqueue(tiles[0]);

        while (queue.Any())
        {
            var mainTile = queue.Dequeue();
            var matches = tiles
                .Where(x => x != mainTile)
                .Select(tile => (tile, result: mainTile.MatchWith(tile)))
                .Where(x => x.result.side != Side.None)
                .ToList();

            // Store list of adjacent tiles
            adjacent[mainTile.Id] = matches.ToDictionary(x => x.result.side, x => x.tile.Id);
            // Transform matches as needed only once, and then find their matches
            foreach (var match in matches)
            {
                var otherTile = match.tile;
                if (!otherTile.Transformed)
                {
                    var adj = ApplyTransforms(otherTile.contents, match.result.transforms);
                    var index = tiles.IndexOf(otherTile);
                    tiles[index] = otherTile with { contents = adj, Transformed = true };
                    queue.Enqueue(tiles[index]);
                }
                else
                {
                    if (match.result.transforms.Any())
                        throw new InvalidOperationException("unexpected transforms");
                }
            }
        }

        return adjacent.Where(x => x.Value.Count == 2).Aggregate(1L, (acc, value) => acc * value.Key);
    }

    public async ValueTask<long> GetPart2()
    {
        var sea = BuildSea();
        var monster = new string[]
        {
            "                  # ",
            "#    ##    ##    ###",
            " #  #  #  #  #  #   ",
        };
        var monsterPattern = monster
            .SelectMany((line, i) => line.Select((ch, j) => (i, j, ch)))
            .Where(x => x.ch == '#')
            .Select(a => (a.i, a.j))
            .ToList();
        var monsterWidth = monsterPattern.Max(c => c.i);
        var monsterHeight = monsterPattern.Max(c => c.j);

        var uniqueTransformSets = new Transformation[][]
        {
            new[] { Transformation.None, },
            new[] { Transformation.FlipVertical },
            new[] { Transformation.FlipHorizontal },
            new[] { Transformation.FlipVertical, Transformation.FlipHorizontal },// = 2 RotateLeft or 2 RotateRight
            new[] { Transformation.RotateLeft },// = RotateRight + FlipHorizontal + FlipVertical
            new[] { Transformation.RotateLeft, Transformation.FlipVertical },// = RotateRight + FlipHorizontal
            new[] { Transformation.RotateLeft, Transformation.FlipHorizontal },// = RotateRight + FlipVertical
            new[] { Transformation.RotateLeft, Transformation.FlipHorizontal, Transformation.FlipVertical },// = RotateRight
        };

        foreach (var transforms in uniqueTransformSets)
        {
            var adjSea = ApplyTransforms(sea, transforms);
            var numMonsters = 0;
            for (var i = 0; i < adjSea[0].Length - monsterWidth; i++)
            for (var j = 0; j < adjSea.Length - monsterHeight; j++)
                if (monsterPattern.All(c => adjSea[i + c.i][j + c.j]))
                    numMonsters++;

            if (numMonsters > 0)
                return sea.Sum(x => x.Count(y => y)) - numMonsters * monsterPattern.Count;
        }

        return 0;
    }

    private bool[][] BuildSea()
    {
        var tileGridSize = (int)Math.Sqrt(tiles.Count);
        var tileGrid = Enumerable.Range(0, tileGridSize).Select(x => new int[tileGridSize]).ToArray();

        var corners = adjacent.Where(x => x.Value.Count == 2).ToList();
        var origin = corners.Single(x => x.Value.ContainsKey(Side.Right) && x.Value.ContainsKey(Side.Bottom));
        tileGrid[0][0] = origin.Key;
        for (var i = 0; i < tileGridSize; i++)
        {
            for (var j = 0; j < tileGridSize; j++)
            {
                if (i == 0 && j == 0)
                    continue;
                if (i == 0)// in first row, use previous column
                    tileGrid[i][j] = adjacent[tileGrid[i][j - 1]][Side.Right];
                else// otherwise go ahead and use previous row
                    tileGrid[i][j] = adjacent[tileGrid[i - 1][j]][Side.Bottom];
            }
        }

        var dict = tiles.ToDictionary(x => x.Id, x => x.contents[1..^1].Select(x => x[1..^1].ToArray()).ToArray());
        var sea = new List<bool[]>();

        for (var i = 0; i < tileGrid.Length; i++)
        {
            var tiles = tileGrid[i].Select(x => dict[x]).ToList();
            for (var j = 0; j < tiles[0].Length; j++)
            {
                sea.Add(tiles.SelectMany((x, y) => x[j]).ToArray());
            }
        }

        return sea.ToArray();
    }

    private T[][] ApplyTransforms<T>(T[][] original, params Transformation[] transformations)
    {
        var adj = original;
        foreach (var transform in transformations)
            adj = ApplyTransform(adj, transform);
        return adj;
    }

    private T[][] ApplyTransform<T>(T[][] original, Transformation transform) => (transform switch
    {
        Transformation.None => original,
        Transformation.FlipVertical => original.Reverse(),
        Transformation.FlipHorizontal => original.Select(x => x.Reverse().ToArray()),
        Transformation.RotateLeft => original.Select((x, i) => original.Select(y => y[i]).ToArray()).Reverse(),
        Transformation.RotateRight => original.Reverse().Select((x, i) => original.Select(y => y[i]).Reverse().ToArray()),
        _ => throw new NotSupportedException(),
    }).ToArray();

    private record Tile(int Id, bool[][] contents, bool Transformed = false)
    {
        private IEnumerable<bool> Top => contents.First();
        private IEnumerable<bool> Right => contents.Select(x => x.Last());
        private IEnumerable<bool> Bottom => contents.Last();
        private IEnumerable<bool> Left => contents.Select(x => x.First());
        private IEnumerable<bool> ReverseTop => Top.Reverse();
        private IEnumerable<bool> ReverseRight => Right.Reverse();
        private IEnumerable<bool> ReverseBottom => Bottom.Reverse();
        private IEnumerable<bool> ReverseLeft => Left.Reverse();

        public Match MatchWith(Tile second)
        {
            if (Top.SequenceEqual(second.Top))
                return new(Side.Top, Transformation.FlipVertical);
            if (Top.SequenceEqual(second.Bottom))
                return new(Side.Top);
            if (Top.SequenceEqual(second.Left))
                return new(Side.Top, Transformation.RotateLeft);
            if (Top.SequenceEqual(second.Right))
                return new(Side.Top, Transformation.RotateLeft, Transformation.FlipVertical);
            if (Top.SequenceEqual(second.ReverseTop))
                return new(Side.Top, Transformation.FlipVertical, Transformation.FlipHorizontal);
            if (Top.SequenceEqual(second.ReverseBottom))
                return new(Side.Top, Transformation.FlipHorizontal);
            if (Top.SequenceEqual(second.ReverseLeft))
                return new(Side.Top, Transformation.RotateLeft, Transformation.FlipHorizontal);
            if (Top.SequenceEqual(second.ReverseRight))
                return new(Side.Top, Transformation.RotateRight);

            if (Bottom.SequenceEqual(second.Top))
                return new(Side.Bottom);
            if (Bottom.SequenceEqual(second.Bottom))
                return new(Side.Bottom, Transformation.FlipVertical);
            if (Bottom.SequenceEqual(second.Left))
                return new(Side.Bottom, Transformation.RotateRight, Transformation.FlipHorizontal);
            if (Bottom.SequenceEqual(second.Right))
                return new(Side.Bottom, Transformation.RotateLeft);
            if (Bottom.SequenceEqual(second.ReverseTop))
                return new(Side.Bottom, Transformation.FlipHorizontal);
            if (Bottom.SequenceEqual(second.ReverseBottom))
                return new(Side.Bottom, Transformation.FlipVertical, Transformation.FlipHorizontal);
            if (Bottom.SequenceEqual(second.ReverseLeft))
                return new(Side.Bottom, Transformation.RotateRight);
            if (Bottom.SequenceEqual(second.ReverseRight))
                return new(Side.Bottom, Transformation.RotateLeft, Transformation.FlipHorizontal);

            if (Left.SequenceEqual(second.Top))
                return new(Side.Left, Transformation.RotateRight);
            if (Left.SequenceEqual(second.Bottom))
                return new(Side.Left, Transformation.RotateLeft, Transformation.FlipVertical);
            if (Left.SequenceEqual(second.Left))
                return new(Side.Left, Transformation.FlipHorizontal);
            if (Left.SequenceEqual(second.Right))
                return new(Side.Left);
            if (Left.SequenceEqual(second.ReverseTop))
                return new(Side.Left, Transformation.RotateRight, Transformation.FlipVertical);
            if (Left.SequenceEqual(second.ReverseBottom))
                return new(Side.Left, Transformation.RotateLeft);
            if (Left.SequenceEqual(second.ReverseLeft))
                return new(Side.Left, Transformation.FlipHorizontal, Transformation.FlipVertical);
            if (Left.SequenceEqual(second.ReverseRight))
                return new(Side.Left, Transformation.FlipVertical);

            if (Right.SequenceEqual(second.Top))
                return new(Side.Right, Transformation.RotateLeft, Transformation.FlipVertical);
            if (Right.SequenceEqual(second.Bottom))
                return new(Side.Right, Transformation.RotateRight);
            if (Right.SequenceEqual(second.Left))
                return new(Side.Right);
            if (Right.SequenceEqual(second.Right))
                return new(Side.Right, Transformation.FlipHorizontal);
            if (Right.SequenceEqual(second.ReverseTop))
                return new(Side.Right, Transformation.RotateLeft);
            if (Right.SequenceEqual(second.ReverseBottom))
                return new(Side.Right, Transformation.RotateRight, Transformation.FlipVertical);
            if (Right.SequenceEqual(second.ReverseLeft))
                return new(Side.Right, Transformation.FlipVertical);
            if (Right.SequenceEqual(second.ReverseRight))
                return new(Side.Right, Transformation.FlipHorizontal, Transformation.FlipVertical);

            return new(Side.None);
        }
    }

    private record Match(Side side, params Transformation[] transforms);

    private enum Side
    {
        None,
        Left,
        Top,
        Right,
        Bottom,
    }

    private enum Transformation
    {
        None,
        FlipVertical,
        FlipHorizontal,
        RotateLeft,
        RotateRight,
    }
}
