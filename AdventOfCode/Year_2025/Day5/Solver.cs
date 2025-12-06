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
        var gesCnt = 0l;

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

        ConsolidateRanges();
        foreach (var rangeItem in _aktRanges)
        {
            gesCnt += rangeItem.Max - rangeItem.Min + 1;
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static void ConsolidateRanges()
    {
        var testCnt = 0;
        var found = false;
        var abbr = false;
        do
        {
            var newRanges = new List<CusRange>();
            var usedRanges = new bool[_aktRanges.Count];
            for (var idx = 0; idx < _aktRanges.Count; idx++)
            {
                found = false;
                var curRange = _aktRanges[idx];
                for (int inner = idx + 1; inner < _aktRanges.Count; inner++)
                {
                    if (!usedRanges[idx] && !usedRanges[inner])
                    {
                        var mergedRange = TryToMergeRanges(curRange, _aktRanges[inner]);
                        if (mergedRange != null)
                        {
                            usedRanges[idx] = true;
                            usedRanges[inner] = true;
                            newRanges.Add(mergedRange);
                            found = true;
                            break;
                            Console.WriteLine("merged: " + idx + "-" + inner);
                        }
                    }
                }

                if ((!found) && (!usedRanges[idx]))
                {
                    newRanges.Add(_aktRanges[idx]);
                }
            }

            if (_aktRanges.Count == newRanges.Count) abbr = true;
            _aktRanges = newRanges;
            testCnt++;
        } while (!abbr);

        var stopStr = "";
    }

    private static CusRange? TryToMergeRanges(CusRange range1, CusRange range2)
    {
        if ((range2.Min <= range1.Max) && (range2.Min >= range1.Min))
        {
            return range1 with { Max = Math.Max(range1.Max, range2.Max) };
        }

        if ((range1.Min <= range2.Max) && (range1.Min >= range2.Min))
        {
            return range2 with { Max = Math.Max(range1.Max, range2.Max) };
        }

        return null;
    }
}