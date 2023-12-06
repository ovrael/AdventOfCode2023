using System.Text;

namespace AdventOfCode2023.Days
{
    public static class Day5
    {
        private static class Almanac
        {
            private static long[] seeds = Array.Empty<long>();
            private static readonly List<MapContainer> mapContainers = new List<MapContainer>();

            public static void LoadSeeds(string seedsLine)
            {
                seeds = seedsLine
                    .Split(':')[1].Trim(' ') // Get only numbers from "seeds: 12 43 41 41 ..."
                    .Split(' ') // Split them: 12,43,41,41
                    .Where(s => s.Length > 0) // Sometimes single digits are prepand with space so will have additional empty "numbers"
                    .Select(s => long.Parse(s.Trim())).ToArray(); // Trim and parse to int
            }

            /// <summary>
            /// BRUTE SEARCH :(
            /// </summary>
            /// <param name="seedsLine"></param>
            /// <returns></returns>
            public static long GetMinLocationForPart2(string seedsLine)
            {
                //56931769
                long minLocation = 56931769;

                long[] readSeeds = seedsLine
                    .Split(':')[1].Trim(' ') // Get only numbers from "seeds: 12 43 41 41 ..."
                    .Split(' ') // Split them: 12,43,41,41
                    .Where(s => s.Length > 0) // Sometimes single digits are prepand with space so will have additional empty "numbers"
                    .Select(s => long.Parse(s.Trim())).ToArray(); // Trim and parse to long

                List<long> seedsRange = new List<long>();
                int batchSize = 1000000;
                int cursor = 2;
                for (int i = 18; i < readSeeds.Length; i += 2)
                {
                    Console.SetCursorPosition(0, 0);
                    Console.Write($"Status: {i / 2 + 1}/{readSeeds.Length / 2}");
                    // For every pair
                    // Big numbers - time for batching
                    int currentBatch = 0;
                    long batchesCount = readSeeds[i + 1] / batchSize + 1;

                    for (long j = 0; j < readSeeds[i + 1]; j++)
                    {
                        seedsRange.Add(readSeeds[i] + j);

                        if (j % batchSize == 0 || j == readSeeds[i + 1] - 1)
                        {
                            Console.SetCursorPosition(0, 1);
                            Console.Write($"Batch: {currentBatch}/{batchesCount}");
                            Console.SetCursorPosition(0, cursor);

                            currentBatch++;
                            seeds = seedsRange.ToArray();
                            long minBatchLocation = GetLocationsForSeeds().Min();

                            if (minBatchLocation < minLocation)
                            {
                                Console.SetCursorPosition(0, cursor);
                                Console.Write($"Min location: {minLocation} -> {minBatchLocation}");
                                cursor++;
                                Console.SetCursorPosition(0, cursor);

                                minLocation = minBatchLocation;
                            }

                            seedsRange.Clear();
                        }
                    }
                }

                return minLocation;
            }

            public static void CreateMapContainer(string mapsText)
            {
                string[] mapsLines = mapsText.Trim().Split('\n');
                mapContainers.Add(new MapContainer(mapsLines));
            }

            public static long[] GetLocationsForSeeds()
            {
                long[] locations = new long[seeds.Length];

                //for (int i = 0; i < 1; i++)
                for (int i = 0; i < seeds.Length; i++)
                {
                    long source = seeds[i];
                    //Console.WriteLine($"\nSeed: {seeds[i]}");
                    for (int j = 0; j < mapContainers.Count; j++)
                    {
                        long destination = mapContainers[j].GetDestination(source);
                        //Console.WriteLine($"{mapContainers[j].Header} {source} -> {destination}");
                        source = destination;
                    }
                    locations[i] = source;
                    //Console.WriteLine($"Location: {locations[i]}");
                }

                return locations;
            }
        }

        private class MapContainer
        {
            private readonly List<Map> maps = new List<Map>();
            public readonly string Header = string.Empty;

            public MapContainer(string[] mapsLines)
            {
                Header = mapsLines[0].Trim();
                mapsLines = mapsLines[1..];

                foreach (var mapData in mapsLines)
                {
                    long[] numbers = mapData
                        .Trim(' ').Split(' ')
                        .Where(s => s.Length > 0)
                        .Select(v => long.Parse(v.Trim(' '))).ToArray();

                    if (numbers.Length == 3)
                    {
                        maps.Add(
                            new Map(numbers[0], numbers[1], numbers[2])
                            );
                    }
                    else
                    {
                        Console.WriteLine($"Something went wrong when reading map! Numbers length: {numbers.Length}");
                        for (int i = 0; i < numbers.Length; i++)
                        {
                            Console.WriteLine($"Number at {i} is {numbers[i]}");
                        }
                    }
                }
            }

            // Any source numbers that aren't mapped correspond to the same destination number.
            // So, seed number 10 corresponds to soil number 10.
            public long GetDestination(long source)
            {
                for (int i = 0; i < maps.Count; i++)
                {
                    long result = maps[i].CheckMap(source);
                    if (result >= 0)
                        return result;
                }
                return source;
            }
        }

        private class Map
        {
            private readonly long destination;
            private readonly long source;
            private readonly long length;

            public Map(long destination, long source, long length)
            {
                this.destination = destination;
                this.source = source;
                this.length = length;
            }

            public long CheckMap(long input)
            {
                if (input < source || input >= source + length)
                {
                    return -1;
                }
                return input + destination - source;
            }
        }

        public static void RunPart1()
        {
            string[] dataLines = Tools.FileManager.LoadTextLines("day5.txt");
            Almanac.LoadSeeds(dataLines[0]);

            StringBuilder mapContainerBuilder = new StringBuilder();
            for (int i = 1; i < dataLines.Length; i++)
            {
                if ((dataLines[i].Length == 0 || i == dataLines.Length - 1) && mapContainerBuilder.Length > 0)
                {
                    Almanac.CreateMapContainer(mapContainerBuilder.ToString());
                    mapContainerBuilder = new StringBuilder();
                    continue;
                }

                mapContainerBuilder.AppendLine(dataLines[i]);
            }

            long[] locations = Almanac.GetLocationsForSeeds();
            Console.WriteLine($"Day 5 Part 1 answer is: {locations.Min()}");
        }

        public static void RunPart2()
        {
            string[] dataLines = Tools.FileManager.LoadTextLines("day5.txt");

            StringBuilder mapContainerBuilder = new StringBuilder();
            for (int i = 1; i < dataLines.Length; i++)
            {
                if ((dataLines[i].Length == 0 || i == dataLines.Length - 1) && mapContainerBuilder.Length > 0)
                {
                    Almanac.CreateMapContainer(mapContainerBuilder.ToString());
                    mapContainerBuilder = new StringBuilder();
                    continue;
                }

                mapContainerBuilder.AppendLine(dataLines[i]);
            }

            long minLocation = Almanac.GetMinLocationForPart2(dataLines[0]);
            Console.WriteLine($"Day 5 Part 2 answer is: {minLocation}");
        }
    }
}
