using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("input.txt");

        // Part 1: Only addition and multiplication
        long totalCalibrationResult1 = ProcessEquations(lines, includeConcatenation: false);
        Console.WriteLine($"Part 1 - Total calibration result: {totalCalibrationResult1}");

        // Part 2: Including concatenation operator
        long totalCalibrationResult2 = ProcessEquations(lines, includeConcatenation: true);
        Console.WriteLine($"Part 2 - Total calibration result: {totalCalibrationResult2}");
    }

    static long ProcessEquations(string[] lines, bool includeConcatenation)
    {
        long sum = 0;
        foreach (var line in lines)
        {
            var parts = line.Split(':');
            long testValue = long.Parse(parts[0].Trim());
            var numbers = parts[1].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                       .Select(long.Parse)
                                       .ToList();

            if (CanProduceTestValue(numbers, testValue, includeConcatenation))
            {
                sum += testValue;
            }
        }
        return sum;
    }

    static bool CanProduceTestValue(List<long> numbers, long targetValue, bool includeConcatenation)
    {
        if (numbers.Count == 1)
            return numbers[0] == targetValue;

        var operators = includeConcatenation
            ? new[] { '+', '*', '|' }  // '|' represents concatenation
            : new[] { '+', '*' };

        return TryAllOperatorCombinations(numbers, operators, targetValue);
    }

    static bool TryAllOperatorCombinations(List<long> numbers, char[] operators, long targetValue)
    {
        int operatorsNeeded = numbers.Count - 1;
        int totalCombinations = (int)Math.Pow(operators.Length, operatorsNeeded);

        for (int i = 0; i < totalCombinations; i++)
        {
            var currentOperators = new char[operatorsNeeded];
            int temp = i;

            // Generate operator combination
            for (int j = 0; j < operatorsNeeded; j++)
            {
                currentOperators[j] = operators[temp % operators.Length];
                temp /= operators.Length;
            }

            // Evaluate expression
            if (EvaluateExpression(numbers, currentOperators) == targetValue)
                return true;
        }

        return false;
    }

    static long EvaluateExpression(List<long> numbers, char[] operators)
    {
        long result = numbers[0];

        for (int i = 0; i < operators.Length; i++)
        {
            switch (operators[i])
            {
                case '+':
                    result += numbers[i + 1];
                    break;
                case '*':
                    result *= numbers[i + 1];
                    break;
                case '|':  // Concatenation operator
                    result = ConcatenateNumbers(result, numbers[i + 1]);
                    break;
            }
        }

        return result;
    }

    static long ConcatenateNumbers(long a, long b)
    {
        // Convert b to string to get its length
        string bStr = b.ToString();
        // Multiply a by 10^(length of b) and add b
        return a * (long)Math.Pow(10, bStr.Length) + b;
    }
}
