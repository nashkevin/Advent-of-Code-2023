namespace Days.Day10
{
    internal static class Shared
    {
        internal static (int x, int y) GetStartingPoint(string[] map)
        {
            var point = (x: 0, y: 0);
            for (int i = 0; i < map.Length; i++)
            {
                for (int j = 0; j < map[i].Length; j++)
                {
                    if (map[i][j] == 'S')
                    {
                        point.y = i;
                        point.x = j;
                        break;
                    }
                }
            }
            return point;
        }

        internal static Direction GetNextDirection(string[] map, int x, int y, Direction? previousDirection = null)
        {
            foreach (Direction direction in Direction.Cardinals)
            {
                (int tx, int ty) = direction.GetStep(x, y);
                if (ty < 0 || map.Length <= ty ||
                    tx < 0 || map[ty].Length <= tx)
                {
                    continue;
                }
                if (!direction.GetReverse().Equals(previousDirection) &&
                    Pipe.GetCanExitPipe(direction, Pipe.GetPipe(map[y][x])) &&
                    Pipe.GetCanEnterPipe(direction, Pipe.GetPipe(map[ty][tx])))
                {
                    return direction;
                }
            }
            throw new Exception("Dead End");
        }


        internal class Pipe
        {
            private static readonly Dictionary<char, Pipe> charToPipe = new()
            {
                { '|', new(Direction.North, Direction.South) },
                { '-', new(Direction.East, Direction.West) },
                { 'L', new(Direction.North, Direction.East) },
                { 'J', new(Direction.North, Direction.West) },
                { '7', new(Direction.South, Direction.West) },
                { 'F', new(Direction.East, Direction.South) },
                { '.', new() },
                { 'S', new(Direction.North, Direction.East, Direction.South, Direction.West) },
            };

            internal static Pipe GetPipe(char c) => charToPipe[c];

            internal static bool GetCanExitPipe(Direction direction, Pipe pipe)
            {
                return pipe.Directions.Contains(direction);
            }

            internal static bool GetCanEnterPipe(Direction direction, Pipe pipe)
            {
                return pipe.Directions.Contains(direction.GetReverse());
            }


            internal HashSet<Direction> Directions { get; init; }

            internal Pipe(params Direction[] directions)
            {
                Directions = new(directions);
            }
        }


        internal class Direction
        {
            internal static readonly Direction North = new((0, -1));
            internal static readonly Direction East = new((1, 0));
            internal static readonly Direction South = new((0, 1));
            internal static readonly Direction West = new((-1, 0));
            internal static readonly Direction[] Cardinals = { North, East, South, West };

            internal static Direction GetReverse(Direction direction)
            {
                return new((direction.Transform.x * -1, direction.Transform.y * -1));
            }


            internal (int x, int y) Transform { get; init; }

            private Direction((int, int) transform) => Transform = transform;

            internal Direction GetReverse() => GetReverse(this);

            internal void DoStep(ref int x, ref int y)
            {
                x += Transform.x;
                y += Transform.y;
            }

            internal (int x, int y) GetStep(int x, int y)
            {
                return (x + Transform.x, y + Transform.y);
            }

            public override bool Equals(object? obj)
            {
                return obj is Direction other && Transform.Equals(other.Transform);
            }

            public override int GetHashCode() => Transform.GetHashCode();
        }
    }
}
