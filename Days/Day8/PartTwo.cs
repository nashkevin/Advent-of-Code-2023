using static Days.Day8.Shared;

namespace Days.Day8
{
    public static class PartTwo
    {
        public static ulong SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static ulong Solve(string[] s)
        {
            Dictionary<string, Node> allNodes = [];
            List<Node> startingNodes = [];
            for (int i = 2; i < s.Length; i++)
            {
                Node node = new(s[i]);
                allNodes.Add(node.Self, node);
                if (node.Self.EndsWith('A'))
                {
                    startingNodes.Add(node);
                }
            }
            int stepIndex = 0;
            ulong stepCounter = 0;
            while (!startingNodes.All(x => x.Self.EndsWith('Z')))
            {
                for (int i = startingNodes.Count - 1; 0 <= i; i--)
                {
                    //if (i == 0)
                    //{
                    //    Console.Write($"{s[0][stepIndex]}: ");
                    //    Console.Write($"{startingStrings[i]} -> ");
                    //}
                    startingNodes[i] = allNodes[s[0][stepIndex] == 'L' ? startingNodes[i].Left : startingNodes[i].Right];
                    //if (i == 0)
                    //{
                    //    Console.WriteLine(startingStrings[i]);
                    //}
                    //if (269 < stepCounter)
                    //{
                    //    break;
                    //}
                }
                stepIndex = stepIndex < s[0].Length - 1 ? stepIndex + 1 : 0;
                stepCounter++;
            }
            return stepCounter;
        }
    }
}
