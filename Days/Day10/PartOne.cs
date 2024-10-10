using static Days.Day10.Shared;

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
                direction.DoStep(ref x, ref y);
                stepCounter++;
            }
            while (x != sx || y != sy);

            return stepCounter / 2;
        }

        private static Direction GetNextDirection(string[] map, int x, int y, Direction? previousDirection = null)
        {
            foreach (Direction direction in Direction.Cardinals)
            {
                (int tx, int ty) = direction.GetStep(x, y);
                if (ty < 0 || map.Length <= ty ||
                    tx < 0 || map[ty].Length <= tx)
                {
                    continue;
                }
                if (!direction.GetReverse().Equals(previousDirection) &&
                    Pipe.GetCanExitPipe(direction, Pipe.GetPipe(map[y][x])) &&
                    Pipe.GetCanEnterPipe(direction, Pipe.GetPipe(map[ty][tx])))
                {
                    return direction;
                }
            }
            throw new Exception("Dead End");
        }
    }
}
