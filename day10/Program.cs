// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Day 10!");


var input = File.ReadAllLines("data/input.txt");


// create a two dimensional array of ints from input
var board = input
    .Select(line => line.ToCharArray().Select(i => int.Parse(i.ToString())).ToArray())
    .ToArray();

// printBoard(board);

var acc = 0;
var rating = 0;

List<(int y, int x)> trailheads = FindTrailheads(board);

foreach (var trailhead in trailheads)
{
    var (y, x) = trailhead;

    var trails = HikeTrail(board, trailhead);

    var trailends = new List<(int y, int x)>();

    var score = trails.Select(t => t[0]).Distinct().Count();

    acc += score;
    rating += trails.Count();


    //Console.WriteLine($"Trail: ({y},{x}) {score}");
    // foreach (var trail in trails)
    // {
    //     foreach (var step in trail)
    //     {
    //         Console.Write($"({step.y}, {step.x}) ");
    //     }
    //     Console.WriteLine();
    // }
}

Console.WriteLine($"Part1: {acc} ");

Console.WriteLine($"Part2: {rating} ");

List<List<(int y, int x)>> HikeTrail(int[][] board, (int y, int x) location)
{
    var trails = new List<List<(int y, int x)>>();

    var locationHeight = board[location.y][location.x];

    if (locationHeight == 9) 
    {
        trails.Add(new List<(int y, int x)>() {location});
        return trails;
    }    

    var nextSteps = new List<(int y, int x)>();

    var north = location.y - 1;
    var south = location.y + 1;
    var east = location.x + 1;
    var west = location.x - 1;

    // check north
    if (north >= 0 && board[north][location.x] == locationHeight + 1) nextSteps.Add((north, location.x));

    // check south
    if (south < board.Length && board[south][location.x] == locationHeight + 1) nextSteps.Add((south, location.x));

    // check east
    if (east < board[0].Length && board[location.y][east] == locationHeight + 1) nextSteps.Add((location.y, east));

    // check west        
    if (west >= 0 && board[location.y][west] == locationHeight + 1) nextSteps.Add((location.y, west));

    foreach (var nextStep in nextSteps)
    {

        var subtrails = HikeTrail(board, nextStep);

        foreach (var trail in subtrails)
        {
            trail.Add(location);
            trails.Add(trail);
        }
    }

    return trails;

}




void printBoard(int[][] board)
{
    for (var y = 0; y < board.Length; y++)
    {
        for (var x = 0; x < board[y].Length; x++)
        {
            Console.Write("{0}", board[y][x]);
        }
        Console.WriteLine();
    }
}

static List<(int y, int x)> FindTrailheads(int[][] board)
{
    var trailheads = new List<(int y, int x)>();

    for (var y = 0; y < board.Length; y++)
    {
        for (var x = 0; x < board[y].Length; x++)
        {
            if (board[y][x] == 0) trailheads.Add((y, x));
        }
    }

    return trailheads;
}