namespace Days.Day5
{
    public static class PartOne
    {
        public static int Solve(string[] almanac)
        {
            List<long> seeds = almanac[0][(almanac[0].IndexOf(' ') + 1)..].ToLongs();
            List<MapRule> rules = [];

            bool withinRules = false;
            for (int i = 0; i < almanac.Length; i++)
            {
                Console.WriteLine(almanac[i]);
                if (0 < almanac[i].Length && char.IsDigit(almanac[i][0]))
                {
                    withinRules = true;

                    rules.Add(new MapRule(almanac[i].ToLongs()));
                }
                else
                {
                    withinRules = false;
                }

                if (!withinRules || i == almanac.Length - 1)
                {
                    for (int j = 0; j < seeds.Count; j++)
                    {
                        foreach (MapRule rule in rules)
                        {
                            if (rule.Process(seeds[j], out long p))
                            {
                                Console.WriteLine($"{j}:\tReplaced {seeds[j]} with {p}");
                                seeds[j] = p;
                                break;
                            }
                        }
                    }
                    Console.WriteLine();
                    rules.Clear();
                    withinRules = false;
                }
            }

            return (int)seeds.Min();
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }


        private static List<long> ToLongs(this string s)
        {
            return s.Split(' ').Select(long.Parse).ToList();
        }

        private class MapRule
        {
            long Min { get; set; } // inclusive
            long Max { get; set; } // inclusive
            long Shift { get; set; }

            public MapRule(long destination, long source, long length)
            {
                Min = source;
                Max = source + length - 1;
                Shift = destination - source;
            }

            public MapRule(List<long> longs) : this(longs[0], longs[1], longs[2]) { }

            public bool Process(long n, out long p)
            {
                p = n + Shift;
                return Min <= n && n <= Max;
            }
        }
    }
}
