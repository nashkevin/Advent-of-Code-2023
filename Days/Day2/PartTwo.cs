namespace Days.Day2
{
    public static class PartTwo
    {
        public static int Solve(string[] games)
        {
            int cubePowerSum = 0;

            for (int i = 0; i < games.Length; i++)
            {
                games[i] = Shared.TrimGameLabel(games[i]);
                cubePowerSum += GetCubePower(games[i]);
            }

            return cubePowerSum;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        private static int GetCubePower(string game)
        {
            Dictionary<string, int> CubeCounts = new()
            {
                { "red", 0 }, { "green", 0 }, {"blue", 0 },
            };

            string[] rounds = game.Split("; ", StringSplitOptions.TrimEntries);
            foreach (string round in rounds)
            {
                string[] colorCounts = round.Split(", ", StringSplitOptions.TrimEntries);
                foreach (string colorCount in colorCounts)
                {
                    string[] rawPair = colorCount.Split(' ');
                    CubeCounts[rawPair[1]] = Math.Max(CubeCounts[rawPair[1]], int.Parse(rawPair[0]));
                }
            }

            return CubeCounts.Values.Aggregate(1, (x, y) => x * y);
        }
    }
}
