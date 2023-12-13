using AdventOfCode2023.Tools;
using System.Text.RegularExpressions;

namespace AdventOfCode2023.Days
{
    public class Day12
    {
        private static readonly Dictionary<string, long> cache = new Dictionary<string, long>();


        private class HotSpring
        {
            private readonly char[] springs = Array.Empty<char>();
            private readonly int[] damaged = Array.Empty<int>();


            public HotSpring(string dataLine)
            {
                string[] parts = dataLine.Split(' ');
                springs = parts[0].Trim().ToArray();
                damaged = parts[1].Trim().Split(',').Select(int.Parse).ToArray();
            }

            public int CountArrangements()
            {
                return CheckSolutions(springs, 0);
            }

            private int CheckSolutions(char[] fields, int start)
            {
                if (!fields.Contains('?'))
                {
                    return CheckArrangement(fields);
                }

                int result = 0;

                for (int i = start; i < fields.Length; i++)
                {
                    if (fields[i] == '?')
                    {
                        fields[i] = '.';
                        result += CheckSolutions(fields.ToArray(), i);
                        fields[i] = '#';
                        result += CheckSolutions(fields, i);
                    }
                }

                return result;
            }

            private int CheckArrangement(char[] fields)
            {
                string pattern = "\\.*";
                for (int i = 0; i < damaged.Length - 1; i++)
                {
                    pattern += $"#{{{damaged[i]}}}\\.+";
                }
                pattern += $"#{{{damaged[damaged.Length - 1]}}}\\.*";

                Regex regex = new Regex(pattern);
                Match match = regex.Match(new string(fields));

                if (!match.Success) return 0;

                return match.Length == fields.Length ? 1 : 0;
            }

            internal static HotSpring CreateUnfolded(string dataLine)
            {
                string[] parts = dataLine.Split(' ');

                string fields = string.Join('?', Enumerable.Repeat(parts[0].Trim(), 5));
                string damaged = string.Join(',', Enumerable.Repeat(parts[1].Trim(), 5));

                return new HotSpring(fields + " " + damaged);
            }

            public long CountArrangements2()
            {
                return Calculate(new string(springs), damaged.ToList());
            }

            private long Calculate(string springs, List<int> groups)
            {
                var key = $"{springs},{string.Join(',', groups)}";  // Cache key: spring pattern + group lengths

                if (cache.TryGetValue(key, out var value))
                {
                    return value;
                }

                value = GetCount(springs, groups);
                cache[key] = value;

                return value;
            }

            private long GetCount(string springs, List<int> groups)
            {
                while (true)
                {
                    if (groups.Count == 0)
                    {
                        return springs.Contains('#') ? 0 : 1; // No more groups to match: if there are no springs left, we have a match
                    }

                    if (string.IsNullOrEmpty(springs))
                    {
                        return 0; // No more springs to match, although we still have groups to match
                    }

                    if (springs.StartsWith('.'))
                    {
                        springs = springs.Trim('.'); // Remove all dots from the beginning
                        continue;
                    }

                    if (springs.StartsWith('?'))
                    {
                        return Calculate("." + springs[1..], groups) + Calculate("#" + springs[1..], groups); // Try both options recursively
                    }

                    if (springs.StartsWith('#')) // Start of a group
                    {
                        if (groups.Count == 0)
                        {
                            return 0; // No more groups to match, although we still have a spring in the input
                        }

                        if (springs.Length < groups[0])
                        {
                            return 0; // Not enough characters to match the group
                        }

                        if (springs[..groups[0]].Contains('.'))
                        {
                            return 0; // Group cannot contain dots for the given length
                        }

                        if (groups.Count > 1)
                        {
                            if (springs.Length < groups[0] + 1 || springs[groups[0]] == '#')
                            {
                                return 0; // Group cannot be followed by a spring, and there must be enough characters left
                            }

                            springs = springs[(groups[0] + 1)..]; // Skip the character after the group - it's either a dot or a question mark
                            groups = groups.GetRange(1, groups.Count - 1);
                            continue;
                        }

                        springs = springs[groups[0]..]; // Last group, no need to check the character after the group
                        groups = groups.GetRange(1, groups.Count - 1);
                        continue;
                    }
                }
            }
        }

        public static void RunPart1()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day12.txt");
            List<HotSpring> hotSprings = dataLines.Select(d => new HotSpring(d)).ToList();
            result = hotSprings.Select(h => h.CountArrangements()).Sum();

            Console.WriteLine($"Result for day 12 part 1 is {result}");
        }

        public static void RunPart2()
        {
            string[] dataLines = FileManager.LoadTextLines("day12.txt");
            List<HotSpring> hotSprings = dataLines.Select(HotSpring.CreateUnfolded).ToList();
            long result = hotSprings.Select(h => h.CountArrangements2()).Sum();

            Console.WriteLine($"Result for day 12 part 2 is {result}");
        }
    }
}
