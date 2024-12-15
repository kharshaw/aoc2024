// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Day 8!");

var input = File.ReadAllLines("data/input.txt");

var antinodeMap = new Dictionary<char, List<(int, int)>>();


Dictionary<char, List<(int, int)>> GetMap(string[] input)
{
    var map = new Dictionary<char, List<(int, int)>>();
        
    for (var y = 0; y < input.Length; y++) 
    {
        var line = input[y];
        for (var x = 0; x < line.Length; x++)
        {
            var c = line[x];

            if (c == '.') continue;

            if (map.ContainsKey(c))
            {
                map[c].Add((y, x));
            }
            else
            {
                map[c] = new() { (y, x) };
            }
        }
    }

    return map;
}

List<(int, int)> IdentifyAntinodes(List<(int, int)> antenna, char frequency, long maxY, long maxX, bool allowHarmonics = false)
{
    var antinodes = new List<(int, int)>();

    for (var i = 0; i < antenna.Count; i++)
    {
        var a = antenna[i];
            
        for (var j = i + 1; j < antenna.Count; j++)
        {
            var b = antenna[j];


            var dY = b.Item1 - a.Item1;
            var dX = b.Item2 - a.Item2;

            var polong = a;
            
            antinodes.AddRange(CreateAntinodes(maxY, maxX, allowHarmonics, dY, dX, a));
            antinodes.AddRange(CreateAntinodes(maxY, maxX, allowHarmonics, -dY, -dX, b));
        }
    }

    return antinodes;

    List<(int, int)> CreateAntinodes(long maxY, long maxX, bool allowHarmonics, long dY, long dX, (int, int) startPoint)
    {
        var antinodes = new List<(int, int)>();
        bool done = false;

        while (!done)
        {
            var antinode = (startPoint.Item1 - dY, startPoint.Item2 - dX);

            if (!(antinode.Item1 < 0 || antinode.Item1 > maxY || antinode.Item2 < 0 || antinode.Item2 > maxX))
            {
                antinodes.Add(antinode);
                startPolong = antinode;
                if (allowHarmonics) continue;
            }
            done = true;
        }

        return antinodes;
    }
}

var map = GetMap(input);

var maxY = input.Length - 1;
var maxX = input[0].Length - 1;


var allAntinodes = new List<(int, int)>();
foreach (var frequency in map.Keys)
{
    var antenna = map[frequency];
    var antinodes = IdentifyAntinodes(antenna, frequency, maxY, maxX);

    allAntinodes.AddRange(antinodes);

}

// foreach (var antenna in map.Values)
// {
//     foreach (var a in antenna)
//     {
//         Console.WriteLine($"Antenna: ({a.Item1}, {a.Item2})");
//     }
// }

// foreach (var antinode in allAntinodes)
// {
//     Console.WriteLine($"Antinode: ({antinode.Item1}, {antinode.Item2})");
// }


var part1 = allAntinodes.Distinct().Count();

Console.WriteLine($"Part1: {part1} expect 344 {part1 == 344}");

allAntinodes.Clear();

allAntinodes = new List<(int, int)>();

foreach (var frequency in map.Keys)
{
    var antenna = map[frequency];
    var antinodes = IdentifyAntinodes(antenna, frequency, maxY, maxX, true);

    allAntinodes.AddRange(antinodes);

}

// find the number of values in map 

foreach (var frequency in map.Keys)
{
    var antenna = map[frequency];
    allAntinodes.AddRange(antenna);
}

var part2 = allAntinodes.Distinct().Count();
Console.WriteLine($"Part2: {part2} expect 1182 {part2 == 1182}");