namespace Puzzles2019.Solutions;

public class Solution12 : ISolution
{
    private readonly List<Coordinates> initialPositions;

    public Solution12(string[] lines)
    {
        initialPositions = lines.Select(line =>
        {
            var nums = Regex.Matches(line, "(-?[0-9]+)+").Select(x => long.Parse(x.ValueSpan)).ToList();
            return new Coordinates(nums[0], nums[1], nums[2]);
        }).ToList();
    }

    public async ValueTask<long> GetPart1()
    {
        // Setup moons in first position with 0 velocity
        var moons = initialPositions.Select(x => new Moon() { Position = x }).ToArray();

        var linked = new MoonWithInfluencingBodies[moons.Length];
        for (var i = 0; i < moons.Length; i++)
        {
            var related = new List<Moon>(moons);
            related.Remove(moons[i]);
            linked[i] = new(moons[i], related.ToArray());
        }

        for (var i = 1; i <= 1_000; i++)
        {
            MoveOneStep(linked);
        }

        return moons.Sum(x => x.GetEnergy());
    }

    public async ValueTask<long> GetPart2()
    {
        // Setup moons in first position with 0 velocity
        var moons = initialPositions.Select(x => new Moon() { Position = x }).ToArray();
        var xes = initialPositions.Select(p => p.x).ToArray();
        var yes = initialPositions.Select(p => p.y).ToArray();
        var zes = initialPositions.Select(p => p.z).ToArray();

        var linked = new MoonWithInfluencingBodies[moons.Length];
        for (var i = 0; i < moons.Length; i++)
        {
            var related = new List<Moon>(moons);
            related.Remove(moons[i]);
            linked[i] = new(moons[i], related.ToArray());
        }

        // Iterate system until all axes have reset at least once
        long equalX = 0, equalY = 0, equalZ = 0, round = 0;
        while (equalX == 0 || equalY == 0 || equalZ == 0)
        {
            MoveOneStep(linked);
            round++;

            if (equalX == 0 && moons.Select(m => m.Position.x).SequenceEqual(xes) && moons.All(m => m.Velocity.x == 0))
                equalX = round;
            if (equalY == 0 && moons.Select(m => m.Position.y).SequenceEqual(yes) && moons.All(m => m.Velocity.y == 0))
                equalY = round;
            if (equalZ == 0 && moons.Select(m => m.Position.z).SequenceEqual(zes) && moons.All(m => m.Velocity.z == 0))
                equalZ = round;
        }

        // then find the first iteration where all 3 are reset
        var sorted = new long[] { equalX, equalY, equalZ }.OrderByDescending(x => x).ToList();
        return LeastCommonMultiple(LeastCommonMultiple(sorted[0], sorted[1]), sorted[2]);
    }

    private static long GreatestCommonDivisor(long greater, long lesser)
    {
        greater = Math.Abs(greater);
        lesser = Math.Abs(lesser);
        while (true)
        {
            long remainder = greater % lesser;
            if (remainder == 0) return lesser;
            greater = lesser;
            lesser = remainder;
        }
    }

    private static long LeastCommonMultiple(long greater, long lesser) => greater * lesser / GreatestCommonDivisor(greater, lesser);

    private static void MoveOneStep(MoonWithInfluencingBodies[] linked)
    {
        // adjust velocities based on other bodies
        foreach (var (moon, bodies) in linked)
        {
            long x = 0, y = 0, z = 0;
            foreach (var body in bodies)
            {
                if (body.Position.x > moon.Position.x)
                    x++;
                else if (body.Position.x < moon.Position.x)
                    x--;

                if (body.Position.y > moon.Position.y)
                    y++;
                else if (body.Position.y < moon.Position.y)
                    y--;

                if (body.Position.z > moon.Position.z)
                    z++;
                else if (body.Position.z < moon.Position.z)
                    z--;
            }

            moon.Velocity += new Coordinates(x, y, z);
        }

        // adjust positions based on velocity
        foreach (var (moon, _) in linked)
        {
            moon.Position += moon.Velocity;
        }
    }

    private readonly record struct Coordinates(long x, long y, long z)
    {
        public static Coordinates operator +(Coordinates a, Coordinates b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        public long GetEnergy() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);
    }

    // not a struct so we can modify its coordinates while still keeping references in MoonWithInfluencingBodies
    private record Moon
    {
        public Coordinates Position { get; set; }
        public Coordinates Velocity { get; set; }
        public long GetEnergy() => Position.GetEnergy() * Velocity.GetEnergy();
    }

    private record MoonWithInfluencingBodies(Moon moon, Moon[] bodies);
}
