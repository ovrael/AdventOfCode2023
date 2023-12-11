namespace AdventOfCode2023.Days
{
    public static class Day04
    {
        public static void RunPart1()
        {
            string[] dataLines = Tools.FileManager.LoadTextLines("day4.txt");
            int pointsSum = 0;

            foreach (var line in dataLines)
            {
                string numbers = line.Split(':')[1];
                string[] numbersParts = numbers.Split('|');

                int[] winningNumbers = numbersParts[0].Trim(' ').Split(' ').Where(t => t.Length > 0).Select(ParseToInt).ToArray();
                int[] scratchNumbers = numbersParts[1].Trim(' ').Split(' ').Where(t => t.Length > 0).Select(ParseToInt).ToArray();

                int winNumbers = 0;
                foreach (var scratchNumber in scratchNumbers)
                {
                    if (winningNumbers.Contains(scratchNumber))
                        winNumbers++;
                }

                pointsSum += CountPoints(winNumbers);
            }

            Console.WriteLine($"Result for Day 4 Part 1 is: {pointsSum}");
        }


        public static void RunPart2()
        {
            string[] dataLines = Tools.FileManager.LoadTextLines("day4.txt");
            int pointsSum = 0;

            int[] resultCards = new int[dataLines.Length];
            //int[] winCards = new int[dataLines.Length];

            for (int i = 0; i < dataLines.Length; i++)
            {
                string numbers = dataLines[i].Split(':')[1];
                string[] numbersParts = numbers.Split('|');

                int[] winningNumbers = numbersParts[0].Trim(' ').Split(' ').Where(t => t.Length > 0).Select(ParseToInt).ToArray();
                int[] scratchNumbers = numbersParts[1].Trim(' ').Split(' ').Where(t => t.Length > 0).Select(ParseToInt).ToArray();

                int winNumbers = 0;
                foreach (var scratchNumber in scratchNumbers)
                {
                    if (winningNumbers.Contains(scratchNumber))
                        winNumbers++;
                }

                // There is always 1 instance
                resultCards[i]++;

                if (winNumbers == 0)
                    continue;

                // Count winnings
                for (int j = 0; j < winNumbers; j++)
                {
                    resultCards[j + i + 1] += resultCards[i];
                }

            }


            Console.WriteLine($"Result for Day 4 Part 2 is: {resultCards.Sum()}");
        }


        private static int ParseToInt(string text)
        {
            if (int.TryParse(text, out int result))
            {
                return result;
            }
            else
            {
                Console.WriteLine($"Cant convert:{text}, into number!");
                return 0;
            }
        }

        private static int CountPoints(int winNumbers)
        {
            if (winNumbers == 0) return 0;
            if (winNumbers == 1) return 1;
            int points = 1;
            for (int i = 1; i < winNumbers; i++)
            {
                points *= 2;
            }
            return points;
        }
    }
}