using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day4;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day4_1.txt";
    private static string[] _inputRaw;
    private static int[,] _map;
    private static int _borderX;
    private static int _borderY;


    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0;
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length + 1;
        _borderX = _inputRaw[0].Length + 1;

        FillInputMap();
        WriteResultMap();

        for (var y = 1; y < _borderY; y++)
        {
            var cntPerRow = 0;
            for (var x = 1; x < _borderX; x++)
            {
                if (_map[x, y] == 1)
                {
                    var towelsInNear = GetTowelcount(x, y);
                    if (towelsInNear < 4)
                    {
                        gesCnt++;
                        cntPerRow++;
                    }
                }
            }

            Console.WriteLine(cntPerRow);
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
        var gesCnt = 0;
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length + 1;
        _borderX = _inputRaw[0].Length + 1;

        FillInputMap();
        WriteResultMap();
        int removedTowweelLastRound;
        do
        {
            removedTowweelLastRound = 0;
            for (var y = 1; y < _borderY; y++)
            {
                for (var x = 1; x < _borderX; x++)
                {
                    if (_map[x, y] == 1)
                    {
                        var towelsInNear = GetTowelcount(x, y);
                        if (towelsInNear < 4)
                        {
                            _map[x, y] = 0;
                            gesCnt++;
                            removedTowweelLastRound++;
                        }
                    }
                }

                Console.WriteLine(removedTowweelLastRound);
            }
        } while (removedTowweelLastRound > 0);


        WriteResultMap();
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static int GetTowelcount(int x, int y)
    {
        var returnVal = 0;
        returnVal += _map[x - 1, y];
        returnVal += _map[x + 1, y];

        returnVal += _map[x + 1, y - 1];
        returnVal += _map[x, y - 1];
        returnVal += _map[x - 1, y - 1];

        returnVal += _map[x + 1, y + 1];
        returnVal += _map[x, y + 1];
        returnVal += _map[x - 1, y + 1];


        return returnVal;
    }

    private static void FillInputMap()
    {
        _map = new int[_inputRaw[0].Length + 2, _inputRaw.Length + 2];
        for (var y = 0; y < _borderY + 1; y++)
        {
            for (var x = 0; x < _borderX + 1; x++)
            {
                if ((x > 0) && (x < _borderX) && (y > 0) && (y < _borderY))
                {
                    if (_inputRaw[y - 1][x - 1] == '@') _map[x, y] = 1;
                    if (_inputRaw[y - 1][x - 1] == '.') _map[x, y] = 0;
                    continue;
                }

                _map[x, y] = 0;
            }
        }
    }

    private static void WriteResultMap()
    {
        var resultCnt = 0;
        for (int y = 0; y < _map.GetLength(1); y++)
        {
            var newLineStr = "";
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                switch (_map[x, y])
                {
                    case 0:
                        newLineStr += '.';
                        break;
                    case 1:
                        newLineStr += '@';
                        break;
                    default:
                        newLineStr += '+';
                        break;
                }
            }

            Console.WriteLine(newLineStr);
        }
    }
}