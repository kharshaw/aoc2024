// See https://aka.ms/new-console-template for more information

using System.Runtime.CompilerServices;

Console.WriteLine("Hello, Day 12!");


var input = File.ReadAllLines("data/sample.txt");


var map = input
    .Select(line => line.ToCharArray())
    .ToArray();

var plotted = new List<(long y, long x)>();





long cost = 0;

var gardens = new List<List<(long y, long x)>>();

for (var y = 0; y < map.Length; y++)
{
    for (var x = 0; x < map[y].Length; x++)
    {
        if (plotted.Contains((y,x))) continue;
        
        (long perimeter, long area) = CalculatePlot((y, x));

        cost += perimeter * area;

        // the last area items in plotter is our garden
        
        var garden = plotted.Skip(plotted.Count - area).ToList();

        gardens.Add(garden);
    }
}

(long perimeter, long area) CalculatePlot((long y, long x) location)
{
    if (plotted.Contains(location) || location.y < 0 || location.x < 0 || location.y >= map.Length || location.x >= map[0].Length) return (0,0);

    plotted.Add(location);

    var perimeter = 0;
    var area = 1;

    var plant = map[location.y][location.x];

    // check north
    if (location.y == 0) perimeter++;
    if (location.y > 0)
    {
        if (map[location.y - 1][location.x] == plant) 
        {
             (var p, var a) = CalculatePlot((location.y - 1, location.x));
            perimeter += p;
            area += a;
        }
        else
        { 
            perimeter++;
        }
    }

    // check south
    if (location.y == map.Length - 1) perimeter++;
    
    if (location.y < map.Length - 1)
    {
        if (map[location.y + 1][location.x] == plant) 
        {
            (var p, var a) = CalculatePlot((location.y + 1, location.x));
            perimeter += p;
            area += a;
        }
        else
        { 
            perimeter++;
        }
    }

    // check east
    if (location.x == map[0].Length - 1) perimeter++;
    if (location.x < map[0].Length - 1)
    {
        if (map[location.y][location.x + 1] == plant) 
        {
            (var p, var a) = CalculatePlot((location.y, location.x + 1));
            perimeter += p;
            area += a;
        }
        else
        { 
            perimeter++;
        }
    }

    // check west
    if (location.x == 0) perimeter++;
    if (location.x > 0)
    {
        if (map[location.y][location.x - 1] == plant) 
        {
            (var p, var a) = CalculatePlot((location.y, location.x - 1));
            perimeter += p;
            area += a;
        }
        else
        { 
            perimeter++;
        }
    }

    return (perimeter, area);
}

Console.WriteLine($"Part1: Cost: {cost}");

long sideCost = 0;

foreach (var garden in gardens)
{
    sideCost += GetNumSides(garden) * garden.Count;
}

Console.WriteLine($"Part2: Side-based Cost: {sideCost}");

    long GetNumSides(List<(int, int)> coordinates)
    {
        foreach (var coordinate in coordinates)
        {
            Console.Write($"({coordinate.Item1}, {coordinate.Item2})");
        }
        Console.WriteLine();

        long numSides = 0;
        for (long i = 0; i < coordinates.Count; i++)
        {
            var (x, y) = coordinates[i];
            if (i == 0)
            {
                numSides += 2; // Top side of the first block
            }
            if (i == coordinates.Count - 1)
            {
                numSides += 2; // Bottom side of the last block
            }
            if (i > 0 && i < coordinates.Count - 1)
            {
                numSides += 4; // Left and right sides of all blocks except the first and last
            }
        }
        return numSides;
    }
/*
// part 2
(long y, long x) NORTH = (-1, 0);
(long y, long x) SOUTH = (1, 0);
(long y, long x) EAST = (0, 1);
(long y, long x) WEST = (0, -1);


var directions = new List<(long y, long x)>()
{
    NORTH, SOUTH, EAST, WEST
};

foreach (var garden in gardens)
{

    var plant = garden[0];
    var start = plant;
    var sides = 0;
    var edgeAt = NORTH;

    plant = FindGardenEdge(garden, plant, edgeAt); 
    sides++;

    {
        var next = directions.Where(c => garden.Contains((plant.y + c.y, plant.x + c.x))).First();


        if (garden.Contains(Move(plant, EAST)))
        {
            plant = Move(plant, EAST);
        }
    } while (plant != start);

    
}

(long y, long x) Move((long y, long x) location, (long y, long x) direction)
{
    return (location.y + direction.y, location.x + direction.x);
}

bool HasEdgeAt(List<(long y, long x)> garden, (long y, long x) location, (long y, long x) EdgeAt)
{
    return !garden.Contains(Move(location, EdgeAt));
}

(long y, long x) FindGardenEdge(List<(long y, long x)> garden, (long y, long x) plant, (long y, long x) move)
{
        var neighbors = directions.Where(c => garden.Contains((plant.y + c.y, plant.x + c.x))).ToList();

        if (neighbors.Count < 4) return plant;

        return FindGardenEdge(garden, (plant.y + move.y, plant.x + move.x), move);
}


*/