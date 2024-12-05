var input = File.ReadAllLines("../../../input.txt");

// Dictionary to store X as key and list of Y values
Dictionary<int, List<int>> mappings = new Dictionary<int, List<int>>();

// List to store the arrays from the second part
List<int[]> numberArrays = new List<int[]>();

bool afterEmptyLine = false;

foreach (var line in input)
{
    if (string.IsNullOrEmpty(line))
    {
        afterEmptyLine = true;
        continue;
    }

    if (!afterEmptyLine)
    {
        var parts = line.Split('|');
        var key = int.Parse(parts[0]);
        var value = int.Parse(parts[1]);

        if (!mappings.ContainsKey(key))
        {
            mappings[key] = new List<int>();
        }
        mappings[key].Add(value);
    }
    else
    {
        var numbers = line.Split(',').Select(int.Parse).ToArray();
        numberArrays.Add(numbers);
    }
}

var sumOfValidArrays = 0;
var sumOfSwappedArrays = 0;

foreach (var currentArray in numberArrays)
{
    var (isValid, requiredSwaps) = ValidateAndSwapArray(currentArray, mappings, 0, currentArray.Length - 1);

    if (isValid)
    {
        if (requiredSwaps)
        {
            sumOfSwappedArrays += currentArray[currentArray.Length / 2];
        }
        else
        {
            sumOfValidArrays += currentArray[currentArray.Length / 2];
        }
    }
}

Console.WriteLine($"\nSum of middle elements from originally valid arrays: {sumOfValidArrays}");
Console.WriteLine($"Sum of middle elements from arrays that required swaps: {sumOfSwappedArrays}");
return;

(bool IsValid, bool RequiredSwaps) ValidateAndSwapArray(int[] array, Dictionary<int, List<int>> successorsByPredecessor, int startIndex, int endIndex)
{
    var isValid = true;
    var foundIndex = -1;

    // Check each element from end to start
    for (var currentIndex = endIndex; currentIndex >= startIndex; currentIndex--)
    {
        var currentElement = array[currentIndex];

        if (successorsByPredecessor.TryGetValue(currentElement, out var dictionaryValues))
        {
            // Check if any dictionary values exist in earlier positions
            for (var checkIndex = 0; checkIndex < currentIndex; checkIndex++)
            {
                if (!dictionaryValues.Contains(array[checkIndex])) continue;
                isValid = false;
                foundIndex = checkIndex;
                break;
            }
        }

        if (isValid) continue;
        // Perform the swap
        (array[currentIndex], array[foundIndex]) = (array[foundIndex], array[currentIndex]);

        // Recursively validate the modified array
        var result = ValidateAndSwapArray(array, successorsByPredecessor, startIndex, currentIndex);
        return (result.IsValid, true);
    }

    return (isValid, false);
}