// See https://aka.ms/new-console-template for more information

Console.WriteLine("Hello, Day 6!");


var input = File.ReadAllLines("data/input.txt");

var guard = new HashSet<char> {'^', 'v', '<', '>'};

var visited = new Dictionary<(long y, long x), int>();

var turns = new List<(long y, long x)>();

(int, int) FindGuard(char[][] board)
{
    for (var y = 0; y < board.Length; y++)
    {
        for (var x = 0; x < board[y].Length; x++)
        {
            if (guard.Contains(board[y][x])) return (y, x);
        }
    }    

    return (-1, -1);
}

void MakeTurn(char[][] board, (long y, long x) location)
{
    var turn = new Dictionary<char, char>()
    {
        {'^', '>'},
        {'v', '<'},
        {'<', '^'},
        {'>', 'v'}
    };

    // make a turn
    if (turns.Count == 0 || !(location == turns[turns.Count - 1])) turns.Add(location);

    // change direction
    board[location.y][location.x] = turn[board[location.y][location.x]];

}

void MarkVisited(long y, long x, char[][] board, bool track)
{
    
        board[y][x] = 'X';
        if (track && !visited.ContainsKey((y, x))) visited[(y, x)] = 1;
        if (track) visited[(y, x)]++;
        
}

(int, int) Move((long y, long x) guard, char[][] board, bool track = true)
{
    var nextStep = new Dictionary<char, (int, int)>()
    {
        {'^', (-1, 0)},
        {'v',(1, 0)},
        {'<', (0, -1)},
        {'>', (0, 1)}
    };

    var (y, x) = guard;

    // get the next step
    var (moveY, moveX) = nextStep[board[y][x]];

    //are we at the edge of the board?
    if (y + moveY < 0 || x + moveX < 0 || y + moveY >= board.Length || x + moveX >= board[y].Length)
    {
        
        MarkVisited(y, x, board, track);
        return (-1, -1);
    } 
    
    // keep going forward if no wall
    if (board[y + moveY][x + moveX] != '#' && board[y + moveY][x + moveX] != 'O')
    {
        board[y+moveY][x+moveX] = board[y][x];
        
        MarkVisited(y, x, board, track);
        
        return (y + moveY, x + moveX);
    }
    

    MakeTurn(board, (y, x));
    
    // and move too
    return Move((y, x), board, track);

}

void PrintBoard(char[][] board)
{
    if (board.Length > 20) return;

    for (var y = 0; y < board.Length; y++)
    {
        for (var x = 0; x < board[y].Length; x++)
        {
            Console.Write(board[y][x]);
        }
        Console.WriteLine();
    }
}

var board = input
    .Select(line => line.ToCharArray())
    .ToArray();

(long y, long x) = FindGuard(board);

while (y != -1 && x != -1)
{
    (y, x) = Move((y, x), board);

}

Console.WriteLine($"Part1: unique locations: {visited.Count}");

bool HasCycle(List<(long y, long x)> turns)
{
    // check if there are any duplicate turns

    return turns.FindAll(t => t == turns[turns.Count - 1]).Count > 1;

    // return turns.GroupBy(t => t)
    //     .Where(g => g.Count() > 1).Any();
}

// part 2

var cycles = 0;

// printBoard(board);

foreach (var visit in visited)
{
    board = input
        .Select(line => line.ToCharArray())
        .ToArray();

    (y, x) = FindGuard(board);

    if ((visit.Key.y, visit.Key.x) == (y, x)) continue;

    board[visit.Key.y][visit.Key.x] = 'O';

    turns.Clear();

    bool done = false;

    while (!done)
    {
        (y, x) = Move((y, x), board, false);

        if (HasCycle(turns))
        {
            done = true;
            cycles++;
        }
        else if (y == -1 || x == -1) done = true;
    }
}

Console.WriteLine($"Part2: cycles: {cycles} ");