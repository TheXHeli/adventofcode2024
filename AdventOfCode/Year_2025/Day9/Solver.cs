using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day9;

public record Point(long X, long Y);

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day9_1.txt";
    private static string[] _inputRaw;
    private static List<Point> _points;

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        _points = new List<Point>();
        foreach (var inputItem in _inputRaw)
        {
            var splitted = inputItem.Split(",");
            _points.Add(new Point(long.Parse(splitted[0]), long.Parse(splitted[1])));
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

    private static long CalcArea(Point point0, Point point1)
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
        _points = new List<Point>();
        foreach (var inputItem in _inputRaw)
        {
            var splitted = inputItem.Split(",");
            _points.Add(new Point(long.Parse(splitted[0]), long.Parse(splitted[1])));
        }

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
                if (CheckIfInsideArea(_points[outIdx], _points[innerIdx]))
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

        Console.WriteLine(biggestArea);
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static bool CheckIfInsideArea(Point p0, Point p1)
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