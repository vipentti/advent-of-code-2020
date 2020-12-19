using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

var contentString = await System.IO.File.ReadAllTextAsync("inputs/day3.txt");

var map = Parse(contentString);

Console.WriteLine($"Map: {map.Width}x{map.Height}");

Part1(map);
Part2(map);

static void Part1(Tilemap map)
{
    const int Right = 3;
    const int Down = 1;

    (int x, int y) = (0, 0);

    int trees = 0;

    do {
        (x, y) = (x + Right, y + Down);

        if (y > map.Height - 1) {
            break;
        }

        if (map.GetTile(x, y) == Tile.Tree) {
            trees++;
        }

    } while(true);

    Console.WriteLine($"Found {trees} trees");
}

static void Part2(Tilemap map)
{
    (int Right, int Down)[] slopes = new[] {
        (1, 1),
        (3, 1),
        (5, 1),
        (7, 1),
        (1, 2),
    };

    long[] results = new long[slopes.Length];

    Parallel.For(0, slopes.Length, index => {
        long trees = CountTrees(map, slopes[index]);
        Console.WriteLine($"Trees {trees} for {slopes[index]}");
        results[index] = trees;
    });

    long total = results.Aggregate((lhs, rhs) => lhs * rhs);

    Console.WriteLine($"Total {total}");
}

static int CountTrees(Tilemap map, (int Right, int Down) slope) {

    (int x, int y) = (0, 0);

    int trees = 0;

    do {
        (x, y) = (x + slope.Right, y + slope.Down);

        if (y > map.Height - 1) {
            break;
        }

        if (map.GetTile(x, y) == Tile.Tree) {
            trees++;
        }

    } while(true);

    return trees;
}


static Tilemap Parse(string input)
{
    var lines = input.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

    int height = lines.Length;
    int width = lines[0].Length;

    var tiles = lines.SelectMany(it => it.AsEnumerable()).Select(ch => ch switch {
        '.' => Tile.Open,
        '#' => Tile.Tree,
        _ => throw new InvalidOperationException($"unknown {ch}"),
    }).ToList();

    return new Tilemap(width, height, tiles);
}

public enum Tile
{
    Open,
    Tree,
}

public class Tilemap
{
    public Tilemap(int width, int height, List<Tile> tiles)
    {
        Width = width;
        Height = height;
        Tiles = tiles;
    }

    public int Width { get; }
    public int Height { get; }
    public List<Tile> Tiles { get; }

    public Tile GetTile(int x, int y)
    {
        if (x > Width - 1)
        {
            x %= Width;
        }

        return Tiles[(y * Width) + x];
    }
}
