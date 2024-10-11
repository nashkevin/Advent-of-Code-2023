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
            // assumes nonzero rectangle
            HashSet<int> emptyRows = new(Enumerable.Range(0, map.Length));
            HashSet<int> emptyColumns = new(Enumerable.Range(0, map[0].Length));

            List<(int x, int y)> galaxies = [];

            for (int y = 0; y < map.Length; y++)
            {
                for (int x = 0; x < map[y].Length; x++)
                {
                    if (map[y][x] == '#')
                    {
                        emptyRows.Remove(y);
                        emptyColumns.Remove(x);
                        galaxies.Add((x, y));
                    }
                }
            }

            long totalDistance = 0;
            for (int i = 0; i < galaxies.Count; i++)
            {
                for (int j = i + 1; j < galaxies.Count; j++)
                {
                    int distance = Math.Abs(galaxies[i].x - galaxies[j].x) + Math.Abs(galaxies[i].y - galaxies[j].y);
                    
                    // empty rows and columns count twice
                    // TODO determining this intersection can be done more efficiently,
                    // possibly by looping over the empties instead
                    for (int k = Math.Min(galaxies[i].x, galaxies[j].x) + 1; k < Math.Max(galaxies[i].x, galaxies[j].x); k++)
                    {
                        if (emptyColumns.Contains(k))
                        {
                            distance++;
                        }
                    }
                    for (int k = Math.Min(galaxies[i].y, galaxies[j].y) + 1; k < Math.Max(galaxies[i].y, galaxies[j].y); k++)
                    {
                        if (emptyRows.Contains(k))
                        {
                            distance++;
                        }
                    }

                    totalDistance += distance;
                }
            }

            return totalDistance;
        }
    }
}
