using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

var inputString = await File.ReadAllTextAsync("inputs/day6.txt");

Console.WriteLine($"Running day6");

var groups = Parse(inputString);

Part1(groups);
Part2(groups);

static List<List<string>> Parse(string input)
{
    var lines = input
        .Replace("\r", "")
        .Split('\n', StringSplitOptions.None);

    List<string> group = new();
    List<List<string>> groups = new();

    foreach (var line in lines)
    {
        if (string.IsNullOrEmpty(line))
        {
            groups.Add(group);
            group = new();
        }
        else
        {
            group.Add(line);
        }
    }

    return groups;
}

static void Part1(List<List<string>> groups)
{
    Console.WriteLine($"Part1: Running day6");

    int total = 0;

    foreach (var group in groups)
    {
        total += group.SelectMany(it => it).Distinct().Count();
    }

    Console.WriteLine($"Part1: Total sum {total}");

    Console.WriteLine($"Part1: Finished day6");
}

static void Part2(List<List<string>> groups)
{
    Console.WriteLine($"Part2: Running day6");

    int total = 0;

    foreach (var group in groups)
    {
        var groupChars = group.SelectMany(it => it).ToList();
        var groupAnswers = group.SelectMany(it => it).ToHashSet();

        foreach (var ch in groupAnswers)
        {
            if (groupChars.Count(it => it == ch) == group.Count)
            {
                total++;
            }
        }
    }
    Console.WriteLine($"Part2: Total sum {total}");

    Console.WriteLine($"Part2: Finished day6");
}

