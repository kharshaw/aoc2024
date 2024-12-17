// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Day 15!");

var input = File.ReadAllLines("/home/keith/github/AoC2024/day15/data/input.txt");


// Find the index of the zero-length string
int separatorIndex = Array.IndexOf(input, "");

// Split the array into two regions
string[] mapRegion = input.Take(separatorIndex).ToArray();
string[] dataRegion = input.Skip(separatorIndex + 1).ToArray();

// Convert the map region into a 2D array of chars
char[,] map = new char[mapRegion.Length, mapRegion[0].Length];
for (int i = 0; i < mapRegion.Length; i++)
{
    for (int j = 0; j < mapRegion[i].Length; j++)
    {
        map[i, j] = mapRegion[i][j];
    }
}

// Convert the data region into a 1D array of chars
char[] moves = dataRegion.SelectMany(s => s).ToArray();


var steps = new Dictionary<char, (int y, int x)>()
{
    {'^', (-1, 0)},
    {'v', (1, 0)},
    {'<', (0, -1)},
    {'>', (0, 1)}
};

(int y, int x) position = FindRobot(map);

// Console.WriteLine($"Position: {position!.y} {position.x}");

// PrintMap();

foreach (var movement in moves)
{
    // Console.WriteLine($"Moving {movement}");
    if (Move(movement, position!))
    {
        position = FindRobot(map);    
    }
    
    // PrintMap();
    // Console.WriteLine($"Position Now: {position!.y} {position.x}");
}

var coord = 0L;

for (var i = 0; i < map.GetLength(0); i++)
{
    for (var j = 0; j < map.GetLength(1); j++)
    {
        if (map[i, j] == 'O')
        {
            coord += (i * 100 + j);
        }
    }
}

PrintMap();
Console.WriteLine($"Part1: {coord}");

bool Move(char direction, (int y, int x) fromPosition)
{
    void Swap((int y, int x) a, (int y, int x) b)
    {
        char temp = map[a.y, a.x];
        map[a.y, a.x] = map[b.y, b.x];
        map[b.y, b.x] = temp;
    }

    (int y, int x) toPosition = (fromPosition.y + steps[direction].y, fromPosition.x + steps[direction].x);

    if (map[toPosition.y, toPosition.x] == '#')
    {
        return false;
    }
    
    if (map[toPosition.y, toPosition.x] == '.')
    {
        Swap(fromPosition, toPosition);
        return true;
    }

    
    var moved = Move(direction, toPosition);
    
    if (moved)
    { 
        Swap(fromPosition, toPosition);
    }


    return moved;


}

void PrintMap()
{
    for (int i = 0; i < map.GetLength(0); i++)
    {
        for (int j = 0; j < map.GetLength(1); j++)
        {
            Console.Write(map[i, j]);
        }
        Console.WriteLine();
    }
}

static (int y, int x) FindRobot(char[,] map)
{
    return Enumerable.Range(0, map.GetLength(0))
        .SelectMany(i => Enumerable.Range(0, map.GetLength(1))
            .Select(j => new { i, j })
            .Where(x => map[x.i, x.j] == '@')
            .Select(x => (y: x.i, x: x.j)))
        .FirstOrDefault();
}