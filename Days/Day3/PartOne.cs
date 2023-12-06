namespace Days.Day3
{
    public static class PartOne
    {
        public static int Solve(string[] schematic) // assumes rectangular array
        {
            int partNumberSum = 0;
            bool isOnPartNumber = false;
            int numberStartIndex = -1;

            for (int y = 0; y < schematic.Length; y++)
            {
                for (int x = 0; x < schematic[y].Length; x++)
                {
                    // reached number first digit
                    if (!isOnPartNumber && char.IsDigit(schematic[y][x]))
                    {
                        isOnPartNumber = true;
                        numberStartIndex = x;
                    }

                    // reached number last digit
                    if (isOnPartNumber && (x == schematic[y].Length - 1 || !char.IsDigit(schematic[y][x + 1])))
                    {
                        isOnPartNumber = false;
                        if (IsSymbolAdjacent(schematic, y, numberStartIndex, x))
                        {
                            partNumberSum += int.Parse(schematic[y][numberStartIndex..(x + 1)]);
                        }
                    }
                }
            }
            return partNumberSum;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        private static bool IsSymbolAdjacent(string[] schematic, int y, int xLeft, int xRight)
        {
            // adjust to avoid OOB
            xLeft = Math.Max(xLeft - 1, 0);
            xRight = Math.Min(xRight + 1, schematic[y].Length - 1);

            for (int x = xLeft; x <= xRight; x++)
            {
                if (0 < y && IsCharSymbol(schematic[y - 1][x]) || IsCharSymbol(schematic[y][x]) || y < schematic.Length - 1 && IsCharSymbol(schematic[y + 1][x]))
                {
                    return true;
                }
            }
            return false;
        }

        private static bool IsCharSymbol(char c) => c != '.' && !char.IsDigit(c);
    }
}
