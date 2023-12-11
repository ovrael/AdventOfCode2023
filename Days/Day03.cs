using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day03
    {
        public static void RunPart1()
        {
            string[] dataLines = FileManager.LoadTextLines("day3.txt");
            int result = 0;

            for (int y = 0; y < dataLines.Length; y++)
            {
                int value = 0;
                bool hasSymbolNearby = false;

                for (int x = 0; x < dataLines[y].Length; x++)
                {
                    char c = dataLines[y][x];
                    if (!char.IsDigit(c))
                    {
                        if (value > 0 && hasSymbolNearby)
                        {
                            result += value;
                            hasSymbolNearby = false;
                        }

                        value = 0;
                        continue;
                    }

                    value = value * 10 + (c - '0');

                    if (hasSymbolNearby)
                    {
                        continue;
                    }

                    // Check Up
                    if (y > 0)
                    {
                        // Center up
                        char up = dataLines[y - 1][x];
                        if (up != '.' && !char.IsDigit(up))
                        {
                            hasSymbolNearby = true;
                            continue;
                        }

                        // Left up
                        if (x > 0)
                        {
                            char leftUp = dataLines[y - 1][x - 1];
                            if (leftUp != '.' && !char.IsDigit(leftUp))
                            {
                                hasSymbolNearby = true;
                                continue;
                            }
                        }

                        // Right up
                        if (x < dataLines[y - 1].Length - 1)
                        {
                            char rightUp = dataLines[y - 1][x + 1];
                            if (rightUp != '.' && !char.IsDigit(rightUp))
                            {
                                hasSymbolNearby = true;
                                continue;
                            }
                        }
                    }

                    // Check current level
                    // Left current
                    if (x > 0)
                    {
                        char leftCurrent = dataLines[y][x - 1];
                        if (leftCurrent != '.' && !char.IsDigit(leftCurrent))
                        {
                            hasSymbolNearby = true;
                            continue;
                        }
                    }

                    // Right current
                    if (x < dataLines[y].Length - 1)
                    {
                        char rightCurrent = dataLines[y][x + 1];
                        if (rightCurrent != '.' && !char.IsDigit(rightCurrent))
                        {
                            hasSymbolNearby = true;
                            continue;
                        }
                    }

                    // Check bottom
                    if (y < dataLines.Length - 1)
                    {
                        // Center bottom
                        char bottom = dataLines[y + 1][x];
                        if (bottom != '.' && !char.IsDigit(bottom))
                        {
                            hasSymbolNearby = true;
                            continue;
                        }

                        // Left bottom
                        if (x > 0)
                        {
                            char leftBottom = dataLines[y + 1][x - 1];
                            if (leftBottom != '.' && !char.IsDigit(leftBottom))
                            {
                                hasSymbolNearby = true;
                                continue;
                            }
                        }

                        // Right bottom
                        if (x < dataLines[y + 1].Length - 1)
                        {
                            char rightBottom = dataLines[y + 1][x + 1];
                            if (rightBottom != '.' && !char.IsDigit(rightBottom))
                            {
                                hasSymbolNearby = true;
                                continue;
                            }
                        }
                    }
                }

                if (value > 0 && hasSymbolNearby)
                {
                    result += value;
                }
            }

            Console.WriteLine($"Result for Day 3 Part 1 is: {result}");
        }

        public static void RunPart2()
        {
            string[] dataLines = FileManager.LoadTextLines("day3.txt");
            int resultSum = 0;

            for (int y = 0; y < dataLines.Length; y++)
            {
                for (int x = 0; x < dataLines[y].Length; x++)
                {
                    char c = dataLines[y][x];

                    if (c != '*')
                    {
                        continue;
                    }

                    List<Position> adjustNumbers = new List<Position>();

                    // Check Up
                    if (y > 0)
                    {
                        // Left up
                        if (x > 0)
                        {
                            if (char.IsDigit(dataLines[y - 1][x - 1]))
                            {
                                adjustNumbers.Add(new Position(x - 1, y - 1));
                            }
                        }

                        // Center up
                        if (char.IsDigit(dataLines[y - 1][x]))
                        {
                            adjustNumbers.Add(new Position(x, y - 1));
                        }


                        // Right up
                        if (x < dataLines[y - 1].Length - 1)
                        {
                            if (char.IsDigit(dataLines[y - 1][x + 1]))
                            {
                                adjustNumbers.Add(new Position(x + 1, y - 1));
                            }
                        }
                    }

                    // Check current level
                    // Left current
                    if (x > 0)
                    {
                        if (char.IsDigit(dataLines[y][x - 1]))
                        {
                            adjustNumbers.Add(new Position(x - 1, y));
                        }
                    }

                    // Right current
                    if (x < dataLines[y].Length - 1)
                    {
                        if (char.IsDigit(dataLines[y][x + 1]))
                        {
                            adjustNumbers.Add(new Position(x + 1, y));
                        }
                    }

                    // Check bottom
                    if (y < dataLines.Length - 1)
                    {
                        // Left bottom
                        if (x > 0)
                        {
                            if (char.IsDigit(dataLines[y + 1][x - 1]))
                            {
                                adjustNumbers.Add(new Position(x - 1, y + 1));
                            }
                        }

                        // Center bottom
                        if (char.IsDigit(dataLines[y + 1][x]))
                        {
                            adjustNumbers.Add(new Position(x, y + 1));
                        }

                        // Right bottom
                        if (x < dataLines[y + 1].Length - 1)
                        {
                            if (char.IsDigit(dataLines[y + 1][x + 1]))
                            {
                                adjustNumbers.Add(new Position(x + 1, y + 1));
                            }
                        }
                    }


                    for (int i = 0; i < adjustNumbers.Count - 1; i++)
                    {
                        if (adjustNumbers[i].Y != adjustNumbers[i + 1].Y) continue;
                        if (adjustNumbers[i].X + 1 == adjustNumbers[i + 1].X)
                        {
                            adjustNumbers.RemoveAt(i);
                            i--;
                        }
                    }

                    if (adjustNumbers.Count != 2) continue;

                    int first = GetAdjustNumber(adjustNumbers[0], dataLines);
                    int second = GetAdjustNumber(adjustNumbers[1], dataLines);

                    resultSum += first * second;
                }
            }

            Console.WriteLine($"Result for Day 3 Part 2 is: {resultSum}");
        }

        private static int GetAdjustNumber(Position pos, string[] dataLines)
        {
            string adjustNumberText = new string(dataLines[pos.Y][pos.X], 1);
            bool endLeft = false;
            bool endRight = false;
            int offset = 0;
            while (!endLeft || !endRight)
            {
                offset++;
                if (pos.X - offset >= 0)
                {
                    if (!endLeft)
                    {

                        char left = dataLines[pos.Y][pos.X - offset];
                        if (char.IsDigit(left))
                        {
                            adjustNumberText = left + adjustNumberText;
                        }
                        else
                        {
                            endLeft = true;
                        }
                    }
                }
                else
                {
                    endLeft = true;
                }

                if (pos.X + offset < dataLines[pos.Y].Length)
                {
                    if (!endRight)
                    {

                        char right = dataLines[pos.Y][pos.X + offset];
                        if (char.IsDigit(right))
                        {
                            adjustNumberText = adjustNumberText + right;
                        }
                        else
                        {
                            endRight = true;
                        }
                    }
                }
                else
                {
                    endRight = true;
                }
            }

            if (int.TryParse(adjustNumberText, out int adjustNumber))
                return adjustNumber;
            else
            {
                Console.WriteLine($"Cannot convert: {adjustNumberText} to number!");
                return 0;
            }
        }
    }
}