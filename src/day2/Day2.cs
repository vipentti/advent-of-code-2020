using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

var contentString = await System.IO.File.ReadAllTextAsync("inputs/day2.txt");

var inputs = Parse(contentString);

Part1(inputs);
Part2(inputs);

static void Part1(List<(PasswordPolicy, string)> values)
{
    int validCount = values.Count(it => it.Item1.IsValid(it.Item2));

    Console.WriteLine($"Part1 Found {validCount} valid passwords");
}

static void Part2(List<(PasswordPolicy, string)> values)
{
    int validCount = values.Count(it => it.Item1.IsValidPart2(it.Item2));

    Console.WriteLine($"Part2 Found {validCount} valid passwords");
}

static List<(PasswordPolicy, string)> Parse(string input)
{
    return input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
        .Select(line =>
        {
            var lineParts = line.Split(':', StringSplitOptions.RemoveEmptyEntries)
                .Select(it => it.Trim())
                .ToList();

            Debug.Assert(lineParts.Count == 2);

            return (PasswordPolicy.From(lineParts[0]), lineParts[1]);
        })
        .ToList();
}

public record PasswordPolicy(int MinCount, int MaxCount, char Character)
{
    public bool IsValid(string password)
    {
        int foundCount = password.Count(it => it == Character);

        return foundCount >= MinCount && foundCount <= MaxCount;
    }

    public bool IsValidPart2(string password)
    {
        int firstIndex = MinCount - 1;
        int secondIndex = MaxCount - 1;

        if (firstIndex > password.Length - 1
           || secondIndex > password.Length - 1)
        {
            return false;
        }

        bool firstIs = password[firstIndex] == Character;
        bool secondIs = password[secondIndex] == Character;

        return firstIs ^ secondIs;
    }

    public static PasswordPolicy From(string input)
    {
        var parts = input.Split(' ', StringSplitOptions.RemoveEmptyEntries);

        Debug.Assert(parts.Length == 2);
        Debug.Assert(parts[1].Length == 1);

        var ranges = parts[0].Split('-', StringSplitOptions.RemoveEmptyEntries);

        Debug.Assert(ranges.Length == 2);

        int min = int.Parse(ranges[0]);
        int max = int.Parse(ranges[1]);

        char ch = parts[1][0];

        return new PasswordPolicy(min, max, ch);
    }
}
