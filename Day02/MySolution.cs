namespace Day02;

public static class MySolution
{
    static (bool, int) IsSafe(int[] levels)
    {
        bool isSafe = true;
        bool isIncreasing = levels[0] < levels[1];
        int removeIndex = -1;
        for (int i = 1; i < levels.Length; i++)
        {
            var prevElement = levels[i - 1];
            var currentElement = levels[i];

            var diff = currentElement - prevElement;
            if (isIncreasing && diff < 0)
            {
                isSafe = false;
                removeIndex = i;
                break;
            }

            if (!isIncreasing && diff > 0)
            {
                isSafe = false;
                removeIndex = i;
                break;
            }

            if (Math.Abs(diff) > 3 || diff == 0)
            {
                isSafe = false;
                removeIndex = i;
                break;
            }
        }

        return (isSafe, removeIndex);
    }

    public static (int, int) Run(IEnumerable<IEnumerable<int>> input)
    {
        var numOfSafeReports = 0;
        var numOfSafeReportsDamp = 0;

        foreach (var line in input)
        {
            var levels = line.ToArray();
            var (isSafe, removeIndex) = IsSafe(levels);
            numOfSafeReports += isSafe ? 1 : 0;
            if (!isSafe)
            {
                var newLevels = levels.ToList();
                newLevels.RemoveAt(removeIndex);
                var (isSafeDamp, _) = IsSafe(newLevels.ToArray());

                if (!isSafeDamp)
                {
                    var reversedLevels = levels.Reverse().ToList();
                    var (_, removeAt) = IsSafe(reversedLevels.ToArray());
                    var newLevels3 = reversedLevels.ToList();
                    newLevels3.RemoveAt(removeAt);
                    var (isSafeDamp3, _) = IsSafe(newLevels3.ToArray());
                    numOfSafeReportsDamp += isSafeDamp3 ? 1 : 0;
                }
                else
                {
                    numOfSafeReportsDamp += 1;
                }
            }
            else
            {
                numOfSafeReportsDamp += 1;
            }
        }

        return (numOfSafeReports, numOfSafeReportsDamp);
    }
}