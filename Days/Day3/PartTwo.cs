using static Days.Day1.Shared;
using static Days.Day3.Shared;

namespace Days.Day3
{
    /// <summary>
    /// Yes, I know int.Parse() exists, but sometimes it's more fun to get
    /// your hands dirty. For a little extra challenge, I specifically avoided
    /// creating substrings.
    /// </summary>
    public static class PartTwo
    {
        private static readonly int[] PlaceValues =
        [
            1, 10, 100, 1000, 10_000, 100_000, 1_000_000,
            10_000_000, 100_000_000, 1_000_000_000
        ];

        public static int Solve(string[] schematic) // assumes rectangular array
        {
            int gearRatioSum = 0;

            for (int y = 0; y < schematic.Length; y++)
            {
                for (int x = 0; x < schematic[y].Length; x++)
                {
                    if (schematic[y][x] == '*' && FindNeighborParts(schematic, x, y, out int gearRatio))
                    {
                        gearRatioSum += gearRatio;
                    }
                }
            }
            return gearRatioSum;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        private static bool FindNeighborParts(string[] schematic, int x, int y, out int gearRatio)
        {
            int neighborPartCount = 0;
            int[] partNumbers = new int[2];
            gearRatio = 0;

            // directly left
            if (TryConvertToIntRightToLeft(schematic[y], x - 1, out int result))
            {
                partNumbers[neighborPartCount] = result;
                neighborPartCount++;
            }

            // directly right
            if (TryConvertToIntLeftToRight(schematic[y], x + 1, out result))
            {
                partNumbers[neighborPartCount] = result;
                neighborPartCount++;
            }

            AdjacentDigits adjacentDigits = new();

            // the row above
            if (0 < y)
            {
                if (!ValidateAndGetNeighbors(schematic[y - 1], x, ref adjacentDigits, ref neighborPartCount, ref partNumbers))
                {
                    return false;
                }
            }

            // the row below
            if (y < schematic.Length - 1)
            {
                if (!ValidateAndGetNeighbors(schematic[y + 1], x, ref adjacentDigits, ref neighborPartCount, ref partNumbers))
                {
                    return false;
                }
            }

            if (neighborPartCount == 2)
            {
                gearRatio = partNumbers[0] * partNumbers[1];
                return true;
            }
            return false;
        }

        private static bool TryConvertToIntRightToLeft(string s, int x, out int result)
        {
            result = 0;
            
            for (int i = x; 0 <= i && TryGetDigit(s[i], out int d); i--)
            {
                result += d * PlaceValues[x - i];
            }
            
            return 0 < result;
        }

        private static bool TryConvertToIntLeftToRight(string s, int x, out int result)
        {
            result = 0;

            Stack<int> digits = new();
            for (int i = x; i < s.Length && TryGetDigit(s[i], out int d); i++)
            {
                digits.Push(d);
            }
            for (int i = 0; digits.TryPop(out int d); i++)
            {
                result += d * PlaceValues[i];
            }

            return 0 < result;
        }        

        private static bool TryConvertToIntFromCenter(string s, int x, out int result)
        {
            result = 0;
            Stack<int> rightDigits = new();

            for (int i = x + 1; i < s.Length && TryGetDigit(s[i], out int d); i++)
            {
                rightDigits.Push(d);
            }
            
            int nextPlaceValue = 0;
            
            while (rightDigits.TryPop(out int d))
            {
                result += d * PlaceValues[nextPlaceValue++];
            }

            for (int i = x; 0 <= i && TryGetDigit(s[i], out int d); i--)
            {
                result += d * PlaceValues[nextPlaceValue++];
            }

            return 0 < result;
        }

        private static bool ValidateAndGetNeighbors(string s, int x, ref AdjacentDigits adjacentDigits, ref int neighborPartCount, ref int[] partNumbers)
        {
            SetAdjacentDigits(s, x, ref adjacentDigits);

            // no neighbors (...)
            if (adjacentDigits == 0)
            {
                return true;
            }
            // one single-digit neighbor (.N.)
            else if (adjacentDigits == AdjacentDigits.Center)
            {
                if (1 < neighborPartCount)
                {
                    return false;
                }
                partNumbers[neighborPartCount] = GetDigit(s[x]);
                neighborPartCount++;
                return true;
            }
            // two neighbors extending left and right (N.N)
            else if (adjacentDigits == (AdjacentDigits.Left | AdjacentDigits.Right))
            {
                if (0 < neighborPartCount)
                {
                    return false;
                }
                if (TryConvertToIntRightToLeft(s, x - 1, out int result))
                {
                    partNumbers[neighborPartCount] = result;
                    neighborPartCount++;
                }
                if (TryConvertToIntLeftToRight(s, x + 1, out result))
                {
                    partNumbers[neighborPartCount] = result;
                    neighborPartCount++;
                }
                return true;
            }
            // one neighbor extending left (N.. or NN.)
            else if (!adjacentDigits.HasFlag(AdjacentDigits.Right))
            {
                if (1 < neighborPartCount)
                {
                    return false;
                }
                if (TryConvertToIntRightToLeft(s, adjacentDigits.HasFlag(AdjacentDigits.Center) ? x : x - 1, out int result))
                {
                    partNumbers[neighborPartCount] = result;
                    neighborPartCount++;
                }
                return true;
            }
            // one neighbor extending right (..N or .NN or NNN)
            else
            {
                if (1 < neighborPartCount)
                {
                    return false;
                }
                if (TryConvertToIntFromCenter(s, x, out int result))
                {
                    partNumbers[neighborPartCount] = result;
                    neighborPartCount++;
                }
                return true;
            }
        }

        private static void SetAdjacentDigits(string s, int x, ref AdjacentDigits adjacentDigits, bool shouldReset=true)
        {
            if (shouldReset)
            {
                adjacentDigits = 0;
            }

            if (0 <= x - 1 && char.IsDigit(s[x - 1]))
            {
                adjacentDigits |= AdjacentDigits.Left;
            }
            if (char.IsDigit(s[x]))
            {
                adjacentDigits |= AdjacentDigits.Center;
            }
            if (x + 1 < s.Length && char.IsDigit(s[x + 1]))
            {
                adjacentDigits |= AdjacentDigits.Right;
            }
        }


        [Flags] private enum AdjacentDigits
        {
            Left = 1,
            Center = 2,
            Right = 4,
        }
    }
}
