using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day6;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day6_1.txt";
    private static string[] _inputRaw;
    private static string[,] _mathItems;
    private static int dimX;
    private static int dimY;

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        var splitToCheckXCount = _inputRaw[0].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        dimX = splitToCheckXCount.Length;
        dimY = _inputRaw.Length;
        _mathItems = new string[dimX, _inputRaw.Length];
        for (int y = 0; y < dimY; y++)
        {
            var splitToAdd = _inputRaw[y].Split(" ", StringSplitOptions.RemoveEmptyEntries);
            for (int x = 0; x < splitToAdd.Length; x++)
            {
                _mathItems[x, y] = splitToAdd[x];
            }
        }

        for (int x = 0; x < dimX; x++)
        {
            gesCnt += CalculateColumn(x);
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static long CalculateColumn(int x)
    {
        var opType = _mathItems[x, dimY - 1];
        var retValue = long.Parse(_mathItems[x, 0]);
        for (int y = 1; y < dimY - 1; y++)
        {
            if (opType == "+")
            {
                retValue += long.Parse(_mathItems[x, y]);
            }

            if (opType == "*")
            {
                retValue *= long.Parse(_mathItems[x, y]);
            }
        }

        return retValue;
    }
    
    
    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();
        var gesCnt = 0l;
        _inputRaw = File.ReadAllLines(InputFile);
        dimY = _inputRaw.Length;
        var splitToCheckXCount = _inputRaw[dimY - 1].Split(" ", StringSplitOptions.RemoveEmptyEntries);
        dimX = splitToCheckXCount.Length;
        _mathItems = new string[dimX, _inputRaw.Length];
        var strToAdd = new List<string>();
        for (int y = 0; y < dimY - 1; y++)
        {
            strToAdd.Add("");
        }

        var parsedItemPos = 0;
        for (int x = 0; x < _inputRaw[0].Length; x++)
        {
            var charListToAdd = new List<char>();
            for (int y = 0; y < dimY - 1; y++)
            {
                charListToAdd.Add(_inputRaw[y][x]);
            }

            if (charListToAdd.All(a => a == ' '))
            {
                for (int y = 0; y < dimY - 1; y++)
                {
                    _mathItems[parsedItemPos, y] = strToAdd[y];
                }

                _mathItems[parsedItemPos, dimY - 1] = splitToCheckXCount[parsedItemPos];
                for (int y = 0; y < dimY - 1; y++)
                {
                    strToAdd[y] = "";
                }

                parsedItemPos++;
                continue;
            }

            for (int y = 0; y < dimY - 1; y++)
            {
                strToAdd[y] += charListToAdd[y];
            }
        }

        for (int y = 0; y < dimY - 1; y++)
        {
            _mathItems[parsedItemPos, y] = strToAdd[y];
        }

        _mathItems[parsedItemPos, dimY - 1] = splitToCheckXCount[parsedItemPos];

        for (int x = 0; x < dimX; x++)
        {
            gesCnt += CalculateColumnV2(x);
        }

        //Bsp: 3263827
        //Lsg: 11052310600986
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static long CalculateColumnV2(int x)
    {
        var opType = _mathItems[x, dimY - 1];
        var itemsToCalc = ConvertItems(x);
        var retValue = itemsToCalc[0];
        for (int i = 1; i < itemsToCalc.Count; i++)
        {
            if (opType == "+")
            {
                retValue += itemsToCalc[i];
            }

            if (opType == "*")
            {
                retValue *= itemsToCalc[i];
            }
        }

        return retValue;
    }

    private static List<long> ConvertItems(int x)
    {
        var retval = new List<long>();
        for (int p = _mathItems[x, 0].Length - 1; p >= 0; p--)
        {
            var numberStr = "";
            for (int y = 0; y < dimY - 1; y++)
            {
                numberStr += _mathItems[x, y][p];
            }

            retval.Add(long.Parse(numberStr.Trim()));
        }

        return retval;
    }
}