using static Days.Day11.Shared;

namespace Days.Day11
{
    public static class PartTwo
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] map)
        {
            return CalculatePairDistances(map, 1_000_000);
        }
    }
}
