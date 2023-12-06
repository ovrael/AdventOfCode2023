using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day1
    {
        private static readonly Dictionary<int, string> textDigits = new Dictionary<int, string>()
        {
            {0, "zero" },
            {1, "one" },
            {2, "two" },
            {3, "three" },
            {4, "four" },
            {5, "five" },
            {6, "six" },
            {7, "seven" },
            {8, "eight" },
            {9, "nine" },
        };

        public static void Run()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day1.txt");

            foreach (var line in dataLines)
            {
                int leftDigit = -1;
                int rightDigit = -1;

                for (int i = 0; i < line.Length; i++)
                {
                    if (leftDigit == -1)
                    {
                        if (char.IsDigit(line[i]))
                            leftDigit = line[i] - '0';
                        else
                            leftDigit = TryGetTextDigit(line, i);
                    }

                    if (rightDigit == -1)
                    {
                        if (char.IsDigit(line[line.Length - i - 1]))
                            rightDigit = line[line.Length - i - 1] - '0';
                        else
                            rightDigit = TryGetTextDigit(line, line.Length - i - 1);
                    }

                    if (leftDigit >= 0 && rightDigit >= 0)
                        break;
                }

                result += 10 * leftDigit + rightDigit;
            }

            Console.WriteLine($"Result for Day 1 Part 2 is: {result}");
        }

        private static int TryGetTextDigit(string line, int i)
        {
            int digit = -1;


            string cutLine = line[i..];
            foreach (var textDigit in textDigits)
            {
                if (cutLine.Length < textDigit.Value.Length)
                    continue;

                if (cutLine.StartsWith(textDigit.Value))
                    return textDigit.Key;
            }


            return digit;
        }
    }
}