// See https://aka.ms/new-console-template for more information
using System.Net.NetworkInformation;

Console.WriteLine("Hello, Day 5");

var input = File.ReadAllLines("data/input.txt");

var rules = new Dictionary<int, List<int>>();

var i = 0;
while (i < input.Length && input[i] != "")  
{
    var split = input[i].Split("|");
    var key = int.Parse(split[0]);
    var value = int.Parse(split[1]);
    
    if (!rules.ContainsKey(key)) rules[key] = new List<int>();
    
    rules[key].Add(value);

    i++;
}

i++;

var updates = new List<List<int>>();
while (i < input.Length)
{
    var split = input[i].Split(",");
    updates.Add(split.Select(int.Parse).ToList());   

    i++;
}

// find updates ordered properly
var ordered = new List<List<int>>();
var unordered = new List<List<int>>();

var isOrdered = true;
foreach (var update in updates)
{
    for (var p =1; p < update.Count; p++)
    {
        var page = update[p];
        if (rules.ContainsKey(page) && rules[page].Intersect(update.Take(p)).Any())
        {
            // rule violation
            isOrdered = false;
            break;
        }
    }
    
    if (isOrdered)
    {
        ordered.Add(update);
    }
    else
    {
        unordered.Add(update);
    }
    
    isOrdered = true;
}

var acc = 0;

foreach (var update in ordered)
{
    acc += update[update.Count/2 ];
}
Console.WriteLine($"Part1: {acc} {acc == 5129}");

acc = 0;

static bool CheckOrder(List<int> update, Dictionary<int, List<int>> rules)
{
    var isOrdered = true;

    for (var p = 0; p < update.Count; p++)
    {
        var page = update[p];
        if (rules.ContainsKey(page) && rules[page].Intersect(update.Take(p)).Any())
        {
            // take out the misordered page
            update.Remove(page);

            var others = rules[page].Intersect(update.Take(p));

            update.Insert(update.IndexOf(others.First()), page);

            isOrdered = false;
        }
    }

    return isOrdered;
}

foreach (var update in unordered)
{
    isOrdered = CheckOrder(update, rules);

    while (!isOrdered)    
    {
        isOrdered = CheckOrder(update, rules);
    }
}

foreach (var update in unordered)
{
    acc += update[update.Count/2 ];
}


Console.WriteLine($"Part2: {acc} {acc == 4077}");