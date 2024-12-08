using System;
using System.Collections.Generic;
using System.IO;

class Program
{
    static void Main(string[] args)
    {
        string[] lines = File.ReadAllLines("input.txt");

        // Create the 2D array
        int rows = lines.Length;
        int cols = lines[0].Length;
        char[,] grid = new char[rows, cols];

        // Create dictionary for character positions
        Dictionary<char, List<(int X, int Y)>> charPositions = new Dictionary<char, List<(int X, int Y)>>();

        // Process each character in the input
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                char currentChar = lines[y][x];
                grid[y, x] = currentChar;

                // If character is not '.', add its position to the dictionary
                if (currentChar != '.')
                {
                    if (!charPositions.ContainsKey(currentChar))
                    {
                        charPositions[currentChar] = new List<(int X, int Y)>();
                    }
                    charPositions[currentChar].Add((x, y));
                }
            }
        }

        // Create data structures for storing found abstract points
        Dictionary<char, HashSet<(int X, int Y)>> charFoundPoints = new Dictionary<char, HashSet<(int X, int Y)>>();

        // Process each character and its positions
        foreach (var kvp in charPositions)
        {
            char currentChar = kvp.Key;
            var positions = kvp.Value;
            charFoundPoints[currentChar] = new HashSet<(int X, int Y)>();

            // Compare pairs of positions
            for (int i = 0; i < positions.Count; i++)
            {
                for (int j = i + 1; j < positions.Count; j++)
                {
                    var point1 = positions[i];
                    var point2 = positions[j];

                    // Calculate distances (this will be our step size)
                    int xDistance = Math.Abs(point1.X - point2.X);
                    int yDistance = Math.Abs(point1.Y - point2.Y);

                    // Calculate direction vectors
                    int xDir = Math.Sign(point2.X - point1.X);
                    int yDir = Math.Sign(point2.Y - point1.Y);

                    // Add the original points to found points
                    if (IsValidPoint(point1.X, point1.Y, rows, cols))
                    {
                        ProcessPoint(point1.X, point1.Y, grid, charFoundPoints[currentChar]);
                    }
                    if (IsValidPoint(point2.X, point2.Y, rows, cols))
                    {
                        ProcessPoint(point2.X, point2.Y, grid, charFoundPoints[currentChar]);
                    }

                    // Check points in both directions from point1
                    var currentPoint = point1;
                    // Forward direction (beyond point2)
                    while (true)
                    {
                        // Move to next point at the same distance
                        currentPoint = (
                            X: currentPoint.X + xDistance * xDir,
                            Y: currentPoint.Y + yDistance * yDir
                        );

                        if (!IsValidPoint(currentPoint.X, currentPoint.Y, rows, cols))
                            break;

                        ProcessPoint(currentPoint.X, currentPoint.Y, grid, charFoundPoints[currentChar]);
                    }

                    // Backward direction (before point1)
                    currentPoint = point1;
                    while (true)
                    {
                        // Move to next point at the same distance in opposite direction
                        currentPoint = (
                            X: currentPoint.X - xDistance * xDir,
                            Y: currentPoint.Y - yDistance * yDir
                        );

                        if (!IsValidPoint(currentPoint.X, currentPoint.Y, rows, cols))
                            break;

                        ProcessPoint(currentPoint.X, currentPoint.Y, grid, charFoundPoints[currentChar]);
                    }
                }
            }
        }

        // Print the updated grid
        Console.WriteLine("Updated grid representation:");
        for (int y = 0; y < rows; y++)
        {
            for (int x = 0; x < cols; x++)
            {
                Console.Write(grid[y, x]);
            }
            Console.WriteLine();
        }

        // Print abstract points by character
        Console.WriteLine("\nAbstract points by character:");
        foreach (var kvp in charFoundPoints)
        {
            Console.WriteLine($"Character '{kvp.Key}':");
            foreach (var point in kvp.Value)
            {
                Console.WriteLine($"  Position: X={point.X}, Y={point.Y}");
            }
        }

        // Print count of found points per character and total distinct points
        Console.WriteLine("\nCount of found points per character:");
        HashSet<(int X, int Y)> allDistinctPoints = new HashSet<(int X, int Y)>();
        foreach (var kvp in charFoundPoints)
        {
            int charCount = kvp.Value.Count;
            Console.WriteLine($"Character '{kvp.Key}': {charCount} points");
            allDistinctPoints.UnionWith(kvp.Value);
        }
        Console.WriteLine($"\nTotal number of distinct points: {allDistinctPoints.Count}");
    }

    static bool IsValidPoint(int x, int y, int rows, int cols)
    {
        return x >= 0 && x < cols && y >= 0 && y < rows;
    }

    static void ProcessPoint(int x, int y, char[,] grid, HashSet<(int X, int Y)> points)
    {
        if (grid[y, x] == '.')
        {
            grid[y, x] = '#';
        }
        points.Add((x, y));
    }
}
