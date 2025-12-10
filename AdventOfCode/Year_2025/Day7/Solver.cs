using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day7;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day7_1.txt";
    private static string[] _inputRaw;
    private static char[,] _map;
    private static int dimX;
    private static int dimY;
    private static int globCnt;


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

        globCnt = 0;
        WriteResultMap();
        gesCnt = MoveBeamAndPotentiallySplit(startX, 1);
        WriteResultMap();

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + globCnt);
    }

    public static int MoveBeamAndPotentiallySplit(int x, int y)
    {
        if ((x < 0) && (x > dimX - 2))
        {
            return 0;
        }

        if (y == dimY - 1)
        {
            return 1;
        }

        var retval = 0;
        if (_map[x, y] == '^')
        {
            /*if (_map[x - 1, y] != '|')
            {
                _map[x - 1, y] = '|';
                retval += MoveBeamAndPotentiallySplit(x - 1, y);
            }

            if (_map[x + 1, y] != '|')
            {
                _map[x + 1, y] = '|';
                retval += MoveBeamAndPotentiallySplit(x + 1, y);
            }*/
            var resR = MoveBeamAndPotentiallySplit(x + 1, y);
            var resL = MoveBeamAndPotentiallySplit(x - 1, y);
            _map[x, y] = 'Ä';
            globCnt += 1;
        }

        if ((_map[x, y] == '.') || (_map[x, y] == '|'))
        {
            _map[x, y] = '|';
            retval += MoveBeamAndPotentiallySplit(x, y + 1);
        }

        return retval;
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