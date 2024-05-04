using static Days.Day8.Shared;

namespace Days.Day8
{
    public static class PartOne
    {
        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static int Solve(string[] s)
        {
            Dictionary<string, Node> nodes = [];
            Node node;
            for (int i = 2; i < s.Length; i++)
            {
                node = new(s[i]);
                nodes.Add(node.Self, node);
            }
            node = nodes["AAA"];
            int index = 0;
            int stepCounter = 0;
            while (node.Self != "ZZZ")
            {
                node = s[0][index] == 'L' ? nodes[node.Left] : nodes[node.Right];
                index = index < s[0].Length - 1 ? index + 1 : 0;
                stepCounter++;
            }
            return stepCounter;
        }
    }
}
