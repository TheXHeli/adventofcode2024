namespace AdventOfCode.Year_2025.Day1;

public static class Solver
{
    const string InputFile = "Year_2025/inputs/day1_1.txt";

    public static void SolveIt_1stPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
        var startValue = 50m;
        var zeroCnt = 0;
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var cleaned = inputRaw[i].Replace("L", "-").Replace("R", "");
            var rotationVal = decimal.Parse(cleaned);
            startValue += rotationVal;
            var zwischenErgebnis = startValue % 100;
            if (zwischenErgebnis == 0) zeroCnt++;
            Console.WriteLine("Rot: " + rotationVal);
        }

        Console.WriteLine("Erg: " + zeroCnt);
    }


    public static void SolveIt_2ndPart()
    {
        var inputRaw = File.ReadAllLines(InputFile);
        var startValue = 50m;
        var zeroCnt = 0;
        for (int i = 0; i < inputRaw.Length; i++)
        {
            var cleaned = inputRaw[i].Replace("L", "-").Replace("R", "");
            var rotationVal = decimal.Parse(cleaned);
            if (rotationVal == -214)
            {
                var stopStr = "";
            }

            var vorzeichenwechsel = false;
            if (startValue < 0)
            {
                startValue += rotationVal;
                if (startValue >= 0) vorzeichenwechsel = true;
            }
            else if (startValue > 0)
            {
                startValue += rotationVal;
                if (startValue <= 0) vorzeichenwechsel = true;
            }
            else
            {
                startValue += rotationVal;
            }

            var zwischenErgebnis = vorzeichenwechsel ? 1 : 0;
            zwischenErgebnis += (int)Math.Floor(Math.Abs(startValue / 100));
            zeroCnt += Math.Abs(zwischenErgebnis);
            startValue %= 100;
            Console.WriteLine("Rot: " + rotationVal + " Stand: " + startValue + " Zwischnerg: " + zwischenErgebnis);
        }

        //Falsch: 7596
        Console.WriteLine("Erg: " + zeroCnt);
    }
}