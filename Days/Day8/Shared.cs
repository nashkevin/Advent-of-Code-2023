namespace Days.Day8
{
    internal class Shared
    {
        internal class Node
        {
            public string Self { get; set; }
            public string Left { get; set; }
            public string Right { get; set; }

            public Node(string s)
            {
                string[] splits = s.Split(" = ");
                Self = splits[0];
                splits = splits[1][1..^1].Split(", ");
                Left = splits[0];
                Right = splits[1];
            }

            public Node(Node other)
            {
                Self = other.Self;
                Left = other.Left;
                Right = other.Right;
            }

            public Node(string self, string left, string right)
            {
                Self = self;
                Left = left;
                Right = right;
            }
        }
    }
}
