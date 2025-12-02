using AdventOfCode.Common;

namespace AdventOfCode.Year_2024.Day2;

public static class Solver
{
    public enum Direction
    {
        Undefined = 0,
        Hoch = 1,
        Runter = 2
    }

    public static void SolveIt_1stPart()
    {
        var rawList = Helper.GetArrayArrayFromFile("inputs/day2_1.txt", " ");
        var safeCount = 0;
        foreach (var rowToAnalyze in rawList)
        {
            Direction dir = Direction.Undefined;
            var itsSafe = true;

            for (int i = 0; i < rowToAnalyze.Count - 1; i++)
            {
                var val1 = rowToAnalyze[i];
                var val2 = rowToAnalyze[i + 1];
                if (dir == Direction.Undefined)
                {
                    dir = CalcCurrentDirection(val1, val2);
                }

                var curDir = CalcCurrentDirection(val1, val2);
                if (curDir != dir) itsSafe = false; //sobald sich die Richtung ändert, ungültige Reihe
                var curDistance = Math.Abs(val2 - val1);
                if ((curDistance < 1) || (curDistance > 3)) itsSafe = false;
            }

            if (itsSafe) safeCount++;
        }

        Console.WriteLine(safeCount);
    }

    public static void SolveIt_2ndPart()
    {
        var rawList = Helper.GetArrayArrayFromFile("inputs/day2_1.txt", " ");
        var safeCount = 0;
        foreach (var rowToAnalyze in rawList)
        {
            Direction dir = Direction.Undefined;
            var dirList = new List<Direction>();
            dirList.Add(Direction.Hoch);
            dirList.Add(Direction.Runter);
            var itsSafe = true;
            for (int m = 0; m < dirList.Count; m++)
            {
                dir = dirList[m];
                for (int g = -1; g < rowToAnalyze.Count; g++) //ignorepos
                {
                    itsSafe = true;
                    var rowWithOutSpecificPostion = rowToAnalyze.Where((_, i) => i != g).ToList();
                    for (int i = 0; i < rowWithOutSpecificPostion.Count - 1; i++)
                    {
                        var val1 = rowWithOutSpecificPostion[i];
                        var val2 = rowWithOutSpecificPostion[i + 1];
                        var curDir = CalcCurrentDirection(val1, val2);
                        if (curDir != dir) itsSafe = false; //sobald sich die Richtung ändert, ungültige Reihe
                        var curDistance = Math.Abs(val2 - val1);
                        if ((curDistance < 1) || (curDistance > 3)) itsSafe = false;
                    }

                    if (itsSafe)
                    {
                        safeCount++;
                        break;
                    }
                }

                if (itsSafe) break;
            }
        }

        Console.WriteLine(safeCount);
    }

    private static Direction CalcCurrentDirection(int val1, int val2)
    {
        return val2 < val1 ? Direction.Runter : Direction.Hoch;
    }
}