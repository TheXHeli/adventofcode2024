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

    public static void SolveIt_1stPart()
    {
        //p=0,4 v=3,-3
        var inputRaw = File.ReadAllLines(InputFile);
        var cleanedInput = new List<InputObj>();
        var regexRobotProps = new Regex("^p=([0-9]+),([0-9]+)\\sv=([\\-0-9]+),([\\-0-9]+)$");
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var inputObjToAdd = new InputObj();
            var robotPropsMatch = regexRobotProps.Match(inputRaw[i]);
            inputObjToAdd.StartX = decimal.Parse(robotPropsMatch.Groups[1].Value);
            inputObjToAdd.StartY = decimal.Parse(robotPropsMatch.Groups[2].Value);
            inputObjToAdd.dX = decimal.Parse(robotPropsMatch.Groups[3].Value);
            inputObjToAdd.dY = decimal.Parse(robotPropsMatch.Groups[4].Value);
            cleanedInput.Add(inputObjToAdd);
        }

        Console.WriteLine(cleanedInput.Count);

        var result = new List<int> { 0, 0, 0, 0 };
        var middleX = RoomWidth / 2;
        var middleY = RoomHeight / 2;
        foreach (var tmpEntry in cleanedInput)
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
    }
}