using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day8;

public record Point(long X, long Y, long Z);

public record PointIdxWithDistance(long SqDistance, int IdxP0, int IdxP1);

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day8_1.txt";
    private static string[] _inputRaw;
    private static List<Point> _points;
    private static List<List<int>> _circuits;
    private static List<PointIdxWithDistance> _topNearPoints;
    private const int TopCountToSort = 1000;
    private const int CircuitLineCount = 1000;

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
            _points.Add(new Point(long.Parse(splitted[0]), long.Parse(splitted[1]), long.Parse(splitted[2])));
        }

        _topNearPoints = [];
        var longestDistanceInTopRange = long.MaxValue;
        for (int outIdx = 0; outIdx < _points.Count; outIdx++)
        {
            for (int innerIdx = outIdx + 1; innerIdx < _points.Count; innerIdx++)
            {
                var sqDistance = GetSqDistance(_points[outIdx], _points[innerIdx]);
                if (sqDistance < longestDistanceInTopRange)
                {
                    longestDistanceInTopRange = InsertSortedInPointsWithDistanceList(sqDistance, outIdx, innerIdx);
                }
            }
        }

        _circuits = [];
        for (int i = 0; i < CircuitLineCount; i++)
        {
            AddToCircuitMembershipOrCreate(_topNearPoints[i].IdxP0, _topNearPoints[i].IdxP1);
            Console.WriteLine(_topNearPoints[i].IdxP0 + "," + _topNearPoints[i].IdxP1);
        }

        var laengen = _circuits.Select(a => a.Count).ToList();
        laengen.Sort();
        gesCnt = laengen[^1];
        for (int i = 2; i < 4; i++)
        {
            gesCnt *= laengen[^i];
        }

        // 2,13,8,17,18
        // 0,7,19,14
        // 9,12
        // 11,16
        //To Low: 11856
        //To Low: 18850
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static int AddToCircuitMembershipOrCreate(int idxP0, int idxP1)
    {
        for (int i = 0; i < _circuits.Count; i++)
        {
            if (_circuits[i].Contains(idxP0) && _circuits[i].Contains(idxP1))
            {
                Console.WriteLine("Ignore Me!");
                return 0;
            }

            if (_circuits[i].Contains(idxP0))
            {
                var otherCircuit = TryToFindIdxOfAnotherCircuit(i, idxP1);
                if (otherCircuit > -1)
                {
                    _circuits[i].AddRange(_circuits[otherCircuit]);
                    _circuits.RemoveAt(otherCircuit);
                    return 1;
                }

                _circuits[i].Add(idxP1);
                return 1;
            }

            if (_circuits[i].Contains(idxP1))
            {
                var otherCircuit = TryToFindIdxOfAnotherCircuit(i, idxP0);
                if (otherCircuit > -1)
                {
                    _circuits[i].AddRange(_circuits[otherCircuit]);
                    _circuits.RemoveAt(otherCircuit);
                    return 1;
                }

                _circuits[i].Add(idxP0);
                return 1;
            }
        }

        _circuits.Add([idxP0, idxP1]);
        return 1;
    }

    private static int TryToFindIdxOfAnotherCircuit(int origIdx, int idxToTryToFind)
    {
        for (int i = 0; i < _circuits.Count; i++)
        {
            if (i == origIdx) continue;
            if (_circuits[i].Contains(idxToTryToFind)) return i;
        }

        return -1;
    }

    private static long InsertSortedInPointsWithDistanceList(long sqDistance, int outIdx, int innerIdx)
    {
        var checkLimit = _topNearPoints.Count > TopCountToSort ? TopCountToSort : _topNearPoints.Count;
        for (var posInList = 0; posInList < checkLimit; posInList++)
        {
            if (sqDistance <
                _topNearPoints[posInList].SqDistance) //durch < auch abgefangen, wenn idx einfach vertauscht
            {
                _topNearPoints.Insert(posInList, new PointIdxWithDistance(sqDistance, outIdx, innerIdx));
                return _topNearPoints[checkLimit - 1].SqDistance;
            }
        }

        if (_topNearPoints.Count == 0)
        {
            _topNearPoints.Add(new PointIdxWithDistance(sqDistance, outIdx, innerIdx));
            return sqDistance;
        }

        return _topNearPoints[checkLimit - 1].SqDistance;
    }


    private static void AddToCircuitMembershipOrCreateNewOne(int outIdx, int innerIdx)
    {
        for (var i = 0; i < _circuits.Count; i++)
        {
            if (AddPointIfConnectedToTailOrHead(outIdx, innerIdx, i)) return;
            if (AddPointIfConnectedToTailOrHead(innerIdx, outIdx, i)) return;
            if (_circuits[i].Contains(outIdx))
            {
                _circuits.Add([innerIdx]);
                return;
            }

            if (_circuits[i].Contains(innerIdx))
            {
                _circuits.Add([outIdx]);
                return;
            }
        }

        _circuits.Add([outIdx, innerIdx]);
    }

    private static bool AddPointIfConnectedToTailOrHead(int point1Idx, int point2Idx, int circuitNumber)
    {
        if (_circuits[circuitNumber][0] == point1Idx)
        {
            _circuits[circuitNumber].Insert(0, point2Idx);
            return true;
        }

        if (_circuits[circuitNumber].Count >= 2)
        {
            if (_circuits[circuitNumber][_circuits[circuitNumber].Count - 1] == point1Idx)
            {
                _circuits[circuitNumber].Add(point2Idx);
                return true;
            }
        }

        return false;
    }

    private static int GetCircuitMembership(int outIdx, int innerIdx)
    {
        for (int i = 0; i < _circuits.Count; i++)
        {
            if (_circuits[i].Contains(outIdx) && _circuits[i].Contains(innerIdx))
            {
                return i;
            }
        }

        return -1;
    }

    private static double GetDistance(Point point0, Point point1)
    {
        var squareDistance = (point0.X - point1.X) * (point0.X - point1.X);
        squareDistance += (point0.Y - point1.Y) * (point0.Y - point1.Y);
        squareDistance += (point0.Z - point1.Z) * (point0.Z - point1.Z);
        return Math.Sqrt(squareDistance);
    }

    private static long GetSqDistance(Point point0, Point point1)
    {
        var squareDistance = (point0.X - point1.X) * (point0.X - point1.X);
        squareDistance += (point0.Y - point1.Y) * (point0.Y - point1.Y);
        squareDistance += (point0.Z - point1.Z) * (point0.Z - point1.Z);
        return squareDistance;
    }

    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }
}