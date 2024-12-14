using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day14;

record InputObj
{
    public decimal StartX { get; set; }
    public decimal StartY { get; set; }
    public decimal dX { get; set; }
    public decimal dY { get; set; }
}

public static class Solver
{
    const string InputFile = "inputs/day14_1.txt";
    private const int RoomWidth = 101;
    private const int RoomHeight = 103;
    private static readonly List<InputObj> CleanedInput = new List<InputObj>();

    public static void SolveIt_1stPart()
    {
        //p=0,4 v=3,-3
        var inputRaw = File.ReadAllLines(InputFile);
        var regexRobotProps = new Regex("^p=([0-9]+),([0-9]+)\\sv=([\\-0-9]+),([\\-0-9]+)$");
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var inputObjToAdd = new InputObj();
            var robotPropsMatch = regexRobotProps.Match(inputRaw[i]);
            inputObjToAdd.StartX = decimal.Parse(robotPropsMatch.Groups[1].Value);
            inputObjToAdd.StartY = decimal.Parse(robotPropsMatch.Groups[2].Value);
            inputObjToAdd.dX = decimal.Parse(robotPropsMatch.Groups[3].Value);
            inputObjToAdd.dY = decimal.Parse(robotPropsMatch.Groups[4].Value);
            CleanedInput.Add(inputObjToAdd);
        }

        Console.WriteLine(CleanedInput.Count);

        var result = new List<int> { 0, 0, 0, 0 };
        var middleX = RoomWidth / 2;
        var middleY = RoomHeight / 2;
        foreach (var tmpEntry in CleanedInput)
        {
            var finalX = (100 * tmpEntry.dX + tmpEntry.StartX) % RoomWidth;
            var finalY = (100 * tmpEntry.dY + tmpEntry.StartY) % RoomHeight;
            if (finalX < 0) finalX += RoomWidth;
            if (finalY < 0) finalY += RoomHeight;
            //--Quadrant ermitteln
            if (finalX < middleX) //Linke Hälfte
            {
                if (finalY < middleY) //Oben Links
                {
                    result[0] += 1;
                }
                else if (finalY > middleY)
                {
                    result[1] += 1;
                }
            }
            else if (finalX > middleX) //Rechte Hälfte
            {
                if (finalY < middleY) //Oben Links
                {
                    result[2] += 1;
                }
                else if (finalY > middleY)
                {
                    result[3] += 1;
                }
            }
        }

        var gesRsult = result[0] * result[1] * result[2] * result[3];
        Console.WriteLine(gesRsult);
    }


    public static void SolveIt_2ndPart()
    {
        //p=0,4 v=3,-3
        var inputRaw = File.ReadAllLines(InputFile);
        var regexRobotProps = new Regex("^p=([0-9]+),([0-9]+)\\sv=([\\-0-9]+),([\\-0-9]+)$");
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var inputObjToAdd = new InputObj();
            var robotPropsMatch = regexRobotProps.Match(inputRaw[i]);
            inputObjToAdd.StartX = decimal.Parse(robotPropsMatch.Groups[1].Value);
            inputObjToAdd.StartY = decimal.Parse(robotPropsMatch.Groups[2].Value);
            inputObjToAdd.dX = decimal.Parse(robotPropsMatch.Groups[3].Value);
            inputObjToAdd.dY = decimal.Parse(robotPropsMatch.Groups[4].Value);
            CleanedInput.Add(inputObjToAdd);
        }

        Console.WriteLine(CleanedInput.Count);

        var result = new List<int> { 0, 0, 0, 0 };
        var newRobotMap = CalculateRobotMap(100);
        PlotRobotMap(newRobotMap);

        var gesRsult = result[0] * result[1] * result[2] * result[3];
        Console.WriteLine(gesRsult);
    }

    private static void PlotRobotMap(int[,] robotMap)
    {
        var middleX = robotMap.GetLength(0) / 2;
        var middleY = robotMap.GetLength(1) / 2;

        for (int y = 0; y < robotMap.GetLength(1); y++)
        {
            if (y == middleY)
            {
                Console.WriteLine(y.ToString().PadLeft(3, ' ') + "  " + "".PadLeft(robotMap.GetLength(0), '-'));
            }
            else
            {
                var outStr = "";
                for (int x = 0; x < robotMap.GetLength(0); x++)
                {
                    if (x == middleX)
                    {
                        outStr += "|";
                    }
                    else
                    {
                        var curValue = robotMap[x, y];
                        if (curValue == 0)
                        {
                            outStr += " ";
                        }
                        else if (curValue <= 9)
                        {
                            outStr += curValue.ToString();
                        }
                        else
                        {
                            outStr += "X";
                        }
                    }
                }

                Console.WriteLine(y.ToString().PadLeft(3, ' ') + "  " + outStr);
            }
        }
    }

    private static int[,] CalculateRobotMap(int blinks)
    {
        int[,] returnMap = new int[RoomWidth, RoomHeight];
        var middleX = RoomWidth / 2;
        var middleY = RoomHeight / 2;
        foreach (var tmpEntry in CleanedInput)
        {
            var finalX = (100 * tmpEntry.dX + tmpEntry.StartX) % RoomWidth;
            var finalY = (100 * tmpEntry.dY + tmpEntry.StartY) % RoomHeight;
            if (finalX < 0) finalX += RoomWidth;
            if (finalY < 0) finalY += RoomHeight;
            if ((finalX != middleX) && (finalY != middleY))
            {
                returnMap[(int)finalX, (int)finalY] += 1;
            }
        }

        return returnMap;
    }
}