using static Days.Day6.Shared;

namespace Days.Day6
{
    public static class PartTwo
    {
        private const int Acceleration = 1; // mm/ms^2

        public static double SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static double Solve(string[] s)
        {
            long time = long.Parse(s[0].Replace(" ", string.Empty)[(s[0].IndexOf(':') + 1)..]);
            long distance = long.Parse(s[1].Replace(" ", string.Empty)[(s[1].IndexOf(':') + 1)..]);

            (double, double) roots = GetQuadraticRootsRounded(-Acceleration, time, -distance);
            return roots.Item2 - roots.Item1 + 1;
        }
    }
}
