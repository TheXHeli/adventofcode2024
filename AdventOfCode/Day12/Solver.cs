using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day12;

public static class Solver
{
    const string InputFile = "inputs/day12_bsp.txt";
    private static int _borderX;
    private static int _borderY;
    private static string[] _inputRaw;
    private static bool[,] _dawarichschonMap;
    private static bool[,] _dawarichgeradeschonMap;
    private static List<int> _blockErgList = new List<int>();
    private static List<int> _blockCnt = new List<int>();

    public static void SolveIt_1stPart()
    {
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length;
        _borderX = _inputRaw[0].Length;
        _dawarichschonMap = new bool[_borderX, _borderY];

        var plantFieldPos = 0;
        for (int x = 0; x < _borderX; x++)
        {
            for (int y = 0; y < _borderY; y++)
            {
                // if (_inputRaw[y][x] == 'A')
                // {
                if (!_dawarichschonMap[x, y])
                {
                    _blockErgList.Add(0);
                    _blockCnt.Add(0);
                    TryToFindField(x, y, _inputRaw[y][x], plantFieldPos);
                    //Console.WriteLine($"{_inputRaw[y][x]} - {fenceLength}");
                    plantFieldPos++;
                }
                //}
            }
        }

        var gesErg = 0;
        for (int idx = 0; idx < _blockErgList.Count; idx++)
        {
            gesErg += _blockErgList[idx] * _blockCnt[idx];
        }

        Console.WriteLine(gesErg);
        var stopStr = "";
    }

    private static int TryToFindField(int x, int y, char curPlantType, int curBlockToCheck)
    {
        if (x < 0 || x >= _borderX || y < 0 || y >= _borderY) return 0;
        if (curPlantType != _inputRaw[y][x]) return 0;
        if (_dawarichschonMap[x, y]) return 0;
        //Block behandeln
        _dawarichschonMap[x, y] = true;
        var fencesToAdd = GetFencesToAdd(x, y, curPlantType);
        _blockErgList[curBlockToCheck] += fencesToAdd;
        _blockCnt[curBlockToCheck]++;
        Console.WriteLine($"{curPlantType} -> {x} - {y}");
        //Neue Positionen finden
        TryToFindField(x + 1, y, curPlantType, curBlockToCheck);
        TryToFindField(x - 1, y, curPlantType, curBlockToCheck);
        TryToFindField(x, y - 1, curPlantType, curBlockToCheck);
        TryToFindField(x, y + 1, curPlantType, curBlockToCheck);
        //}
        return 0;
    }

    private static int TryToFindFieldAndDrawFences(int x, int y, char curPlantType, int curBlockToCheck,
        bool[,] fenceMap)
    {
        if (x < 0 || x >= _borderX || y < 0 || y >= _borderY) return 0;
        if (curPlantType != _inputRaw[y][x]) return 0;
        if (_dawarichschonMap[x, y]) return 0;
        //Block behandeln
        _dawarichschonMap[x, y] = true;
        // var fencesToAdd = GetFencesToAdd(x, y, curPlantType);
        // _blockErgList[curBlockToCheck] += fencesToAdd;
        // _blockCnt[curBlockToCheck]++;
        WriteFencesToMap(x, y, curPlantType, fenceMap);
        //Console.WriteLine($"{curPlantType} -> {x} - {y}");
        //Neue Positionen finden
        TryToFindFieldAndDrawFences(x + 1, y, curPlantType, curBlockToCheck, fenceMap);
        TryToFindFieldAndDrawFences(x - 1, y, curPlantType, curBlockToCheck, fenceMap);
        TryToFindFieldAndDrawFences(x, y - 1, curPlantType, curBlockToCheck, fenceMap);
        TryToFindFieldAndDrawFences(x, y + 1, curPlantType, curBlockToCheck, fenceMap);
        //}
        return 0;
    }

    private static void WriteFencesToMap(int x, int y, char curPlantType, bool[,] fenceMap)
    {
        //#BB //BB
        // #B //AB
        //AAAA
        if (curPlantType == 'B')
        {
            var stopStr = "";
            if (x == 1 && y == 1)
            {
                stopStr = "";
            }
        }

        DrawSingleFence(x - 1, y - 1, curPlantType, fenceMap);
        DrawSingleFence(x - 1, y, curPlantType, fenceMap);
        DrawSingleFence(x - 1, y + 1, curPlantType, fenceMap);

        DrawSingleFence(x, y - 1, curPlantType, fenceMap);
        DrawSingleFence(x, y + 1, curPlantType, fenceMap);

        DrawSingleFence(x + 1, y - 1, curPlantType, fenceMap);
        DrawSingleFence(x + 1, y, curPlantType, fenceMap);
        DrawSingleFence(x + 1, y + 1, curPlantType, fenceMap);
    }

    private static void DrawSingleFence(int x, int y, char curPlantType, bool[,] fenceMap)
    {
        if (x < 0 || x >= _borderX || y < 0 || y >= _borderY) //wenn irgendwie außerhalb
        {
            fenceMap[x + 1, y + 1] = true;
        }
        else
        {
            if (_inputRaw[y][x] == curPlantType) return;
            fenceMap[x + 1, y + 1] = true;
        }
    }

    private static void PlotFenceMap(bool[,] fenceMap, int fenceBorderX,
        int fenceBorderY)
    {
        for (int y = 0; y < fenceBorderY; y++)
        {
            var outStr = "";
            for (int x = 0; x < fenceBorderX; x++)
            {
                outStr += $"{(fenceMap[x, y] ? "#" : ".")}";
            }

            Console.WriteLine(outStr);
        }
    }

    private static int GetFencesToAdd(int x, int y, char curPlantType)
    {
        var curFencesCnt = 0;
        if (x > 0)
        {
            if (_inputRaw[y][x - 1] != curPlantType) curFencesCnt++;
        }
        else
        {
            curFencesCnt++;
        }

        if (x < _borderX - 1)
        {
            if (_inputRaw[y][x + 1] != curPlantType) curFencesCnt++;
        }
        else
        {
            curFencesCnt++;
        }

        if (y > 0)
        {
            if (_inputRaw[y - 1][x] != curPlantType) curFencesCnt++;
        }
        else
        {
            curFencesCnt++;
        }

        if (y < _borderY - 1)
        {
            if (_inputRaw[y + 1][x] != curPlantType) curFencesCnt++;
        }
        else
        {
            curFencesCnt++;
        }

        return curFencesCnt;
    }

    public static void SolveIt_2ndPart()
    {
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length;
        _borderX = _inputRaw[0].Length;
        _dawarichschonMap = new bool[_borderX, _borderY];
        bool[,] _fenceMap;

        var plantFieldPos = 0;
        for (int x = 0; x < _borderX; x++)
        {
            for (int y = 0; y < _borderY; y++)
            {
                // if (_inputRaw[y][x] == 'A')
                // {
                if (!_dawarichschonMap[x, y])
                {
                    Console.WriteLine($"{_inputRaw[y][x]}------------");
                    _blockErgList.Add(0);
                    _blockCnt.Add(0);
                    _fenceMap = new bool[_borderX + 2, _borderY + 2];
                    TryToFindFieldAndDrawFences(x, y, _inputRaw[y][x], plantFieldPos, _fenceMap);
                    PlotFenceMap(_fenceMap, 6, 6);
                    int fenceLineCnt = CalculateFenceLines(_fenceMap);
                    Console.WriteLine($"{_inputRaw[y][x]} - {fenceLineCnt}");
                    plantFieldPos++;
                }
                //}
            }
        }

        var gesErg = 0;
        for (int idx = 0; idx < _blockErgList.Count; idx++)
        {
            gesErg += _blockErgList[idx] * _blockCnt[idx];
        }

        Console.WriteLine(gesErg);
        var stopStr = "";
    }

    private static int CalculateFenceLines(bool[,] fenceMap)
    {
        var retVal = 0;
        //Spaltenweise prüfen
        for (int x = 0; x < _borderX + 2; x++)
        {
            var onALine = false;
            var cntCounted = false;
            for (int y = 0; y < _borderY + 2; y++)
            {
                if (fenceMap[x, y] && !onALine)
                {
                    onALine = true;
                    continue;
                }
                if (fenceMap[x, y] && onALine && !cntCounted)
                {
                    retVal++;
                    cntCounted = true;
                    continue;
                }

                if (!fenceMap[x, y])
                {
                    onALine = false;
                    cntCounted = false;
                }
            }
        }
        
        //Zeilenweise prüfen
        for (int y = 0; y < _borderX + 2; y++)
        {
            var onALine = false;
            var cntCounted = false;
            for (int x = 0; x < _borderY + 2; x++)
            {
                if (fenceMap[x, y] && !onALine)
                {
                    onALine = true;
                    continue;
                }
                if (fenceMap[x, y] && onALine && !cntCounted)
                {
                    retVal++;
                    cntCounted = true;
                    continue;
                }

                if (!fenceMap[x, y])
                {
                    onALine = false;
                    cntCounted = false;
                }
            }
        }

        return retVal;
    }
}