using System.Diagnostics;

namespace AdventOfCode.Year_2024.Day20;

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
    private static List<int> _finalBreadCrump;
    private static List<int> _tmpFoundBreadCrump;

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

        PlotWarehouseMap(_startX, _startY, _finalBreadCrump);
        CalcAbkuerzungen(_finalBreadCrump);
        Console.WriteLine($"{_globCnt}");
        //--
    }

    private static void CalcAbkuerzungen(List<int> finalBreadCrump)
    {
        var listeGespart = new List<int>();
        int gesResultCnt = 0;
        for (int pathPos = 0; pathPos < finalBreadCrump.Count; pathPos++)
        {
            //curY * _borderX + curX
            var curX = finalBreadCrump[pathPos] % _borderX;
            var curY = (finalBreadCrump[pathPos] - curX) / _borderX;
            var breadCrumCodedPos = -1;
            for (int i = 0; i < 4; i++)
            {
                switch (i)
                {
                    case 0:
                        breadCrumCodedPos = CheckDurchbruchMoeglich(curX, curY, -1, 0);
                        break;
                    case 1:
                        breadCrumCodedPos = CheckDurchbruchMoeglich(curX, curY, 1, 0);
                        break;
                    case 2:
                        breadCrumCodedPos = CheckDurchbruchMoeglich(curX, curY, 0, -1);
                        break;
                    case 3:
                        breadCrumCodedPos = CheckDurchbruchMoeglich(curX, curY, 0, 1);
                        break;
                }

                if (finalBreadCrump.Contains(breadCrumCodedPos))
                {
                    var posImPfadNachAbkuerz = finalBreadCrump.IndexOf(breadCrumCodedPos);
                    if (posImPfadNachAbkuerz > pathPos)
                    {
                        var stopSrt = "";
                        var laengeGelaufen = pathPos + 1 + 2 + (finalBreadCrump.Count - posImPfadNachAbkuerz - 1);
                        var gespart = finalBreadCrump.Count - laengeGelaufen;
                        //listeGespart.Add(gespart);
                        if (gespart >= 100) gesResultCnt++;
                    }
                }
            }
        }

        Console.WriteLine("Erg = " + gesResultCnt);
    }

    private static int CheckDurchbruchMoeglich(int curX, int curY, int dX, int dY)
    {
        var obstacleX = curX + dX;
        var obstacleY = curY + dY;
        if ((obstacleX >= 0) && (obstacleY >= 0) && (obstacleX < _borderX) && (obstacleY < _borderY) &&
            _cpuMap[obstacleX, obstacleY] == '#')
        {
            var potPfadX = obstacleX + dX;
            var potPfadY = obstacleY + dY;
            if ((potPfadX >= 0) && (potPfadY >= 0) && (potPfadX < _borderX) && (potPfadY < _borderY) &&
                _cpuMap[potPfadX, potPfadY] == '.')
            {
                return potPfadY * _borderX + potPfadX;
            }
        }

        return -1;
    }

    private static List<int> CloneList(List<int> list)
    {
        return list.Select(a => a).ToList();
    }

    private static void TryToFindTheWay(int curX, int curY, int schritteBisher, List<int> breadcrump, int shortenCnt,
        int dX, int dY)
    {
        //PlotWarehouseMapLupe(curX, curY, breadcrump, 5);

        if (breadcrump.Contains(curY * _borderX + curX)) return;
        if ((curX < 0) || (curY < 0) || (curY >= _borderY) || (curX >= _borderX))
        {
            return;
        }

        if (_cpuMap[curX, curY] == '#')
        {
            // var shortendTake = false;
            // if (shortenCnt == 0)
            // {
            //     var testX = curX + dX;
            //     var testY = curY + dY;
            //     if ((testX > 0) && (testY >= 0) && (testY < _borderY) && (testX < _borderX) &&
            //         _cpuMap[testX, testY] == '.')
            //     {
            //         shortenCnt++;
            //         schritteBisher++;
            //         shortendTake = true;
            //         //breadcrump.Add(curY * (_borderX + 2) + curX);
            //         curX += dX;
            //         curY += dY;
            //     }
            // }
            //if (!shortendTake) return;
            return;
        }

        breadcrump.Add(curY * _borderX + curX);

        //if (schritteBisher > 350) return;
        if (curX == _endX && curY == _endY)
        {
            _globCnt++;
            //PlotWarehouseMap(curX, curY, breadcrump);
            _finalBreadCrump = CloneList(breadcrump);
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
                else if (breadcrump.Contains(y * _borderX + x))
                {
                    outStr += breadcrump.IndexOf(y * _borderX + x) % 10;
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
                else if (breadcrump.Contains(y * _borderX + x))
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

        PlotWarehouseMap(_startX, _startY, _finalBreadCrump);
        CalcAbkuerzungen_V2(_finalBreadCrump);
        Console.WriteLine($"{_globCnt}");
        //--
    }

    private static void CalcAbkuerzungen_V2(List<int> finalBreadCrump)
    {
        var listeGespart = new List<int>();
        _globCnt = 0;
        var stopW = new Stopwatch();
        stopW.Start();
        for (int pathPos = 0; pathPos < finalBreadCrump.Count; pathPos++)
        {
            //curY * _borderX + curX
            var curX = finalBreadCrump[pathPos] % _borderX;
            var curY = (finalBreadCrump[pathPos] - curX) / _borderX;
            ScanSystematic(curX, curY, pathPos);
            //Console.WriteLine(pathPos);
        }
        stopW.Stop();
        Console.WriteLine($"Zeit = {stopW.ElapsedMilliseconds}");
        //Bsp: 285
        Console.WriteLine("Erg = " + _globCnt);
    }

    private static void ScanSystematic(int curX, int curY, int startPosInBreadcrump)
    {
        //var brCrp = new List<int>();
        var umkreis = 20;
        for (int y = curY - umkreis; y <= curY + umkreis; y++)
        {
            var umkreisRedX = Math.Abs(umkreis - Math.Abs(curY - (y + umkreis)));
            for (int x = curX - (umkreis - umkreisRedX); x <= curX + (umkreis - umkreisRedX); x++)
            {
                if ((x >= 0) && (y >= 0) && (x < _borderX) && (y < _borderY))
                {
                    //-------
                    //brCrp.Add(y * _borderX + x);
                    var schritteBisher = Math.Abs(x - curX) + Math.Abs(y - curY);
                    var breadCrumCodedPos = y * _borderX + x;
                    if (_finalBreadCrump.Contains(breadCrumCodedPos))
                    {
                        var posImPfadNachAbkuerz = _finalBreadCrump.IndexOf(breadCrumCodedPos);
                        if (posImPfadNachAbkuerz > startPosInBreadcrump)
                        {
                            var laengeGelaufen = startPosInBreadcrump + 1 + schritteBisher +
                                                 (_finalBreadCrump.Count - posImPfadNachAbkuerz - 1);
                            var gespart = _finalBreadCrump.Count - laengeGelaufen;
                            if (gespart >= 100)
                            {
                                _globCnt++;
                                //_tmpFoundBreadCrump.Add(breadCrumCodedPos);
                            }
                        }
                    }
                    //----------
                }
            }
        }

        //PlotWarehouseMap(curX, curY, brCrp);
    }


    private static void TryToFindTheWayBackToBreadcrump(int curX, int curY, int schritteBisher, List<int> breadcrump,
        int startPosInBreadcrump)
    {
        if (schritteBisher > 20) return;

        if (breadcrump.Contains(curY * _borderX + curX)) return;
        if ((curX < 0) || (curY < 0) || (curY >= _borderY) || (curX >= _borderX))
        {
            return;
        }

        if (_cpuMap[curX, curY] != '#')
        {
            //auf breadcrump gelandet ?
            var breadCrumCodedPos = curY * _borderX + curX;
            if (_finalBreadCrump.Contains(breadCrumCodedPos))
            {
                var posImPfadNachAbkuerz = _finalBreadCrump.IndexOf(breadCrumCodedPos);
                if (posImPfadNachAbkuerz > startPosInBreadcrump)
                {
                    var laengeGelaufen = startPosInBreadcrump + 1 + schritteBisher +
                                         (_finalBreadCrump.Count - posImPfadNachAbkuerz - 1);
                    var gespart = _finalBreadCrump.Count - laengeGelaufen;
                    if (!_tmpFoundBreadCrump.Contains(breadCrumCodedPos))
                    {
                        if (gespart >= 76)
                        {
                            _globCnt++;
                            _tmpFoundBreadCrump.Add(breadCrumCodedPos);
                        }
                    }
                }
            }
        }

        breadcrump.Add(curY * _borderX + curX);

        TryToFindTheWayBackToBreadcrump(curX + 1, curY, schritteBisher + 1, CloneList(breadcrump),
            startPosInBreadcrump);
        TryToFindTheWayBackToBreadcrump(curX - 1, curY, schritteBisher + 1, CloneList(breadcrump),
            startPosInBreadcrump);
        TryToFindTheWayBackToBreadcrump(curX, curY + 1, schritteBisher + 1, CloneList(breadcrump),
            startPosInBreadcrump);
        TryToFindTheWayBackToBreadcrump(curX, curY - 1, schritteBisher + 1, CloneList(breadcrump),
            startPosInBreadcrump);

        if (schritteBisher > 50)
        {
            //PlotWarehouseMap(curX, curY, breadcrump);
        }
    }
}