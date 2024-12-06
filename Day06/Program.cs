using System.Text;

string[] lines = File.ReadAllLines("input.txt");
var map = new Map(lines);

Console.WriteLine("Part 1 - Simulating guard movement:");
int distinctPositions = map.CountDistinctPositionsVisited(true);
Console.WriteLine($"\nPart 1: {distinctPositions} distinct positions visited");

Console.WriteLine("\nPart 2 - Finding loop positions:");
int loopPositions = map.CountPossibleLoopPositions();
Console.WriteLine($"\nPart 2: {loopPositions} possible positions for obstruction");

class Map
{
    private char[,] grid;
    private (int row, int col) startPos;
    private char startDir;
    private int rows, cols;
    private const int MAX_STEPS = 100000;
    private HashSet<(int, int)> visitedPositions = new();

    public Map(string[] lines)
    {
        rows = lines.Length;
        cols = lines[0].Length;
        grid = new char[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                grid[i, j] = lines[i][j];
                if (lines[i][j] is '^' or 'v' or '<' or '>')
                {
                    startPos = (i, j);
                    startDir = lines[i][j];
                    grid[i, j] = '.';
                }
            }
        }
    }

    private void PrintCurrentState((int row, int col) pos, char dir, Dictionary<(int, int, char), int> visits = null)
    {
        Console.SetCursorPosition(0, Console.CursorTop);
        var sb = new StringBuilder();
        sb.AppendLine();

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if ((i, j) == pos && IsInBounds(pos))
                {
                    sb.Append(dir);
                }
                else if (visits != null && visits.Keys.Any(k => k.Item1 == i && k.Item2 == j))
                {
                    var directions = visits.Keys.Where(k => k.Item1 == i && k.Item2 == j)
                        .Select(k => k.Item3)
                        .ToList();

                    if (directions.Count > 1 || visits[(i, j, directions[0])] > 1)
                    {
                        sb.Append('+');
                    }
                    else
                    {
                        sb.Append(directions[0] is '^' or 'v' ? '|' : '-');
                    }
                }
                else if (visitedPositions.Contains((i, j)))
                {
                    sb.Append('X');
                }
                else
                {
                    sb.Append(grid[i, j]);
                }
            }

            sb.AppendLine();
        }

        Console.Write(sb.ToString());
        // Thread.Sleep(100);
    }

    private bool IsInBounds((int row, int col) pos)
    {
        return pos.row >= 0 && pos.row < rows && pos.col >= 0 && pos.col < cols;
    }

    private (int row, int col) GetNextPosition((int row, int col) pos, char dir)
    {
        return dir switch
        {
            '^' => (pos.row - 1, pos.col),
            'v' => (pos.row + 1, pos.col),
            '<' => (pos.row, pos.col - 1),
            '>' => (pos.row, pos.col + 1),
            _ => pos
        };
    }

    private char TurnRight(char dir)
    {
        return dir switch
        {
            '^' => '>',
            '>' => 'v',
            'v' => '<',
            '<' => '^',
            _ => dir
        };
    }

    private bool HasObstacle((int row, int col) pos)
    {
        return IsInBounds(pos) && grid[pos.row, pos.col] == '#';
    }

    public int CountDistinctPositionsVisited(bool visualize = false)
    {
        visitedPositions.Clear();
        var pos = startPos;
        var dir = startDir;

        while (IsInBounds(pos))
        {
            visitedPositions.Add(pos);
            if (visualize)
            {
                PrintCurrentState(pos, dir);
            }

            var nextPos = GetNextPosition(pos, dir);
            if (!IsInBounds(nextPos))
            {
                break;
            }

            if (HasObstacle(nextPos))
            {
                dir = TurnRight(dir);
            }
            else
            {
                pos = nextPos;
            }
        }

        return visitedPositions.Count;
    }

    public int CountPossibleLoopPositions()
    {
        int count = 0;
        int total = rows * cols;
        int current = 0;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                current++;
                if (grid[i, j] == '.' && (i != startPos.row || j != startPos.col))
                {
                    Console.Write(
                        $"\rTesting position {current}/{total} ({(current * 100.0 / total):F1}%) - Found {count} loops");
                    grid[i, j] = '#';
                    if (CreatesLoop((i, j), out var visits))
                    {
                        count++;
                        // PrintCurrentState((i, j), 'O', visits);
                        Console.WriteLine($"\nFound loop at position ({i}, {j})");
                        // Thread.Sleep(500);
                    }

                    grid[i, j] = '.';
                }
            }
        }

        Console.WriteLine();
        return count;
    }

    private bool CreatesLoop((int row, int col) obstaclePos, out Dictionary<(int, int, char), int> visits)
    {
        visits = new Dictionary<(int, int, char), int>();
        var pos = startPos;
        var dir = startDir;
        int steps = 0;

        while (IsInBounds(pos) && steps < MAX_STEPS)
        {
            var state = (pos.row, pos.col, dir);
            if (!visits.TryAdd(state, 1))
            {
                visits[state]++;
                return true; // Same position and direction - it's a true loop
            }

            var nextPos = GetNextPosition(pos, dir);
            if (!IsInBounds(nextPos))
            {
                return false; // Guard moves off the map - not a loop
            }

            if (HasObstacle(nextPos) || nextPos == obstaclePos)
            {
                dir = TurnRight(dir);
            }
            else
            {
                pos = nextPos;
            }

            steps++;
        }

        return false;
    }
}