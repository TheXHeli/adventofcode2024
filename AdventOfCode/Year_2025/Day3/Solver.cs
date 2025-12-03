using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day3;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day3_1.txt";
    private static byte[,] _batteries;
    private static string[] _inputRaw;
    private static int _borderX;
    private static int _borderY;

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0;
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length;
        _borderX = _inputRaw[0].Length;
        _batteries = new byte[_borderX, _borderY];

        for (var y = 0; y < _borderY; y++)
        {
            byte[] joltage = [0, 0];
            for (var x = 0; x < _borderX; x++)
            {
                var aktZiffer = (byte)((byte)_inputRaw[y][x] - 48);
                if ((aktZiffer > joltage[0]) && (x < _borderX - 1))
                {
                    joltage[0] = aktZiffer;
                    joltage[1] = 0;
                }
                else if (aktZiffer > joltage[1])
                {
                    joltage[1] = aktZiffer;
                }
            }

            Console.WriteLine(joltage[0] + "-" + joltage[1]);
            gesCnt += joltage[0] * 10 + joltage[1];
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }


    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length;
        _borderX = _inputRaw[0].Length;
        _batteries = new byte[_borderX, _borderY];

        for (var y = 0; y < _borderY; y++)
        {
            var aktMinimumSpace = 11;
            var firstFoundPosition = 0;
            var aktMaxZiffer = 0;
            var aktJoltage = 0l;
            for (var x = 0; x < _borderX; x++)
            {
                var aktZiffer = (byte)((byte)_inputRaw[y][x] - 48);
                if (x < _borderX  - aktMinimumSpace)
                {
                    if (aktZiffer > aktMaxZiffer)
                    {
                        aktMaxZiffer = aktZiffer;
                        firstFoundPosition = x;
                    }
                }
                else //es sind nicht mehr genug Stellen Platz um die Restlichen Ziffern zu belegen
                {
                    aktJoltage += (long)(Math.Pow(10, aktMinimumSpace) * aktMaxZiffer);
                    x = firstFoundPosition;
                    firstFoundPosition = 0;
                    aktMaxZiffer = 0;
                    aktMinimumSpace--;
                    if (aktMinimumSpace < 0) break;
                }
            }

            aktJoltage += aktMaxZiffer;
            gesCnt += aktJoltage;
            var stopSTr = "";
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }
}