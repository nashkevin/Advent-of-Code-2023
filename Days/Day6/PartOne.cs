using static Days.Day6.Shared;

namespace Days.Day6
{
    public static class PartOne
    {
        private const int Acceleration = 1; // mm/ms^2

        public static double SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static double Solve(string[] s)
        {
            List<RaceRecord> raceRecords = GetInts(s[0]).Zip(GetInts(s[1]),
                (times, distances) => new RaceRecord() { Time = times, Distance = distances }).ToList();

            double result = 1;
            foreach (RaceRecord raceRecord in raceRecords)
            {
                // d = vt
                // distance = Acceleration * buttonTime * (raceTime - buttonTime)
                (double, double) roots = GetQuadraticRootsRounded(-Acceleration, raceRecord.Time, -raceRecord.Distance);
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

        private class RaceRecord()
        {
            public int Time { get; set; }
            public int Distance { get; set; }
        }
    }
}
