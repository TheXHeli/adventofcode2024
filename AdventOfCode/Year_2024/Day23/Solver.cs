namespace AdventOfCode.Year_2024.Day23;

public static class Solver
{
    const string InputFile = "inputs/day23_bsp.txt";

    public static void SolveIt_1stPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
        var connectDict = new Dictionary<string, List<string>>();
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var addrs = inputRaw[i].Split("-");
            var leftNetAddr = addrs[0];
            var rightNetAddr = addrs[1];
            //--
            if (!connectDict.ContainsKey(leftNetAddr))
            {
                connectDict.Add(leftNetAddr, new List<string>());
            }
            connectDict[leftNetAddr].Add(rightNetAddr);
            
            //--
            if (!connectDict.ContainsKey(rightNetAddr))
            {
                connectDict.Add(rightNetAddr, new List<string>());
            }

            connectDict[rightNetAddr].Add(leftNetAddr);
        }
        //-----
        var connectedAddrs = new List<List<string>>();
        foreach (var tmpEntry in connectDict)
        {
            var list1 = tmpEntry.Value;
            foreach (var tmpInner in tmpEntry.Value)
            {
                var list2 = connectDict[tmpInner];
                var interSec = list2.Intersect(list1);
            }
            // var tmpConAddrs = new List<string>();
            // var found = false;
            // do
            // {
            //     found = false;
            //     if (connectDict.ContainsKey(tmpEntry.Key))
            //     {
            //         
            //     }
            //
            // } while (found);

            //connectedAddrs.Add(tmpConAddrs);
        }
    }

    public static void SolveIt_2ndPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
    }
}