using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day7;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day7_bsp.txt";
    private static string[] _inputRaw;
    private static char[,] _map;
    private static int dimX;
    private static int dimY;


    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        dimX = _inputRaw[0].Length;
        dimY = _inputRaw.Length;
        _map = new char[dimX, dimY];

        for (int y = 0; y < dimY; y++)
        {
            for (int x = 0; x < dimX; x++)
            {
                _map[x, y] = _inputRaw[y][x];
            }
        }

        var startX = 0;
        for (int x = 0; x < dimX; x++)
        {
            if (_map[x, 0] == 'S')
            {
                startX = x;
                break;
            }
        }

        WriteResultMap();

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
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
    
    private static void WriteResultMap()
    {
        for (int y = 0; y < dimY; y++)
        {
            var newLineStr = "";
            for (int x = 0; x < dimX; x++)
            {
                newLineStr += _map[x, y];
            }
            Console.WriteLine(newLineStr);
        }
    }
}