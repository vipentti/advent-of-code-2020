using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

var inputString = await File.ReadAllTextAsync("inputs/day5.txt");

var seats = Parse(inputString);

Part1(seats);
Part2(seats);

static void Part1(List<Seat> seats)
{
    var maxId = seats.Max(it => it.SeatId);

    Console.WriteLine($"Part1: Maximum SeatID = {maxId}");
}

static void Part2(List<Seat> seats)
{
    var ordered = seats.OrderBy(it => it.SeatId).ToList();

    for (int i = 0; i < ordered.Count - 1; ++i)
    {
        var seat = ordered[i];

        var next = ordered[i + 1];

        // Gap found
        if (next.SeatId != seat.SeatId + 1)
        {
            Console.WriteLine($"Candidate between {seat.SeatId} and {next.SeatId} = {seat.SeatId + 1}");
        }
    }
}

static List<Seat> Parse(string input)
{
    var lines = input.Replace("\r", "").Split('\n', StringSplitOptions.RemoveEmptyEntries);

    return lines.Select(Seat.From).ToList();
}

public record Seat(int Row, int Column)
{
    public int SeatId => Row * 8 + Column;

    public static Seat From(string input)
    {
        var rowRange = input.Take(7).Aggregate(new Range(0, 127), (range, ch) => ch switch
        {
            'B' => range.UpperHalf(),
            'F' => range.LowerHalf(),
            _ => throw new InvalidOperationException($"Invalid char '{ch}'"),
        });

        var columnRange = input.Skip(7).Aggregate(new Range(0, 7), (range, ch) => ch switch
        {
            'R' => range.UpperHalf(),
            'L' => range.LowerHalf(),
            _ => throw new InvalidOperationException($"Invalid char '{ch}'"),
        });

        return new Seat(rowRange.Min, columnRange.Min);
    }
}

public record Range(int Min, int Max)
{
    public int RowCount => (Max - Min) / 2;

    public Range LowerHalf()
    {
        return new Range(Min, Min + RowCount);
    }

    public Range UpperHalf()
    {
        return new Range(Min + RowCount + 1, Max);
    }
}
