using Days;

namespace AdventConsole
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string input = "123\n1\n000\nfoo9bar\n777";
            Console.WriteLine(input);
            Console.WriteLine();
            Console.WriteLine($"output: {Day1.Solve(input)}");
        }
    }
}
