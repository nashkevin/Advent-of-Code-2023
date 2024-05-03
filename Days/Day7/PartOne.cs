using static Days.Day7.Shared;

namespace Days.Day7
{
    public static class PartOne
    {
        public static int SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }

        public static int Solve(string[] s)
        {
            List<Hand> hands = [];
            for (int i = 0; i < s.Length; i++)
            {
                hands.Add(new Hand(s[i]));
            }
            hands.Sort();

            int result = 0;
            for (int i = 0; i < hands.Count; i++)
            {
                result += hands[i].Bid * (i + 1);
            }
            return result;
        }
    }
}
