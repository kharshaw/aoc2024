// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Day 16!");

var input = File.ReadAllLines("data/input.txt");

// parse input into a 2d array of char

char[][] maze = input
    .Select(line => line.ToCharArray())
    .ToArray();


(int y, int x) start = FindToken(maze, 'S');
(int y, int x) end = FindToken(maze, 'E');

var herd = new List<Reindeer>();
var completed = new List<Reindeer>();

Dictionary<(int y, int x), long> visited = new Dictionary<(int y, int x), long>();

var stopwatch = new System.Diagnostics.Stopwatch();

stopwatch.Start();

herd.Add( new Reindeer(herd, completed, visited, maze, start, 'e'));

while (herd.Count > 0)
{
    var reindeer = herd.First();

    reindeer.Move(); 
        

}

// find the lowest Score of the Reindeer in the herd

var lowest = completed.Count == 0 ? 0 : completed.Min(r => r.Score);

// foreach (var r in completed.OrderBy(r => r.Score))
// {
//     Console.WriteLine($"Reindeer: {r.Id} Score: {r.Score}");
// }

Console.WriteLine($"Part1: {lowest} in {stopwatch.ElapsedMilliseconds}ms");

stopwatch.Restart();

var seats = new HashSet<(int y, int x)>();


// add every item in Path for each completed reindeer

foreach (var r in completed)
{
    foreach (var p in r.Path)
    {
        seats.Add(p);
    }   
}
Console.WriteLine($"Part2: {seats.Count} in {stopwatch.ElapsedMilliseconds}ms");

stopwatch.Stop();







(int y, int x) FindToken(char[][] maze, char token)
{
    for (var y = 0; y < maze.Length; y++)
    {
        for (var x = 0; x < maze[y].Length; x++)
        {
            if (maze[y][x] == token) return (y, x);
        }
    }

    return (-1, -1);
}

public class Reindeer
{
    static readonly char[] directions = { 'n', 'e', 's', 'w' };

    static readonly Dictionary<char, (int y, int x)> directionMap = new Dictionary<char, (int y, int x)>
    {
        {'n', (-1, 0)},
        {'e', (0, 1)},
        {'s', (1, 0)},
        {'w', (0, -1)}
    };

    public Dictionary<(int y, int x), long> Visited { get; private set; }

    public Guid Id { get; private set; }
    public (int y, int x) Position { get; private set; }
    public char Direction { get; private set; }

    public List<(int y, int x)> Path { get; set; } = new List<(int y, int x)>();
    List<(int y, int x)> Turns { get; set; } = new List<(int y, int x)>();

    public long Score { get; private set; }

    private char[][] _maze;

    private List<Reindeer> _herd;
    private List<Reindeer> _completed;



    public Reindeer(List<Reindeer> herd, List<Reindeer> completed, Dictionary<(int y, int x), long> visited, char[][] maze,(int y, int x) position, char direction)
    {
        Id = Guid.NewGuid();
        _maze = maze;
        Position = position;
        Direction = direction;
        Score = 0;
        _herd = herd;
        _completed = completed;
        Visited = visited;
        //Path.Add(position);
        
    }

    public Reindeer(List<Reindeer> herd, List<Reindeer> completed, Dictionary<(int y, int x), long> visited, char[][] maze, (int y, int x) position, char direction, List<(int y, int x)> path, long score) : this(herd, completed, visited,maze, position, direction) 
    {
        Path = new List<(int y, int x)>(path);
        Score = score;
    }

    public Reindeer Clone(char direction, (int y, int x) position)
    {
        var clone = new Reindeer(_herd,_completed, Visited,_maze, position, Direction);
        clone.Path = new List<(int y, int x)>(Path) { Position };
        clone.Turns = new List<(int y, int x)>(Turns) { Position};
        clone.Direction = direction;
        clone.Score = Score + 1000;
        clone.Score++;
        
        return clone;
    }

    
    private void AddVisit()
    {
        if (!Visited.TryAdd(Position, Score)) Visited[Position] = Score;

        Path.Add(Position);
    }

    public bool Move()
    {
        if (_maze[Position.y][Position.x] == 'E') 
        {
            AddVisit();
            _completed.Add(this);
            _herd.Remove(this);
            return true;
        }

        MakeTurns();

        (int y, int x) nextPosition = (Position.y + directionMap[Direction].y, Position.x + directionMap[Direction].x);

        if (nextPosition.y < 0 || nextPosition.y >= _maze.Length || nextPosition.x < 0 || nextPosition.x >= _maze[0].Length)
        {
            _herd.Remove(this);
            return false;
        }
        

        if (_maze[nextPosition.y][nextPosition.x] == '#' 
            || Path.Contains(nextPosition) 
            || Visited.ContainsKey(nextPosition) && Visited[nextPosition] < Score)
        {
            _herd.Remove(this); 
            return false;
        }


        Score++;
        AddVisit();
        
        Position = nextPosition;

        //if (_maze[nextPosition.y][nextPosition.x] == 'E') return true;


        return Move();
        
    }

    public void MakeTurns()
    {
        // look left
        var left = directions[(Array.IndexOf(directions, Direction) - 1 + directions.Length) % directions.Length];
        (int y, int x) leftPosition = (Position.y + directionMap[left].y, Position.x + directionMap[left].x);
        var leftOpen = _maze[leftPosition.y][leftPosition.x] != '#';

        if (leftOpen)
        {

            var clone = this.Clone(left, leftPosition);
            
            _herd.Add(clone);
            //Move();
        }

        // look right
        var right = directions[(Array.IndexOf(directions, Direction) + 1) % directions.Length];
        (int y, int x) rightPosition = (Position.y + directionMap[right].y, Position.x + directionMap[right].x); 
        var rightOpen = _maze[rightPosition.y][rightPosition.x] != '#';

        if (rightOpen)
        {

            var clone = this.Clone(right, rightPosition);

            _herd.Add(clone);
            //Move();
        }
    }




}

public class PathCycleException : ApplicationException { }