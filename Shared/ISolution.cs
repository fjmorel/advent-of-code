/// <summary>
/// Basic interface for defining a puzzle solution
/// </summary>
public interface ISolution
{
    ValueTask<long> GetPart1() => new (0L);
    ValueTask<long> GetPart2() => new (0L);

    async ValueTask<string> GetPart1String() => (await GetPart1()).ToString();
    async ValueTask<string> GetPart2String() => (await GetPart2()).ToString();
}