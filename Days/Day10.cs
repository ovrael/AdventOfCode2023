using AdventOfCode2023.Tools;

namespace AdventOfCode2023.Days
{
    public static class Day10
    {
        private enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        private record Connection
        {
            public Direction Previous { get; set; }
            public char CurrentTile { get; set; }

            public Connection(Direction previous, char currentTile)
            {
                Previous = previous;
                CurrentTile = currentTile;
            }
        }

        private class Point
        {
            public int X { get; set; }
            public int Y { get; set; }
            public Direction NextDirection { get; set; }
            public char Pipe { get; set; }

            public bool Compare(Point other)
            {
                //Console.WriteLine($"Compare: THIS::({X}, {Y}) [{Pipe}]  OTHER::({other.X}, {other.Y}) [{other.Pipe}]");
                return other.X == X && other.Y == Y;
            }
        }

        private class Path
        {
            public List<Point> Points;

            public Path(Point first)
            {
                Points = new List<Point>() { first };
            }

            public bool FindNext(string[] dataLines)
            {
                Point last = Points[^1];
                (int X, int Y) nextPos = (last.X, last.Y);
                Direction from = last.NextDirection;

                switch (last.NextDirection)
                {
                    case Direction.Up:
                        if (last.Y <= 0) return false;
                        nextPos.Y--;
                        from = Direction.Down;
                        break;

                    case Direction.Down:
                        if (last.Y >= dataLines.Length - 1) return false;
                        nextPos.Y++;
                        from = Direction.Up;
                        break;

                    case Direction.Left:
                        if (last.X <= 0) return false;
                        nextPos.X--;
                        from = Direction.Right;
                        break;

                    case Direction.Right:
                        if (last.X >= dataLines[last.Y].Length - 1) return false;
                        nextPos.X++;
                        from = Direction.Left;
                        break;

                    default:
                        return false;
                }

                Point? next = GetNext(nextPos.X, nextPos.Y, from, dataLines);
                if (next == null) return false;
                Points.Add(next);
                return true;
            }

            public void Print()
            {
                foreach (Point p in Points)
                {
                    Console.Write(p.Pipe + " ");
                }
                Console.WriteLine();
            }

            public bool Contains(int x, int y)
            {
                return Points.Any(p => p.X == x && p.Y == y);
            }

            public int CountLeftBorders(int x, int y, string[] dataLines)
            {
                int collisions = 0;
                for (int i = 0; i < x; i++)
                {
                    if (Contains(i, y))
                    {
                        char c = dataLines[y][i];
                        if (c != '-' && c != 'F' && c != '7') collisions++;
                    }
                }

                return collisions;
            }
        }

        private static (int X, int Y) FindStart(string[] dataLines)
        {
            for (int y = 0; y < dataLines.Length; y++)
            {
                for (int x = 0; x < dataLines[y].Length; x++)
                {
                    if (dataLines[y][x] == 'S') return (x, y);
                }
            }
            return (-1, -1);
        }

        private static Point? GetNext(int x, int y, Direction previousDirection, string[] dataLines)
        {
            char next = dataLines[y][x];
            if (directionMap.ContainsKey(new Connection(previousDirection, next)))
            {
                return new Point()
                {
                    Y = y,
                    X = x,
                    NextDirection = directionMap[new Connection(previousDirection, next)],
                    Pipe = dataLines[y][x]
                };
            }
            return null;
        }


        private static readonly Dictionary<Connection, Direction> directionMap = new Dictionary<Connection, Direction>()
        {
            {new Connection(Direction.Up, 'J'), Direction.Left },
            {new Connection(Direction.Up, 'L'), Direction.Right },
            {new Connection(Direction.Up, '|'), Direction.Down },

            {new Connection(Direction.Down, '7'), Direction.Left },
            {new Connection(Direction.Down, 'F'), Direction.Right },
            {new Connection(Direction.Down, '|'), Direction.Up },

            {new Connection(Direction.Left, 'J'), Direction.Up },
            {new Connection(Direction.Left, '7'), Direction.Down },
            {new Connection(Direction.Left, '-'), Direction.Right },

            {new Connection(Direction.Right, 'L'), Direction.Up },
            {new Connection(Direction.Right, 'F'), Direction.Down },
            {new Connection(Direction.Right, '-'), Direction.Left },
        };

        private static Path? bestPath = null;

        public static void RunPart1()
        {
            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day10.txt");
            var start = FindStart(dataLines);
            if (start.X == -1 || start.Y == -1)
            {
                Console.WriteLine($"Something is wrong with data! Start position = ({start.X}, {start.Y})");
                return;
            }


            List<Path> paths = new List<Path>();

            // Check left to start
            if (start.X > 0)
            {
                Point? leftStart = GetNext(start.X - 1, start.Y, Direction.Right, dataLines);
                if (leftStart != null) paths.Add(new Path(leftStart));
            }

            // Check right to start
            if (start.X < dataLines[start.Y].Length - 1)
            {
                Point? rightStart = GetNext(start.X + 1, start.Y, Direction.Left, dataLines);
                if (rightStart != null) paths.Add(new Path(rightStart));
            }

            // Check up to start
            if (start.Y > 0)
            {
                Point? upStart = GetNext(start.X, start.Y - 1, Direction.Down, dataLines);
                if (upStart != null) paths.Add(new Path(upStart));
            }

            // Check down to start
            if (start.Y < dataLines.Length - 1)
            {
                Point? downStart = GetNext(start.X, start.Y + 1, Direction.Up, dataLines);
                if (downStart != null) paths.Add(new Path(downStart));
            }


            Console.WriteLine("Paths: " + paths.Count);

            int pathIndex = -1;
            while (pathIndex == -1)
            {
                if (paths.Count == 0) break;

                for (int i = 0; i < paths.Count; i++)
                {
                    if (!paths[i].FindNext(dataLines))
                    {
                        paths.RemoveAt(i);
                        continue;
                    }

                    for (int j = 0; j < paths.Count; j++)
                    {
                        if (j == i) continue;

                        if (paths[j].Points[^1].Compare(paths[i].Points[^1]))
                        {
                            pathIndex = i;
                            break;
                        }
                    }
                }
            }

            result = paths[pathIndex].Points.Count;

            Console.WriteLine($"Result for Day 10 Part 1 is: {result}");

            bestPath = paths[pathIndex];
        }

        private static void PrintData(string[] dataLines, Path path)
        {
            for (int y = 0; y < dataLines.Length; y++)
            {
                Console.Write(y + "  ");

                for (int x = 0; x < dataLines[y].Length; x++)
                {
                    if (x >= 10)
                        Console.Write(" ");

                    if (dataLines[y][x] == 'X')
                    {
                        Console.BackgroundColor = ConsoleColor.Red;
                    }
                    else if (path.Contains(x, y))
                    {
                        Console.BackgroundColor = ConsoleColor.DarkBlue;
                    }
                    else
                    {
                        Console.BackgroundColor = ConsoleColor.Black;
                    }
                    Console.Write(dataLines[y][x]);

                    Console.BackgroundColor = ConsoleColor.Black;
                    Console.Write("  ");
                }
                Console.WriteLine();
            }
            Console.Write("   ");
            for (int x = 0; x < dataLines[0].Length; x++)
            {
                Console.Write(x + "  ");
            }
            Console.WriteLine();
        }

        public static void RunPart2()
        {
            RunPart1();
            if (bestPath == null)
            {
                Console.WriteLine("Cannot find best path from part 1 of task");
                return;
            }
            Console.WriteLine("Day 10 part 2 start");

            int result = 0;
            string[] dataLines = FileManager.LoadTextLines("day10.txt");
            Path path = new Path(bestPath.Points[0]);

            var start = FindStart(dataLines);

            path.Points = path.Points.Prepend(new Point()
            {
                X = start.X,
                Y = start.Y,
                Pipe = dataLines[start.Y][start.X]
            }).ToList();


            while (path.FindNext(dataLines)) ;

            //PrintData(dataLines, path);

            for (int y = 1; y < dataLines.Length - 1; y++)
            {
                for (int x = 1; x < dataLines[y].Length - 1; x++)
                {
                    if (path.Contains(x, y)) continue;
                    int leftBorders = path.CountLeftBorders(x, y, dataLines);

                    if (leftBorders == 0) continue;

                    if (leftBorders % 2 == 1)
                    {
                        dataLines[y] = dataLines[y][..x] + "X" + dataLines[y][(x + 1)..];
                        result++;
                        //Console.WriteLine($"Add ({x},{y}) tile: {dataLines[y][x]}");
                        continue;
                    }
                }
            }
            //PrintData(dataLines, path);


            Console.WriteLine($"Result for Day 10 Part 2 is: {result}");
        }
    }
}
