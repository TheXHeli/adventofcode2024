using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day11;

public static class Solver
{
    const string InputFile = "inputs/day11_1.txt";

    public static void SolveIt_1stPart()
    {
        var input = File.ReadAllText(InputFile).Split(" ");
        var blinkCnt = 0;
        var newList = new List<string>();
        do
        {
            newList = new List<string>();
            for (int i = 0; i < input.Length; i++)
            {
                if (i % 100000 == 0)
                {
                    Console.WriteLine($"{Math.Floor(((decimal)i / input.Length) * 100)}%");
                }

                if (input[i] == "0")
                {
                    //input[i] = "1";
                    newList.Add("1");
                    continue;
                }

                var curStoneLength = input[i].Length;
                var halfStoneLength = curStoneLength / 2;
                if (curStoneLength % 2 == 0)
                {
                    var newRight = input[i].Substring(halfStoneLength, halfStoneLength).TrimStart('0');
                    var newLeft = input[i].Remove(halfStoneLength).TrimStart('0');
                    if (newLeft == "") newLeft = "0";
                    if (newRight == "") newRight = "0";
                    newList.Add(newLeft);
                    newList.Add(newRight);
                    continue;
                }

                var curValue = long.Parse(input[i]);
                var newValue = curValue * 2024;
                newList.Add(newValue.ToString());
            }

            input = newList.ToArray();
            //DebugOutput(input);
            blinkCnt++;
            Console.WriteLine($"blink cnt: {blinkCnt} size: {newList.Count}");
        } while (blinkCnt < 25);

        Console.WriteLine(newList.Count);
    }

    public static void DebugOutput(string[] input)
    {
        Console.WriteLine(string.Join(' ', input));
    }

    public static void DebugOutput(Dictionary<double, int> input)
    {
        var outStr = "";
        foreach (var tmpEntry in input)
        {
            if (tmpEntry.Value != 0)
            {
                outStr += " " + tmpEntry.Key + "(" + tmpEntry.Value + ") ";
            }
        }

        Console.WriteLine(outStr);
    }

    public static void DebugOutput(Dictionary<decimal, long> input)
    {
        var outStr = "";
        foreach (var tmpEntry in input)
        {
            if (tmpEntry.Value != 0)
            {
                outStr += " " + tmpEntry.Key + "(" + tmpEntry.Value + ") ";
            }
        }

        Console.WriteLine(outStr);
    }

    public static void SolveIt_2ndPart(int maxBlinkCount)
    {
        var inputRaw = File.ReadAllText(InputFile).Split(" ").Select(double.Parse).ToArray();
        var stopwatch = Stopwatch.StartNew();
        var blinkCnt = 0;
        var workingDict = new Dictionary<double, long>(); //key = Zahl auf Stein, Value = Cnt
        foreach (var tmpEntry in inputRaw)
        {
            workingDict.Add(tmpEntry, 1);
        }

        var faktorTable = new List<int>{1, 10, 100, 1000, 10000, 100000, 1000000, 10000000, 100000000, 1000000000};

        do
        {
            var curIterDict = new Dictionary<double, long>();
            foreach (var tmpEntry in workingDict.Where(kvp => kvp.Value > 0))
            {
                curIterDict.Add(tmpEntry.Key, tmpEntry.Value);
            }

            var keys = curIterDict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                //das sind die werte aus dem aktuellen blink-setting
                var curCnt = curIterDict[keys[i]];
                var curNumber = keys[i];

                if (curNumber == 0)
                {
                    workingDict[1] += curCnt;
                    workingDict[0] -= curCnt;
                    continue;
                }

                var curStoneLength = Math.Floor(Math.Log10(curNumber)) + 1;
                var halfStoneLength = (int)curStoneLength >> 1;
                if (curStoneLength % 2 == 0)
                {
                    var faktor = faktorTable[halfStoneLength];
                    var newLeft = Math.Floor(curNumber / faktor);
                    var newRight = curNumber - newLeft * faktor;
                    //---
                    if (!workingDict.ContainsKey(newLeft))
                    {
                        workingDict.Add(newLeft, curCnt);
                    }
                    else
                    {
                        workingDict[newLeft] += curCnt;
                    }

                    //---
                    if (!workingDict.ContainsKey(newRight))
                    {
                        workingDict.Add(newRight, curCnt);
                    }
                    else
                    {
                        workingDict[newRight] += curCnt;
                    }

                    workingDict[curNumber] -= curCnt;
                    continue;
                }

                var newNumber = curNumber * 2024;
                if (!workingDict.ContainsKey(newNumber))
                {
                    workingDict.Add(newNumber, curCnt);
                }
                else
                {
                    workingDict[newNumber] += curCnt;
                }

                workingDict[curNumber] -= curCnt;
            }

            //DebugOutput(workingDict);
            blinkCnt++;
            //Console.WriteLine($"blink cnt: {blinkCnt} size: {workingDict.Count}");
        } while (blinkCnt < maxBlinkCount);

        long gesCnt = 0;
        foreach (var tmpEntry in workingDict)
        {
            gesCnt += tmpEntry.Value;
        }

        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
        Console.WriteLine(gesCnt);
    }

    public static void SolveIt_2ndPart_V3()
    {
        var stopWatch = Stopwatch.StartNew();
        var inputRaw = File.ReadAllText(InputFile).Split(" ").Select(decimal.Parse).ToArray();
        var blinkCnt = 0;
        var workingDict = new Dictionary<decimal, long>(); //key = Zahl auf Stein, Value = Cnt
        foreach (var tmpEntry in inputRaw)
        {
            workingDict.Add(tmpEntry, 1);
        }

        do
        {
            var curIterDict = new Dictionary<decimal, long>();
            foreach (var tmpEntry in workingDict.Where(kvp => kvp.Value > 0))
            {
                curIterDict.Add(tmpEntry.Key, tmpEntry.Value);
            }

            var keys = curIterDict.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                // if (i % 1000000 == 0)
                // {
                //     Console.WriteLine($"{Math.Floor(((decimal)i / input.Length) * 100)}%");
                // }
                //das sind die werte aus dem aktuellen blink-setting
                var curCnt = curIterDict[keys[i]];
                var curNumber = keys[i];
                var curNumberAsString = curNumber.ToString();

                if (curNumber == 0)
                {
                    //newList.Add(1);
                    workingDict[1] += curCnt;
                    workingDict[0] -= curCnt;
                    continue;
                }

                if (curNumber == 1)
                {
                    var stopStr = "";
                }

                if (curNumber == 2)
                {
                    var stopStr = "";
                }

                var curStoneLength = curNumberAsString.Length;
                var halfStoneLength = curStoneLength / 2;

                if (curStoneLength % 2 == 0)
                {
                    //
                    var newRightStr = curNumberAsString.Substring(halfStoneLength, halfStoneLength).TrimStart('0');
                    var newLeftStr = curNumberAsString.Remove(halfStoneLength).TrimStart('0');
                    if (newRightStr == "") newRightStr = "0";
                    if (newLeftStr == "") newLeftStr = "0";
                    var newLeft = decimal.Parse(newLeftStr);
                    var newRight = decimal.Parse(newRightStr);
                    //---
                    if (!workingDict.ContainsKey(newLeft))
                    {
                        workingDict.Add(newLeft, curCnt);
                    }
                    else
                    {
                        workingDict[newLeft] += curCnt;
                    }

                    //---
                    if (!workingDict.ContainsKey(newRight))
                    {
                        workingDict.Add(newRight, curCnt);
                    }
                    else
                    {
                        workingDict[newRight] += curCnt;
                    }

                    workingDict[curNumber] -= curCnt;
                    continue;
                }

                var newNumber = curNumber * 2024;
                if (!workingDict.ContainsKey(newNumber))
                {
                    workingDict.Add(newNumber, curCnt);
                }
                else
                {
                    workingDict[newNumber] += curCnt;
                }

                workingDict[curNumber] -= curCnt;
            }

            //DebugOutput(workingDict);
            blinkCnt++;
            //Console.WriteLine($"blink cnt: {blinkCnt} size: {workingDict.Count}");
        } while (blinkCnt < 75);

        long gesCnt = 0;
        foreach (var tmpEntry in workingDict)
        {
            gesCnt += tmpEntry.Value;
        }

        stopWatch.Stop();
        Console.WriteLine(stopWatch.Elapsed);
        Console.WriteLine(gesCnt);
    }

    //Initial arrangement:
    // 125 17
    // 
    // After 1 blink:
    // 253000 1 7
    // 
    // After 2 blinks:
    // 253 0 2024 14168
    // 
    // After 3 blinks:
    // 512072 1 20 24 28676032
    // 
    // After 4 blinks:
    // 512 72 2024 2 0 2 4 2867 6032
    // 
    // After 5 blinks:
    // 1036288 7 2 20 24 4048 1 4048 8096 28 67 60 32
    // 
    // After 6 blinks:
    // 2097446912 14168 4048 2 0 2 4 40 48 2024 40 48 80 96 2 8 6 7 6 0 3 2
    //26: 55312

    //829854673
}