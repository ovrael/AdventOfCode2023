using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day13
    {

        private static bool CompareColumns(List<string> map, int col1, int col2)
        {
            for (int y = 0; y < map.Count; y++)
            {
                if (map[y][col1] != map[y][col2]) return false;
            }
            return true;
        }
        private static int[] CompareColumns2(List<string> map, int col1, int col2)
        {
            List<int> result = new List<int>();
            for (int y = 0; y < map.Count; y++)
            {
                if (map[y][col1] != map[y][col2]) result.Add(y);
            }
            return result.ToArray();
        }

        private static int[] CompareRows2(List<string> map, int row1, int row2)
        {
            List<int> result = new List<int>();
            for (int x = 0; x < map[0].Length; x++)
            {
                if (map[row1][x] != map[row2][x]) result.Add(x);
            }
            return result.ToArray();
        }

        private static bool SearchVertical(List<string> map, int col)
        {
            int max = col < map[0].Length / 2 ? col + 1 : map[0].Length - col - 1;

            for (int x = 0; x < max; x++)
            {
                if (!CompareColumns(map, col - x, col + x + 1))
                {
                    return false;
                }
            }

            return true;
        }

        private static bool SearchHorizontal(List<string> map, int row)
        {
            int max = row < map.Count / 2 ? row + 1 : map.Count - row - 1;

            for (int y = 0; y < max; y++)
            {
                if (map[row - y] != map[row + y + 1])
                {
                    return false;
                }
            }

            return true;
        }

        private static int FindReflections(List<string> map)
        {
            for (int x = 0; x < map[0].Length - 1; x++)
                if (SearchVertical(map, x))
                    return x + 1;

            for (int y = 0; y < map.Count - 1; y++)
                if (SearchHorizontal(map, y))
                    return 100 * (y + 1);

            return 0;
        }

        private static int SearchVertical2(List<string> map, int col)
        {
            int max = col < map[0].Length / 2 ? col + 1 : map[0].Length - col - 1;

            List<int> rows = new List<int>();
            for (int x = 0; x < max; x++)
                rows.AddRange(CompareColumns2(map, col - x, col + x + 1));

            if (rows.Count == 1) return rows[0];
            return -1;
        }

        private static int SearchHorizontal2(List<string> map, int row)
        {
            int max = row < map.Count / 2 ? row + 1 : map.Count - row - 1;

            List<int> cols = new List<int>();
            for (int y = 0; y < max; y++)
                cols.AddRange(CompareRows2(map, row - y, row + y + 1));

            if (cols.Count == 1) return cols[0];
            return -1;
        }

        private static int FindSmudges(List<string> map)
        {
            for (int x = 0; x < map[0].Length - 1; x++)
                if (SearchVertical2(map, x) >= 0)
                    return x + 1;


            for (int y = 0; y < map.Count - 1; y++)
                if (SearchHorizontal2(map, y) >= 0)
                    return 100 * (y + 1);

            return 0;
        }

        public static void RunPart1()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day13.txt");

            List<string> map = new List<string>();
            for (int i = 0; i < dataLines.Length; i++)
            {
                if (dataLines[i].Length == 0 || i == dataLines.Length - 1)
                {
                    result += FindReflections(map);
                    map.Clear();
                }
                else
                {
                    map.Add(dataLines[i]);
                }
            }

            Console.WriteLine($"Result for day 13 part 1 is {result}");
        }

        public static void RunPart2()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day13.txt");
            List<string> map = new List<string>();

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < dataLines.Length; i++)
            {
                if (dataLines[i].Length == 0 || i == dataLines.Length - 1)
                {
                    result += FindSmudges(map);
                    map.Clear();
                }
                else
                {
                    map.Add(dataLines[i]);
                }
            }
            // the code that you want to measure comes here
            watch.Stop();
            var elapsedMs = watch.ElapsedMilliseconds;
            Console.WriteLine($"Result for day 13 part 2 is {result} in {elapsedMs}ms");
        }
    }
}
