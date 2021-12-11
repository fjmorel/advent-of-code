/// <summary>
/// Basic interface for defining a puzzle solution
/// </summary>
public interface ISolution
{
    ValueTask<long> GetPart1();
    ValueTask<long> GetPart2();
}