namespace AdventOfCode.Year_2024.Day22;

public static class Solver
{
    const string InputFile = "inputs/day22_1.txt";
   
    public static void SolveIt_1stPart()
    {
        long gesResult = 0;
        var inputRaw = File.ReadAllLines(InputFile);
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var nextSecret = CalcNextSecret(long.Parse(inputRaw[i]));    
            for (int wdhl = 0; wdhl < 1999; wdhl++)
            {
                nextSecret = CalcNextSecret(nextSecret);
            }
            gesResult += nextSecret;
            Console.WriteLine(nextSecret);
        }
        Console.WriteLine(gesResult);
    }

    private static long CalcNextSecret(long curSecret)
    {
        long retVal = ((curSecret << 6) ^ curSecret) & 16777215;
        retVal = ((retVal >> 5) ^ retVal) & 16777215;
        retVal = ((retVal << 11) ^ retVal) & 16777215;
        return retVal;
    }

    public static void SolveIt_2ndPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
    }
}