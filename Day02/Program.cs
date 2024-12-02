// See https://aka.ms/new-console-template for more information

var input = File.ReadAllLines("../../../input.txt");

var numOfSafeReports = 0;
foreach (var line in input)
{
    var levels = line.Split(" ").Select(int.Parse).ToArray();
    
    bool isSafe = true;
    bool isIncreasing = levels[0] < levels[1];
    for(int i = 1; i < levels.Length; i++)
    {
        var prevElement = levels[i - 1];
        var currentElement = levels[i];
        
        var diff = currentElement - prevElement;
        if(isIncreasing && diff < 0)
        {
            isSafe = false;
            break;
        }
        if(!isIncreasing && diff > 0)
        {
            isSafe = false;
            break;
        }
        
        if(Math.Abs(diff) > 3 || diff == 0)
        {
            isSafe = false;
            break;
        }
    }
    numOfSafeReports += isSafe ? 1 : 0;
}

Console.WriteLine(numOfSafeReports);