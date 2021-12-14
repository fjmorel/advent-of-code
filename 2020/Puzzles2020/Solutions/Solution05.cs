namespace Puzzles2020.Solutions;

public class Solution05 : ISolution
{
    private readonly List<int> seats;

    public Solution05(string[] lines)
    {
        seats = lines.Select(line => line.Aggregate(new Seat(0, 127, 0, 7), (seat, letter) => letter switch
        {
            'B' => seat with { rowMin = seat.rowMin + (seat.rowMax - seat.rowMin + 1) / 2 },
            'F' => seat with { rowMax = seat.rowMax - (seat.rowMax - seat.rowMin + 1) / 2 },
            'R' => seat with { colMin = seat.colMin + (seat.colMax - seat.colMin + 1) / 2 },
            'L' => seat with { colMax = seat.colMax - (seat.colMax - seat.colMin + 1) / 2 },
            _ => throw new NotSupportedException(),
        }).Id).ToList();
    }

    public async ValueTask<long> GetPart1() => seats.Max();

    public async ValueTask<long> GetPart2()
    {
        var missing = Enumerable.Range(0, 127 * 8 + 7).Except(seats).ToHashSet();
        return missing.First(x => !missing.Contains(x + 1) && !missing.Contains(x - 1));
    }


    private record Seat(int rowMin, int rowMax, int colMin, int colMax)
    {
        public int Id => rowMax * 8 + colMax;
    }
}
