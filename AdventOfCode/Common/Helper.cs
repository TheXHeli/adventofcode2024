namespace AdventOfCode.Common;

public static class Helper
{
    public static List<List<int>> ConvertInputToArrayArray(string[] inputStr, string splitStr)
    {
        var retVal = new List<List<int>>();
        foreach (var tmpEntry in inputStr)
        {
            var elems = tmpEntry.Split(splitStr).Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x))
                .ToList();
            retVal.Add(elems);
        }

        return retVal;
    }

    public static List<List<int>> GetArrayArrayFromFile(string fileName, string splitStr)
    {
        var inputStr = File.ReadAllLines(fileName);
        return ConvertInputToArrayArray(inputStr, splitStr);
    }
}