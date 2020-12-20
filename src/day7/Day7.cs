using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

var inputString = await File.ReadAllTextAsync("inputs/day7.txt");

Console.WriteLine($"Running day7");

Console.WriteLine(Bag.From("posh teal bags contain 2 faded coral bags, 3 striped crimson bags, 1 faded red bag."));

Part1();
Part2();

static void Part1()
{
    Console.WriteLine($"Part1: Running day7");

    Console.WriteLine($"Part1: Finished day7");
}

static void Part2()
{
    Console.WriteLine($"Part2: Running day7");

    Console.WriteLine($"Part2: Finished day7");
}


public record Bag
{
    public Bag()
    {
    }

    public Bag(string color)
    {
        Color = color;
    }

    public string Color { get; init; } = "";

    public ValueList<(int Count, Bag Bag)> Bags { get; init; } = new();

    public static Bag From(string input)
    {
        const string ContainText = "bags contain";

        var containIndex = input.IndexOf(ContainText, StringComparison.Ordinal);

        var color = input[..containIndex].Trim();

        if (input.EndsWith("no other bags."))
        {
            return new Bag(color);
        }

        var restOfTheBags = input[(containIndex + ContainText.Length + 1)..]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(it => it.Trim(new[] { ' ', '.' }))
            .ToList();

        Console.WriteLine($"parts {string.Join("|", restOfTheBags)}");

        var subBags = restOfTheBags
            .Select(text => {
                var digits = string.Join("", text.TakeWhile(char.IsDigit));
                int count = int.Parse(digits);

                string bagText = text.Substring(digits.Length);

                string color = bagText.Replace("bags", "").Replace("bag", "").Trim();

                return (count, new Bag(color));
            })
            .ToValueList();

        return new Bag(color)
        {
            Bags = subBags,
        };
    }
}

public static class Extensions
{
    public static ValueList<T> ToValueList<T>(this IEnumerable<T> values)
        => new ValueList<T>(values);
}

public class ValueList<T> : IEnumerable<T>, System.Collections.IEnumerable, IEquatable<ValueList<T>>
{
    private readonly List<T> _items = new();

    public int Count => _items.Count;

    public ValueList()
    {
    }

    public ValueList(IEnumerable<T> values)
    {
        _items = values.ToList();
    }

    public void Add(T value)
    {
        _items.Add(value);
    }

    public IEnumerator<T> GetEnumerator()
    {
        return _items.GetEnumerator();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }

    public bool Equals(ValueList<T>? other)
    {
        return other is not null && other.Count == Count && this.SequenceEqual(other);
    }

    public override bool Equals(object? obj)
    {
        return obj is ValueList<T> other && Equals(other);
    }

    public override int GetHashCode()
    {
        return _items.GetHashCode();
    }

    public override string ToString()
    {
        StringBuilder stringBuilder = new();
        stringBuilder.Append(GetType().Name);
        stringBuilder.Append('[');

        bool first = true;

        foreach (var item in _items)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                stringBuilder.Append(", ");
            }
            stringBuilder.Append(item);
        }

        stringBuilder.Append(']');

        return stringBuilder.ToString();
    }
}
