using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

class Program
{
    static void Main()
    {
        string input = File.ReadAllText("input.txt").Trim();
        var result = ProcessDiskMap(input);
        Console.WriteLine($"Part 1 Filesystem checksum: {result}");
        var result2 = ProcessDiskMapPart2(input);
        Console.WriteLine($"Part 2 Filesystem checksum: {result2}");
    }

    static long ProcessDiskMap(string diskMap)
    {
        var blocks = ParseDiskMap(diskMap);
        var diskState = CreateInitialDiskState(blocks);
        var compactedDisk = CompactDisk(diskState);
        return CalculateChecksum(compactedDisk);
    }

    static List<int> ParseDiskMap(string diskMap)
    {
        return diskMap.Select(c => int.Parse(c.ToString())).ToList();
    }

    static List<int> CreateInitialDiskState(List<int> blocks)
    {
        var diskState = new List<int>();
        int fileId = 0;
        bool isFile = true;

        foreach (var length in blocks)
        {
            if (isFile)
            {
                for (int i = 0; i < length; i++)
                {
                    diskState.Add(fileId);
                }
                fileId++;
            }
            else
            {
                for (int i = 0; i < length; i++)
                {
                    diskState.Add(-1);
                }
            }
            isFile = !isFile;
        }
        return diskState;
    }

    static List<int> CompactDisk(List<int> diskState)
    {
        bool madeChange;
        do
        {
            madeChange = false;
            int rightmostFileIndex = diskState.Count - 1;
            while (rightmostFileIndex >= 0 && diskState[rightmostFileIndex] == -1)
            {
                rightmostFileIndex--;
            }

            if (rightmostFileIndex <= 0) break;

            int leftmostSpaceIndex = 0;
            while (leftmostSpaceIndex < rightmostFileIndex && diskState[leftmostSpaceIndex] != -1)
            {
                leftmostSpaceIndex++;
            }

            if (leftmostSpaceIndex < rightmostFileIndex && diskState[rightmostFileIndex] != -1)
            {
                diskState[leftmostSpaceIndex] = diskState[rightmostFileIndex];
                diskState[rightmostFileIndex] = -1;
                madeChange = true;
            }

        } while (madeChange);

        return diskState;
    }

    static long ProcessDiskMapPart2(string diskMap)
    {
        var blocks = ParseDiskMap(diskMap);
        var diskState = CreateInitialDiskState(blocks);
        var compactedDisk = CompactDiskPart2(diskState);
        return CalculateChecksum(compactedDisk);
    }

    static List<int> CompactDiskPart2(List<int> diskState)
    {
        int maxFileId = diskState.Max();

        for (int fileId = maxFileId; fileId >= 0; fileId--)
        {
            var fileInfo = GetFileInfo(diskState, fileId);
            if (!fileInfo.HasValue) continue;

            var (startIndex, fileSize) = fileInfo.Value;

            var targetSpace = FindLeftmostSuitableSpace(diskState, startIndex, fileSize);
            if (!targetSpace.HasValue) continue;

            MoveFile(diskState, startIndex, targetSpace.Value, fileSize);
        }

        return diskState;
    }

    static (int startIndex, int size)? GetFileInfo(List<int> diskState, int fileId)
    {
        int startIndex = -1;
        int size = 0;
        bool foundFile = false;

        for (int i = 0; i < diskState.Count; i++)
        {
            if (diskState[i] == fileId)
            {
                if (!foundFile)
                {
                    startIndex = i;
                    foundFile = true;
                }
                size++;
            }
        }

        return foundFile ? ((int startIndex, int size)?)new(startIndex, size) : null;
    }

    static int? FindLeftmostSuitableSpace(List<int> diskState, int currentPosition, int requiredSize)
    {
        int consecutiveSpace = 0;
        int spaceStartIndex = -1;

        for (int i = 0; i < currentPosition; i++)
        {
            if (diskState[i] == -1)
            {
                if (consecutiveSpace == 0)
                {
                    spaceStartIndex = i;
                }
                consecutiveSpace++;

                if (consecutiveSpace >= requiredSize)
                {
                    return spaceStartIndex;
                }
            }
            else
            {
                consecutiveSpace = 0;
            }
        }

        return null;
    }

    static void MoveFile(List<int> diskState, int sourceStart, int targetStart, int size)
    {
        for (int i = 0; i < size; i++)
        {
            diskState[targetStart + i] = diskState[sourceStart + i];
        }

        for (int i = 0; i < size; i++)
        {
            diskState[sourceStart + i] = -1;
        }
    }

    static long CalculateChecksum(List<int> diskState)
    {
        long checksum = 0;
        for (int i = 0; i < diskState.Count; i++)
        {
            if (diskState[i] != -1)
            {
                checksum += (long)i * diskState[i];
            }
        }
        return checksum;
    }
}
