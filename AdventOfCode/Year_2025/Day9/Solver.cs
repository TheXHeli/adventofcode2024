using System.Diagnostics;
using System.Drawing;

namespace AdventOfCode.Year_2025.Day9;

public record CusPoint(long X, long Y);

public record CusLine(long X0, long Y0, long X1, long Y1);

public record CusLineOneDim(long c0, long c1);

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day9_1.txt";
    private static string[] _inputRaw;
    private static List<CusPoint> _points;
    private static Dictionary<int, List<int>> _pointsXDict;
    private static Dictionary<int, List<int>> _pointsYDict;
    private static Dictionary<int, List<CusLineOneDim>> _linesXDict;
    private static Dictionary<int, List<CusLineOneDim>> _linesYDict;
    private static List<CusLine> _polygonLines;

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        _points = new List<CusPoint>();
        foreach (var inputItem in _inputRaw)
        {
            var splitted = inputItem.Split(",");
            _points.Add(new CusPoint(long.Parse(splitted[0]), long.Parse(splitted[1])));
        }

        var biggestArea = 0l;
        for (int outIdx = 0; outIdx < _points.Count; outIdx++)
        {
            for (int innerIdx = outIdx + 1; innerIdx < _points.Count; innerIdx++)
            {
                var area = CalcArea(_points[outIdx], _points[innerIdx]);
                if (area > biggestArea)
                {
                    biggestArea = area;
                }
            }
        }

        Console.WriteLine(biggestArea);
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static long CalcArea(CusPoint point0, CusPoint point1)
    {
        var dX = (point0.X - point1.X);
        var dY = (point0.Y - point1.Y);
        dX = dX < 0 ? dX * -1 : dX;
        dY = dY < 0 ? dY * -1 : dY;
        return (dX + 1) * (dY + 1);
    }

    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        _points = new List<CusPoint>();
        _pointsXDict = new Dictionary<int, List<int>>();
        _pointsYDict = new Dictionary<int, List<int>>();
        foreach (var inputItem in _inputRaw)
        {
            var splitted = inputItem.Split(",");
            _points.Add(new CusPoint(long.Parse(splitted[0]), long.Parse(splitted[1])));
            var tmpX = int.Parse(splitted[0]);
            var tmpY = int.Parse(splitted[1]);
            if (!_pointsXDict.ContainsKey(tmpX)) _pointsXDict.Add(tmpX, []);
            if (!_pointsYDict.ContainsKey(tmpY)) _pointsYDict.Add(tmpY, []);
            _pointsXDict[tmpX].Add(tmpY);
            _pointsYDict[tmpY].Add(tmpX);
        }

        CalcPolygoneLinesDicts();

        var biggestArea = 0l;
        for (int outIdx = 0; outIdx < _points.Count; outIdx++)
        {
            for (int innerIdx = outIdx + 1; innerIdx < _points.Count; innerIdx++)
            {
                /*var area = CalcArea(_points[outIdx], _points[innerIdx]);
                if (area > biggestArea)
                {
                    biggestArea = area;
                }*/
                if (CheckIfIntersectWithLine(_points[outIdx], _points[innerIdx]))
                {
                    var stopStr = "";
                }
                else
                {
                    var area = CalcArea(_points[outIdx], _points[innerIdx]);
                    if (area > biggestArea)
                    {
                        Console.WriteLine(_points[outIdx] + " - " + _points[innerIdx]);
                        biggestArea = area;
                    }
                }
            }
        }

        //Too high:    4624346420
        //Passed:      1498673376
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + biggestArea);
    }

    private static bool CheckIfIntersectWithLine(CusPoint p0, CusPoint p1)
    {
        //machtseinfacher weil x0 und y0 immer die kleineren Werte sind
        var x0 = 0l;
        var x1 = 0l;
        var y0 = 0l;
        var y1 = 0l;
        if (p0.X < p1.X)
        {
            x0 = p0.X;
            x1 = p1.X;
        }
        else
        {
            x0 = p1.X;
            x1 = p0.X;
        }

        if (p0.Y < p1.Y)
        {
            y0 = p0.Y;
            y1 = p1.Y;
        }
        else
        {
            y0 = p1.Y;
            y1 = p0.Y;
        }

        //vertikale Linien prüfen
        foreach (var yDirectedLines in _linesXDict.Where(a => a.Key > x0 && a.Key < x1))
        {
            foreach (var singleLine in yDirectedLines.Value)
            {
                //eine der Enden der Linie ist im Rechteck:
                if ((singleLine.c0 > y0 && singleLine.c0 < y1) || (singleLine.c1 > y0 && singleLine.c1 < y1))
                {
                    return true;
                }

                // Beide Punkte außerhalb, dann intersect, wenn ein Punkt drüber und einer drunter ist
                // oder direkt auf der außenkante des Rechtecks, --> c0 und c1 sind immer sortiert, also c0 ist immer kleiner c1
                if (singleLine.c0 <= y0 && singleLine.c1 >= y1)
                {
                    return true;
                }
            }
        }
        //horizontale Linien prüfen
        foreach (var xDirectedLines in _linesYDict.Where(a => a.Key > y0 && a.Key < y1))
        {
            foreach (var singleLine in xDirectedLines.Value)
            {
                //eine der Enden der Linie ist im Rechteck:
                if ((singleLine.c0 > x0 && singleLine.c0 < x1) || (singleLine.c1 > x0 && singleLine.c1 < x1))
                {
                    return true;
                }

                // Beide Punkte außerhalb, dann intersect, wenn ein Punkt drüber und einer drunter ist
                // oder direkt auf der außenkante des Rechtecks, --> c0 und c1 sind immer sortiert, also c0 ist immer kleiner c1
                if (singleLine.c0 <= x0 && singleLine.c1 >= x1)
                {
                    return true;
                }
            }
        }

        return false;
    }

    private static void CalcPolygoneLines()
    {
        _polygonLines = new List<CusLine>();
        //senkrechte Linien ermitteln
        foreach (var yPoints in _pointsXDict)
        {
            yPoints.Value.Sort();
            for (var i = 0; i < yPoints.Value.Count; i += 2)
            {
                _polygonLines.Add(new CusLine(yPoints.Key, yPoints.Value[i], yPoints.Key, yPoints.Value[i + 1]));
            }
        }

        foreach (var xPoints in _pointsYDict)
        {
            xPoints.Value.Sort();
            for (var i = 0; i < xPoints.Value.Count; i += 2)
            {
                _polygonLines.Add(new CusLine(xPoints.Value[i], xPoints.Key, xPoints.Value[i + 1], xPoints.Key));
            }
        }
    }

    private static void CalcPolygoneLinesDicts()
    {
        _linesXDict = new Dictionary<int, List<CusLineOneDim>>();
        _linesYDict = new Dictionary<int, List<CusLineOneDim>>();
        //senkrechte Linien ermitteln
        foreach (var yPoints in _pointsXDict)
        {
            if (!_linesXDict.ContainsKey(yPoints.Key)) _linesXDict.Add(yPoints.Key, []);
            yPoints.Value.Sort();
            for (var i = 0; i < yPoints.Value.Count; i += 2)
            {
                _linesXDict[yPoints.Key].Add(new CusLineOneDim(yPoints.Value[i], yPoints.Value[i + 1]));
            }
        }

        foreach (var xPoints in _pointsYDict)
        {
            if (!_linesYDict.ContainsKey(xPoints.Key)) _linesYDict.Add(xPoints.Key, []);
            xPoints.Value.Sort();
            for (var i = 0; i < xPoints.Value.Count; i += 2)
            {
                _linesYDict[xPoints.Key].Add(new CusLineOneDim(xPoints.Value[i], xPoints.Value[i + 1]));
            }
        }
    }

    private static bool CheckIfInsideArea(CusPoint p0, CusPoint p1)
    {
        var x0 = 0l;
        var x1 = 0l;
        var y0 = 0l;
        var y1 = 0l;
        if (p0.X < p1.X)
        {
            x0 = p0.X;
            x1 = p1.X;
        }
        else
        {
            x0 = p1.X;
            x1 = p0.X;
        }

        if (p0.Y < p1.Y)
        {
            y0 = p0.Y;
            y1 = p1.Y;
        }
        else
        {
            y0 = p1.Y;
            y1 = p0.Y;
        }

        for (var i = 0; i < _points.Count; i++)
        {
            var curPoint = _points[i];
            if (curPoint.X > x0 && curPoint.X < x1 && (curPoint.Y > y0 && curPoint.Y < y1))
            {
                return true;
            }
        }

        return false;
    }
}