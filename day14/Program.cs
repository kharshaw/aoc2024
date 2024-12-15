// See https://aka.ms/new-console-template for more information
using System.Data;
using System.Dynamic;
using System.Text.RegularExpressions;

Console.WriteLine("Hello, Day 14!");


// var input = File.ReadAllLines("data/sample.txt");
var input = File.ReadAllLines("data/input.txt");

const int WIDTH = 101;
const int HEIGHT = 103;

const int TIME = 100;

var map = new long[HEIGHT, WIDTH];

// initialize map with 0
for (var y = 0; y < HEIGHT; y++)
{
    for (var x = 0; x < WIDTH; x++)
    {
        map[y, x] = 0;
    }
}


var robots = new List<Robot>();


var regex = new Regex(@"^p=([-?\d]+),([-?\d]+) v=([-?\d]+),([-?\d]+)$");
foreach (var line in input)
{
    var match = regex.Match(line);

    var p = (Int64.Parse(match.Groups[2].Value), Int64.Parse(match.Groups[1].Value));
    var v = (Int64.Parse(match.Groups[4].Value), Int64.Parse(match.Groups[3].Value));

    robots.Add(new Robot(p, v, map));

}

// PrintMap(map);

for (var i = 0; i < TIME; i++)
{
    foreach (var robot in robots)
    {
        robot.Step();
    }
}

// PrintMap(map);

var quad1 = 0L;
var quad2 = 0L;
var quad3 = 0L;
var quad4 = 0L;

for (var y = 0; y < HEIGHT; y++)
{
    for (var x = 0; x < WIDTH; x++)
    {
        if (y < HEIGHT / 2 && x < WIDTH / 2) quad1 += map[y, x];
        else if (y < HEIGHT / 2 && x >= WIDTH / 2 + 1) quad2 += map[y, x];
        else if (y >= HEIGHT / 2 + 1 && x < WIDTH / 2) quad3 += map[y, x];
        else if (y >= HEIGHT / 2 + 1 && x >= WIDTH / 2 + 1) quad4 += map[y, x];
    }
}

Console.WriteLine($"Part1: {quad1 * quad2 * quad3 * quad4}");

Console.WriteLine($"Robots: {robots.Count}");

void PrintMap(long[,] map)
{
    for (var y = 0; y < HEIGHT; y++)
    {
        for (var x = 0; x < WIDTH; x++)
        {
            Console.Write(map[y, x]);
        }
        Console.WriteLine();
    }
    Console.WriteLine();
}


public class Robot
{
    public (long y, long x) Position { get; private set; }
    public(long y, long x) Velocity { get; private set; }
    public long[,] Map { get; private set; }

    public Robot((long y, long x) position, (long y, long x) velocity, long[,] map)
    {
        this.Position = position;
        this.Velocity = velocity;
        this.Map = map;

        Map[position.y, position.x]++;
    }

/*************  ✨ Codeium Command ⭐  *************/
        /// <summary>
        /// Move the robot according to its velocity, taking wrap-around into account.
        /// </summary>
/******  9a154f9c-8700-4e26-8eff-d891ab5dbe93  *******/ 
    public void Step()
    {
        var newY = Position.y + Velocity.y;

        newY = newY < 0 ? Map.GetLength(0) + newY : newY > Map.GetLength(0) - 1 ? newY - Map.GetLength(0) : newY;

        var newX = Position.x + Velocity.x;
        newX = newX < 0 ? Map.GetLength(1) + newX : newX > Map.GetLength(1) - 1 ? newX - Map.GetLength(1) : newX;

        Map[Position.y, Position.x]--;

        Map[newY, newX]++;

        Position = (newY, newX);

        return;
    }

    public void Print()
    {
        Console.WriteLine($"Position: X: {Position.x} Y: {Position.y}");
        Console.WriteLine($"Velocity: X: {Velocity.x} Y: {Velocity.y}");
        Console.WriteLine();
    }
}