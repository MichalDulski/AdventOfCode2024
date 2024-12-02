// See https://aka.ms/new-console-template for more information

var input = File.ReadAllLines("../../../input.txt");

List<int> leftCol = [];
List<int> rightCol = [];

Dictionary<int, int> occurrencesLeft = new Dictionary<int, int>();
Dictionary<int, int> occurrencesRight = new Dictionary<int, int>();

var linesLength = input.Length;

foreach (var line in input)
{
    var parts = line.Split("   ");
    leftCol.Add(int.Parse(parts[0]));
    rightCol.Add(int.Parse(parts[1]));
    occurrencesLeft[int.Parse(parts[0])] = occurrencesLeft.ContainsKey(int.Parse(parts[0])) ? occurrencesLeft[int.Parse(parts[0])] + 1 : 1;
    occurrencesRight[int.Parse(parts[1])] = occurrencesRight.ContainsKey(int.Parse(parts[1])) ? occurrencesRight[int.Parse(parts[1])] + 1 : 1;
}

leftCol.Sort();
rightCol.Sort();

var sum = 0;

for(int i = 0; i < linesLength; i++)
{
    sum += Math.Abs(leftCol[i] - rightCol[i]);
}

Console.WriteLine(sum);

var similarity = 0;
foreach(var key in occurrencesLeft.Keys)
{
    if(occurrencesRight.TryGetValue(key, out var value))
        similarity += occurrencesLeft[key] * key * value;
}

Console.WriteLine(similarity);