// See https://aka.ms/new-console-template for more information

using System.Text.RegularExpressions;

var input = File.ReadAllLines("../../../input.txt");

const string pattern = @"mul\((\d+),\s*(\d+)\)";
Regex regex = new Regex(pattern);

var result = 0;
foreach (var line in input)
{
    MatchCollection matches = regex.Matches(line);
    foreach(Match match in matches)
    {
        var part1 = match.Groups[0].Value;
        var part2 = match.Groups[1].Value;
        var part3 = match.Groups[2].Value;
        result += int.Parse(part3) * int.Parse(part2);
    }
}

Console.WriteLine(result);


const string donPattern = @"mul\((\d+),\s*(\d+)\)|do\(\)|don't\(\)";

regex = new Regex(donPattern);

var resultDon = 0;
bool doMul = true;
foreach (var line in input)
{
    MatchCollection matches = regex.Matches(line);
    foreach(Match match in matches)
    {
        var part1 = match.Groups[0].Value;
        if(part1 == "do()")
        {
            doMul = true;
        }
        else if(part1 == "don't()")
        {
            doMul = false;
        }
        else if (doMul)
        {
            var part2 = match.Groups[1].Value;
            var part3 = match.Groups[2].Value;
            resultDon += int.Parse(part3) * int.Parse(part2);
            
        }
    }
}

Console.WriteLine(resultDon);
