using static Days.Day1.Shared;

namespace Days.Day4
{
    public static class PartTwo
    {
        public static int Solve(string[] cards)
        {
            Dictionary<int, int> matchCounts = [];
            int cardCount = 0;

            for (int i = 0; i < cards.Length; i++)
            {
                matchCounts[i] = CountMatches(cards[i]);
            }

            Stack<int> stack = new(matchCounts.Keys);

            while (stack.TryPop(out int result))
            {
                cardCount++;
                if (matchCounts.TryGetValue(result, out int matchCount) && 0 < matchCount)
                {
                    for (int i = 1; i <= matchCount && i + result < cards.Length; i++)
                    {
                        stack.Push(i + result);
                    }
                }
            }

            return cardCount;
        }

        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        private static int CountMatches(string card)
        {
            HashSet<int> winningNumbers = [];
            int currentNumber = -1;
            int barIndex = card.IndexOf('|');

            for (int i = barIndex - 2; 0 < i && card[i] != ':'; i--)
            {
                if (TryGetDigit(card[i], out int d))
                {
                    currentNumber += 0 <= currentNumber ? 10 * d : 1 + d;
                }
                else if (0 <= currentNumber)
                {
                    winningNumbers.Add(currentNumber);
                    currentNumber = -1;
                }
            }

            int matchCount = 0;
            for (int i = card.Length - 1; 0 < i && barIndex < i; i--)
            {
                if (TryGetDigit(card[i], out int d))
                {
                    currentNumber += 0 <= currentNumber ? 10 * d : 1 + d;
                }
                else if (0 <= currentNumber)
                {
                    if (winningNumbers.Contains(currentNumber))
                    {
                        matchCount++;
                    }
                    currentNumber = -1;
                }
            }
            
            return matchCount;
        }
    }
}
