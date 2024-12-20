using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day20;

public static class Solver
{
    const string InputFile = "inputs/day20_1.txt";
    private static int _borderX;
    private static int _borderY;
    private static string[] _inputRaw;
    private static char[,] _cpuMap;
    private static int _startX;
    private static int _startY;
    private static int _endX;
    private static int _endY;
    private static long _globCnt;

    public static void SolveIt_1stPart()
    {
        _inputRaw = File.ReadAllLines(InputFile);
        _borderX = _inputRaw[0].Length - 2;
        _borderY = _inputRaw.Length - 2;

        //----
        _cpuMap = new char[_borderX, _borderY];
        for (int y = 1; y <= _borderY; y++)
        {
            for (int x = 1; x <= _borderX; x++)
            {
                if (_inputRaw[y][x] == 'S')
                {
                    _startX = x - 1;
                    _startY = y - 1;
                    _cpuMap[x - 1, y - 1] = '.';
                }
                else if (_inputRaw[y][x] == 'E')
                {
                    _endX = x - 1;
                    _endY = y - 1;
                    _cpuMap[x - 1, y - 1] = '.';
                }
                else
                {
                    _cpuMap[x - 1, y - 1] = _inputRaw[y][x];
                }
            }
        }

        TryToFindTheWay(_startX, _startY, 0, new List<int>(), 0, 0, 0);

        PlotWarehouseMap(_startX, _startY, new List<int>());
        Console.WriteLine($"{_globCnt}");
        //--
    }

    private static List<int> CloneList(List<int> list)
    {
        return list.Select(a => a).ToList();
    }

    private static void TryToFindTheWay(int curX, int curY, int schritteBisher, List<int> breadcrump, int shortenCnt,
        int dX, int dY)
    {
        //PlotWarehouseMapLupe(curX, curY, breadcrump, 5);
        
        if (breadcrump.Contains(curY * (_borderX + 2) + curX)) return;
        if ((curX < 0) || (curY < 0) || (curY >= _borderY) || (curX >= _borderX))
        {
            return;
        }

        if (_cpuMap[curX, curY] == '#')
        {
            var shortendTake = false;
            if (shortenCnt == 0)
            {
                var testX = curX + dX;
                var testY = curY + dY;
                if ((testX > 0) && (testY >= 0) && (testY < _borderY) && (testX < _borderX) &&
                    _cpuMap[testX, testY] == '.')
                {
                    shortenCnt++;
                    schritteBisher++;
                    shortendTake = true;
                    //breadcrump.Add(curY * (_borderX + 2) + curX);
                    curX += dX;
                    curY += dY;
                }
            }

            if (!shortendTake) return;
        }

        breadcrump.Add(curY * (_borderX + 2) + curX);

        if (schritteBisher > 350) return;
        if (curX == _endX && curY == _endY)
        {
            _globCnt++;
            PlotWarehouseMap(curX, curY, breadcrump);
            return;
        }

        TryToFindTheWay(curX + 1, curY, schritteBisher + 1, CloneList(breadcrump), shortenCnt, 1, 0);
        TryToFindTheWay(curX - 1, curY, schritteBisher + 1, CloneList(breadcrump), shortenCnt, -1, 0);
        TryToFindTheWay(curX, curY + 1, schritteBisher + 1, CloneList(breadcrump), shortenCnt, 0, 1);
        TryToFindTheWay(curX, curY - 1, schritteBisher + 1, CloneList(breadcrump), shortenCnt, 0, -1);

        if (schritteBisher > 50)
        {
            //PlotWarehouseMap(curX, curY, breadcrump);
        }
    }

    private static void PlotWarehouseMap(int curPosX, int curPosY, List<int> breadcrump)
    {
        Console.WriteLine(" ".PadLeft(_cpuMap.GetLength(0)));
        for (int y = 0; y < _cpuMap.GetLength(1); y++)
        {
            var outStr = "";
            for (int x = 0; x < _cpuMap.GetLength(0); x++)
            {
                if (x == curPosX && y == curPosY)
                {
                    outStr += "@";
                }
                else if (x == _endX && y == _endY)
                {
                    outStr += "E";
                }
                else if (breadcrump.Contains(y * (_borderX + 2) + x))
                {
                    outStr += "+";
                }
                else
                {
                    outStr += _cpuMap[x, y];
                }
            }

            Console.WriteLine(outStr);
        }
    }

    private static void PlotWarehouseMapLupe(int curPosX, int curPosY, List<int> breadcrump, int lupeAmount)
    {
        var plotStartX = curPosX - lupeAmount;
        if (plotStartX < 0) plotStartX = 0;
        var plotStartY = curPosY - lupeAmount;
        if (plotStartY < 0) plotStartY = 0;
        
        var plotEndX = curPosX + lupeAmount;
        if (plotEndX > _cpuMap.GetLength(0)) plotEndX = _cpuMap.GetLength(0);
        var plotEndY = curPosY + lupeAmount;
        if (plotEndY > _cpuMap.GetLength(1)) plotEndY = _cpuMap.GetLength(1);
        
        Console.WriteLine(" ".PadLeft(_cpuMap.GetLength(0)));
        for (int y = plotStartY; y < plotEndY; y++)
        {
            var outStr = "";
            for (int x = plotStartX; x < plotEndX; x++)
            {
                if (x == curPosX && y == curPosY)
                {
                    outStr += "@";
                }
                else if (x == _endX && y == _endY)
                {
                    outStr += "E";
                }
                else if (breadcrump.Contains(y * (_borderX + 2) + x))
                {
                    outStr += "+";
                }
                else
                {
                    outStr += _cpuMap[x, y];
                }
            }

            Console.WriteLine(outStr);
        }
    }

    public static void SolveIt_2ndPart()
    {
    }
}