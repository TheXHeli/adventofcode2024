using System.Diagnostics;
using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day13;

record InputObj
{
    public decimal ButtonAX { get; set; }
    public decimal ButtonAY { get; set; }
    public decimal ButtonBX { get; set; }
    public decimal ButtonBY { get; set; }
    public decimal ResultX { get; set; }
    public decimal ResultY { get; set; }
}

public static class Solver
{
    const string InputFile = "inputs/day13_1.txt";


    public static void SolveIt_1stPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
        var cleanedInput = new List<InputObj>();
        var regexA = new Regex("^Button A: X\\+([0-9]+),\\sY\\+([0-9]+)$");
        var regexB = new Regex("^Button B: X\\+([0-9]+),\\sY\\+([0-9]+)$");
        var regexResult = new Regex("^Prize: X=([0-9]+),\\sY=([0-9]+)$");
        for (int i = 0; i < (inputRaw.Length / 4) + 1; i++)
        {
            var inputObjToAdd = new InputObj();
            var matchesButtonA = regexA.Match(inputRaw[i * 4]);
            var matchesButtonB = regexB.Match(inputRaw[i * 4 + 1]);
            var matchesResult = regexResult.Match(inputRaw[i * 4 + 2]);

            inputObjToAdd.ButtonAX = int.Parse(matchesButtonA.Groups[1].Value);
            inputObjToAdd.ButtonAY = int.Parse(matchesButtonA.Groups[2].Value);
            inputObjToAdd.ButtonBX = int.Parse(matchesButtonB.Groups[1].Value);
            inputObjToAdd.ButtonBY = int.Parse(matchesButtonB.Groups[2].Value);
            inputObjToAdd.ResultX = int.Parse(matchesResult.Groups[1].Value);
            inputObjToAdd.ResultY = int.Parse(matchesResult.Groups[2].Value);
            cleanedInput.Add(inputObjToAdd);
        }

        //33686 zu hoch
        //Richtig 30413
        decimal gesCosts = 0;
        foreach (var tmpEntry in cleanedInput)
        {
            var pressesB = ((tmpEntry.ResultX * tmpEntry.ButtonAY) - (tmpEntry.ResultY * tmpEntry.ButtonAX)) /
                           ((tmpEntry.ButtonBX * tmpEntry.ButtonAY) - (tmpEntry.ButtonBY * tmpEntry.ButtonAX));
            var pressesA = (tmpEntry.ResultX - pressesB * tmpEntry.ButtonBX) / (tmpEntry.ButtonAX);

            if ((pressesA <= 100) && (pressesB <= 100) && (pressesA > 0) && (pressesB > 0))
            {
                if ((Math.Ceiling(pressesA) == pressesA) && (Math.Ceiling(pressesB) == pressesB))
                {
                    var costs = pressesA * 3 + pressesB;
                    gesCosts += costs;
                    //Console.WriteLine(costs);
                    var controlX = tmpEntry.ButtonAX * pressesA + tmpEntry.ButtonBX * pressesB;
                    var controlY = tmpEntry.ButtonAY * pressesA + tmpEntry.ButtonBY * pressesB;
                    if ((controlX != tmpEntry.ResultX) || (controlY != tmpEntry.ResultY))
                    {
                        Console.WriteLine($"PROBLEM");
                    }
                }
                
            }
        }

        Console.WriteLine(gesCosts);
    }


    public static void SolveIt_2ndPart()
    {
    }
}