using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day5;

record CusRange(long Min, long Max);

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day5_1.txt";
    private static string[] _inputRaw;
    private static List<CusRange> _aktRanges;

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0;
        _aktRanges = new List<CusRange>();
        var ingredientsList = new List<long>();
        _inputRaw = File.ReadAllLines(InputFile);

        var curPos = 0;
        var abbr = false;
        do
        {
            if (string.IsNullOrWhiteSpace(_inputRaw[curPos]))
            {
                abbr = true;
            }
            else
            {
                var splittedStr = _inputRaw[curPos].Split('-');
                _aktRanges.Add(new CusRange(long.Parse(splittedStr[0]), long.Parse(splittedStr[1])));
            }

            curPos++;
        } while (!abbr);

        for (int i = curPos; i < _inputRaw.Length; i++)
        {
            ingredientsList.Add(long.Parse(_inputRaw[i]));
        }

        foreach (var ingredientItem in ingredientsList)
        {
            if (CheckIfInRange(ingredientItem))
            {
                gesCnt++;
            }
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static bool CheckIfInRange(long ingredientItem)
    {
        return _aktRanges.Any(a => ingredientItem >= a.Min && ingredientItem <= a.Max);
    }


    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0;

        _aktRanges = new List<CusRange>();
        _inputRaw = File.ReadAllLines(InputFile);

        var curPos = 0;
        var abbr = false;
        do
        {
            if (string.IsNullOrWhiteSpace(_inputRaw[curPos]))
            {
                abbr = true;
            }
            else
            {
                var splittedStr = _inputRaw[curPos].Split('-');
                _aktRanges.Add(new CusRange(long.Parse(splittedStr[0]), long.Parse(splittedStr[1])));
            }

            curPos++;
        } while (!abbr);
        
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }
}