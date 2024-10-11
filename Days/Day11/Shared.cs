namespace Days.Day11
{
    internal static class Shared
    {
        internal static long CalculatePairDistances(string[] map, int emptySpaceMultiplier)
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

                    // increase distance based on empty line intersections
                    foreach (int emptyColumn in emptyColumns)
                    {
                        if (xMin < emptyColumn && emptyColumn < xMax)
                        {
                            distance += emptySpaceMultiplier - 1; // minus one because the distance
                        }                                         // formula inherently counts it once
                    }
                    foreach (int emptyRow in emptyRows)
                    {
                        if (yMin < emptyRow && emptyRow < yMax)
                        {
                            distance += emptySpaceMultiplier - 1;
                        }
                    }

                    totalDistance += distance;
                }
            }

            return totalDistance;
        }
    }
}
