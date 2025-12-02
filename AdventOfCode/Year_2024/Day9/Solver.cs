namespace AdventOfCode.Year_2024.Day9;

public static class Solver
{
    const string InputFile = "inputs/day9_1.txt";

    public static void SolveIt_1stPart()
    {
        var inputRaw = File.ReadAllText(InputFile);
        var memory = new List<int>();
        //Jeder Arrayeintrag = ID der Datei, oder -1 wenn Frei
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var blockCnt = inputRaw[i] - 48;
            if (i % 2 == 0)
            {
                var fileID = i / 2;
                for (int j = 0; j < blockCnt; j++)
                {
                    memory.Add(fileID);
                }
            }
            else
            {
                for (int j = 0; j < blockCnt; j++)
                {
                    memory.Add(-1);
                }
            }
        }

        //DebugOutMemory(memory);
        var curPos = 0;
        var lastBlockWithDataIn = memory.Count - 1;
        while (curPos < lastBlockWithDataIn)
        {
            if (memory[curPos] == -1)
            {
                var tmp = memory[lastBlockWithDataIn];
                memory[curPos] = tmp;
                memory[lastBlockWithDataIn] = -1;
                lastBlockWithDataIn = CalcNewBlockWithDataInIt(memory, lastBlockWithDataIn);
            }

            curPos++;
        }

        curPos = 0;
        decimal gesErg = 0;
        while (memory[curPos] != -1)
        {
            gesErg += curPos * memory[curPos];
            curPos++;
        }

        //DebugOutMemory(memory);
        Console.WriteLine(gesErg);
    }

    private static int CalcNewBlockWithDataInIt(List<int> memory, int lastBlockWithDataIn)
    {
        var retIdx = lastBlockWithDataIn;
        while (memory[retIdx] == -1)
        {
            retIdx--;
        }

        return retIdx;
    }

    private static void DebugOutMemory(List<int> memory)
    {
        var elemToWrite = memory.Select(x => x == -1 ? "." : x.ToString()).ToList();
        Console.WriteLine(string.Join("", elemToWrite));
    }


    public static void SolveIt_2ndPart()
    {
        var inputRaw = File.ReadAllText(InputFile);
        var memory = new List<int>();
        //muss beim Einsetzen aktualisiert werden !!! Endposition nach EInfuegen ist neuer Start und Laenge entsprechend bis zum naechsten Block
        var leerSpeicherDict = new SortedDictionary<int, int>();
        var fileGroesseDict = new Dictionary<int, int>(); //key = ID der Datei
        var filePosDict = new Dictionary<int, int>(); //key = ID der Datei, value = pos
        //Jeder Arrayeintrag = ID der Datei, oder -1 wenn Frei
        var fileID = 0;
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var blockCnt = inputRaw[i] - 48;
            if (i % 2 == 0)
            {
                fileID = i / 2;
                fileGroesseDict.Add(fileID, blockCnt);
                filePosDict.Add(fileID, memory.Count);
                for (int j = 0; j < blockCnt; j++)
                {
                    memory.Add(fileID);
                }
            }
            else
            {
                if (blockCnt > 0)
                {
                    leerSpeicherDict.Add(memory.Count, blockCnt);
                    for (int j = 0; j < blockCnt; j++)
                    {
                        memory.Add(-1);
                    }
                }
            }
        }

        //DebugOutMemory(memory);
        var curPos = 0;
        var curFileId = fileID;
        //var lastBlockWithDataIn = memory.Count - 1;
        while (curFileId >= 0)
        {
            var curSizeOfFile = fileGroesseDict[curFileId];
            var foundSpeicherLueckePos = -1;
            foreach (var tmpSpeicherLuecke in leerSpeicherDict)
            {
                if (tmpSpeicherLuecke.Value >= curSizeOfFile)
                {
                    foundSpeicherLueckePos = tmpSpeicherLuecke.Key;
                    break;
                }
            }

            if ((foundSpeicherLueckePos >= 0) && (filePosDict[curFileId] > foundSpeicherLueckePos))
            {
                for (int i = 0; i < curSizeOfFile; i++)
                {
                    memory[foundSpeicherLueckePos + i] = curFileId;
                    memory[filePosDict[curFileId] + i] = -1;
                }

                var luckenGroesse = leerSpeicherDict[foundSpeicherLueckePos];
                leerSpeicherDict.Remove(foundSpeicherLueckePos);
                leerSpeicherDict.Add(foundSpeicherLueckePos + curSizeOfFile, luckenGroesse - curSizeOfFile);

                //DebugOutMemory(memory);
            }

            curFileId--;
        }

        decimal gesErg = 0;
        for (int i = 0; i < memory.Count; i++)
        {
            if (memory[i] != -1)
            {
                gesErg += i * memory[i];
            }
        }

        //DebugOutMemory(memory);
        Console.WriteLine(gesErg);
    }
}