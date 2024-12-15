using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day15;

record DirDef
{
    public decimal dX { get; set; }
    public decimal dY { get; set; }
}

public static class Solver
{
    const string InputFile = "inputs/day15_bsp.txt";
    private static int _borderX;
    private static int _borderY;
    private static string[] _inputRaw;
    private static char[,] _warehouseMap;
    private static List<DirDef> _roboterMoves;
    private static int _robStartX;
    private static int _robStartY;

    public static void SolveIt_1stPart()
    {
        _inputRaw = File.ReadAllLines(InputFile);
        _borderX = _inputRaw[0].Length;
        for (int i = 0; i < _inputRaw.Length; i++)
        {
            if (_inputRaw[i].Length == 0)
            {
                _borderY = i;
            }
        }

        //----
        _warehouseMap = new char[_borderX - 2, _borderY - 2];
        for (int y = 1; y < _borderY - 1; y++)
        {
            for (int x = 1; x < _borderX - 1; x++)
            {
                if (_inputRaw[y][x] == '@')
                {
                    _robStartX = x - 1;
                    _robStartY = y - 1;
                }
                else
                {
                    _warehouseMap[x - 1, y - 1] = _inputRaw[y][x];
                }
            }
        }

        _borderX -= 2;
        _borderY -= 2;
        PlotWarehouseMap(_robStartX, _robStartY);
        //--
        _roboterMoves = new List<DirDef>();
        for (int r = _borderY + 3; r < _inputRaw.Length; r++)
        {
            for (int x = 0; x < _inputRaw[r].Length; x++)
            {
                var curElem = _inputRaw[r][x];
                var dirObjToAdd = new DirDef();
                switch (curElem)
                {
                    case '<':
                        dirObjToAdd.dX = -1;
                        dirObjToAdd.dY = 0;
                        break;
                    case '>':
                        dirObjToAdd.dX = 1;
                        dirObjToAdd.dY = 0;
                        break;
                    case 'v':
                        dirObjToAdd.dX = 0;
                        dirObjToAdd.dY = 1;
                        break;
                    case '^':
                        dirObjToAdd.dX = 0;
                        dirObjToAdd.dY = -1;
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                _roboterMoves.Add(dirObjToAdd);
            }
        }

        Console.WriteLine("Day 15 - Puzzle 15");
    }

    private static void PlotWarehouseMap(int robStartX, int robStartY)
    {
        for (int y = 0; y < _warehouseMap.GetLength(1); y++)
        {
            var outStr = "";
            for (int x = 0; x < _warehouseMap.GetLength(0); x++)
            {
                if (x == robStartX && y == robStartY)
                {
                    outStr += "@";
                }
                else
                {
                    outStr += _warehouseMap[x, y];
                }
            }

            Console.WriteLine(outStr);
        }
    }


    public static void SolveIt_2ndPart()
    {
        _inputRaw = File.ReadAllLines(InputFile);
        _borderY = _inputRaw.Length;
        _borderX = _inputRaw[0].Length;
    }
}