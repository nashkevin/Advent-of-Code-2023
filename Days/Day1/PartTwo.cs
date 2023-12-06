using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Days.Day1
{
    public static class PartTwo
    {
        private static readonly (string, int)[] DigitWords =
        [
            ("zzero", 99),
            ("zero", 0), ("one", 1), ("two", 2), ("three", 3), ("four", 4),
            ("five", 5), ("six", 6), ("seven", 7), ("eight", 8), ("nine", 9),
        ];
        private static readonly int MaxWordLength = DigitWords.Max(x => x.Item1.Length);

        public static int Solve(string s)
        {
            LetterTreeNode.CreateTree(DigitWords);
            return -1;
        }

        private class DigitWord
        {
            private readonly char[] buffer = new char[MaxWordLength];
            private int head = 0;

            bool Enqueue(char c, out int digit)
            {
                digit = -999;
                head.Advance();
                buffer[head] = c;
                return false;
            }            
        }

        private class LetterTreeNode
        {
            private bool isLeaf;
            private int digit;
            private readonly Dictionary<char, LetterTreeNode> children;

            LetterTreeNode() => children = [];

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
