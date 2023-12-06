namespace Days.Day2
{
    public static class PartOne
    {
        private static readonly Dictionary<string, int> CubeCounts = new()
        {
            { "red", 12 }, { "green", 13 }, {"blue", 14 },
        };

        public static int Solve(string[] games)
        {
            int gameIdSum = 0;

            for (int i = 0; i < games.Length; i++)
            {
                games[i] = Shared.TrimGameLabel(games[i]);
                if (ValidateValues(games[i]))
                {
                    gameIdSum += i + 1;
                }
            }

            return gameIdSum;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        private static bool ValidateValues(string game)
        {
            string[] rounds = game.Split("; ", StringSplitOptions.TrimEntries);
            foreach (string round in rounds)
            {
                string[] colorCounts = round.Split(", ", StringSplitOptions.TrimEntries);
                foreach (string colorCount in colorCounts)
                {
                    string[] rawPair = colorCount.Split(' ');
                    if (CubeCounts[rawPair[1]] < int.Parse(rawPair[0]))
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
