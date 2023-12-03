using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day3
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

//---Day 3: Gear Ratios ---
//You and the Elf eventually reach a gondola lift station; he says the gondola lift will take you up to the water source, but this is as far as he can bring you. You go inside.

//It doesn't take long to find the gondolas, but there seems to be a problem: they're not moving.

//"Aaah!"

//You turn around to see a slightly-greasy Elf with a wrench and a look of surprise. "Sorry, I wasn't expecting anyone! The gondola lift isn't working right now; it'll still be a while before I can fix it." You offer to help.

//The engineer explains that an engine part seems to be missing from the engine, but nobody can figure out which one. If you can add up all the part numbers in the engine schematic, it should be easy to work out which part is missing.

//The engine schematic (your puzzle input) consists of a visual representation of the engine. There are lots of numbers and symbols you don't really understand, but apparently any number adjacent to a symbol, even diagonally, is a "part number" and should be included in your sum. (Periods (.) do not count as a symbol.)

//Here is an example engine schematic:

//467..114.....*........35..633.
//          ......#...
//617 * ...........+.58.
//            ..592.....
//......755.
//...$.*.....664.598..In this schematic, two numbers are not part numbers because they are not adjacent to a symbol: 114(top right) and 58 (middle right). Every other number is adjacent to a symbol and so is a part number; their sum is 4361.

//Of course, the actual engine schematic is much larger. What is the sum of all of the part numbers in the engine schematic?

//---Part Two-- -
//The engineer finds the missing part and installs it in the engine! As the engine springs to life, you jump in the closest gondola, finally ready to ascend to the water source.

//You don't seem to be going very fast, though. Maybe something is still wrong? Fortunately, the gondola has a phone labeled "help", so you pick it up and the engineer answers.

//Before you can explain the situation, she suggests that you look out the window. There stands the engineer, holding a phone in one hand and waving with the other. You're going so slowly that you haven't even left the station. You exit the gondola.

//The missing part wasn't the only issue - one of the gears in the engine is wrong. A gear is any * symbol that is adjacent to exactly two part numbers. Its gear ratio is the result of multiplying those two numbers together.

//This time, you need to find the gear ratio of every gear and add them all up so that the engineer can figure out which gear needs to be replaced.

//Consider the same engine schematic again:

//467..114.....*........35..633.
//          ......#...
//617 * ...........+.58.
//            ..592.....
//......755.
//...$.*.....664.598..In this schematic, there are two gears. The first is in the top left; it has part numbers 467 and 35, so its gear ratio is 16345. The second gear is in the lower right; its gear ratio is 451490. (The * adjacent to 617 is not a gear because it is only adjacent to one part number.) Adding up all of the gear ratios produces 467835.

//What is the sum of all of the gear ratios in your engine schematic?