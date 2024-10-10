namespace Days.Day9
{
    internal static class Shared
    {
        internal static long[] ToLongs(this string s)
        {
            return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }

        public static long Solve(string[] s, bool isPrepend)
        {
            long[][] input = s.Select(x => x.ToLongs()).ToArray();
            long output = 0;

            for (int i = 0; i < input.Length; i++)
            {
                output += GetNextInSequence(input[i], isPrepend);
            }

            return output;
        }

        private static long GetNextInSequence(long[] a, bool isPrepend)
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

            long shift = isLinear ? deltas[0] : GetNextInSequence(deltas, isPrepend);

            return isPrepend ? a[0] - shift : a[^1] + shift;
        }
    }
}
