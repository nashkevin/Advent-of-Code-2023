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
            char[][] map2 = map.Select(x => x.ToCharArray()).ToArray();
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

                if (Pipe.GetPipe(map[y][x]).Directions.Contains(Direction.North))
                {
                    map2[y][x] = '█';
                }
                else
                {
                    map2[y][x] = '▒';
                }
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
                        map2[row.Key][i] = '☺';
                    }
                }
            }

            for (int i = 0; i < map2.Length; i++)
            {
                for (int j = 0; j < map2[i].Length; j++)
                {
                    Console.Write(map2[i][j]);
                }
                Console.WriteLine();
            }

            return interiorCount;
        }
    }
}
