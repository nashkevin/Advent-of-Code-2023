using static Days.Day11.Shared;

namespace Days.Day11
{
    public static class PartOne
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] map)
        {
            return CalculatePairDistances(map, 2);
        }
    }
}
