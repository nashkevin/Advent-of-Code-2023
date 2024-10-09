using static Days.Day8.Shared;

namespace Days.Day8
{
    /// <summary>
    /// <see href="https://en.wikipedia.org/wiki/Syzygy_(astronomy)">
    /// What we really have to do is figure out when all our orbits meet.
    /// </see>
    /// </summary>
    public static class PartTwo
    {
        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static long Solve(string[] s)
        {
            Dictionary<string, Node> nodes = [];
            List<Node> paths = [];
            List<long> pathLengths = [];
            for (int i = 2; i < s.Length; i++)
            {
                Node node = new(s[i]);
                nodes.Add(node.Self, node);
                if (node.Self.EndsWith('A'))
                {
                    paths.Add(node);
                }
            }
            int stepIndex = 0;
            long stepCounter = 0;

            while (0 < paths.Count)
            {
                stepCounter++;

                for (int i = paths.Count - 1; 0 <= i; i--)
                {
                    paths[i] = nodes[s[0][stepIndex] == 'L' ? paths[i].Left : paths[i].Right];
                    if (paths[i].Self.EndsWith('Z'))
                    {
                        // this path has reached an end, record its length
                        paths.RemoveAt(i);
                        pathLengths.Add(stepCounter);
                    }
                }

                stepIndex = stepIndex < s[0].Length - 1 ? stepIndex + 1 : 0;
            }

            return LCM(pathLengths);
        }

        private static long LCM(List<long> numbers) => numbers.Aggregate(LCM);
        private static long LCM(long a, long b) => Math.Abs(a * b) / GCD(a, b);
        private static long GCD(long a, long b) => b == 0 ? a : GCD(b, a % b);
    }
}
