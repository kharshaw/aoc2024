// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Day 12!");


var input = File.ReadAllLines("data/simple.txt");


var map = input
    .Select(line => line.ToCharArray())
    .ToArray();

var plotted = new List<(int y, int x)>();





int cost = 0;

var gardens = new List<List<(int y, int x)>>();

for (var y = 0; y < map.Length; y++)
{
    for (var x = 0; x < map[y].Length; x++)
    {
        if (plotted.Contains((y,x))) continue;
        
        (int perimeter, int area) = CalculatePlot((y, x));

        cost += perimeter * area;

        // the last area items in plotter is our garden
        
        var garden = plotted.Skip(plotted.Count - area).ToList();

        gardens.Add(garden);
    }
}

Console.WriteLine($"Part1: Cost: {cost}");

// part 2

foreach (var garden in gardens)
{
    var checks = new List<(int y, int x)>()
    {
        (-1, 0), (1, 0), (0, -1), (0, 1)
    };

    var plant = garden[0];

    var sides = 1;

    
}


(int perimeter, int area) CalculatePlot((int y, int x) location)
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