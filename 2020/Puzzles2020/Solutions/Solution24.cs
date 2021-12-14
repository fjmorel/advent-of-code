namespace Puzzles2020.Solutions;

public class Solution24 : ISolution
{
    private readonly string[] _lines;
    private readonly Dictionary<HexCoordinate, bool> tiles = new();

    public Solution24(string[] lines)
    {
        _lines = lines;
    }

    public async ValueTask<long> GetPart1()
    {
        foreach (var line in _lines)
        {
            var position = ParseLine(line).Aggregate(HexCoordinate.Origin, (pos, dir) => pos.WithMove(dir));
            tiles[position] = !tiles.GetValueOrDefault(position, false);
        }

        return tiles.Count(x => x.Value);
    }

    public async ValueTask<long> GetPart2()
    {
        var blackTiles = tiles.Where(x => x.Value).Select(x => x.Key).ToHashSet();
        for (var i = 1; i <= 100; i++)
        {
            blackTiles = blackTiles
                // Add all neighbors as possible new black tiles
                .Concat(blackTiles.SelectMany(x => x.GetNeighbors()))
                // Only check each tile once and in parallel
                .Distinct().AsParallel()
                // Check if the tile should now be black
                .Where(x => x.ShouldBeBlack(blackTiles))
                .ToHashSet();
        }

        return blackTiles.Count;
    }

    private IEnumerable<Direction> ParseLine(string line)
    {
        for (var i = 0; i < line.Length; i++)
        {
            if (line[i] == 'e')
                yield return Direction.E;
            else if (line[i] == 'w')
                yield return Direction.W;
            else
            {
                yield return line[i..(i + 2)] switch
                {
                    "se" => Direction.SE,
                    "sw" => Direction.SW,
                    "ne" => Direction.NE,
                    "nw" => Direction.NW,
                    _ => throw new NotSupportedException(),
                };
                i++;
            }
        }
    }

    // X = W<-->E, Y = NW<-->SE, Z = NE<-->SW. x+y+z==0
    // https://www.redblobgames.com/grids/hexagons/ Cube coordinates
    private record HexCoordinate(int X, int Y, int Z)
    {
        public static readonly HexCoordinate Origin = new(0, 0, 0);

        public HexCoordinate WithMove(Direction direction) => direction switch
        {
            Direction.E => this with { X = X + 1, Y = Y - 1 },
            Direction.W => this with { X = X - 1, Y = Y + 1 },

            Direction.SE => this with { Z = Z + 1, Y = Y - 1 },
            Direction.NW => this with { Z = Z - 1, Y = Y + 1 },

            Direction.NE => this with { X = X + 1, Z = Z - 1 },
            Direction.SW => this with { X = X - 1, Z = Z + 1 },

            _ => throw new NotSupportedException(),
        };

        public IEnumerable<HexCoordinate> GetNeighbors()
        {
            yield return WithMove(Direction.E);
            yield return WithMove(Direction.SE);
            yield return WithMove(Direction.NE);
            yield return WithMove(Direction.W);
            yield return WithMove(Direction.SW);
            yield return WithMove(Direction.NW);
        }

        public bool ShouldBeBlack(HashSet<HexCoordinate> allCurrentlyBlack)
        {
            var blackNeighbors = GetNeighbors().Count(x => allCurrentlyBlack.Contains(x));
            return (blackNeighbors == 2 || (blackNeighbors == 1 && allCurrentlyBlack.Contains(this)));
        }
    }

    private enum Direction
    {
        E,
        SE,
        NE,
        W,
        SW,
        NW,
    }
}
