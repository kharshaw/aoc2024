using System.Runtime.Serialization;
using System.Text.RegularExpressions;

var lines = File.ReadAllLines("data/input.txt");


Int64 acc = 0;

foreach (var line in lines)
{
    var input = line;

    string pattern = @"mul\((\d{1,3}),(\d{1,3})\)";

    MatchCollection matches = Regex.Matches(input, pattern);

    foreach (Match match in matches)
    {
        var op1 = Int64.Parse(match.Groups[1].Value);
        var op2 = Int64.Parse(match.Groups[2].Value);

        acc += op1 * op2;    
    }
}
Console.WriteLine($"Part1: {acc}");

acc = 0;
bool go = true;
    
foreach (var line in lines)
{
    var input = line;

    string pattern = @"mul\((\d{1,3}),(\d{1,3})\)|don't\(\)|do\(\)";

    MatchCollection matches = Regex.Matches(input, pattern);

    foreach (Match match in matches)
    {
        // Console.WriteLine(match.Value);
        if (match.Value.StartsWith("don't")) 
        {
            go = false;
        }
        else if (match.Value.StartsWith("do"))
        {
            go = true;
        }
        else if (match.Value.StartsWith("mul"))
        {
            var op1 = Int64.Parse(match.Groups[1].Value);
            var op2 = Int64.Parse(match.Groups[2].Value);

            if (go)
            {   
                acc += op1 * op2;
                // Console.WriteLine($"DO: {op1} * {op2} = {acc}"); 
            }   
        }
        // Console.WriteLine($"go: {go}");
    }
}
Console.WriteLine($"Part2: {acc}");