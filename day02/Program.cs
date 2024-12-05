

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

safeCount = 0;

static bool CheckLevels(List<int> levels, out int failIndex)
{
    var levelsAreIncreasing = isIncreasing(levels[0], levels[1]);

    var i = 0;
    var isSafe = true;

    failIndex = -1;

    while ((i < levels.Count - 1 ) && isSafe)
    {
        if ((levelsAreIncreasing && isIncreasing(levels[i], levels[i + 1])) || (!levelsAreIncreasing && !isIncreasing(levels[i], levels[i + 1])))
        {
            var delta = int.Abs(levels[i + 1] - levels[i]);

            if (delta < 1 || delta > 3)
            {
                failIndex = i;
                isSafe = false;
            }
        }
        else
        {
            failIndex = i;
            isSafe = false;
        }
        i++;
    }

    // Console.WriteLine($"Safe: {isSafe} FailIndex: {failIndex} for {string.Join(", ", levels)}");
    return isSafe;

}
foreach (var report in input)
{
    var levels = report.Split().Select(int.Parse).ToList();

    var levelsAreIncreasing = isIncreasing(levels[0], levels[1]);

    int failIndex = -1;

    var isSafe = CheckLevels(levels, out failIndex);

    var index = 0;

    while (!isSafe && index < levels.Count)
    {
        var newLevels = new List<int>(levels);
        newLevels.RemoveAt(index);

        isSafe = CheckLevels(newLevels, out failIndex);

        index++;
    }



    if (isSafe) safeCount++;
}


Console.WriteLine($"Part2: {safeCount}");