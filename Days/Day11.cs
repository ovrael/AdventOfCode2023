using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day11
    {
        private class Galaxy
        {
            public int X { get; set; }
            public int Y { get; set; }

            public int ComputeDistanceTo(Galaxy other)
            {
                return Math.Abs(other.X - X) + Math.Abs(other.Y - Y);
            }

            public long ComputeDistanceTo(Galaxy other, int expanseScalar, List<List<char>> map)
            {
                //Console.WriteLine($"Compute distance from ({X},{Y}) to ({other.X},{other.Y})");
                (int Start, int End) col = X < other.X ? (X, other.X) : (other.X, X);
                (int Start, int End) row = Y < other.Y ? (Y, other.Y) : (other.Y, Y);

                int distance = Y == other.Y ? 0 : 1; // Corner

                for (int y = row.Start + 1; y < row.End; y++)
                {
                    if (map[y].Contains('#'))
                    {
                        //Console.WriteLine($"Row {y} has galaxy +1");
                        distance++;
                    }
                    else
                    {
                        //Console.WriteLine($"Row {y} is empty +{expanseScalar}");
                        distance += expanseScalar;
                    }
                }

                for (int x = col.Start + 1; x <= col.End; x++)
                {
                    if (ColumnContainsGalaxy(map, x))
                    {
                        //Console.WriteLine($"Col {x} has galaxy +1");
                        distance++;
                    }
                    else
                    {
                        //Console.WriteLine($"Col {x} is empty +{expanseScalar}");
                        distance += expanseScalar;
                    }
                }

                //Console.WriteLine($"Distance from ({X},{Y}) to ({other.X},{other.Y}) equals {distance}");

                return distance;
            }
        }

        private static bool ColumnContainsGalaxy(List<List<char>> map, int col)
        {
            for (int i = 0; i < map.Count; i++)
            {
                if (map[i][col] == '#')
                    return true;
            }
            return false;
        }

        private static void PrintMap(List<List<char>> map)
        {
            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    Console.Write(map[y][x] + " ");
                }
                Console.WriteLine();
            }
        }

        public static void RunPart1()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day11.txt");

            List<List<char>> map = new List<List<char>>();
            foreach (string line in dataLines)
            {
                map.Add(line.ToList());
            }

            //Console.WriteLine("Before");
            //PrintMap(map);

            for (int i = 0; i < map[0].Count; i++)
            {
                if (ColumnContainsGalaxy(map, i))
                {
                    continue;
                }

                for (int y = 0; y < map.Count; y++)
                {
                    map[y].Insert(i, '.');
                }
                i++;
            }

            for (int i = 0; i < map.Count; i++)
            {
                if (map[i].Contains('#'))
                {
                    continue;
                }
                map.Insert(i, map[i]);
                i++;
            }

            //Console.WriteLine("\nAfter");
            //PrintMap(map);

            List<Galaxy> galaxies = new List<Galaxy>();

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == '#')
                    {
                        galaxies.Add(new Galaxy() { X = x, Y = y });
                    }
                }
            }


            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    result += galaxies[i].ComputeDistanceTo(galaxies[j]);
                }
            }

            Console.WriteLine($"Result for Day 11 Part 1 is: {result}");
        }

        public static void RunPart2()
        {
            long result = 0;
            string[] dataLines = FileManager.LoadTextLines("day11.txt");

            List<List<char>> map = new List<List<char>>();
            foreach (string line in dataLines)
            {
                map.Add(line.ToList());
            }

            int expanseScalar = 1000000;

            List<Galaxy> galaxies = new List<Galaxy>();

            for (int y = 0; y < map.Count; y++)
            {
                for (int x = 0; x < map[y].Count; x++)
                {
                    if (map[y][x] == '#')
                    {
                        galaxies.Add(new Galaxy() { X = x, Y = y });
                    }
                }
            }
            //Console.WriteLine("Before");
            //PrintMap(map);
            //galaxies[4].ComputeDistanceTo(galaxies[8], expanseScalar, map);

            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    //Console.WriteLine($"Distance from {i} to {j}");
                    result += galaxies[i].ComputeDistanceTo(galaxies[j], expanseScalar, map);
                }
            }

            Console.WriteLine($"Result for Day 11 Part 1 is: {result}");
        }
    }
}
