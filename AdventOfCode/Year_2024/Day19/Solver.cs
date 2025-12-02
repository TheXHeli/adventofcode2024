namespace AdventOfCode.Year_2024.Day19;

public static class Solver
{
    const string InputFile = "inputs/day19_1.txt";
    private static string[] _towelTypes;
    private static List<string> _duplicates;
    private static int _globCnt;
    private static Dictionary<string, long> reachedResults;

    public static void SolveIt_1stPart()
    {
        var input = File.ReadAllLines(InputFile);
        _towelTypes = input[0].Split(',', StringSplitOptions.TrimEntries);
        //var regex = new Regex(@"^([r|wr|b|g|bwu|rb|gb|br])+$");
        var resCnt = 0;
        for (int i = 2; i < input.Length; i++)
        {
            _duplicates = new List<string>();
            var result = TryToParse(input[i]);
            if (result) resCnt++;
            Console.WriteLine($"{i.ToString(),3} - {input[i]} - {result}");
        }

        Console.WriteLine(resCnt);
    }

    private static bool TryToParse(string curStrToTry)
    {
        if (_duplicates.Contains(curStrToTry))
        {
            return false;
        }

        if (curStrToTry.Length == 0) return true;
        foreach (var towelType in _towelTypes.Where(curStrToTry.Contains))
        {
            var found = false;
            if (curStrToTry.StartsWith(towelType))
            {
                found |= TryToParse(curStrToTry.Substring(towelType.Length));
                if (found) return true;
            }
        }

        if (!_duplicates.Contains(curStrToTry))
        {
            _duplicates.Add(curStrToTry);
            Console.WriteLine(curStrToTry);
        }

        return false;
    }

    public static void SolveIt_2ndPart()
    {
        var input = File.ReadAllLines(InputFile);
        _towelTypes = input[0].Split(',', StringSplitOptions.TrimEntries);
        //var regex = new Regex(@"^([r|wr|b|g|bwu|rb|gb|br])+$");
        long resCnt = 0;
        _globCnt = 0;
        reachedResults = new Dictionary<string, long>();
        for (int i = 2; i < input.Length; i++)
        {
            _duplicates = new List<string>();
            var result = TryToParse_V3(input[i]);
            resCnt += result;
            Console.WriteLine($"{i.ToString(),3} - {input[i]} - {result}");
        }


        Console.WriteLine(resCnt);
    }

    private static long TryToParse_V3(string curStrToTry)
    {
        if (curStrToTry.Length == 0)
        {
            return 1;
        }

        long curSumme = 0;
        if (reachedResults.ContainsKey(curStrToTry))
        {
            return reachedResults[curStrToTry];
        }

        foreach (var towelType in _towelTypes.Where(curStrToTry.Contains))
        {
            if (curStrToTry.StartsWith(towelType))
            {
                curSumme += TryToParse_V3(curStrToTry.Substring(towelType.Length));
            }
        }

        reachedResults.Add(curStrToTry, curSumme);
        return curSumme;

        if (!_duplicates.Contains(curStrToTry))
        {
            _duplicates.Add(curStrToTry);
            Console.WriteLine(curStrToTry);
        }

        return 0;
    }
}