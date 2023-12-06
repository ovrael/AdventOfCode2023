using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day6
    {
        // Time:        62     64     91     90
        // Distance:   553   1010   1473   1074
        public static void RunPart1()
        {
            string[] dataLines = FileManager.LoadTextLines("day6.txt");
            int[] times = dataLines[0]
                .Split(separator: ':')[1].Trim()    // Get times as text
                .Split(separator: ' ')              // Split times
                .Where(t => t.Length > 0)           // Only if splitted text exists
                .Select(int.Parse)                  // To int
                .ToArray();

            int[] distances = dataLines[1]
                .Split(separator: ':')[1].Trim()    // Get distances as text
                .Split(separator: ' ')              // Split distances
                .Where(t => t.Length > 0)           // Only if splitted text exists
                .Select(int.Parse)                  // To int
                .ToArray();

            if (times.Length != distances.Length)
            {
                Console.WriteLine($"Times length ({times.Length}) is different than distances length ({distances.Length})");
                return;
            }

            int[] possibilities = new int[distances.Length];
            int index = 0;

            foreach (var (time, distanceToBeat) in times.Zip(distances))
            {
                for (int i = 0; i < time / 2; i++)
                {
                    int distance = (time - i) * i;
                    if (distance > distanceToBeat)
                    {
                        possibilities[index] += 2;
                    }
                }

                int midDistance = time * time / 4;
                if (midDistance > distanceToBeat)
                {
                    possibilities[index] += 1 + time % 2;
                }

                index++;
            }

            int result = possibilities.Aggregate((v1, v2) => v1 * v2);
            Console.WriteLine($"Result for Day 6 Part 1 is: {result}");
        }

        // Time:        62     64     91     90
        // Distance:   553   1010   1473   1074
        public static void RunPart2()
        {
            string[] dataLines = FileManager.LoadTextLines("day6.txt");
            long time = long.Parse(
                dataLines[0]
                .Split(separator: ':')[1].Trim()         // Get times as text
                .Replace(oldValue: " ", string.Empty)    // Concat times into single value
            );

            long distanceToBeat = long.Parse(
                dataLines[1]
                .Split(separator: ':')[1].Trim()         // Get distances as text
                .Replace(oldValue: " ", string.Empty)    // Concat distances into single value
            );

            if (time == 0 || distanceToBeat == 0)
            {
                Console.WriteLine($"Time: {time} or distance: {distanceToBeat} is 0!");
                return;
            }

            long possibilities = 0;
            for (long i = 0; i < time / 2; i++)
            {
                long distance = (time - i) * i;
                if (distance > distanceToBeat)
                {
                    possibilities += 2;
                }
            }

            long midDistance = time * time / 4;
            if (midDistance > distanceToBeat)
            {
                possibilities += 1 + time % 2;
            }

            Console.WriteLine($"Result for Day 6 Part 2 is: {possibilities}");
        }
    }

}
