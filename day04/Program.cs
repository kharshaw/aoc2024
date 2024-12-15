using System.Security.Cryptography.X509Certificates;
using System.Xml.Serialization;

var lines  = File.ReadAllLines("data/input.txt");


static bool findEast(long y, long x, char[][] board)
{
    if (x + 3 > board[y].Length - 1) return false;

    if (board[y][x] != 'X') return false;
    if (board[y][x + 1] != 'M') return false;   
    if (board[y][x + 2] != 'A') return false;
    if (board[y][x + 3] != 'S') return false;

    return true;
}

static bool findWest(long y, long x, char[][] board)
{
    if (x - 3 < 0) return false;

    if (board[y][x] != 'X') return false;
    if (board[y][x - 1] != 'M') return false;   
    if (board[y][x - 2] != 'A') return false;
    if (board[y][x - 3] != 'S') return false;

    return true;
}

static bool findNorth(long y, long x, char[][] board)
{
    if (y - 3 < 0) return false;

    if (board[y][x] != 'X') return false;
    if (board[y-1][x] != 'M') return false;   
    if (board[y-2][x] != 'A') return false;
    if (board[y-3][x] != 'S') return false;

    return true;
}

static bool findSouth(long y, long x, char[][] board)
{
    if (y + 3 > board.Length -1) return false;

    if (board[y][x] != 'X') return false;
    if (board[y+1][x] != 'M') return false;   
    if (board[y+2][x] != 'A') return false;
    if (board[y+3][x] != 'S') return false;

    return true;
}

static bool findSouthEast(long y, long x, char[][] board)
{
    // diagonal down
    if (y + 3 > board.Length -1 || x + 3 > board[y].Length  -1)return false;

    if (board[y][x] != 'X') return false;
    if (board[y+1][x+1] != 'M') return false;   
    if (board[y+2][x+2] != 'A') return false;
    if (board[y+3][x+3] != 'S') return false;

    return true;
}

static bool findNorthWest(long y, long x, char[][] board)
{
    // diagonal up, backwards
    if (y - 3 < 0 || x - 3 < 0)return false;

    if (board[y][x] != 'X') return false;
    if (board[y-1][x-1] != 'M') return false;   
    if (board[y-2][x-2] != 'A') return false;
    if (board[y-3][x-3] != 'S') return false;

    return true;
}

static bool findNorthEast(long y, long x, char[][] board)
{
    // diagonal up
    if (y - 3 < 0 || x + 3 > board[y].Length - 1)return false;

    if (board[y][x] != 'X') return false;
    if (board[y-1][x+1] != 'M') return false;   
    if (board[y-2][x+2] != 'A') return false;
    if (board[y-3][x+3] != 'S') return false;

    return true;
}

static bool findSouthWest(long y, long x, char[][] board)
{
    // diagonal down, backwards
    if (y + 3 > board.Length - 1 || x - 3 < 0)return false;

    if (board[y][x] != 'X') return false;
    if (board[y+1][x-1] != 'M') return false;   
    if (board[y+2][x-2] != 'A') return false;
    if (board[y+3][x-3] != 'S') return false;

    return true;
}

static bool findCrossedMas(long y, long x, char[][] board)
{
    if (x + 2 > board[y].Length - 1 || y + 2 > board.Length - 1) return false;

    bool southeast = ((board[y][x] == 'M' && board[y+1][x+1] == 'A' && board[y+2][x+2] == 'S') ) || ((board[y][x] == 'S' && board[y+1][x+1] == 'A' && board[y+2][x+2] == 'M') );
    bool northeast = ((board[y+2][x] == 'M' && board[y+1][x+1] == 'A' && board[y][x+2] == 'S') ) || ((board[y+2][x] == 'S' && board[y+1][x+1] == 'A' && board[y][x+2] == 'M') );
    return southeast && northeast;
}

// create a two dimensional array from lines
var board = lines
    .Select(line => line.ToCharArray())
    .ToArray();

var north = 0;
var south = 0;
var west = 0;
var east = 0;
var northwest = 0;
var northeast = 0;
var southeast = 0;
var southwest = 0;
var xmas = 0;


for (var y = 0; y < board.Length; y++)
{
    for (var x = 0; x < board[y].Length; x++)
    {
        
        if (findEast(y, x, board)) east++;
        if (findWest(y, x, board)) west++;
        if (findNorth(y, x, board)) north++;
        if (findSouth(y, x, board)) south++;
        if (findSouthEast(y, x, board)) southeast++;
        if (findNorthWest(y, x, board)) northwest++;
        if (findNorthEast(y, x, board)) northeast++;
        if (findSouthWest(y, x, board)) southwest++;
        if (findCrossedMas(y, x, board)) xmas++;
    }
}

Console.WriteLine($"Part1: {north + south + west + east + northwest + northeast + southeast + southwest}");

Console.WriteLine($"Part2: {xmas}");