using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day6;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var inputMap = ReadFileCodedAsByteArray(out var curX, out var curY);
        var stopWatch = Stopwatch.StartNew();
        int borderX = inputMap.GetLength(1);
        int borderY = inputMap.GetLength(0);
        bool[,] resultMap = new bool[inputMap.GetLength(0), inputMap.GetLength(1)];
        var dir = 0; //0 = Oben, 1 = Rechts, 2 = Unten, 3 = Links
        var dirX = 0; // -1 = nach Links in X Richtung, 1 = nach Rechts in X Richtung
        var dirY = -1; // -1 = nach Oben in Y-Richtung, 1 = nach Unten in Y Richtung
        do
        {
            if (inputMap[curY, curX] == 1) //Hinderniss
            {
                //Was auch immer gemacht wurde, einen Schritt zurück
                curX -= dirX;
                curY -= dirY;
                //Richtung ändern
                dir = (dir + 1) % 4;
                switch (dir)
                {
                    case 0:
                        dirY = -1;
                        dirX = 0;
                        break;
                    case 1:
                        dirX = 1;
                        dirY = 0;
                        break;
                    case 2:
                        dirX = 0;
                        dirY = 1;
                        break;
                    case 3:
                        dirX = -1;
                        dirY = 0;
                        break;
                }
            }
            else
            {
                resultMap[curY, curX] = true;
            }

            curX += dirX;
            curY += dirY;
        } while (curX >= 0 && curY >= 0 && curX < borderX && curY < borderY);

        int result = 0;
        for (int y = 0; y < borderY; y++)
        {
            for (int x = 0; x < borderX; x++)
            {
                if (resultMap[y, x]) result++;
            }
        }

        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalMicroseconds} us");
        Console.WriteLine(result);
    }

    public static void SolveIt_2ndPart()
    {
        var stopWatch = Stopwatch.StartNew();
        var inputMap = ReadFileCodedAsByteArray(out var curX, out var curY);
        int borderX = inputMap.GetLength(1);
        int borderY = inputMap.GetLength(0);
        var startXSaved = curX;
        var startYSaved = curY;

        var usedPfad = GetBeschrittenenPfad(inputMap, curX, curY);
        var realyUsedPfad = new List<int>();
        var extraCnt = 0;
        var result = 0;
        var listY = Enumerable.Range(0, inputMap.GetLength(0));
        Parallel.ForEach(listY, y =>
        {
            for (int x = 0; x < borderX; x++)
            {
                if (usedPfad.Contains((y << 8) | x))
                {
                    curY = startYSaved;
                    curX = startXSaved;
                    byte[,] saveMap = new byte[borderY, borderX];
                    Array.Copy(inputMap, saveMap, borderY * borderX);
                    var hasExited = WithExit(saveMap, curY, curX, borderX, borderY, y, x);
                    if (!hasExited)
                    {
                        Interlocked.Increment(ref result);
                    }
                }
            }
        });
        //var resultMap = realyUsedPfad.Except(usedPfad).ToList();

        Console.WriteLine("Bla:" + extraCnt);

        stopWatch.Stop();
        Console.WriteLine($"{stopWatch.Elapsed.TotalMicroseconds} us");
        Console.WriteLine(result);
    }

    private static List<int> GetBeschrittenenPfad(byte[,] inputMap, int curX, int curY)
    {
        var beschrittenenPfad = new List<int>();
        int borderX = inputMap.GetLength(1);
        int borderY = inputMap.GetLength(0);
        bool[,] resultMap = new bool[inputMap.GetLength(0), inputMap.GetLength(1)];
        var dir = 0; //0 = Oben, 1 = Rechts, 2 = Unten, 3 = Links
        var dirX = 0; // -1 = nach Links in X Richtung, 1 = nach Rechts in X Richtung
        var dirY = -1; // -1 = nach Oben in Y-Richtung, 1 = nach Unten in Y Richtung
        do
        {
            if (inputMap[curY, curX] == 1) //Hinderniss
            {
                //Was auch immer gemacht wurde, einen Schritt zurück
                curX -= dirX;
                curY -= dirY;
                //Richtung ändern
                dir = (dir + 1) % 4;
                switch (dir)
                {
                    case 0:
                        dirY = -1;
                        dirX = 0;
                        break;
                    case 1:
                        dirX = 1;
                        dirY = 0;
                        break;
                    case 2:
                        dirX = 0;
                        dirY = 1;
                        break;
                    case 3:
                        dirX = -1;
                        dirY = 0;
                        break;
                }
            }
            else
            {
                resultMap[curY, curX] = true;
            }

            curX += dirX;
            curY += dirY;
        } while (curX >= 0 && curY >= 0 && curX < borderX && curY < borderY);

        int result = 0;
        for (int y = 0; y < borderY; y++)
        {
            for (int x = 0; x < borderX; x++)
            {
                if (resultMap[y, x])
                {
                    beschrittenenPfad.Add((y << 8) | x);
                    result++;
                }
            }
        }

        Console.WriteLine(result);
        return beschrittenenPfad;
    }

    private static bool WithExit(byte[,] inputMap, int curY, int curX, int borderX, int borderY,
        int obstacleY, int obstacleX)
    {
        int[,] wowarichschonMap = new int[inputMap.GetLength(0), inputMap.GetLength(1)];
        var breadCrump = new List<int>();
        byte dir = 0; //0 = Oben, 1 = Rechts, 2 = Unten, 3 = Links
        var dirX = 0; // -1 = nach Links in X Richtung, 1 = nach Rechts in X Richtung
        var dirY = -1; // -1 = nach Oben in Y-Richtung, 1 = nach Unten in Y Richtung
        //if (inputMap[obstacleY, obstacleX] == 1) return true; //wenn eh schon hinderniss
        if (curX == obstacleX && curY == obstacleY) return true; //wenn auf startpos auch direkt aussteigen
        //var valueBefore = inputMap[obstacleY, obstacleX];
        inputMap[obstacleY, obstacleX] = 1;
        do
        {
            if ((wowarichschonMap[curY, curX] & (1 << dir)) == (1 << dir))
            {
                return false;
            }
            else
            {
                wowarichschonMap[curY, curX] = (wowarichschonMap[curY, curX] | (1 << dir));
            }

            if (inputMap[curY, curX] == 1) //Hinderniss
            {
                //Was auch immer gemacht wurde, einen Schritt zurück
                curX -= dirX;
                curY -= dirY;
                //Richtung ändern
                dir = (byte)((dir + 1) % 4);
                switch (dir)
                {
                    case 0:
                        dirY = -1;
                        dirX = 0;
                        break;
                    case 1:
                        dirX = 1;
                        dirY = 0;
                        break;
                    case 2:
                        dirX = 0;
                        dirY = 1;
                        break;
                    case 3:
                        dirX = -1;
                        dirY = 0;
                        break;
                }
            }

            curX += dirX;
            curY += dirY;
            // var curPositionCode = ((curY << 8 | curX) << 2) | dir;
            // if (breadCrump.Contains(curPositionCode))
            // {
            //     //inputMap[obstacleY, obstacleX] = valueBefore;
            //     return false;
            // }
            //
            // breadCrump.Add(curPositionCode);
        } while (curX >= 0 && curY >= 0 && curX < borderX && curY < borderY);

        //inputMap[obstacleY, obstacleX] = valueBefore;
        return true;
    }

    private static byte[,] ReadFileCodedAsByteArray(out int startX, out int startY)
    {
        var mapLinesRaw = File.ReadAllLines("inputs/day6_1.txt");
        startX = 0;
        startY = 0;
        byte[,] mapLinesByte = new byte[mapLinesRaw.Length, mapLinesRaw[0].Length];
        for (int y = 0; y < mapLinesRaw.Length; y++)
        {
            for (int x = 0; x < mapLinesRaw[y].Length; x++)
            {
                if (mapLinesRaw[y][x] == '#') mapLinesByte[y, x] = 1;
                if (mapLinesRaw[y][x] == '^')
                {
                    startX = x;
                    startY = y;
                }
            }
        }

        return mapLinesByte;
    }
}