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
                    int xMax = Math.Max(galaxies[i].x, galaxies[j].x);
                    int xMin = Math.Min(galaxies[i].x, galaxies[j].x);
                    int yMax = Math.Max(galaxies[i].y, galaxies[j].y);
                    int yMin = Math.Min(galaxies[i].y, galaxies[j].y);

                    int distance = xMax - xMin + yMax - yMin;

                    // empty rows and columns count twice
                    foreach (int emptyColumn in emptyColumns)
                    {
                        if (xMin <= emptyColumn && emptyColumn <= xMax)
                        {
                            distance++;
                        }
                    }
                    foreach (int emptyRow in emptyRows)
                    {
                        if (yMin <= emptyRow && emptyRow <= yMax)
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
