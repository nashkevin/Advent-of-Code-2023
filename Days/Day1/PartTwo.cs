namespace Days.Day1
{
    public static class PartTwo
    {
        private static readonly (string, int)[] DigitWords =
        [
            ("zero", 0), ("one", 1), ("two", 2), ("three", 3), ("four", 4),
            ("five", 5), ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9),
        ];
        private static readonly int MaxWordLength = DigitWords.Max(x => x.Item1.Length);

        public static int Solve(string s)
        {
            int calibrationSum = 0;

            int firstDigit = -1;
            int lastDigit = -1;

            WordFinder wordFinder = new();

            for (int i = 0; i < s.Length; i++)
            {
                wordFinder.Enqueue(s[i]);

                if (Shared.TryGetDigit(s[i], out int d) || wordFinder.TryGetDigit(out d))
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

        private class WordFinder
        {
            private readonly LetterTreeNode wordTree;
            
            private readonly char[] buffer = new char[MaxWordLength];
            private int head = 0;

            internal WordFinder()
            {
                wordTree = LetterTreeNode.CreateTree(DigitWords);
            }

            internal void Enqueue(char c)
            {
                head.Advance();
                buffer[head] = c;
            }

            internal bool TryGetDigit(out int digit)
            {
                if (wordTree.TryFindMatch(buffer, head, out int matchedValue))
                {
                    digit = matchedValue;
                    return true;
                }
                digit = -1;
                return false;
            }            
        }

        private class LetterTreeNode
        {
            private bool isLeaf;
            private int digit;
            private readonly Dictionary<char, LetterTreeNode> children;

            LetterTreeNode() => children = [];

            internal bool TryFindMatch(char[] buffer, int head, out int digit)
            {
                if (isLeaf)
                {
                    digit = this.digit;
                    return true;
                }
                else if (children.TryGetValue(buffer[head], out LetterTreeNode? childNode))
                {
                    head.Retract();
                    return childNode.TryFindMatch(buffer, head, out digit);
                }
                else
                {
                    digit = -1;
                    return false;
                }
                
            }

            private void AddChild(IList<char> letters, int digit)
            {
                if (0 < letters.Count)
                {
                    char next = letters[^1];
                    letters.RemoveAt(letters.Count - 1);
                    if (children.TryGetValue(next, out var childNode))
                    {
                        childNode.AddChild(letters, digit);
                    }
                    else
                    {
                        children[next] = new LetterTreeNode();
                        children[next].AddChild(letters, digit);
                    }
                }
                else
                {
                    isLeaf = true;
                    this.digit = digit;
                }
            }

            internal static LetterTreeNode CreateTree((string, int)[] words)
            {
                LetterTreeNode root = new();
                foreach ((string, int) word in words)
                {
                    List<char> letters = [.. word.Item1];
                    root.AddChild(letters, word.Item2);
                }
                return root;
            }
        }

        private static void Advance(ref this int n)
        {
            n = n == MaxWordLength - 1 ? 0 : n + 1;
        }

        private static void Retract(ref this int n)
        {
            n = n == 0 ? MaxWordLength - 1 : n - 1;
        }
    }
}
