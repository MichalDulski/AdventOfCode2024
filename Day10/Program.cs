using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static int[][] grid;
    static int rows;
    static int cols;
    static readonly int[] dx = { -1, 1, 0, 0 }; // left, right
    static readonly int[] dy = { 0, 0, -1, 1 }; // up, down

    static void Main()
    {
        string[] lines = File.ReadAllLines("input.txt");
        rows = lines.Length;
        cols = lines[0].Length;
        grid = new int[rows][];

        // Parse input
        for (int i = 0; i < rows; i++)
        {
            grid[i] = lines[i].Select(c => c - '0').ToArray();
        }

        // Part 1
        Console.WriteLine("Part 1:");
        int totalScorePart1 = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i][j] == 0)
                {
                    totalScorePart1 += CountReachableNines(i, j);
                }
            }
        }
        Console.WriteLine($"Sum of trailhead scores: {totalScorePart1}");

        // Part 2
        Console.WriteLine("\nPart 2:");
        int totalScorePart2 = 0;
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (grid[i][j] == 0)
                {
                    int pathCount = CountUniquePaths(i, j);
                    totalScorePart2 += pathCount;
                    Console.WriteLine($"Trailhead at ({i},{j}) has {pathCount} paths");
                }
            }
        }
        Console.WriteLine($"Sum of trailhead ratings: {totalScorePart2}");
    }

    // Part 1: Count reachable nines
    static int CountReachableNines(int startX, int startY)
    {
        var visited = new HashSet<(int, int)>();
        var reachableNines = new HashSet<(int, int)>();
        DFSPart1(startX, startY, 0, visited, reachableNines);
        return reachableNines.Count;
    }

    static void DFSPart1(int x, int y, int currentHeight, HashSet<(int, int)> visited, HashSet<(int, int)> reachableNines)
    {
        if (grid[x][y] == 9)
        {
            reachableNines.Add((x, y));
            return;
        }

        visited.Add((x, y));

        // Try all four directions
        for (int i = 0; i < 4; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            // Check bounds and if the new position is valid
            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
            {
                // Check if the height increases by exactly 1 and we haven't visited this position
                if (grid[newX][newY] == currentHeight + 1 && !visited.Contains((newX, newY)))
                {
                    DFSPart1(newX, newY, currentHeight + 1, visited, reachableNines);
                }
            }
        }
    }

    // Part 2: Count unique paths
    static int CountUniquePaths(int startX, int startY)
    {
        var visited = new HashSet<(int, int)>();
        int pathCount = 0;
        DFSPart2(startX, startY, 0, visited, ref pathCount);
        return pathCount;
    }

    static void DFSPart2(int x, int y, int currentHeight, HashSet<(int, int)> visited, ref int pathCount)
    {
        // If we reached a 9, we found a valid path
        if (grid[x][y] == 9)
        {
            pathCount++;
            return;
        }

        visited.Add((x, y));

        // Try all four directions
        for (int i = 0; i < 4; i++)
        {
            int newX = x + dx[i];
            int newY = y + dy[i];

            // Check bounds and if the new position is valid
            if (newX >= 0 && newX < rows && newY >= 0 && newY < cols)
            {
                // Check if the height increases by exactly 1 and we haven't visited this position
                if (grid[newX][newY] == currentHeight + 1 && !visited.Contains((newX, newY)))
                {
                    // Create a new visited set for this path
                    var newVisited = new HashSet<(int, int)>(visited);
                    DFSPart2(newX, newY, currentHeight + 1, newVisited, ref pathCount);
                }
            }
        }
    }
}
