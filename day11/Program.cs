// See https://aka.ms/new-console-template for more information
using System.ComponentModel.DataAnnotations;
using System.Net.Http.Headers;
using System.Numerics;
using System.Reflection.Metadata;

Console.WriteLine("Hello, Day 11!");


var input = File.ReadAllLines("data/input.txt");

var blinks = 25;

if (args.Length > 0) blinks = int.Parse(args[0]);

Dictionary<(long stone, long blink), long > memo = new Dictionary<(long stone, long blink), long >();

if (true)
{
    var stopwatch = new System.Diagnostics.Stopwatch();

    var stuff = input[0].Split(" ").Select(s => long.Parse(s)).ToList();
    long stonesCreated = 0;

    stopwatch.Start();

    foreach (var stone in stuff)
    {
        stonesCreated++;
        var newStones  = Blink2(stone, 1, blinks);  

        stonesCreated += newStones;

        Console.WriteLine($"Finished stone {stone} finding {newStones} stones in {stopwatch.ElapsedMilliseconds} ms");

    }

    stopwatch.Stop();
    Console.WriteLine($"Completed in {stopwatch.ElapsedMilliseconds} ms");

    Console.WriteLine($"Solution after {blinks} blinks: {stonesCreated}");
    return;
}

var stones = new BigIntegerLinkedList();
input[0].Split(" ").Select(i => BigInteger.Parse(i)).ToList().ForEach(i => stones.Add(i));

PrintStones(stones);

for (var i = 0; i < 25; i++)
{
    Blink(stones);
    // PrintStones(stones);
}

Console.WriteLine($"Part1: {stones.Count}");

// return;


for (var i = 0; i < 50; i++)
{
    Blink(stones);

    // PrintStones(stones);
    
    Console.WriteLine($"Blink: {i + 25}");
}

Console.WriteLine($"Part2: {stones.Count}");

void PrintStones(BigIntegerLinkedList stones)
{
    for (var i = 0; i < stones.Count; i++)
    {
        Console.Write("{0} ", stones[i]);
    }
    Console.WriteLine();
}

void Blink(BigIntegerLinkedList stones)
{

    var stone = stones.Head!;
    
    // check if stone.Next is null and return


    do
    {
        
        if (stone.Value == 0)
        {
            stones.UpdateNode(stone, BigInteger.One);
            stone = stone.Next;
            continue;
        }

        var digits = (long)BigInteger.Log10(stone.Value) + 1;
        
        if (digits % 2 == 0)
        {
            var (firstPart, secondPart) = SplitBigInteger(stone.Value);
            stone.Value = firstPart;
            stones.InsertAfter(stone, secondPart);
            stone = stone.Next!.Next;

            continue;
        }

        var value = stone.Value;
        stone.Value = BigInteger.Multiply(value, 2024);
        stone = stone.Next;
        
    } while (stone != null);
    
}


long Blink2(long stone, long blink, int max)
{

    if (blink > max) return 0;

    if (memo.ContainsKey((stone, blink)))
    {
        return memo[(stone, blink)];
    }

    long created = 0;

    var digits = (long)Math.Log10(stone) + 1;
  
    if (stone == 0)
    {
        created += Blink2(1, blink + 1, max);
    }
    else if (digits % 2 == 0)
    {
        var (firstPart, secondPart) = SplitNumber(stone);

        created++;

        created += Blink2(firstPart, blink + 1, max);
        created += Blink2(secondPart, blink + 1, max);
        
    } 
    else
    {
        created += Blink2(stone * 2024, blink + 1, max);   
    } 

    if (!memo.ContainsKey((stone, blink)))  memo.Add((stone, blink), created);

    return created;
    
}

(BigInteger, BigInteger) SplitBigInteger(BigInteger big)
{
    var digits = (int)BigInteger.Log10(big) + 1;
    int halfDigits = digits / 2;

    BigInteger divisor = BigInteger.Pow(10, halfDigits);

    var firstPart = new BigInteger();

    var secondPart = BigInteger.DivRem(big, divisor, out firstPart);

    return (firstPart, secondPart);
}

(long firstPart, long secondPart) SplitNumber(long number)
{
    var digits = (int)Math.Log10(number) + 1;
    int halfDigits = digits / 2;

    var divisor = (long)Math.Pow(10, halfDigits);

    var firstPart = number/divisor;

    var secondPart = number % divisor;

    return (firstPart, secondPart);
}

#region helpers
/*

 Linked List

 */
 
public class BigIntegerNode
{
    public BigInteger Value { get; set; }
    public BigIntegerNode? Next { get; set; }

    public BigIntegerNode(BigInteger value)
    {
        Value = value;
        Next = null;
    }
}

public class BigIntegerLinkedList
{
    public BigIntegerNode? Head { get; set; }
    public BigIntegerNode? Tail { get; set; }
    public BigInteger Count { get; set; }

    public BigIntegerLinkedList()
    {
        Head = null;
        Tail = null;
        Count = 0;
    }

    public void Add(BigInteger value)
    {
        BigIntegerNode node = new BigIntegerNode(value);
        if (Head == null)
        {
            Head = node;
            Tail = node;
        }
        else
        {
            Tail!.Next = node;
            Tail = node;
        }
        Count++;
    }

    public void Remove(BigInteger value)
    {
        if (Head == null) return;

        if (Head.Value.Equals(value))
        {
            Head = Head.Next;
            if (Head == null) Tail = null;
            Count--;
            return;
        }

        BigIntegerNode current = Head;
        while (current.Next != null)
        {
            if (current.Next.Value.Equals(value))
            {
                current.Next = current.Next.Next;
                if (current.Next == null) Tail = current;
                Count--;
                return;
            }
            current = current.Next;
        }
    }

    public void RemoveNode(BigIntegerNode node)
    {
        if (Head == null) return;

        if (Head == node)
        {
            Head = Head.Next;
            if (Head == null) Tail = null;
            Count--;
            return;
        }

        BigIntegerNode current = Head;
        while (current.Next != null)
        {
            if (current.Next == node)
            {
                current.Next = current.Next.Next;
                if (current.Next == null) Tail = current;
                Count--;
                return;
            }
            current = current.Next;
        }
    }

    public void InsertAfter(BigIntegerNode node, BigInteger value)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));

        BigIntegerNode newNode = new BigIntegerNode(value);
        newNode.Next = node.Next;
        node.Next = newNode;

        if (newNode.Next == null) Tail = newNode;
        Count++;
    }

    public void UpdateNode(BigIntegerNode node, BigInteger newValue)
    {
        if (node == null) throw new ArgumentNullException(nameof(node));
        node.Value = newValue;
    }

    public BigInteger this[int index]
    {
        get
        {
            if (index < 0 || index >= Count) throw new IndexOutOfRangeException();
            BigIntegerNode? current = Head;
            for (int i = 0; i < index; i++)
            {
                current = current!.Next;
            }
            return current!.Value;
        }
    }
}

#endregion