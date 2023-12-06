using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days
{
    public static class Day1
    {
        public static int Solve(string s)
        {
            int calibrationSum = 0;

            int firstDigit = -1;
            int lastDigit = -1;

            for (int i = 0; i < s.Length; i++)
            {
                if (TryGetDigit(s[i], out int d))
                {
                    lastDigit = d;
                    if (firstDigit < 0)
                    {
                        firstDigit = d;
                    }
                }
                if ((s[i] == '\n' || i == s.Length - 1) && 0 <= firstDigit)
                {
                    calibrationSum += 10 * firstDigit + lastDigit;
                    firstDigit = -1;
                    lastDigit = -1;
                }
            }

            return calibrationSum;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllText(path));
        }

        private static bool TryGetDigit(char c, out int d)
        {
            d = c - '0';
            return char.IsDigit(c);
        }
    }
}
