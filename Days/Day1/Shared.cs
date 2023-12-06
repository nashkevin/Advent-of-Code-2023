namespace Days.Day1
{
    internal static class Shared
    {
        internal static bool TryGetDigit(char c, out int d)
        {
            d = c - '0';
            return char.IsDigit(c);
        }
    }
}
