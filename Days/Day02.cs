using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day02
    {
        private static readonly Dictionary<string, int> maxCubes = new Dictionary<string, int>()
        {
            {"red", 12 },
            {"green", 13 },
            {"blue", 14 }
        };

        public static void RunPart1()
        {
            string[] dataLines = FileManager.LoadTextLines("day2.txt");
            int gameIdSum = 0;

            foreach (var line in dataLines)
            {
                bool goodGame = true;
                string[] gameParts = line.Split(':');
                string[] reveals = gameParts[1].Split(';');

                foreach (var reveal in reveals)
                {
                    Dictionary<string, int> cubes = new Dictionary<string, int>();

                    string[] cubesCounts = reveal.Split(',');

                    foreach (var cubesCount in cubesCounts)
                    {
                        string[] cubesCountParts = cubesCount.Trim().Split(' ');
                        int.TryParse(cubesCountParts[0], out int colorCount);
                        cubes.Add(cubesCountParts[1], colorCount);
                    }

                    foreach (var color in cubes.Keys)
                    {
                        if (cubes[color] > maxCubes[color])
                        {
                            goodGame = false;
                            break;
                        }
                    }

                    if (!goodGame)
                    {
                        break;
                    }
                }

                if (goodGame)
                {
                    int.TryParse(gameParts[0].Split(' ')[1], out int gameId);
                    gameIdSum += gameId;
                }
            }

            Console.WriteLine($"Result for Day 2 is: {gameIdSum}");
        }

        public static void RunPart2()
        {
            string[] dataLines = FileManager.LoadTextLines("day2.txt");
            int resultSum = 0;

            foreach (var line in dataLines)
            {
                string[] gameParts = line.Split(':');
                string[] reveals = gameParts[1].Split(';');

                Dictionary<string, int> minCubes = new Dictionary<string, int>()
                {
                    {"red", 0 },
                    {"green", 0 },
                    {"blue", 0 }
                };

                foreach (var reveal in reveals)
                {
                    Dictionary<string, int> cubes = new Dictionary<string, int>();

                    string[] cubesCounts = reveal.Split(',');

                    foreach (var cubesCount in cubesCounts)
                    {
                        string[] cubesCountParts = cubesCount.Trim().Split(' ');
                        int.TryParse(cubesCountParts[0], out int colorCount);
                        cubes.Add(cubesCountParts[1], colorCount);
                    }

                    foreach (var color in cubes.Keys)
                    {
                        if (cubes[color] > minCubes[color])
                        {
                            minCubes[color] = cubes[color];
                        }
                    }
                }

                resultSum += minCubes["red"] * minCubes["green"] * minCubes["blue"];
            }

            Console.WriteLine($"Result for Day 2 Part 2 is: {resultSum}");
        }
    }
}