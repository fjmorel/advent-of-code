namespace Puzzles2020.Solutions;

public class Solution25 : ISolution
{
    private readonly string[] _lines;

    public Solution25(string[] lines)
    {
        _lines = lines;
    }

    public async ValueTask<long> GetPart1()
    {
        var publicKeys = _lines.ParseLongs();
        var cardPublic = publicKeys[0];
        var doorPublic = publicKeys[1];

        var cardLoop = GetLoopSizeFromPublicKey(cardPublic);
        var doorLoop = GetLoopSizeFromPublicKey(doorPublic);

        var encryption1 = PublicToEncryptionKey(doorPublic, cardLoop);
        var encryption2 = PublicToEncryptionKey(cardPublic, doorLoop);
        Debug.Assert(encryption1 == encryption2);
        return encryption1;
    }

    public async ValueTask<long> GetPart2()
    {
        return 0;
    }

    long PublicToEncryptionKey(long publicKey, long loopSize)
    {
        long subject = publicKey, current = 1;
        for (var i = 1; i <= loopSize; i++)
        {
            current = (current * subject) % 20201227;
        }

        return current;
    }

    long GetLoopSizeFromPublicKey(long publicKey)
    {
        int subject = 7, loopSize = 0, current = 1;
        while (current != publicKey)
        {
            loopSize++;
            current = (current * subject) % 20201227;
        }

        return loopSize;
    }
}