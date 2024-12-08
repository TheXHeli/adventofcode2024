using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day7;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var inputStr = File.ReadAllLines("inputs/day7_1.txt");
        var newCleanedArray = inputStr.Select(x => x.Replace(":", "")).ToArray();
        var rawList = Helper.ConvertInputToArrayDecimalArray(newCleanedArray, " ");
        // var maxCnt = 0;
        // foreach (var tmpEntry in rawList)
        // {
        //     if (tmpEntry.Count > maxCnt) maxCnt = tmpEntry.Count;
        // }
        decimal gesamtZahl = 0;
        for (var row = 0; row < rawList.Count; row++)
        {
            var aktEintrag = rawList[row];
            var zielWert = rawList[row][0];
            var kombinationen = 1 << (aktEintrag.Count - 2);
            for (var aktKombination = 0; aktKombination < kombinationen; aktKombination++)
            {
                //590877201219: 9 985 5 9 7 8 76 8 174
                //11111111
                //00000001 & AND
                //
                //00000001
                //
                var aktWert = rawList[row][1];
                for (int j = 2; j < rawList[row].Count; j++)
                {
                    if ((aktKombination & (1 << (j - 2))) == (1 << (j - 2)))
                    {
                        aktWert = aktWert + rawList[row][j];
                    }
                    else
                    {
                        aktWert = aktWert * rawList[row][j];
                    }
                }

                if (aktWert == zielWert)
                {
                    gesamtZahl += zielWert;
                    break;
                }
            }
        }

        Console.WriteLine(gesamtZahl);
    }

    public static void SolveIt_2ndPart()
    {
        var inputStr = File.ReadAllLines("inputs/day7_1.txt");
        var newCleanedArray = inputStr.Select(x => x.Replace(":", "")).ToArray();
        var rawList = Helper.ConvertInputToArrayDecimalArray(newCleanedArray, " ");
        var rowsValidFrom1stRun = GetRows("inputs/day7_1.txt");
        decimal gesamtZahl = 0;
        for (var row = 0; row < rawList.Count; row++)
        {
            Console.WriteLine($"{row}");
            var aktEintrag = rawList[row];
            var zielWert = rawList[row][0];
            long kombinationen = 1 << (aktEintrag.Count - 2) * 2;
            //Console.WriteLine($"max = {kombinationen:b32}");
            for (long aktKombination = 0; aktKombination < kombinationen; aktKombination++)
            {
                //590877201219: 9 985 5 9 7 8 76 8 174
                //0111
                //0100  1<<2 + 0
                //1000  1<<2 + 1
                //1100  1<<2 + 2
                //0001  1<<0 
                //0010  1<<0 + 1
                //0011  1<<0 + 2
                var aktWert = rawList[row][1];
                // if (aktKombination == 892538)
                // {
                //     var stopStr = "";
                // }
                for (int j = 2; j < rawList[row].Count; j++)
                {
                    var maskValueA = (1) << ((j - 2) * 2);
                    var maskValueB = (2) << ((j - 2) * 2);
                    var maskValueC = (3) << ((j - 2) * 2);
                    //Console.WriteLine($"k{aktKombination:b16}");
                    // Console.WriteLine($"a{maskValueA:b16}");
                    // Console.WriteLine($"b{maskValueB:b16}");
                    // Console.WriteLine($"c{maskValueC:b16}");
                    if ((aktKombination & maskValueC) == maskValueC)
                    {
                        // var mult = getMultiplierFromNumber(rawList[row][j]);
                        // aktWert = aktWert * mult + rawList[row][j];
                        var resultStr = aktWert.ToString() + rawList[row][j].ToString();
                        aktWert = decimal.Parse(resultStr);
                        // aktWert = -1;
                        // break;
                    }
                    else if ((aktKombination & maskValueA) == maskValueA)
                    {
                        aktWert += rawList[row][j];
                    }
                    else if ((aktKombination & maskValueB) == maskValueB)
                    {
                        aktWert *= rawList[row][j];
                    }
                    else
                    {
                        aktWert = -1;
                        break;
                    }
                }

                if (aktWert == zielWert)
                {
                    // if (!usedNewOperator)
                    // {
                    if (!rowsValidFrom1stRun.Contains(row))
                    {
                        Console.WriteLine($"k = {aktKombination:b16}");
                        var stopStr = "";
                        gesamtZahl += zielWert;
                    }
                    //gesamtZahl += zielWert;
                    //}

                    break;
                }
            }
        }

        //  538191549061
        //34074575868240
        //var loesung1 = GetRows("inputs/day7_1.txt");
        //Console.WriteLine(loesung1);
        //Console.WriteLine("---");
        Console.WriteLine(gesamtZahl);
        //Console.WriteLine(gesamtZahl + loesung1);
    }

    private static decimal getMultiplierFromNumber(decimal justanumber)
    {
        return (decimal)Math.Pow(10, justanumber.ToString().Length);
    }

    public static List<long> GetRows(string filename)
    {
        var inputStr = File.ReadAllLines(filename);
        var newCleanedArray = inputStr.Select(x => x.Replace(":", "")).ToArray();
        var rawList = Helper.ConvertInputToArrayDecimalArray(newCleanedArray, " ");
        var retVal = new List<long>();
        decimal gesamtZahl = 0;
        for (var row = 0; row < rawList.Count; row++)
        {
            var aktEintrag = rawList[row];
            var zielWert = rawList[row][0];
            var kombinationen = 1 << (aktEintrag.Count - 2);
            for (var aktKombination = 0; aktKombination < kombinationen; aktKombination++)
            {
                //590877201219: 9 985 5 9 7 8 76 8 174
                //11111111
                //00000001 & AND
                //
                //00000001
                //
                var aktWert = rawList[row][1];
                for (int j = 2; j < rawList[row].Count; j++)
                {
                    if ((aktKombination & (1 << (j - 2))) == (1 << (j - 2)))
                    {
                        aktWert = aktWert + rawList[row][j];
                    }
                    else
                    {
                        aktWert = aktWert * rawList[row][j];
                    }
                }

                if (aktWert == zielWert)
                {
                    retVal.Add(row);
                    gesamtZahl += zielWert;
                    break;
                }
            }
        }

        Console.WriteLine(gesamtZahl);
        return retVal;
    }
}