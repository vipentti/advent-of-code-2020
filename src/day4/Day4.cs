using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var inputString = await File.ReadAllTextAsync("inputs/day4.txt");

var passports = Parse(inputString);

Part1(passports);
Part2(passports);

static void Part1(List<Passport> passports)
{
    var validCount = passports.Count(it => it.IsValid());

    Console.WriteLine($"Part1: Found {validCount} valid passwords");
}

static void Part2(List<Passport> passports)
{
    var validCount = passports.Count(it => it.IsValidPart2());

    Console.WriteLine($"Part2: Found {validCount} valid passwords");
}

static List<Passport> Parse(string input)
{
    // Blank lines are significant
    var lines = input.Replace("\r", "").Split('\n', StringSplitOptions.None);

    List<string> passportData = new();

    string currentData = "";

    for (int index = 0; index < lines.Length; ++index)
    {
        var line = lines[index];

        if (string.IsNullOrEmpty(line))
        {
            if (!string.IsNullOrEmpty(currentData))
            {
                passportData.Add(currentData);
            }
            currentData = "";
            continue;
        }

        currentData += " " + line;
    }

    return passportData.Select(Passport.From).ToList();
}

public record Passport
{
    public int? BirthYear { get; init; }
    public int? IssueYear { get; init; }
    public int? ExpirationYear { get; init; }
    public string? Height { get; init; }
    public string? HairColor { get; init; }
    public string? EyeColor { get; init; }
    public string? PassportId { get; init; }
    public string? CountryId { get; init; }

    public bool IsValid()
    {
        // CountryId is optional

        return BirthYear is not null
            && IssueYear is not null
            && ExpirationYear is not null
            && Height is not null
            && HairColor is not null
            && EyeColor is not null
            && PassportId is not null
            ;
    }

    public bool IsValidPart2()
    {
        if (!IsValid())
        {
            return false;
        }

        static bool ValidateHeight(string height)
        {
            if (height.EndsWith("cm"))
            {
                int value = int.Parse(height.Replace("cm", ""));
                return value >= 150 && value <= 193;
            }
            else if (height.EndsWith("in"))
            {
                int value = int.Parse(height.Replace("in", ""));
                return value >= 59 && value <= 76;
            }

            return false;
        }

        static bool ValidateHairColor(string color)
        {
            if (!color.StartsWith("#") || color.Length != 7)
            {
                return false;
            }

            const string allowedChars = "0123456789abcdef";

            return color.Skip(1).All(ch => allowedChars.Contains(ch));
        }

        static bool ValidateEyeColor(string color)
        {
            const string allowed = "amb blu brn gry grn hzl oth";

            return color.Length == 3 && allowed.Contains(color);
        }

        static bool ValidatePassportId(string id)
        {
            return id.Length == 9 && id.All(char.IsDigit);
        }

        return (BirthYear >= 1920 && BirthYear <= 2002)
            && (IssueYear >= 2010 && IssueYear <= 2020)
            && (ExpirationYear >= 2020 && ExpirationYear <= 2030)
            && ValidateHeight(Height ?? "")
            && ValidateHairColor(HairColor ?? "")
            && ValidateEyeColor(EyeColor ?? "")
            && ValidatePassportId(PassportId ?? "")
            ;
    }

    public static Passport From(string input)
    {
        var fields = input
            .Split(' ', StringSplitOptions.RemoveEmptyEntries)
            .Select(it => it.Trim())
            .ToList();

        Passport result = new();

        foreach (var field in fields)
        {
            var parts = field.Split(':');

            var key = parts[0];
            var value = parts[1];

            /*
            byr (Birth Year)
            iyr (Issue Year)
            eyr (Expiration Year)
            hgt (Height)
            hcl (Hair Color)
            ecl (Eye Color)
            pid (Passport ID)
            cid (Country ID)
            */
            result = key switch
            {
                "byr" => result with { BirthYear = int.Parse(value) },
                "iyr" => result with { IssueYear = int.Parse(value) },
                "eyr" => result with { ExpirationYear = int.Parse(value) },
                "hgt" => result with { Height = value },
                "hcl" => result with { HairColor = value },
                "ecl" => result with { EyeColor = value },
                "pid" => result with { PassportId = value },
                "cid" => result with { CountryId = value },
                _ => throw new InvalidOperationException($"Unknown key '{key}'"),
            };
        }

        return result;
    }
}
