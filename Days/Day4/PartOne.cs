using static Days.Day1.Shared;

namespace Days.Day4
{
    public static class PartOne
    {
        public static int Solve(string[] cards)
        {
            HashSet<int> winningNumbers = [];
            int totalScore = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                int currentNumber = -1;
                int barIndex = cards[i].IndexOf('|');
                
                for (int j = barIndex - 2; 0 < j && cards[i][j] != ':'; j--)
                {
                    if (TryGetDigit(cards[i][j], out int d))
                    {
                        currentNumber += 0 <= currentNumber ? 10 * d : 1 + d;
                    }
                    else if (0 <= currentNumber)
                    {
                        winningNumbers.Add(currentNumber);
                        currentNumber = -1;
                    }
                }

                int currentScore = 0;
                for (int j = cards[i].Length - 1; 0 < j && barIndex < j; j--)
                {
                    if (TryGetDigit(cards[i][j], out int d))
                    {
                        currentNumber += 0 <= currentNumber ? 10 * d : 1 + d;
                    }
                    else if (0 <= currentNumber)
                    {
                        if (winningNumbers.Contains(currentNumber))
                        {
                            currentScore = currentScore == 0 ? 1 : currentScore * 2;
                        }
                        currentNumber = -1;
                    }
                }
                totalScore += currentScore;
                winningNumbers.Clear();
            }

            return totalScore;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }
    }
}
