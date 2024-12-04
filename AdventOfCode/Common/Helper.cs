namespace AdventOfCode.Common;

public static class Helper
{
    public static List<List<int>> ConvertInputToArrayArray(string[] inputStr)
    {
        var retVal = new List<List<int>>();
        foreach (var tmpEntry in inputStr)
        {
            var elems = tmpEntry.Split(" ").Where(x => !string.IsNullOrWhiteSpace(x)).Select(x => int.Parse(x))
                .ToList();
            retVal.Add(elems);
        }

        return retVal;
    }

    public static List<List<int>> GetArrayArrayFromFile(string fileName)
    {
        var inputStr = File.ReadAllLines(fileName);
        return ConvertInputToArrayArray(inputStr);
    }
}