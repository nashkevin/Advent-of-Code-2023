﻿namespace Days.Day9
{
    public static class PartTwo
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] s)
        {
            return Shared.Solve(s, true);
        }
    }
}
