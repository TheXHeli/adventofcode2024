using System.Diagnostics;

namespace AdventOfCode.Year_2025.Day2;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day2_1.txt";

    public static void SolveIt_1stPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();

        var inputRaw = File.ReadAllLines(InputFile);
        var startValue = 50m;
        var gesCnt = 0l;

        var splitCommas = inputRaw[0].Split(",");
        foreach (var rangeItem in splitCommas)
        {
            var splitedRange = rangeItem.Split("-");
            var startVal = long.Parse(splitedRange[0]);
            var endVal = long.Parse(splitedRange[1]);
            for (long i = startVal; i <= endVal; i++)
            {
                gesCnt += EvaluateIfInvalidId(i);
            }
        }

        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);

        //1227775554, 38158151648
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static long EvaluateIfInvalidId(long numberToCheck)
    {
        var valAsStr = numberToCheck.ToString();
        if (valAsStr.Length % 2 != 0) return 0;
        var firstHalf = valAsStr.Substring(0, valAsStr.Length / 2);
        var secondHalf = valAsStr.Substring(valAsStr.Length / 2, valAsStr.Length / 2);
        if (firstHalf == secondHalf) return numberToCheck;
        Console.WriteLine(firstHalf + ".." + secondHalf);
        return 0;
    }


    public static void SolveIt_2ndPart()
    {
        var stopW = new Stopwatch();
        stopW.Start();

        var inputRaw = File.ReadAllLines(InputFile);
        var startValue = 50m;
        var gesCnt = 0l;

        var splitCommas = inputRaw[0].Split(",");
        foreach (var rangeItem in splitCommas)
        {
            var splitedRange = rangeItem.Split("-");
            var startVal = long.Parse(splitedRange[0]);
            var endVal = long.Parse(splitedRange[1]);
            for (long i = startVal; i <= endVal; i++)
            {
                gesCnt += EvaluateIfInvalidIdV2(i);
            }
        }
        stopW.Stop();
        Console.WriteLine(stopW.Elapsed);

        //4174379265, ??
        Console.WriteLine("Erg: " + gesCnt);
    }

    private static long EvaluateIfInvalidIdV2(long numberToCheck)
    {
        //Console.WriteLine("NumberToCHeck: " + numberToCheck);
        var valAsStr = numberToCheck.ToString();
        for (int i = 2; i <= valAsStr.Length; i++)
        {
            if (valAsStr.Length % i != 0) continue;
            var splitedStr = SplitStringIntoNParts(valAsStr, i);
            var distinctList = splitedStr.Distinct();
            if (distinctList.Count() == 1)
            {
                return numberToCheck;
                //Console.WriteLine(i + "->" + string.Join(',', splitedStr));
            }
        }
        return 0;
    }

    private static List<string> SplitStringIntoNParts(string strToSplit, int numberOfParts)
    {
        var retval = new List<string>();
        var lengthOfOneSegement = strToSplit.Length / numberOfParts;
        for (int i = 0; i < numberOfParts; i++)
        {
            retval.Add(strToSplit.Substring(lengthOfOneSegement * i, lengthOfOneSegement));
        }

        return retval;
    }
}