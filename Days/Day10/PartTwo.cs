using static Days.Day10.Shared;

namespace Days.Day10
{
    public static class PartTwo
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] map)
        {
            SortedDictionary<int, SortedSet<int>> loopCoordinates = [];

            (int sx, int sy) = GetStartingPoint(map);

            int x = sx;
            int y = sy;
            Direction? direction = null;
            do
            {
                if (loopCoordinates.TryGetValue(y, out var xList))
                {
                    xList.Add(x);
                }
                else
                {
                    loopCoordinates[y] = [x];
                }

                direction = GetNextDirection(map, x, y, direction);
                direction.DoStep(ref x, ref y);
            }
            while (x != sx || y != sy);

            int interiorCount = 0;
            foreach (KeyValuePair<int, SortedSet<int>> row in loopCoordinates)
            {
                bool isInterior = false;
                for (int i = row.Value.Min; i < row.Value.Max; i++)
                {
                    if (row.Value.Contains(i))
                    {
                        if (Pipe.GetPipe(map[row.Key][i]).Directions.Contains(Direction.North))
                        {
                            isInterior = !isInterior;
                        }
                    }
                    else if (isInterior)
                    {
                        interiorCount++;
                    }
                }
            }

            return interiorCount;
        }
    }
}
