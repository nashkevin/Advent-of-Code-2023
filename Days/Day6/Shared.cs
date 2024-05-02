namespace Days.Day6
{
    internal static class Shared
    {
        internal static (double, double) GetQuadraticRootsRounded(long a, long b, long c)
        {
            double sqrt = (b * b) - (4 * a * c);
            return (
                Math.Floor(((-1) * b + Math.Sqrt(sqrt)) / (2 * a)) + 1,  // ± 1 in order to
                Math.Ceiling(((-1) * b - Math.Sqrt(sqrt)) / (2 * a)) - 1 // exclude ties
            );
        }
    }
}
