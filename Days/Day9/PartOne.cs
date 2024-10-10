namespace Days.Day9
{
    public static class PartOne
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] s)
        {
            long[][] input = s.Select(x => x.ToLongs()).ToArray();
            long output = 0;

            for (int i = 0; i < input.Length; i++)
            {
                output += GetNextInSequence(input[i]);
            }

            return output;
        }


        private static long[] ToLongs(this string s)
        {
            return s.Split(' ').Select(long.Parse).ToArray();
        }

        private static long GetNextInSequence(long[] a)
        {
            long[] deltas = new long[a.Length - 1];
            bool isLinear = true;
            for (int i = 0; i < a.Length - 1; i++)
            {
                deltas[i] = a[i + 1] - a[i];
                if (0 < i)
                {
                    isLinear &= deltas[i] == deltas[i - 1];
                }                
            }
            return a[^1] + (isLinear ? deltas[0] : GetNextInSequence(deltas));
        }
    }
}
