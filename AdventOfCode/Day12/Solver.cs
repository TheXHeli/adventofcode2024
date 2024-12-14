using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day12;

//945953 (O)
//946952 falsch
//960000 falsch
//Ergebnis aus Py-Lösung: 946084 Unterschied: 131
public static class Solver
{
    const string InputFile = "inputs/day12_1.txt";
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
        _blockCnt[curBlockToCheck]++;
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
        if (curPlantType == 'B')
        {
            var stopStr = "";
            if (x == 1 && y == 1)
            {
                stopStr = "";
            }
        }

        if (x == 0)
        {
            DrawFenceLeft(x, y, fenceMap);
        }
        else
        {
            if (_inputRaw[y][x - 1] != curPlantType) DrawFenceLeft(x, y, fenceMap);
        }

        if (x == _borderX - 1)
        {
            DrawFenceRight(x, y, fenceMap);
        }
        else
        {
            if (_inputRaw[y][x + 1] != curPlantType) DrawFenceRight(x, y, fenceMap);
        }

        if (y == 0)
        {
            DrawFenceTop(x, y, fenceMap);
        }
        else
        {
            if (_inputRaw[y - 1][x] != curPlantType) DrawFenceTop(x, y, fenceMap);
        }

        if (y == _borderY - 1)
        {
            DrawFenceDown(x, y, fenceMap);
        }
        else
        {
            if (_inputRaw[y + 1][x] != curPlantType) DrawFenceDown(x, y, fenceMap);
        }
    }

    private static void DrawFenceLeft(int x, int y, bool[,] fenceMap)
    {
        fenceMap[x * 2, y * 2] = true;
        fenceMap[x * 2, y * 2 + 1] = true;
        fenceMap[x * 2, y * 2 + 2] = true;
    }

    private static void DrawFenceRight(int x, int y, bool[,] fenceMap)
    {
        fenceMap[x * 2 + 2, y * 2] = true;
        fenceMap[x * 2 + 2, y * 2 + 1] = true;
        fenceMap[x * 2 + 2, y * 2 + 2] = true;
    }

    private static void DrawFenceTop(int x, int y, bool[,] fenceMap)
    {
        fenceMap[x * 2, y * 2] = true;
        fenceMap[x * 2 + 1, y * 2] = true;
        fenceMap[x * 2 + 2, y * 2] = true;
    }

    private static void DrawFenceDown(int x, int y, bool[,] fenceMap)
    {
        fenceMap[x * 2, y * 2 + 2] = true;
        fenceMap[x * 2 + 1, y * 2 + 2] = true;
        fenceMap[x * 2 + 2, y * 2 + 2] = true;
    }

    private static void PlotFenceMap(bool[,] fenceMap, int fenceBorderX,
        int fenceBorderY)
    {
        for (int y = 0; y < fenceBorderY; y++)
        {
            var outStr = "";
            for (int x = 0; x < fenceBorderX; x++)
            {
                char altChar = '.';
                if (y % 2 == 1 && x % 2 == 1)
                {
                    //altChar = _inputRaw[(y / 2) - 0][(x / 2) - 0];
                    var stopStr = "";
                }

                outStr += $"{(fenceMap[x, y] ? "#" : altChar)}";
            }

            Console.WriteLine(y.ToString().PadLeft(3, ' ') + outStr);
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
        //945953
        var plantFieldPos = 0;
        for (int x = 0; x < _borderX; x++)
        {
            for (int y = 0; y < _borderY; y++)
            {
                // if (_inputRaw[y][x] == 'A')
                // {
                if (!_dawarichschonMap[x, y])
                {
                    //Console.WriteLine($"{_inputRaw[y][x]}------------");
                    _blockErgList.Add(0);
                    _blockCnt.Add(0);
                    _fenceMap = new bool[_borderX * 2 + 1, _borderY * 2 + 1];
                    TryToFindFieldAndDrawFences(x, y, _inputRaw[y][x], plantFieldPos, _fenceMap);
                    //TryFindBorderCrossings(_fenceMap);
                    var foundSomeToEleminate = EleminateCrossFences(_fenceMap, x, y);
                    if (foundSomeToEleminate)
                    {
                        //PlotFenceMap(_fenceMap, _borderX * 2 + 1, _borderY * 2 + 1);
                    }


                    int fenceLineCnt = CalculateFenceLines(_fenceMap);
                    if (_blockCnt[plantFieldPos] == 131)
                    {
                        PlotFenceMap(_fenceMap, _borderX * 2 + 1, _borderY * 2 + 1);
                    }
                    _blockErgList[plantFieldPos] = fenceLineCnt;
                    //Console.WriteLine($"{_inputRaw[y][x]} - Fences: {fenceLineCnt} - Plants: {_blockCnt[^1]}");
                    if (x == 4 && y == 131)
                    {
                        var stopStr2 = "";
                    }

                    plantFieldPos++;
                }
                //}
            }
        }

        long gesErg = 0;
        var blockCnt = 0;
        for (int idx = 0; idx < _blockErgList.Count; idx++)
        {
            gesErg += _blockErgList[idx] * _blockCnt[idx];
            Console.WriteLine($"{_blockErgList[idx]}: {_blockCnt[idx]}");
            blockCnt += _blockCnt[idx];
        }

        Console.WriteLine(gesErg);
        Console.WriteLine($"Real: {blockCnt} - Calculated: {_borderX * _borderY}");
        var stopStr = "";
    }

    private static void TryFindBorderCrossings(bool[,] fenceMap)
    {
        for (int y = 1; y < _borderY; y++)
        {
            if (fenceMap[0, y] && fenceMap[0, y - 1] && fenceMap[0, y + 1] && fenceMap[1, y])
            {
                Console.WriteLine($"Found border crossing at Y {y}");
            }

            if (fenceMap[_borderX - 1, y] && fenceMap[_borderX - 1, y - 1] && fenceMap[_borderX - 1, y + 1] &&
                fenceMap[_borderX - 2, y])
            {
                Console.WriteLine($"Found border crossing at Y {y}");
            }
        }

        for (int x = 1; x < _borderX; x++)
        {
            if (fenceMap[x, 0] && fenceMap[x - 1, 0] && fenceMap[x + 1, 0] && fenceMap[x, 1])
            {
                Console.WriteLine($"Found border crossing at X {x}");
            }

            if (fenceMap[_borderY - 1, x] && fenceMap[_borderY - 1, x - 1] && fenceMap[_borderY - 1, x + 1] &&
                fenceMap[_borderY - 2, x])
            {
                Console.WriteLine($"Found border crossing at X {x}");
            }
        }
    }

    private static bool EleminateCrossFences(bool[,] fenceMap, int xx, int yy)
    {
        var retval = false;
        for (int x = 1; x < _borderX * 2; x++)
        {
            for (int y = 1; y < _borderY * 2; y++)
            {
                if (fenceMap[x, y])
                {
                    if ((fenceMap[x - 1, y] && fenceMap[x + 1, y] && fenceMap[x, y - 1]) ||
                        (fenceMap[x - 1, y] && fenceMap[x + 1, y] && fenceMap[x, y + 1])
                        || (fenceMap[x, y - 1] && fenceMap[x, y + 1] && fenceMap[x - 1, y])
                        || (fenceMap[x, y - 1] && fenceMap[x, y + 1] && fenceMap[x + 1, y]))
                    {
                        fenceMap[x, y] = false;
                        retval = true;
                        Console.WriteLine(
                            $"Eleminated border crossing at {x}, {y} - Buchstabe: {_inputRaw[yy][xx]} - Explorestart: at {xx}, {yy}");
                    }
                }
            }
        }

        return retval;
    }

    private static int CalculateFenceLines(bool[,] fenceMap)
    {
        var retVal = 0;
        //Spaltenweise prüfen
        for (int x = 0; x < _borderX * 2 + 1; x++)
        {
            var onALine = false;
            var cntCounted = false;
            for (int y = 0; y < _borderY * 2 + 1; y++)
            {
                if (fenceMap[x, y] && !onALine)
                {
                    onALine = true;
                    //retVal++;
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
        for (int y = 0; y < _borderY * 2 + 1; y++)
        {
            var onALine = false;
            var cntCounted = false;
            for (int x = 0; x < _borderX * 2 + 1; x++)
            {
                if (fenceMap[x, y] && !onALine)
                {
                    onALine = true;
                    //retVal++;
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

    private static int CalculateFenceLines_V2(bool[,] fenceMap)
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
                    retVal++;
                    continue;
                }

                if (!fenceMap[x, y])
                {
                    onALine = false;
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
                    retVal++;
                    continue;
                }

                if (!fenceMap[x, y])
                {
                    onALine = false;
                }
            }
        }

        return retVal;
    }
}