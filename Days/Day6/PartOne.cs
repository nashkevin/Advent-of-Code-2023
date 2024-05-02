using System;
using System.Net.Http.Headers;

namespace Days.Day6
{
    public static class PartOne
    {
        private const int Acceleration = 1; // mm/ms^2

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static int Solve(string[] s)
        {
            List<RaceRecord> raceRecords = GetInts(s[0]).Zip(GetInts(s[1]),
                (times, distances) => new RaceRecord() { Time = times, Distance = distances }).ToList();

            int result = 1;
            foreach (RaceRecord raceRecord in raceRecords)
            {
                // d = vt
                // distance = Acceleration * buttonTime * (raceTime - buttonTime)
                (int, int) roots = GetQuadraticRootsRounded(-Acceleration, raceRecord.Time, -raceRecord.Distance);
                result *= roots.Item2 - roots.Item1 + 1;
            }
            
            return result;
        }

        
        private static IEnumerable<int> GetInts(string s)
        {
            return s.Split(' ')
                .Select(x => { bool isInt = int.TryParse(x, out int value); return new { value, isInt }; })
                .Where(x => x.isInt)
                .Select(x => x.value);
        }

        private static (int, int) GetQuadraticRootsRounded(int a, int b, int c)
        {
            double sqrt = (b * b) - (4 * a * c);
            return (
                (int)Math.Floor(((-1) * b + Math.Sqrt(sqrt)) / (2 * a)) + 1,  // ± 1 in order to
                (int)Math.Ceiling(((-1) * b - Math.Sqrt(sqrt)) / (2 * a)) - 1 // exclude ties
            );
        }

        private class RaceRecord()
        {
            public int Time { get; set; }
            public int Distance { get; set; }
        }
    }
}
