internal static class Extensions
{
    internal static List<long> ParseLongs(this string[] lines) => lines.Select(x => long.Parse(x)).ToList();
    internal static List<int> ParseInts(this string[] lines) => lines.Select(x => int.Parse(x)).ToList();
    internal static int[] ParseCsvInts(this string line) => line.Split(',').Select(int.Parse).ToArray();
}
