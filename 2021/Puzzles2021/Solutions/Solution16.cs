using System.Globalization;

namespace Puzzles2021.Solutions;

public class Solution16 : ISolution
{
    private readonly IPacket _parsed;

    public Solution16(string[] lines)
    {
        var bits = lines[0]
            .SelectMany(static symbol => Convert.ToString(int.Parse(symbol.ToString(), NumberStyles.HexNumber), 2)
                .PadLeft(4, '0')
                .Select(x => x == '1'))
            .ToArray().AsSpan();
        var offset = 0;
        _parsed = ParsePacket(bits, ref offset);
    }

    public async ValueTask<long> GetPart1() => _parsed.GetTotalVersion();

    // 2904570676 too low!?
    public async ValueTask<long> GetPart2() => _parsed.GetTotal();

    private static long Read(int size, ReadOnlySpan<bool> bits, ref int offset) => ToNumber(bits[offset..(offset += size)]);

    private static IPacket ParsePacket(ReadOnlySpan<bool> bits, ref int offset)
    {
        var version = Read(3, bits, ref offset);
        var type = Read(3, bits, ref offset);
        return type switch
        {
            4 => new LiteralValue(version, ParseLiteralValue(bits, ref offset)),
            _ => new Operator(version, type, ParseOperator(bits, ref offset)),
        };
    }

    private static long ParseLiteralValue(ReadOnlySpan<bool> bits, ref int offset)
    {
        var numberBits = new List<bool>();
        var isLast = false;
        while (!isLast)
        {
            isLast = !bits[offset];
            foreach (var bit in bits[(offset += 1)..(offset += 4)])
                numberBits.Add(bit);
        }

        return ToNumber(numberBits.ToArray().AsSpan());
    }

    private static List<IPacket> ParseOperator(ReadOnlySpan<bool> bits, ref int offset)
    {
        var lengthType = Read(1, bits, ref offset);
        return lengthType switch
        {
            0 => ParseLengthOperator(bits, ref offset),
            1 => ParseSubPacketsOperator(bits, ref offset),
            _ => throw new ArgumentException("Unexpected type: " + lengthType),
        };
    }

    private static List<IPacket> ParseLengthOperator(ReadOnlySpan<bool> bits, ref int offset)
    {
        var length = (int)Read(15, bits, ref offset);
        var section = bits[offset..(offset += length)];
        var sectionOffset = 0;

        var children = new List<IPacket>();
        while (sectionOffset < section.Length && !AllZeros(section[sectionOffset..]))
            children.Add(ParsePacket(section, ref sectionOffset));
        return children;
    }

    private static bool AllZeros(ReadOnlySpan<bool> bits)
    {
        foreach (var bit in bits)
            if (bit)
                return false;
        return true;
    }

    private static List<IPacket> ParseSubPacketsOperator(ReadOnlySpan<bool> bits, ref int offset)
    {
        var length = (int)Read(11, bits, ref offset);
        var children = new List<IPacket>(length);
        for (var i = 0; i < length; i++)
            children.Add(ParsePacket(bits, ref offset));
        return children;
    }

    private static long ToNumber(ReadOnlySpan<bool> bits)
    {
        var num = 0L;
        var factor = 1L;
        for (var i = bits.Length - 1; i >= 0; i--)
        {
            num += (bits[i] ? 1 : 0) * factor;
            factor *= 2;
        }

        return num;
    }

    public interface IPacket
    {
        public long GetTotalVersion();
        public long GetTotal();
    }

    public record LiteralValue(long Version, long Value) : IPacket
    {
        public long GetTotalVersion() => Version;
        public long GetTotal() => Value;
    }

    public record Operator(long Version, long Type, List<IPacket> Children) : IPacket
    {
        public long GetTotalVersion() => Version + Children.Sum(x => x.GetTotalVersion());

        public long GetTotal() => Type switch
        {
            0 => Children.Sum(x => x.GetTotal()),
            1 => Children.Aggregate(1L, (product, packet) => product * packet.GetTotal()),
            2 => Children.Min(x => x.GetTotal()),
            3 => Children.Max(x => x.GetTotal()),
            5 => Children[0].GetTotal() > Children[1].GetTotal() ? 1 : 0,
            6 => Children[0].GetTotal() < Children[1].GetTotal() ? 1 : 0,
            7 => Children[0].GetTotal() == Children[1].GetTotal() ? 1 : 0,
            _ => throw new ArgumentException("Unexpected type: " + Type),
        };
    }
}
