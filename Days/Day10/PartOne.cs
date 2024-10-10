using System.ComponentModel;

namespace Days.Day10
{
    public static class PartOne
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] map)
        {
            int sx = 0;
            int sy = 0;

            // find the starting point
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        sy = i;
                        sx = j;
                        break;
                    }
                }
            }

            int x = sx;
            int y = sy;
            int stepCounter = 0;
            Direction? direction = null;
            do
            {
                direction = GetNextDirection(map, x, y, direction);
                var t = CoordinateChange[direction ?? Direction.East]; // east is arbitrary, will never be null
                x += t.x;
                y += t.y;
                stepCounter++;
            }
            while (x != sx || y != sy);

            return stepCounter / 2;
        }

        private static Direction GetNextDirection(string[] map, int x, int y, Direction? previousDirection = null)
        {
            foreach (var direction in Enum.GetValues<Direction>())
            {
                var t = CoordinateChange[direction];
                if (previousDirection != ReverseDirections[direction] &&
                    GetCanExitPipe(direction, map[y][x]) &&
                    GetCanEnterPipe(direction, map[y + t.y][x + t.x]))
                {
                    return direction;
                }
            }
            throw new Exception("Dead End");
        }

        private static bool GetCanExitPipe(Direction direction, char pipe)
        {
            return Pipes[pipe].Contains(direction);
        }

        private static bool GetCanEnterPipe(Direction direction, char pipe)
        {
            return Pipes[pipe].Contains(ReverseDirections[direction]);
        }


        private static readonly Dictionary<char, HashSet<Direction>> Pipes = new()
        {
            { '|', new() { Direction.North, Direction.South } },
            { '-', new() { Direction.East, Direction.West } },
            { 'L', new() { Direction.North, Direction.East } },
            { 'J', new() { Direction.North, Direction.West } },
            { '7', new() { Direction.South, Direction.West } },
            { 'F', new() { Direction.East, Direction.South } },
            { '.', [] },
            { 'S', new() { Direction.North, Direction.East, Direction.South, Direction.West } },
        };

        private static readonly Dictionary<Direction, Direction> ReverseDirections = new()
        {
            { Direction.North, Direction.South },
            { Direction.South, Direction.North },
            { Direction.East, Direction.West },
            { Direction.West, Direction.East },
        };

        private static readonly Dictionary<Direction, (int x, int y)> CoordinateChange = new()
        {
            { Direction.North, (0, -1) },
            { Direction.East, (1, 0) },
            { Direction.South, (0, 1) },
            { Direction.West, (-1, 0) },
        };


        internal enum Direction
        {
            North, East, South, West,
        }
    }
}
