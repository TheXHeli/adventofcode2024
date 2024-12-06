using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day4;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var linesToParse = File.ReadAllLines("inputs/day4_1.txt");
        var stopWatch = Stopwatch.StartNew();
        var cnt = GetXMasCnt(linesToParse, "XMAS");
        cnt += GetXMasCnt(linesToParse, "SAMX");
        var newLinesToParse = Transpose(linesToParse);
        cnt += GetXMasCnt(newLinesToParse, "XMAS");
        cnt += GetXMasCnt(newLinesToParse, "SAMX");
        newLinesToParse = ConstructDiagonalLines(linesToParse);
        cnt += GetXMasCnt(newLinesToParse, "XMAS");
        cnt += GetXMasCnt(newLinesToParse, "SAMX");
        
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed}");
        Console.WriteLine(cnt);
    }

    private static string[] Transpose(string[] linesToParse)
    {
        var retVal = new List<string>();
        for (int x = 0; x < linesToParse[0].Length; x++) //Spalten iterieren
        {
            var tmpstr = "";
            for (int y = 0; y < linesToParse.Length; y++) //jeweils Zeilen
            {
                tmpstr += linesToParse[y][x];
            }

            retVal.Add(tmpstr);
        }

        return retVal.ToArray();
    }

    private static string[] ConstructDiagonalLines(string[] linesToParse)
    {
        var allResultLines = new List<string>();
        var firstShiftLines = new List<string>();
        var secondShiftLines = new List<string>();

        //
        for (int y = 0; y < linesToParse.Length; y++)
        {
            var tmpstr = linesToParse[y].PadLeft(linesToParse[y].Length + y, ' ');
            tmpstr = tmpstr.PadRight(linesToParse[y].Length * 2, ' ');
            firstShiftLines.Add(tmpstr);
        }

        allResultLines.AddRange(Transpose(firstShiftLines.ToArray()));

        //
        for (int y = 0; y < linesToParse.Length; y++)
        {
            var tmpstr = linesToParse[y].PadRight(linesToParse[y].Length + y, ' ');
            tmpstr = tmpstr.PadLeft(linesToParse[y].Length * 2, ' ');
            secondShiftLines.Add(tmpstr);
        }

        allResultLines.AddRange(Transpose(secondShiftLines.ToArray()));


        return allResultLines.ToArray();
    }

    private static int GetXMasCnt(string[] linesToParse, string strToFind)
    {
        var foundCnt = 0;
        var found = false;
        for (int i = 0; i < linesToParse.Length; i++)
        {
            var startPos = 0;
            do
            {
                found = false;
                var pos1 = linesToParse[i].IndexOf(strToFind, startPos);
                if (pos1 != -1)
                {
                    foundCnt++;
                    startPos = pos1 + 1;
                    found = true;
                }
            } while (found);
        }

        return foundCnt;
    }

    public static void SolveIt_2ndPart()
    {
        var xmasFoundCnt = 0;
        var linesToParse = File.ReadAllLines("inputs/day4_1.txt");
        var stopWatch = Stopwatch.StartNew();
        for (int x = 1; x < linesToParse[0].Length - 1; x++)
        {
            for (int y = 1; y < linesToParse.Length - 1; y++)
            {
                var centralChar = linesToParse[y][x];
                if (centralChar == 'A')
                {
                    var word1 = string.Concat(linesToParse[y - 1][x - 1] , linesToParse[y + 1][x + 1]);
                    var word2 = string.Concat(linesToParse[y + 1][x - 1] , linesToParse[y - 1][x + 1]);
                    if (word1 == "MS" || word1 == "SM")
                    {
                        if (word2 == "MS" || word2 == "SM")
                        {
                            xmasFoundCnt++;
                        }
                    }
                    // if (((linesToParse[y - 1][x - 1] == 'M' && linesToParse[y + 1][x + 1] == 'S')) ||
                    //     ((linesToParse[y - 1][x - 1] == 'S' && linesToParse[y + 1][x + 1] == 'M')))
                    // {
                    //     if (((linesToParse[y + 1][x - 1] == 'M' && linesToParse[y - 1][x + 1] == 'S')) ||
                    //         ((linesToParse[y + 1][x - 1] == 'S' && linesToParse[y - 1][x + 1] == 'M')))
                    //     {
                    //         xmasFoundCnt++;
                    //     }
                    // }
                }
            }
        }
        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed}");
        Console.WriteLine(xmasFoundCnt);
    }
}