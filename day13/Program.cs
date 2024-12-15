// See https://aka.ms/new-console-template for more information
using System.Text.RegularExpressions;

#pragma warning disable CS8321

Console.WriteLine("Hello, Day13!");


var input = File.ReadAllLines("data/input.txt");

var games = new List<((long x, long y), (long x, long y), (long x, long y))>();

for (var i = 0; i < input.Length; i += 3)
{
    var inputA = input[i];
    Match matchA = Regex.Match(inputA, @"^Button\s+(\w+):\s*X\+(\d+),\s*Y\+(\d+)$");

    var inputP = input[i+1];
    Match matchB = Regex.Match(inputP, @"^Button\s+(\w+):\s*X\+(\d+),\s*Y\+(\d+)$");

    var inputPrize = input[i+2];
    var matchPrize = Regex.Match(inputPrize, @"^Prize:\s*X=(\d+),\s*Y=(\d+)$");

    var buttonA = (Int64.Parse(matchA.Groups[2].Value), Int64.Parse(matchA.Groups[3].Value));
    var buttonB = (Int64.Parse(matchB.Groups[2].Value), Int64.Parse(matchB.Groups[3].Value));
    var prize = (Int64.Parse(matchPrize.Groups[1].Value), Int64.Parse(matchPrize.Groups[2].Value));

    games.Add((buttonA, buttonB, prize));

    i++;
}

var totalCost = 0L;

foreach (var game in games)
{
    var solutions = PlayGame2(game);

    // find the solution with lowest value of a*3 + b*1 

    if (solutions.Count == 0) continue;

    var bestSolution = solutions.OrderBy(x => x.a * 3 + x.b * 1).Last();

    var cost = bestSolution.a * 3 + bestSolution.b * 1; 
    totalCost += cost;


    // foreach (var solution in solutions)
    // {
    //     Console.WriteLine($"Solution: a presses: {solution.a} b presses: {solution.b}");
    // }

    // Console.WriteLine($"======================Solutions: {solutions.Count}=================");
}

Console.WriteLine($"Part1: {totalCost}");

totalCost = 0;

for (var g = 0; g < games.Count; g++) 
{
    var game = games[g];

    game.Item3.x += 10000000000000;
    game.Item3.y += 10000000000000;

    var solutions = PlayGame2(game);

    // find the solution with lowest value of a*3 + b*1 

    if (solutions.Count == 0) continue;

    var bestSolution = solutions.OrderBy(x => x.a * 3 + x.b * 1).Last();

    var cost = bestSolution.a * 3 + bestSolution.b * 1; 
    totalCost += cost;

    foreach (var solution in solutions)
    {
        Console.WriteLine($"Solution: a presses: {solution.a} b presses: {solution.b}");
    }

    Console.WriteLine($"======================Solutions: {solutions.Count}=================");
}

Console.WriteLine($"Part2: {totalCost}");


#region simple

List<(long a, long b)> PlayGame(((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize) game)
{

    /*
        solve this system of equations

        ax(a) + bx(b) = px
        ay(a) + by(b) = py
    */
    var solutions = new List<(long x, long y)>();

    var px = game.prize.x;
    var py = game.prize.y;

    var ax = game.buttonA.x;
    var ay = game.buttonA.y;

    var bx = game.buttonB.x;
    var by = game.buttonB.y;

    long max = Math.Max(px, py);

    var memo = new Dictionary<long, long>();
    
    long gcd = GCDFromList(new List<long> { ax, bx, px, ay, by, py });
    ax /= gcd;
    ay /= gcd;
    bx /= gcd;
    by /= gcd;
    px /= gcd;
    py /= gcd;

    for (long a = 0; a <= max; a++)
    {
        if (bx == 0)
        {
            continue;
        }
        long b;
        if (memo.TryGetValue(px - ax * a, out b))
        {
            if (ay * a + by * b == py)
            {
                solutions.Add((a, b));
            }
        }
        else
        {
            double tempB = (double)(px - ax * a) / bx;
            if (tempB == Math.Floor(tempB))
            {
                b = (long)tempB;
                memo[px - ax * a] = b;
                if (ay * a + by * b == py)
                {
                    solutions.Add((a, b));
                }
            }
        }
    }

    return solutions;
}

long GCDFromList(List<long> numbers)
{
    return numbers.Aggregate(GCD);
}

long GCD(long a, long b)
{
    while (b != 0)
    {
        long temp = b;
        b = a % b;
        a = temp;
    }
    return a;
}

#endregion

#region GCD and memoization
List<(long a, long b)> PlayGame2(((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize) game)
{
    
    var px = game.prize.x;
    var py = game.prize.y;

    var ax = game.buttonA.x;
    var ay = game.buttonA.y;

    var bx = game.buttonB.x;
    var by = game.buttonB.y;

    long gcd = GCD(ax, bx);
    long a = ax / gcd;
    long b = bx / gcd;

    long x = px / gcd;
    long y = py / gcd;

    Dictionary<long, long> memo = new Dictionary<long, long>();

    List<(long a, long b)> solutions = new List<(long, long)>();

    long a1 = a;
    long b1 = b;
    long x1 = x;
    long y1 = y;

    while (true)
    {
        if (a1 == 1)
        {
            solutions.Add((x1, y1));
        }
        else if (b1 == 1)
        {
            solutions.Add((x1, y1));
        }
        else
        {
            long gcd1 = GCD(a1, b1);
            a1 /= gcd1;
            b1 /= gcd1;

            x1 /= gcd1;
            y1 /= gcd1;

            long x2 = x1 + b1;
            long y2 = y1 - a1;

            if (memo.ContainsKey(x2))
            {
                break;
            }

            memo[x2] = 1;

            x1 = x2;
            y1 = y2;
        }
    }


    return solutions;

}

#endregion

#region Extended Euclidean Algorithm
    List<(long a, long b)> PlayGame3(((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize) game)
    {

        var px = game.prize.x;
        var py = game.prize.y;

        var ax = game.buttonA.x;
        var ay = game.buttonA.y;    

        var bx = game.buttonB.x;    
        var by = game.buttonB.y;
        
        long gcdAB = ExtendedGCD(ax, bx, out long x, out long y);
        long gcdABY = ExtendedGCD(gcdAB, ay, out long x1, out long y1);

        long pxMod = px % gcdABY;
        long pyMod = py % gcdABY;

        if (pxMod != 0 || pyMod != 0)
        {
            return new List<(long, long)>();
        }

        px /= gcdABY;
        py /= gcdABY;

        long a = x * x1 * px;
        long b = y * x1 * px + y1 * py;

        return new List<(long, long)>() { (a, b) };
    }

    long ExtendedGCD(long a, long b, out long x, out long y)
    {
        if (b == 0)
        {
            x = 1;
            y = 0;
            return a;
        }
        long gcd = ExtendedGCD(b, a % b, out long x1, out long y1);
        x = y1;
        y = x1 - (a / b) * y1;
        return gcd;
    }
#endregion


#region matrix multiplication

List<(long a, long b)> PlayGame4(((long x, long y) buttonA, (long x, long y) buttonB, (long x, long y) prize) game)
{
    var px = game.prize.x;
    var py = game.prize.y;

    var ax = game.buttonA.x;
    var ay = game.buttonA.y;

    var bx = game.buttonB.x;
    var by = game.buttonB.y;

    long[,] matrix = new long[,] { { ax, bx, px }, { ay, by, py } };

    // Perform Gaussian elimination
    int rows = matrix.GetLength(0);
    int cols = matrix.GetLength(1);
    for (int i = 0; i < rows; i++)
    {
        int maxIndex = i;
        for (int j = i + 1; j < rows; j++)
        {
            if (Math.Abs(matrix[j, i]) > Math.Abs(matrix[maxIndex, i]))
            {
                maxIndex = j;
            }
        }
        SwapRows(matrix, i, maxIndex);
        long pivot = matrix[i, i];
        for (int j = i + 1; j < rows; j++)
        {
            long factor = matrix[j, i] / pivot;
            for (int k = 0; k < cols; k++)
            {
                matrix[j, k] -= factor * matrix[i, k];
            }
        }
    }

    // Back-substitution
    List<(long, long)> solution = new List<(long, long)>();
    for (int i = rows - 1; i >= 0; i--)
    {
        if (matrix[i, i] == 0)
        {
            Console.WriteLine("No unique solution");
            return solution;
        }
        long numerator = matrix[i, cols - 1];
        long denominator = matrix[i, i];
        solution.Add((numerator, denominator));
    }

    return solution;
}

void SwapRows(long[,] matrix, int row1, int row2)
{
    for (int i = 0; i < matrix.GetLength(1); i++)
    {
        long temp = matrix[row1, i];
        matrix[row1, i] = matrix[row2, i];
        matrix[row2, i] = temp;
    }
}
#endregion
// PrintGames(games);




void PrintGames(List<((long x, long y), (long x, long y), (long x, long y))> games)
{
    foreach (var game in games)
    {
        Console.WriteLine($"Button A: X: {game.Item1.x} Y: {game.Item1.y}");
        Console.WriteLine($"Button B: X: {game.Item2.x} Y: {game.Item2.y}");
        Console.WriteLine($"Prize: X: {game.Item3.x} Y: {game.Item3.y}");
        Console.WriteLine();
    }
}