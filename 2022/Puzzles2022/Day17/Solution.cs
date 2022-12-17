using System.Collections;

namespace Puzzles2022.Day17;

public record Solution(char[] _gasJets) : ISolution<Solution>
{
    private static readonly Point LEFT = new(-1, 0);
    private static readonly Point RIGHT = new(1, 0);
    private static readonly Point DOWN = new(0, -1);

    private static readonly Point[][] SHAPES =
    {
        // ####
        new Point[] { new(0, 0), new(1, 0), new(2, 0), new(3, 0) },
        // .#.
        // ###
        // .#.
        new Point[] { new(1, 0), new(0, 1), new(1, 1), new(1, 2), new(2, 1) },

        // ..#
        // ..#
        // ###
        new Point[] { new(0, 0), new(1, 0), new(2, 0), new(2, 1), new(2, 2) },
        // #
        // #
        // #
        // #
        new Point[] { new(0, 0), new(0, 1), new(0, 2), new(0, 3) },
        // ##
        // ##
        new Point[] { new(0, 0), new(1, 0), new(1, 1), new(0, 1) },
    };

    public static Solution Init(string[] lines) => new(lines[0].ToCharArray());


    private Cycle _cycle;

    public async ValueTask<long> GetPart1() => GetHeightAfter(2022);

    public async ValueTask<long> GetPart2()
    {
        var factor = 2;
        while (_cycle == default)
            GetHeightAfter(2022 * factor++);

        var heightBeforeCycle = GetHeightAfter(_cycle.start);
        var heightAfterCycle = GetHeightAfter(_cycle.start + _cycle.length);
        var heightPerCycle = heightAfterCycle - heightBeforeCycle;

        long pieces = 1_000_000_000_000 - _cycle.length;
        var (numberOfCycles, piecesLeft) = long.DivRem(pieces, _cycle.length);
        var startAndEndHeights = GetHeightAfter(piecesLeft + _cycle.length);

        return numberOfCycles * heightPerCycle + startAndEndHeights;
    }

    public long GetHeightAfter(long pieceLimit)
    {
        int highest = 0, placed = 0, nextShapeIndex = 0, nextJetIndex = 0;
        var occupied = new List<BitArray>();
        var operations = new List<Operation>();

        while (placed++ < pieceLimit)
        {
            // Spawn above the board
            var shapeIndex = nextShapeIndex;
            var shape = SHAPES[nextShapeIndex++];
            nextShapeIndex = nextShapeIndex.LoopIndex(SHAPES);
            TryMove(shape, new(3, highest + 4), occupied, out shape);

            // Move sideways then down until we can't move down
            var gasStart = nextJetIndex;
            int moves = 0;
            do
            {
                var jet = _gasJets[nextJetIndex++];
                nextJetIndex = nextJetIndex.LoopIndex(_gasJets);
                _ = TryMove(shape, jet == '<' ? LEFT : RIGHT, occupied, out shape);
                moves++;
            } while (TryMove(shape, DOWN, occupied, out shape));

            foreach (var pt in shape)
                occupied[pt.y][pt.x] = true;

            highest = int.Max(highest, shape.Max(x => x.y));

            // while we haven't found a cycle, track operations so we can figure it out
            if (_cycle == default)
            {
                operations.Add(new(shapeIndex, gasStart, moves));
                _cycle = FindRepeatedCycle(operations);
            }
        }

        // Prints grid upside down
        // var board = new StringBuilder();
        // board.AppendLine("⬜⬜⬜⬜⬜⬜⬜⬜⬜");
        // foreach (var line in occupied.Skip(1))
        // {
        //     board.Append('⬜');
        //     for (var i = 1; i < line.Count; i++)
        //         board.Append(line[i] ? '❌' : '⬛');
        //     board.Append('⬜');
        //     board.AppendLine();
        // }
        // var testBoard = board.ToString();

        return highest;
    }

    private static Cycle FindRepeatedCycle(List<Operation> operations)
    {
        if (operations.Count < 2)
            return default;
        var previousMatch = operations.LastIndexOf(operations[^1], operations.Count - 2);
        if (previousMatch < 1)
            return default;
        var furtherBack = operations.LastIndexOf(operations[^1], previousMatch - 1);
        if (furtherBack < 0)
            return default;

        var length = previousMatch - furtherBack;

        var first = operations.Skip(previousMatch - 1).Take(length);
        var second = operations.Skip(furtherBack - 1).Take(length);

        if (first.SequenceEqual(second))
            return new(furtherBack, length);

        return default;
    }

    public static bool TryMove(Point[] original, Point move, List<BitArray> occupied, out Point[] shape)
    {
        var newLocations = new Point[original.Length];
        for (var i = 0; i < original.Length; i++)
        {
            newLocations[i] = original[i] + move;
        }

        foreach (var location in newLocations)
        {
            while (occupied.Count < location.y + 1)
                occupied.Add(new BitArray(8));
            if (location.x is < 1 or > 7 || location.y < 1 || occupied[location.y][location.x])
            {
                shape = original;
                return false;
            }
        }

        shape = newLocations;
        return true;
    }

    public record struct Cycle(int start, int length);

    public record struct Operation(int pieceIndex, int gasStart, int moves);
}
