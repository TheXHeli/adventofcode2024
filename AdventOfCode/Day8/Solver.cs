using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day8;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var mapLinesRaw = File.ReadAllLines("inputs/day8_1.txt");
        //byte[,] mapLinesByte = new byte[mapLinesRaw.Length, mapLinesRaw[0].Length];
        var antennen = new Dictionary<char, List<(int, int)>>();
        for (int y = 0; y < mapLinesRaw.Length; y++)
        {
            for (int x = 0; x < mapLinesRaw[y].Length; x++)
            {
                if (mapLinesRaw[y][x] == '.') continue;
                if (!antennen.ContainsKey(mapLinesRaw[y][x]))
                {
                    antennen.Add(mapLinesRaw[y][x], new List<(int, int)>());
                }

                antennen[mapLinesRaw[y][x]].Add((x, y));
            }
        }

        //---
        var borderX = mapLinesRaw.Length;
        var borderY = mapLinesRaw[0].Length;
        bool[,] resultMap = new bool[borderX, borderY];
        foreach (var antennenDaten in antennen)
        {
            var antennenListe = antennenDaten.Value;
            for (int s = 0; s < antennenListe.Count; s++)
            {
                for (int e = 0; e < antennenListe.Count; e++)
                {
                    if (s != e)
                    {
                        var dX = antennenListe[s].Item1 - antennenListe[e].Item1;
                        var dY = antennenListe[s].Item2 - antennenListe[e].Item2;
                        var newX1 = antennenListe[s].Item1 + dX;
                        var newY1 = antennenListe[s].Item2 + dY;
                        var newX2 = antennenListe[e].Item1 - dX;
                        var newY2 = antennenListe[e].Item2 - dY;
                        if (newX1 >= 0 && newY1 >= 0 && newX1 < borderX && newY1 < borderY)
                        {
                            //if (mapLinesRaw[newY1][newX1] == '.')
                            //{
                            resultMap[newX1, newY1] = true;
                            Console.WriteLine($"Antinodes: {newX1}, {newY1}");
                            //}
                        }
                        //Console.WriteLine($"{s},{e}");
                    }
                }
            }
        }

        var result = WriteResultMap(resultMap);
        Console.WriteLine(result);
    }

    private static int WriteResultMap(bool[,] resultMap)
    {
        var resultCnt = 0;
        for (int y = 0; y < resultMap.GetLength(1); y++)
        {
            var newLineStr = "";
            for (int x = 0; x < resultMap.GetLength(0); x++)
            {
                if (resultMap[x, y])
                {
                    newLineStr += "#";
                    resultCnt++;
                }
                else
                {
                    newLineStr += ".";
                }
            }

            Console.WriteLine(newLineStr);
        }

        return resultCnt;
    }

    public static void SolveIt_2ndPart()
    {
        var mapLinesRaw = File.ReadAllLines("inputs/day8_1.txt");
        //byte[,] mapLinesByte = new byte[mapLinesRaw.Length, mapLinesRaw[0].Length];
        var antennen = new Dictionary<char, List<(int, int)>>();
        for (int y = 0; y < mapLinesRaw.Length; y++)
        {
            for (int x = 0; x < mapLinesRaw[y].Length; x++)
            {
                if (mapLinesRaw[y][x] == '.') continue;
                if (!antennen.ContainsKey(mapLinesRaw[y][x]))
                {
                    antennen.Add(mapLinesRaw[y][x], new List<(int, int)>());
                }

                antennen[mapLinesRaw[y][x]].Add((x, y));
            }
        }

        //---
        var borderX = mapLinesRaw.Length;
        var borderY = mapLinesRaw[0].Length;
        bool[,] resultMap = new bool[borderX, borderY];
        foreach (var antennenDaten in antennen)
        {
            var antennenListe = antennenDaten.Value;
            for (int s = 0; s < antennenListe.Count; s++)
            {
                for (int e = 0; e < antennenListe.Count; e++)
                {
                    resultMap[antennenListe[s].Item1, antennenListe[s].Item2] = true; 
                    resultMap[antennenListe[e].Item1, antennenListe[e].Item2] = true; 
                    
                    if (s != e)
                    {
                        var dX = antennenListe[s].Item1 - antennenListe[e].Item1;
                        var dY = antennenListe[s].Item2 - antennenListe[e].Item2;
                        var newStartX1 = antennenListe[s].Item1 + dX;
                        var newStartY1 = antennenListe[s].Item2 + dY;
                        var newX1 = newStartX1;
                        var newY1 = newStartY1;
                        //var newX2 = antennenListe[e].Item1 - dX;
                        //var newY2 = antennenListe[e].Item2 - dY;
                        while (newX1 >= 0 && newY1 >= 0 && newX1 < borderX && newY1 < borderY)
                        {
                            //wenn hier reingegangen wird, dann strahlt die Antenne (hat also mehr als eine) unbd kann dann als gesetzt betrachtet werden
                            resultMap[antennenListe[s].Item1, antennenListe[s].Item2] = true; 
                            resultMap[antennenListe[e].Item1, antennenListe[e].Item2] = true; 
                            
                            resultMap[newX1, newY1] = true;
                            Console.WriteLine($"Antinodes: {newX1}, {newY1}");
                            newX1 += dX;
                            newY1 += dY;
                        }


                        //Console.WriteLine($"{s},{e}");
                    }
                }
            }
        }

        var result = WriteResultMap(resultMap);
        Console.WriteLine(result);
    }
}