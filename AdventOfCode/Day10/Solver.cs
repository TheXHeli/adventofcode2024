using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day10;

public static class Solver
{
    const string InputFile = "inputs/day10_1.txt";
    private static int _borderX;
    private static int _borderY;
    private static string[] _inputMap;

    public static void SolveIt_1stPart()
    {
        _inputMap = File.ReadAllLines(InputFile);
        _borderY = _inputMap.Length;
        _borderX = _inputMap[0].Length;
        var gesCnt = 0;
        var stopwatch = Stopwatch.StartNew();
        for (int x = 0; x < _inputMap[0].Length; x++)
        {
            for (int y = 0; y < _inputMap.Length; y++)
            {
                if (_inputMap[y][x] == '0')
                {
                    var ninerReachedList = GetReachedNiner(x, y, (char)47);
                    gesCnt += ninerReachedList.Count;
                    var stopStr = "";
                }
            }
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
        Console.WriteLine(gesCnt);
    }

    private static List<int> GetReachedNiner(int x, int y, char valueBefore)
    {
        Console.WriteLine($"x: {x}, y: {y}");
        if (x < 0 || x >= _borderX || y < 0 || y >= _borderY) return [];
        if (_inputMap[y][x] - valueBefore != 1) return [];
        if (_inputMap[y][x] == '9') return [(y << 6) | x];
        var resultsFromLeftStep = GetReachedNiner(x - 1, y, _inputMap[y][x]);
        var resultsFromRigthStep = GetReachedNiner(x + 1, y, _inputMap[y][x]);
        var resultsFromUpStep = GetReachedNiner(x, y - 1, _inputMap[y][x]);
        var resultsFromDownStep = GetReachedNiner(x, y + 1, _inputMap[y][x]);
        return resultsFromLeftStep.Concat(resultsFromRigthStep).Concat(resultsFromUpStep).Concat(resultsFromDownStep)
            .Distinct().ToList();
    }


    public static void SolveIt_2ndPart()
    {
        _inputMap = File.ReadAllLines(InputFile);
        _borderY = _inputMap.Length;
        _borderX = _inputMap[0].Length;
        var gesCnt = 0;
        var stopwatch = Stopwatch.StartNew();
        for (int x = 0; x < _inputMap[0].Length; x++)
        {
            for (int y = 0; y < _inputMap.Length; y++)
            {
                if (_inputMap[y][x] == '0')
                {
                    var ninerReachedList = GetReachedNiner_NonDistinct(x, y, (char)47);
                    gesCnt += ninerReachedList.Count;
                    var stopStr = "";
                }
            }
        }
        stopwatch.Stop();
        Console.WriteLine(stopwatch.Elapsed);
        Console.WriteLine(gesCnt);
    }
    
    private static List<int> GetReachedNiner_NonDistinct(int x, int y, char valueBefore)
    {
        Console.WriteLine($"x: {x}, y: {y}");
        if (x < 0 || x >= _borderX || y < 0 || y >= _borderY) return [];
        if (_inputMap[y][x] - valueBefore != 1) return [];
        if (_inputMap[y][x] == '9') return [(y << 6) | x];
        var resultsFromLeftStep = GetReachedNiner_NonDistinct(x - 1, y, _inputMap[y][x]);
        var resultsFromRigthStep = GetReachedNiner_NonDistinct(x + 1, y, _inputMap[y][x]);
        var resultsFromUpStep = GetReachedNiner_NonDistinct(x, y - 1, _inputMap[y][x]);
        var resultsFromDownStep = GetReachedNiner_NonDistinct(x, y + 1, _inputMap[y][x]);
        return resultsFromLeftStep.Concat(resultsFromRigthStep).Concat(resultsFromUpStep).Concat(resultsFromDownStep)
            .ToList();
    }
}