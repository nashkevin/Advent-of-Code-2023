using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Days.Day9
{
    internal static class Shared
    {
        internal static long[] ToLongs(this string s)
        {
            return s.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(long.Parse).ToArray();
        }
    }
}
