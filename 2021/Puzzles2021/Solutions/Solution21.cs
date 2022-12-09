namespace Puzzles2021.Solutions;

public record Solution21(Solution21.Player _p1, Solution21.Player _p2) : ISolution<Solution21>
{
    private static readonly int[] _probabilities = { 0, 0, 0, 1, 3, 6, 7, 6, 3, 1 };//3-9

    public static Solution21 Init(string[] lines)
    {
        var p1 = new Player(int.Parse(lines[0].AsSpan()[^1..]), IsP1: true);
        var p2 = new Player(int.Parse(lines[1].AsSpan()[^1..]));
        return new(p1, p2);
    }

    public async ValueTask<long> GetPart1()
    {
        var (next, previous) = (_p1, _p2);
        int rolls = 0, die = 0;

        do
        {
            var total = 0;
            for (var i = 1; i <= 3; i++)
            {
                rolls++;
                die++;
                if (die > 100)
                    die -= 100;
                total += die;
            }

            (next, previous) = (previous, next.WithMove(total));
        } while (previous.Score < 1000 & next.Score < 1000);

        return rolls * Math.Min(previous.Score, next.Score);
    }

    public async ValueTask<long> GetPart2()
    {
        var wins = FindWins(_p1, _p2, 1);
        return Math.Max(wins.p1, wins.p2);
    }

    public (long p1, long p2) FindWins(Player next, Player previous, long multiplier)
    {
        long p1 = 0L, p2 = 0L;
        for (var i = 3; i <= 9; i++)
        {
            var wins = FindWinsForRoll(i, next, previous, multiplier * _probabilities[i]);
            p1 += wins.p1;
            p2 += wins.p2;
        }

        return (p1, p2);
    }

    private (long p1, long p2) FindWinsForRoll(int roll, Player next, Player previous, long multiplier)
    {
        var moved = next.WithMove(roll);
        if (moved.Score >= 21)
            return moved.IsP1 ? (multiplier, 0) : (0, multiplier);
        return FindWins(previous, moved, multiplier);
    }

    public readonly record struct Player(int Position, int Score = 0, bool IsP1 = false)
    {
        public Player WithMove(int add)
        {
            var newPosition = Position + add;
            while (newPosition > 10)
                newPosition -= 10;
            return this with { Position = newPosition, Score = Score + newPosition };
        }
    }
}
