using System;
using System.Collections.Generic;
using System.Linq;


var contentString = await System.IO.File.ReadAllTextAsync("inputs/day1.txt");

var values = contentString
    .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
    .Select(int.Parse)
    .OrderBy(it => it)
    .ToList();

const int Target = 2020;

static void Part1(List<int> values)
{
    for (int index = 0; index < values.Count - 1; ++index)
    {
        int currentValue = values[index];

        int valueToFind = Target - currentValue;

        int foundIndex = values.BinarySearch(valueToFind);

        if (foundIndex >= 0)
        {
            var foundValue = values[foundIndex];
            Console.WriteLine($"Found {foundIndex} with value {foundValue}");

            Console.WriteLine($"Multiplied: {currentValue}x{foundValue}={currentValue * foundValue}");

            break;
        }
    }
}

static void Part2(List<int> values)
{
    for (int index = 0; index < values.Count - 1; ++index)
    {
        int currentValue = values[index];

        for (int other = index + 1; other < values.Count - 1; ++other) {
            int nextValue = values[other];

            int valueToFind = Target - currentValue - nextValue;


            int foundIndex = values.BinarySearch(valueToFind);

            if (foundIndex >= 0)
            {
                var foundValue = values[foundIndex];
                Console.WriteLine($"Found {foundIndex} with value {foundValue}");

                Console.WriteLine($"Multiplied: {currentValue}x{nextValue}x{foundValue}={currentValue * foundValue * nextValue}");

                return;
            }
        }
    }
}

Part1(values);
Part2(values);



// Console.WriteLine("Hello World!");
// Console.WriteLine($"{string.Join("\n", values.Select(it => it.ToString()))}");
