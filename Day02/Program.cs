// See https://aka.ms/new-console-template for more information

using Day02;

var input = File.ReadAllLines("../../../input.txt").Select(s => s.Split(' ').Select(int.Parse));

var (part1, part2) = MySolution.Run(input);

Console.WriteLine($"Part 1: {part1}");
Console.WriteLine($"Part 2: {part2}");
    
