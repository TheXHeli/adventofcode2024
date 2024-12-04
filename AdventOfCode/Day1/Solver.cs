using AdventOfCode.Common;

namespace AdventOfCode.Day1;

public static class Solver
{
    public static void SolveIt_1stPart()
    {
        var rawList = Helper.GetArrayArrayFromFile("inputs/day1_1.txt");
        var list1 = rawList.Select(a => a[0]).ToList();
        var list2 = rawList.Select(a => a[1]).ToList();
        list1.Sort();
        list2.Sort();
        var result = 0;
        for (var i = 0; i < list1.Count; i++)
        {
            result += Math.Abs(list1[i] - list2[i]);
        }

        Console.WriteLine(result);
    }

    public static void SolveIt_2ndPart()
    {
        var rawList = Helper.GetArrayArrayFromFile("inputs/day1_1.txt");
        var list1 = rawList.Select(a => a[0]).ToList();
        var list2 = rawList.Select(a => a[1]).ToList();
        list1.Sort();
        list2.Sort();
        var result = 0;
        var rightListGrouped = list2.GroupBy(a => a).ToDictionary(a => a.Key, a => a.Count());
        for (var i = 0; i < list1.Count; i++)
        {
            if (rightListGrouped.ContainsKey(list1[i]))
            {
                result += rightListGrouped[list1[i]] * list1[i];
            }
        }
        Console.WriteLine(result);
    }
}