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
            (int sx, int sy) = GetStartingPoint(map);

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
    }
}
