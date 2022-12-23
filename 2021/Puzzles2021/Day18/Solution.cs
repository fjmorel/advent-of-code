using System.Text.Json.Nodes;

namespace Puzzles2021.Day18;

public record Solution(string[] _lines) : ISolution<Solution>
{
    public static Solution Init(string[] lines) => new(lines);

    public async ValueTask<long> GetPart1()
    {
        var pairs = _lines.Select(Convert).Select(Simplify).ToList();
        var final = pairs.Skip(1).Aggregate(pairs[0], (left, right) => Simplify(new Pair(left, right)));
        return GetMagnitude(final);
    }

    public async ValueTask<long> GetPart2()
    {
        var max = 0;
        var lineCount = _lines.Length;
        var pairs = Enumerable.Range(0, lineCount).SelectMany(a => Enumerable.Range(0, lineCount).Select(b => (a, b)));
        foreach (var (a, b) in pairs)
        {
            if (a == b)
                continue;
            {
                var pairA = Simplify(Convert(_lines[a]));
                var pairB = Simplify(Convert(_lines[b]));
                var total = GetMagnitude(Simplify(new Pair(pairA, pairB)));
                if (total > max)
                    max = total;
            }
            {
                var pairA = Simplify(Convert(_lines[a]));
                var pairB = Simplify(Convert(_lines[b]));
                var total = GetMagnitude(Simplify(new Pair(pairB, pairA)));
                if (total > max)
                    max = total;
            }
        }

        return max;
    }

    private static void Print(INode node, int level = 0)
    {
        switch (node)
        {
            case Pair pair:
                Print(pair.left, level + 1);
                Print(pair.right, level + 1);
                break;
            case Value val:
                AnsiConsole.WriteLine(new string('-', level) + val.value);
                break;
        }
    }

    private static int GetMagnitude(INode node)
    {
        var value = node switch
        {
            Value leaf => leaf.value,
            Pair pair => GetMagnitude(pair.left) + GetMagnitude(pair.right),
            _ => throw new ArgumentOutOfRangeException(nameof(node), node, null),
        };
        return ((!node.IsRight && !node.IsLeft) ? 1 : node.IsLeft ? 3 : 2) * value;
    }

    private static Pair Simplify(Pair pair)
    {
        // AnsiConsole.WriteLine("Before");
        // Print(pair);
        while (TryExplode(pair, 1) || TrySplit(pair)) { }
        // AnsiConsole.WriteLine("After");
        // Print(pair);

        return pair;
    }

    private static bool TryExplode(Pair pair, int level)
    {
        // Explode, or keep searching
        if (level > 4 && pair.left is Value lValue && pair.right is Value rValue)
        {
            var isRight = (pair as INode).IsRight;
            var parent = pair.Parent!;
            ExplodeLeft(lValue.value, isRight, parent);
            ExplodeRight(rValue.value, isRight, parent);
            // Then replace the exploded pair with 0
            if (isRight)
                parent.right = new Value(0) { Parent = parent };
            else
                parent.left = new Value(0) { Parent = parent };
            return true;
        }

        // Keep searching down the left side, but fall back to the right side
        var subLevel = level + 1;
        return (pair.left is Pair lPair && TryExplode(lPair, subLevel)) || (pair.right is Pair rPair && TryExplode(rPair, subLevel));
    }

    private static void ExplodeRight(int num, bool isRight, Pair? parent)
    {
        // As long as the parent is on the right side, keep going up /
        if (isRight)
        {
            while (parent is INode { IsRight: true })
                parent = parent.Parent;
            // If no further to go, then the pair was right-most
            // Then do nothing
            if (parent?.Parent == null)
                return;
            parent = parent.Parent;
        }

        // Otherwise go down right
        // If it's a value, go ahead and modify it
        var node = parent!.right;
        if (node is Value rValue)
        {
            parent.right = rValue with { value = rValue.value + num };
            return;
        }

        // Otherwise keep down the left and modify the left-most value
        parent = node as Pair;
        while (parent!.left is Pair child)
            parent = child;

        var lValue = parent.left as Value;
        parent.left = lValue! with { value = lValue.value + num };
    }

    private static void ExplodeLeft(int num, bool isRight, Pair? parent)
    {
        // As long as the parent is on the left side, keep going up /
        if (!isRight)
        {
            while (parent is INode { IsLeft: true })
                parent = parent.Parent;
            // If no further to go, then the pair was left-most
            // Then do nothing
            if (parent?.Parent == null)
                return;
            parent = parent.Parent;
        }

        // Otherwise go down left
        // If it's a value, go ahead and modify it
        var node = parent!.left;
        if (node is Value lValue)
        {
            parent.left = lValue with { value = lValue.value + num };
            return;
        }

        // Otherwise keep going down right and modify the right-most value
        parent = node as Pair;
        while (parent!.right is Pair child)
            parent = child;

        var rValue = parent.right as Value;
        parent.right = rValue! with { value = rValue.value + num };
    }

    private static bool TrySplit(Pair pair)
    {
        if (pair.left is Value left)
        {
            if (left.value >= 10)
            {
                pair.left = left.Split(pair);
                return true;
            }
        }
        else
        {
            var found = TrySplit((Pair)pair.left);
            if (found)
                return true;
        }

        if (pair.right is Value right)
        {
            if (right.value >= 10)
            {
                pair.right = right.Split(pair);
                return true;
            }
        }
        else
        {
            var found = TrySplit((Pair)pair.right);
            if (found)
                return true;
        }

        return false;
    }

    private static Pair Convert(string line) => (Convert(JsonNode.Parse(line)) as Pair)!;

    private static INode Convert(JsonNode? node) => node switch
    {
        JsonValue value => new Value(value.GetValue<int>()),
        JsonArray array => new Pair(Convert(array[0]!), Convert(array[1]!)),
        _ => throw new ArgumentException("Unexpected JsonNode type"),
    };

    private interface INode
    {
        Pair? Parent { get; set; }

        bool IsRight => Parent != null && Parent.right == this;
        bool IsLeft => Parent != null && Parent.left == this;
    }

    private record Pair : INode
    {
        public Pair(INode left, INode right)
        {
            this.left = left;
            this.left.Parent = this;

            this.right = right;
            this.right.Parent = this;
        }

        public INode left { get; set; }
        public INode right { get; set; }
        public Pair? Parent { get; set; }

        public Pair GetTopNode()
        {
            var pair = this;
            while (pair.Parent != null)
                pair = pair.Parent;
            return pair;
        }
    };

    private record Value(int value) : INode
    {
        public Pair? Parent { get; set; }

        public Pair Split(Pair parent)
        {
            // Left is rounded down
            // Right is rounded up (so see if it's even first to see if we should add one to get the next value)
            var newLeft = value / 2;
            var newRight = (value + (value % 2 == 0 ? 0 : 1)) / 2;
            return new Pair(new Value(newLeft), new Value(newRight)) { Parent = parent };
        }
    }
}
