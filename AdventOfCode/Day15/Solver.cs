using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day15;

record DirDef
{
    public int dX { get; set; }
    public int dY { get; set; }
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
                    _warehouseMap[x - 1, y - 1] = '.';
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

        Solve_1();
        Console.WriteLine("Day 15 - Puzzle 15");
    }

    private static void Solve_1()
    {
        var curPosX = _robStartX;
        var curPosY = _robStartY;
        for (int i = 0; i < _roboterMoves.Count; i++)
        {
            var curMoveInstruction = _roboterMoves[i];
            var newPosX = curPosX + curMoveInstruction.dX;
            var newPosY = curPosY + curMoveInstruction.dY;
            if ((newPosX >= 0) && (newPosX < _borderX) && (newPosY >= 0) && (newPosY < _borderY))
            {
                if (_warehouseMap[newPosX, newPosY] == 'O')
                {
                    //nach aktuellem Dir Statement solange weiter in die Richtung gehen, bis kein O mehr kommt
                    //dann erstes neue Roboterpos setzen und O von der neuen Roboteros nach vorne verschieben
                    //Wird vorher der Rand erreicht, dann bleibt roboter einfach stehen
                    var verschiebenGeklappt = KistenVerschieben(newPosX, newPosY, curMoveInstruction);
                    if (verschiebenGeklappt)
                    {
                        _warehouseMap[curPosX, curPosY] = '.';
                        curPosX = newPosX;
                        curPosY = newPosY;
                        _warehouseMap[curPosX, curPosY] = '.'; //Unter dem Roboter ist nichts
                    }
                }
                else if (_warehouseMap[newPosX, newPosY] == '#')
                {
                    //Roboter bleibt stehen, nix machen
                }
                else
                {
                    curPosX = newPosX;
                    curPosY = newPosY;
                }
            }

            Console.WriteLine($"-------- Map nach Schritt: {i} --------");
            PlotWarehouseMap(curPosX, curPosY);
        }
    }

    private static bool KistenVerschieben(int kistenStartX, int kistenStartY, DirDef curMoveInstruction)
    {
        var curPosX = kistenStartX;
        var curPosY = kistenStartY;
        while (_warehouseMap[curPosX, curPosY] == 'O')
        {
            curPosX += curMoveInstruction.dX;
            curPosY += curMoveInstruction.dY;
            if ((curPosX >= 0) && (curPosX < _borderX) && (curPosY >= 0) && (curPosY < _borderY))
            {
                if (_warehouseMap[curPosX, curPosY] == '.')
                {
                    _warehouseMap[curPosX, curPosY] = 'O';
                    return true;
                }

                if (_warehouseMap[curPosX, curPosY] == '#')
                {
                    return false;
                }
            }
            else //ausserhalb der Map, keine Verschiebung notwendig, da vorher keine Lücke gefunden wurde, wo noch ne Kiste reingepasst hätte
            {
                return false;
            }
        }

        return false;
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