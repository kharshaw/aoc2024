

var input = File.ReadAllLines("data/input.txt");


static bool isIncreasing(int a, int b) => a < b;

var safeCount = 0;

foreach (var report in input)
{
    var levels = report.Split().Select(int.Parse).ToList();

    var levelsAreIncreasing = isIncreasing(levels[0], levels[1]);

    var i = 0;
    var isSafe = true;

    while ((i < levels.Count - 1 ) && isSafe)
    {
        if ((levelsAreIncreasing && isIncreasing(levels[i], levels[i + 1])) || (!levelsAreIncreasing && !isIncreasing(levels[i], levels[i + 1])))
        {
            var delta = int.Abs(levels[i + 1] - levels[i]);

            if (delta < 1 || delta > 3)
            {
                isSafe = false;
            }
        }
        else
        {
            isSafe = false;
        }
        i++;

    }

    if (isSafe) safeCount++;
}

Console.WriteLine($"Part1: {safeCount}");