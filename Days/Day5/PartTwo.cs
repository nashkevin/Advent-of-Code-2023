using System;
using System.Data.Common;

namespace Days.Day5
{
    public static class PartTwo
    {
        public static long Solve(string[] almanac)
        {
            List<Range> ranges = almanac[0][(almanac[0].IndexOf(' ') + 1)..].ToRanges();
            ranges.Sort();
            List<MapLayer> mapLayers = [new MapLayer()];

            for (int i = 0; i < almanac.Length; i++)
            {
                Console.WriteLine(almanac[i]);
                if (0 < almanac[i].Length && char.IsDigit(almanac[i][0]))
                {
                    mapLayers.Last().AddMap(new RangeMap(almanac[i].ToLongs()));
                }
                else if (!mapLayers.Last().IsEmpty())
                {
                    mapLayers.Add(new MapLayer());
                }
            }

            foreach (MapLayer layer in mapLayers)
            {
                ranges = layer.PassThrough(ranges);
            }
            
            return ranges.Min()?.Min ?? -1;
        }

        public static long SolveFromFile(string path)
        {
            return Solve(File.ReadAllLines(path));
        }


        private static List<Range> ToRanges(this string s)
        {
            List<Range> ranges = [];
            List<long> seedInputs = ToLongs(s);

            for (int i = 1; i < seedInputs.Count; i += 2)
            {
                ranges.Add(new Range(seedInputs[i - 1], seedInputs[i]));
            }

            return ranges;
        }

        private static List<long> ToLongs(this string s)
        {
            return s.Split(' ').Select(long.Parse).ToList();
        }

        private class Range : IComparable<Range>
        {
            public long Min { get; set; }
            public long Max { get; set; }

            public Range(long min, long length)
            {
                Min = min;
                Max = min + length - 1;
            }

            public void ApplyShift(long shift)
            {
                Min += shift;
                Max += shift;
            }

            public Range SplitLeft(long index)
            {
                long leftSplitLength = index - Min;
                Min = index;
                return new Range(Min - leftSplitLength, leftSplitLength);
            }

            public Range SplitRight(long index)
            {
                long rightSplitLength = Max - index;
                Max = index;
                return new Range(Max + 1, rightSplitLength);
            }

            public Edge GetLeftEdgeType(Range other)
            {
                return (Edge)Min.CompareTo(other.Min);
            }

            public Edge GetRightEdgeType(Range other)
            {
                return (Edge)other.Max.CompareTo(Max);
            }

            public bool IsOverlapping(Range? other)
            {
                return other != null && !(Max < other.Min || other.Max < Min);
            }

            public int CompareTo(Range? other)
            {
                return Min.CompareTo(other?.Min);
            }

            public enum Edge
            {
                Overhang = -1,
                Meeting = 0,
                Underhang = 1,
            }
        }

        private class RangeMap : Range
        {
            long Shift { get; set; }

            public RangeMap(long destination, long source, long length) : base(source, length)
            {
                Shift = destination - source;
            }

            public RangeMap(List<long> longs) : this(longs[0], longs[1], longs[2]) { }


            /// <summary>
            /// Shifts and trims a given <see cref="Range"/> according to the map rules
            /// and provides the newly split ranges as output.
            /// </summary>
            /// <returns><c>true</c> if a shift was applied to <paramref name="other"/>,
            /// <c>false</c> otherwise</returns>
            public bool TryApplyOverlapShift(Range other, out List<Range> splits)
            {
                splits = [];

                if (!IsOverlapping(other))
                {
                    return false;
                }

                switch (GetLeftEdgeType(other))
                {
                    case Edge.Overhang:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                goto case Edge.Meeting;
                            case Edge.Meeting:
                                other.ApplyShift(Shift);
                                break;
                            case Edge.Underhang:
                                splits.Add(other.SplitRight(Max));
                                other.ApplyShift(Shift);
                                break;
                        }
                        break;
                    case Edge.Meeting:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                goto case Edge.Meeting;
                            case Edge.Meeting:
                                other.ApplyShift(Shift);
                                break;
                            case Edge.Underhang:
                                splits.Add(other.SplitRight(Max));
                                other.ApplyShift(Shift);
                                break;
                        }
                        break;
                    case Edge.Underhang:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                goto case Edge.Meeting;
                            case Edge.Meeting:
                                splits.Add(other.SplitLeft(Min));
                                other.ApplyShift(Shift);
                                break;
                            case Edge.Underhang:
                                splits.Add(other.SplitLeft(Min));
                                splits.Add(other.SplitRight(Max));
                                other.ApplyShift(Shift);
                                break;
                        }
                        break;
                }
                return true;
            }
        }

        private class MapLayer
        {
            private readonly List<RangeMap> maps = [];

            
            public void AddMap(RangeMap map)
            {
                maps.Add(map);
                maps.Sort();
            }

            public bool IsEmpty() => maps.Count == 0;

            public List<Range> PassThrough(List<Range> ranges)
            {
                List<Range> finished = [];
                List<Range> allSplits = [];
                for (int i = ranges.Count - 1; 0 <= i; i--)
                {
                    foreach (RangeMap map in maps)
                    {
                        if (map.TryApplyOverlapShift(ranges[i], out List<Range> splits))
                        {
                            finished.Add(ranges[i]);
                            ranges.RemoveAt(i);
                            allSplits.AddRange(splits);
                            break;
                        }
                        allSplits.AddRange(splits);
                    }
                }
                if (0 <  allSplits.Count)
                {
                    finished.AddRange(PassThrough(allSplits));
                }
                finished.AddRange(ranges);
                finished.Sort();
                return finished;
            }
        }
    }
}
