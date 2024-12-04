using System.Text.RegularExpressions;
using AdventOfCode.Common;

namespace AdventOfCode.Day3;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var stringToParse = File.ReadAllText("inputs/day3_1.txt");
        var tokensToFind = new List<string> { "mul(", ",", ")" };
        var whishedResults = new List<int> { 0, 1 }; //die Lücke NACH index 1 und 3 nehmen
        var whishedResultRegexCheck = new List<string> { "^[0-9]+$", "^[0-9]+$" };
        var startTryParsePosition = 0;
        var lastStartTryParsePosition = -1;
        var gesamtErgebnis = 0;
        do
        {
            var resultTokens = new List<string>();
            var curSearchPos = startTryParsePosition;
            var lastTokenFoundPos = 0;
            for (int t = 0; t < tokensToFind.Count - 1; t++)
            {
                var pos1 = stringToParse.IndexOf(tokensToFind[t], curSearchPos);
                if (pos1 == -1) break;
                var pos2 = stringToParse.IndexOf(tokensToFind[t + 1], pos1);
                if (pos2 == -1) break;
                curSearchPos = pos2;
                lastTokenFoundPos = pos2;
                if (t == whishedResults[resultTokens.Count])
                {
                    var extractedToken = stringToParse.Substring(pos1 + tokensToFind[t].Length,
                        pos2 - pos1 - tokensToFind[t].Length);
                    var isValid = Regex.IsMatch(extractedToken, whishedResultRegexCheck[resultTokens.Count]);
                    resultTokens.Add(isValid ? extractedToken : null);
                }
            }

            lastStartTryParsePosition = startTryParsePosition;
            if (resultTokens.Count(a => a != null) == whishedResults.Count)
            {
                gesamtErgebnis += int.Parse(resultTokens[0]) * int.Parse(resultTokens[1]);
                startTryParsePosition = lastTokenFoundPos;
            }
            else
            {
                startTryParsePosition++;
            }
        } while ((startTryParsePosition > lastStartTryParsePosition) && (startTryParsePosition < stringToParse.Length));

        Console.WriteLine(gesamtErgebnis);
    }

    public static void SolveIt_2ndPart()
    {
        var stringToParse = File.ReadAllText("inputs/day3_1.txt");
        stringToParse = RemoveNotAllowedBlocks(stringToParse);
        var tokensToFind = new List<string> { "mul(", ",", ")" };
        var whishedResults = new List<int> { 0, 1 }; //die Lücke NACH index 1 und 3 nehmen
        var whishedResultRegexCheck = new List<string> { "^[0-9]+$", "^[0-9]+$" };
        var startTryParsePosition = 0;
        var lastStartTryParsePosition = -1;
        var gesamtErgebnis = 0;
        do
        {
            var resultTokens = new List<string>();
            var curSearchPos = startTryParsePosition;
            var lastTokenFoundPos = 0;
            for (int t = 0; t < tokensToFind.Count - 1; t++)
            {
                var pos1 = stringToParse.IndexOf(tokensToFind[t], curSearchPos);
                if (pos1 == -1) break;
                var pos2 = stringToParse.IndexOf(tokensToFind[t + 1], pos1);
                if (pos2 == -1) break;
                curSearchPos = pos2;
                lastTokenFoundPos = pos2;
                if (t == whishedResults[resultTokens.Count])
                {
                    var extractedToken = stringToParse.Substring(pos1 + tokensToFind[t].Length,
                        pos2 - pos1 - tokensToFind[t].Length);
                    var isValid = Regex.IsMatch(extractedToken, whishedResultRegexCheck[resultTokens.Count]);
                    resultTokens.Add(isValid ? extractedToken : null);
                }
            }

            lastStartTryParsePosition = startTryParsePosition;
            if (resultTokens.Count(a => a != null) == whishedResults.Count)
            {
                gesamtErgebnis += int.Parse(resultTokens[0]) * int.Parse(resultTokens[1]);
                startTryParsePosition = lastTokenFoundPos;
            }
            else
            {
                startTryParsePosition++;
            }
        } while ((startTryParsePosition > lastStartTryParsePosition) && (startTryParsePosition < stringToParse.Length));

        Console.WriteLine(gesamtErgebnis);
    }

    private static string RemoveNotAllowedBlocks(string stringToParse)
    {
        var found = false;
        stringToParse = stringToParse.Replace("\n", "").Replace("\r", "").Replace("\t", "");
        do
        {
            found = false;
            var pos1 = stringToParse.IndexOf("don't()");
            if (pos1 != -1)
            {
                var pos2 = stringToParse.IndexOf("do()", pos1);
                if (pos2 != -1)
                {
                    stringToParse = stringToParse.Substring(0, pos1 + 1) +
                                    stringToParse.Substring(pos2 + 4, stringToParse.Length - (pos2 + 4));
                }
                found = true;
            }
        } while (found);

        return stringToParse;
    }
}