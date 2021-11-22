using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");

var publicKeys = list.Select(long.Parse).ToList();
var cardPublic = publicKeys[0];
var doorPublic = publicKeys[1];


var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var cardLoop = GetLoopSizeFromPublickKey(cardPublic);
	var doorLoop = GetLoopSizeFromPublickKey(doorPublic);

	var encryption1 = PublicToEncryptionKey(doorPublic, cardLoop);
	var encryption2 = PublicToEncryptionKey(cardPublic, doorLoop);
	Debug.Assert(encryption1 == encryption2);
	return encryption1;
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

long GetLoopSizeFromPublickKey(long publicKey)
{
	int subject = 7, loopSize = 0, current = 1;
	while (current != publicKey)
	{
		loopSize++;
		current = (current * subject) % 20201227;
	}
	return loopSize;
}