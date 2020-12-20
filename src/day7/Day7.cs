using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using System.Linq;

var inputString = await File.ReadAllTextAsync("inputs/day7.txt");

Console.WriteLine($"Running day7");

var data = Parse(inputString);

Part1(data);
Part2(data);

static BagData Parse(string input)
{
    return BagData.From(input);
}

static void Part1(BagData input)
{
    Console.WriteLine($"Part1: Running day7");

    var bagToSearch = new Bag("shiny gold");

    var bags = input.BagsWhichCanContain(bagToSearch).ToHashSet();

    Console.WriteLine($"Part1: Found {bags.Count} bags for {bagToSearch}");

    Console.WriteLine($"Part1: Finished day7");
}

static void Part2(BagData input)
{
    Console.WriteLine($"Part2: Running day7");

    var bagToSearch = new Bag("shiny gold");

    Console.WriteLine($"Part2: {bagToSearch} contains {input.CountTotalBagsFor(bagToSearch)} bags");

    Console.WriteLine($"Part2: Finished day7");
}

public class BagData
{
    public Dictionary<Bag, ValueList<(int Count, Bag Bag)>> Bags { get; } = new();

    public Dictionary<Bag, ValueList<Bag>> Parents { get; } = new();

    public int CountTotalBagsFor(Bag bagToSearch)
    {
        if (!Bags.ContainsKey(bagToSearch))
        {
            return 0;
        }

        var bags = Bags[bagToSearch];

        int total = 0;

        foreach (var (count, bag) in bags)
        {
            var subTotal = CountTotalBagsFor(bag);

            total += count;
            total += count * subTotal;
        }

        return total;
    }

    public IEnumerable<Bag> BagsWhichCanContain(Bag bagToSearch)
    {
        if (!Parents.ContainsKey(bagToSearch))
        {
            return Enumerable.Empty<Bag>();
        }

        var parents = Parents[bagToSearch];

        var children = parents.SelectMany(BagsWhichCanContain);

        return parents.Union(children).ToHashSet();
    }

    public override string ToString()
    {

        StringBuilder stringBuilder = new();
        stringBuilder.Append(GetType().Name);
        stringBuilder.Append('{');

        bool first = true;

        foreach (var item in Bags)
        {
            if (first)
            {
                first = false;
            }
            else
            {
                stringBuilder.Append(", ");
            }
            stringBuilder.Append('{').Append(item).Append('}');
        }

        stringBuilder.Append('}');

        return stringBuilder.ToString();
    }

    public static BagData From(string input)
    {
        var lines = input
            .Replace("\r", "")
            .Split('\n', StringSplitOptions.RemoveEmptyEntries);

        BagData result = new();

        foreach (var bag in lines.Select(Bag.From))
        {
            result.AddBag(bag.Bag, bag.Bags);
        }

        return result;
    }

    public void AddBag(Bag bag, ValueList<(int Count, Bag Bag)> bags)
    {
        if (Bags.ContainsKey(bag))
        {
            Bags[bag].AddRange(bags);
        }
        else
        {
            Bags[bag] = bags;
        }

        foreach (var b in bags)
        {
            AddBag(b.Bag, new());
            AddParent(b.Bag, bag);
        }
    }

    public void AddParent(Bag bag, Bag parent)
    {
        if (Parents.ContainsKey(bag))
        {
            Parents[bag].Add(parent);
        }
        else
        {
            Parents[bag] = new() { parent };
        }
    }
}


public record Bag(string Color)
{
    public static (Bag Bag, ValueList<(int Count, Bag Bag)> Bags) From(string input)
    {
        const string ContainText = "bags contain";

        var containIndex = input.IndexOf(ContainText, StringComparison.Ordinal);

        var color = input[..containIndex].Trim();

        if (input.EndsWith("no other bags."))
        {
            return (new Bag(color), new());
        }

        var restOfTheBags = input[(containIndex + ContainText.Length + 1)..]
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(it => it.Trim(new[] { ' ', '.' }))
            .ToList();

        var subBags = restOfTheBags
            .Select(text =>
            {
                var digits = string.Join("", text.TakeWhile(char.IsDigit));
                int count = int.Parse(digits);

                string bagText = text.Substring(digits.Length);

                string color = bagText.Replace("bags", "").Replace("bag", "").Trim();

                return (count, new Bag(color));
            })
            .ToValueList();

        return (new Bag(color), subBags);
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

    public void AddRange(IEnumerable<T> collection)
    {
        _items.AddRange(collection);
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
