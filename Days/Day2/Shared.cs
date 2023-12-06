namespace Days.Day2
{
    internal static class Shared
    {
        internal static string TrimGameLabel(string s) => s[(s.IndexOf(':') + 2)..];
    }
}
