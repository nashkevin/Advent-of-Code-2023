﻿using System.Data.Common;

namespace Days.Day5
{
    public static class PartTwo
    {
        public static long Solve(string[] almanac)
        {
            List<Range> ranges = almanac[0][(almanac[0].IndexOf(' ') + 1)..].ToRanges();
            List<MapRule> rules = [];

            bool isWithinRules = false;
            bool isRulesJustFinished = false;
            for (int i = 0; i < almanac.Length; i++)
            {
                Console.WriteLine(almanac[i]);
                if (0 < almanac[i].Length && char.IsDigit(almanac[i][0]))
                {
                    isWithinRules = true;

                    rules.Add(new MapRule(almanac[i].ToLongs()));
                }
                else
                {
                    if (isWithinRules)
                    {
                        isRulesJustFinished = true;
                    }
                    isWithinRules = false;
                }

                if (isRulesJustFinished || i == almanac.Length - 1)
                {
                    for (int j = ranges.Count - 1; j >= 0; j--)
                    {
                        foreach (MapRule rule in rules)
                        {
                            if (rule.TryApplyOverlapShift(ranges[j], out List<Range> newRanges))
                            {
                                ranges.AddRange(newRanges);
                                break;
                            }                            
                        }
                    }
                    rules.Clear();
                    isWithinRules = false;
                    isRulesJustFinished = false;
                }
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

            public Range(Range other)
            {
                Min = other.Min;
                Max = other.Max;
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
                if (Min < other.Min)
                {
                    return Edge.Overhang;
                }
                if (other.Min < Min)
                {
                    return Edge.Underhang;
                }
                return Edge.Meeting;
            }

            public Edge GetRightEdgeType(Range other)
            {
                if (other.Max < Max)
                {
                    return Edge.Overhang;
                }
                if (Max < other.Max)
                {
                    return Edge.Underhang;
                }
                return Edge.Meeting;
            }

            public bool IsOverlapping(Range? other)
            {
                if (other == null)
                {
                    return false;
                }
                return !(Max < other.Min || other.Max < Min);
            }

            public int CompareTo(Range? other) => (int)(other == null ? -1 : Min - other.Min);

            public override string ToString()
            {
                return $"[{Min},{Max}]";
            }

            public enum Edge
            {
                Meeting,
                Overhang,
                Underhang,
            }
        }

        private class MapRule : Range
        {
            long Shift { get; set; }

            public MapRule(long destination, long source, long length) : base(source, length)
            {
                Shift = destination - source;
            }

            public MapRule(List<long> longs) : this(longs[0], longs[1], longs[2]) { }


            public bool Process(long n, out long p)
            {
                p = n + Shift;
                return Min <= n && n <= Max;
            }

            public bool TryApplyOverlapShift(Range other, out List<Range> newRanges)
            {
                newRanges = [];

                if (!IsOverlapping(other))
                {
                    //Console.WriteLine($"{other} did not overlap {this}");
                    return false;
                }

                switch (GetLeftEdgeType(other))
                {
                    case Edge.Overhang:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                Console.WriteLine($"{other} matched {this} L:Overhang R:Overhang");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            //goto case Edge.Meeting;
                            case Edge.Meeting:
                                Console.WriteLine($"{other} matched {this} L:Overhang R:Meeting");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            case Edge.Underhang:
                                Console.WriteLine($"{other} matched {this} L:Overhang R:Underhang");
                                newRanges.Add(other.SplitRight(Max));
                                Console.WriteLine($"\t└applied right split, now {other} AND {newRanges.Last()}");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                        }
                        break;
                    case Edge.Meeting:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                Console.WriteLine($"{other} matched {this} L:Meeting R:Overhang");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            //goto case Edge.Meeting;
                            case Edge.Meeting:
                                Console.WriteLine($"{other} matched {this} L:Meeting R:Meeting");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            case Edge.Underhang:
                                Console.WriteLine($"{other} matched {this} L:Meeting R:Underhang");
                                newRanges.Add(other.SplitRight(Max));
                                Console.WriteLine($"\t└applied right split, now {other} AND {newRanges.Last()}");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                        }
                        break;
                    case Edge.Underhang:
                        switch (GetRightEdgeType(other))
                        {
                            case Edge.Overhang:
                                Console.WriteLine($"{other} matched {this} L:Underhang R:Overhang");
                                newRanges.Add(other.SplitLeft(Min));
                                Console.WriteLine($"\t└applied left split, now {newRanges.Last()} AND {other}");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            //goto case Edge.Meeting;
                            case Edge.Meeting:
                                Console.WriteLine($"{other} matched {this} L:Underhang R:Meeting");
                                newRanges.Add(other.SplitLeft(Min));
                                Console.WriteLine($"\t└applied left split, now {newRanges.Last()} AND {other}");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                            case Edge.Underhang:
                                Console.WriteLine($"{other} matched {this} L:Underhang R:Underhang");
                                newRanges.Add(other.SplitLeft(Min));
                                Console.WriteLine($"\t└applied left split, now {newRanges.Last()} AND {other}");
                                newRanges.Add(other.SplitRight(Max));
                                Console.WriteLine($"\t└applied right split, now {newRanges.Last()} AND {other}");
                                other.ApplyShift(Shift);
                                Console.WriteLine($"\t└applied shift {Shift}, now {other}");
                                break;
                        }
                        break;
                }
                return true;
            }

            public override string ToString()
            {
                return $"R{base.ToString()}s{Shift}";
            }
        }
    }
}
