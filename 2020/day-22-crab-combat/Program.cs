using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using static System.Console;

var list = System.IO.File.ReadAllLines("input.txt");


var timer = Stopwatch.StartNew();
WriteLine($"{Part1()} :: {timer.Elapsed}");
timer.Restart();
WriteLine($"{Part2()} :: {timer.Elapsed}");
timer.Stop();

long Part1()
{
	var (p1Deck, p2Deck) = ResetDecks();
	while (p1Deck.Any() && p2Deck.Any())
	{
		int p1 = p1Deck.Dequeue(), p2 = p2Deck.Dequeue();
		if (p1 > p2)
		{
			p1Deck.Enqueue(p1);
			p1Deck.Enqueue(p2);
		}
		else if (p1 < p2)
		{
			p2Deck.Enqueue(p2);
			p2Deck.Enqueue(p1);
		}
		else
		{
			p1Deck.Enqueue(p1);
			p2Deck.Enqueue(p2);
		}
	}
	var size = p1Deck.Count + p2Deck.Count;
	return p1Deck.Concat(p2Deck).Select((card, i) => card * (size - i)).Sum();
}

long Part2()
{
	var (p1Deck, p2Deck) = ResetDecks();
	var result = PlayRecursive(p1Deck, p2Deck);

	var size = result.p1Deck.Count() + result.p2Deck.Count();
	return result.p1Deck.Concat(result.p2Deck).Select((card, i) => card * (size - i)).Sum();
}

(Queue<int> p1Deck, Queue<int> p2Deck) PlayRecursive(Queue<int> p1Deck, Queue<int> p2Deck)
{
	var startingDecks = new List<State>();

	while (p1Deck.Any() && p2Deck.Any())
	{
		if (startingDecks.Any(x => x.p1.SequenceEqual(p1Deck) && x.p2.SequenceEqual(p2Deck)))
		{
			p2Deck.Clear();
			break;
		}
		var p1deckImmutable = ImmutableQueue.CreateRange(p1Deck);
		var p2deckImmutable = ImmutableQueue.CreateRange(p2Deck);
		startingDecks.Add(new(p1deckImmutable, p2deckImmutable));

		int p1 = p1Deck.Dequeue(), p2 = p2Deck.Dequeue();
		if (p1 <= p1Deck.Count && p2 <= p2Deck.Count)
		{
			var result = PlayRecursive(new(p1Deck.Take(p1)), new(p2Deck.Take(p2)));
			var p1Size = result.p1Deck.Count;
			var p2Size = result.p2Deck.Count;
			if (p1Size > p2Size)
			{
				p1Deck.Enqueue(p1);
				p1Deck.Enqueue(p2);
			}
			else if (p2Size > p1Size)
			{
				p2Deck.Enqueue(p2);
				p2Deck.Enqueue(p1);
			}
			else
			{
				p1Deck.Enqueue(p1);
				p2Deck.Enqueue(p2);
			}
		}
		else
		{
			if (p1 > p2)
			{
				p1Deck.Enqueue(p1);
				p1Deck.Enqueue(p2);
			}
			else if (p1 < p2)
			{
				p2Deck.Enqueue(p2);
				p2Deck.Enqueue(p1);
			}
			else
			{
				p1Deck.Enqueue(p1);
				p2Deck.Enqueue(p2);
			}
		}
	}

	return (p1Deck, p2Deck);
}

(Queue<int> p1Deck, Queue<int> p2Deck) ResetDecks()
{
	Queue<int> p1deck = new(), p2deck = new();
	var i = 1;
	for (; list[i] != ""; i++)
		p1deck.Enqueue(int.Parse(list[i]));

	i++; i++;
	for (; i < list.Length; i++)
		p2deck.Enqueue(int.Parse(list[i]));
	return (p1deck, p2deck);
}

record State(ImmutableQueue<int> p1, ImmutableQueue<int> p2);
