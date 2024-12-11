var line = File.ReadAllText("../../../input.txt");
var stones = line.Split(' ').Select(long.Parse).ToList();

Console.WriteLine("Enter number of blinks:");
var numberOfBlinks = long.Parse(Console.ReadLine() ?? string.Empty);

Dictionary<long, long> stoneCount = new();
foreach (var stone in stones)
{
    if (!stoneCount.ContainsKey(stone))
        stoneCount[stone] = 0;
    stoneCount[stone]++;
}

for (int blink = 0; blink < numberOfBlinks; blink++)
{
    Dictionary<long, long> newStoneCount = new();

    foreach (var kvp in stoneCount)
    {
        var stone = kvp.Key;
        var count = kvp.Value;

        if (stone == 0)
        {
            AddToDict(newStoneCount, 1, count);
            continue;
        }

        // Count digits without string conversion
        long temp = stone;
        int digitCount = 0;
        while (temp > 0)
        {
            digitCount++;
            temp /= 10;
        }

        if (digitCount % 2 == 0)
        {
            long divFactor = (long)Math.Pow(10, digitCount / 2);
            long right = stone % divFactor;
            long left = stone / divFactor;

            AddToDict(newStoneCount, left, count);
            AddToDict(newStoneCount, right, count);
        }
        else
        {
            AddToDict(newStoneCount, stone * 2024, count);
        }
    }

    stoneCount = newStoneCount;
}

Console.WriteLine("Result: " + stoneCount.Values.Sum());

void AddToDict(Dictionary<long, long> dict, long key, long value)
{
    if (!dict.ContainsKey(key))
        dict[key] = 0;
    dict[key] += value;
}