// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, Day 7!");


var input = File.ReadAllLines("data/input.txt");

var equations = new Dictionary<long, List<int>>();

foreach (var line in input)
{
    var split = line.Split(": ");
    var testValue = long.Parse(split[0]);
    var calcValues = split[1].Split(" ").Select(int.Parse).ToList();

    equations[testValue] = calcValues;
}

IEnumerable<string> GenerateOperators(int neededOperators, char[] operators)
{
    var result = new List<string> { "" };

    for (int i = 0; i < neededOperators; i++)
    {
        var newResult = new List<string>();
        foreach (var str in result)
        {
            foreach (var o in operators)
            {
                newResult.Add(str + o);
            }
        }
        result = newResult;
    }

    return result;
}

List<long> FindGoodTests(Dictionary<long, List<int>> equations, char[] operators)
{
    var goodTests = new List<long>();

    foreach (var equation in equations)
    {
        var opsPermutations = GenerateOperators(equation.Value.Count - 1, operators).ToList();

        var checkResult = equation.Key;
        long currentResult = 0;

        foreach (var ops in opsPermutations)
        {
            currentResult = equation.Value[0];

            // run the equation for this permutation of operators
            for (int i = 1, j = 0; i < equation.Value.Count; i++, j++)
            {
                if (ops[j] == '+') 
                {
                    currentResult += equation.Value[i];
                }
                else if (ops[j] == '*') 
                {
                    currentResult *= equation.Value[i];
                }
                else if (ops[j] == '|') 
                {
                    currentResult = long.Parse(currentResult.ToString() +  equation.Value[i].ToString()); 
                }

                // if (currentResult > checkResult) break;
            }

            if (checkResult == currentResult )
            { 
                // we have a match, no need to find another, let check next equation
                // Console.WriteLine($"Found match for {equation.Key}: {ops} = {currentResult}");
                goodTests.Add(equation.Key);
                break;
            }
        }
    }

    return goodTests;
}

var goodTests = FindGoodTests(equations, ['+', '*']);

Console.WriteLine($"Part1: {goodTests.Sum()} expect 20281182715321 ");




goodTests = FindGoodTests(equations, ['+', '*', '|']);

Console.WriteLine($"Part2: {goodTests.Sum()} expect 159490400628354");