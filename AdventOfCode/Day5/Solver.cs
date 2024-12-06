using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day5;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var relationDict = new Dictionary<int, List<int>>();
        var rawList = Helper.GetArrayArrayFromFile("inputs/day5_1.txt", "|");
        foreach (var tmpEntry in rawList)
        {
            if (!relationDict.ContainsKey(tmpEntry[0]))
            {
                relationDict.Add(tmpEntry[0], new List<int>());
            }

            relationDict[tmpEntry[0]].Add(tmpEntry[1]);
        }

        var result = 0;
        var listToCheck = Helper.GetArrayArrayFromFile("inputs/day5_2.txt", ",");
        var stopWatch = Stopwatch.StartNew();
        foreach (var tmpEntry in listToCheck)
        {
            var isValid = CheckValid(tmpEntry, relationDict);
            if (isValid)
            {
                result += tmpEntry[tmpEntry.Count >> 1];
            }
        }

        stopWatch.Stop();
        Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds);
        Console.WriteLine(result);
    }

    private static bool CheckValid(List<int> tmpEntry, Dictionary<int, List<int>> relationDict)
    {
        for (int i = 0; i < tmpEntry.Count - 1; i++)
        {
            var validList = relationDict[tmpEntry[i]];
            var testArray = tmpEntry.Slice(i + 1, tmpEntry.Count - i - 1);
            var interSectResult = testArray.Intersect(relationDict[tmpEntry[i]]);
            if (interSectResult.Count() != testArray.Count)
            {
                return false;
            }
        }

        return true;
    }

    public static void SolveIt_2ndPart()
    {
        var relationDict = new Dictionary<int, List<int>>();
        var rawList = Helper.GetArrayArrayFromFile("inputs/day5_1.txt", "|");
        foreach (var tmpEntry in rawList)
        {
            if (!relationDict.ContainsKey(tmpEntry[0]))
            {
                relationDict.Add(tmpEntry[0], new List<int>());
            }

            relationDict[tmpEntry[0]].Add(tmpEntry[1]);
        }

        var result = 0;
        var listToCheck = Helper.GetArrayArrayFromFile("inputs/day5_2.txt", ",");
        var stopWatch = Stopwatch.StartNew();
        foreach (var tmpEntry in listToCheck)
        {
            var isValid = CheckValid(tmpEntry, relationDict);
            if (!isValid)
            {
                TryReordering(tmpEntry, relationDict);
                result += tmpEntry[tmpEntry.Count >> 1];
            }
        }

        stopWatch.Stop();
        Console.WriteLine(stopWatch.Elapsed.TotalMilliseconds);
        Console.WriteLine(result);
    }

    private static List<int> TryReordering(List<int> tmpEntry, Dictionary<int, List<int>> relationDict)
    {
        for (int i = 0; i < tmpEntry.Count - 1; i++)
        {
            bool correct;
            do
            {
                correct = true;
                var testArray = tmpEntry.Slice(i + 1, tmpEntry.Count - i - 1);
                var interSectResult = testArray.Except(relationDict[tmpEntry[i]]).ToList();
                if (interSectResult.Any())
                {
                    correct = false;
                    var idxToSwitch = tmpEntry.IndexOf(interSectResult[0]);
                    tmpEntry[idxToSwitch] = tmpEntry[i];
                    tmpEntry[i] = interSectResult[0];
                }
            } while (!correct);
        }

        return tmpEntry;
    }
}