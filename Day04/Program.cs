var grid = GridReader.ReadFromFile("../../../input.txt");
var patternFinder = new PatternFinder(grid);

// Find regular XMAS patterns
var (totalMatches, markedGrid) = patternFinder.FindXMASPatterns();
Console.WriteLine($"Total XMAS matches: {totalMatches}");

// Find diagonal MAS patterns
var diagonalMatches = patternFinder.FindDiagonalMASPatterns();
Console.WriteLine($"Total diagonal MAS matches: {diagonalMatches}");

static class GridReader
{
    public static char[,] ReadFromFile(string filePath)
    {
        var lines = File.ReadAllLines(filePath);
        var grid = new char[lines.Length, lines[0].Length];

        for (int i = 0; i < lines.Length; i++)
        {
            for (int j = 0; j < lines[0].Length; j++)
            {
                grid[i, j] = lines[i][j];
            }
        }

        return grid;
    }
}

class PatternFinder
{
    private readonly char[,] _grid;
    private readonly int _rows;
    private readonly int _cols;

    private static readonly char[] XMAS_PATTERN = "XMAS".ToCharArray();
    private static readonly char[] MAS_PATTERN = "MAS".ToCharArray();

    public PatternFinder(char[,] grid)
    {
        _grid = grid;
        _rows = grid.GetLength(0);
        _cols = grid.GetLength(1);
    }

    public (int matches, char[,] markedGrid) FindXMASPatterns()
    {
        var matches = 0;
        var markedGrid = (char[,])_grid.Clone();

        for (int row = 0; row < _rows; row++)
        {
            for (int col = 0; col < _cols; col++)
            {
                foreach (Direction direction in Enum.GetValues(typeof(Direction)))
                {
                    if (CheckXMASPattern(row, col, direction, out var positions))
                    {
                        matches++;
                        foreach (var (x, y) in positions)
                        {
                            markedGrid[x, y] = 'O';
                        }
                    }
                }
            }
        }

        return (matches, markedGrid);
    }

    public int FindDiagonalMASPatterns()
    {
        var matches = 0;

        for (int row = 1; row < _rows - 1; row++)
        {
            for (int col = 1; col < _cols - 1; col++)
            {
                foreach (var direction in new[] { Direction.Up, Direction.Down, Direction.Left, Direction.Right })
                {
                    if (CheckDiagonalMASPattern(row, col, direction))
                    {
                        matches++;
                    }
                }
            }
        }

        return matches;
    }

    private bool CheckXMASPattern(int row, int col, Direction direction, out List<(int x, int y)> positions)
    {
        positions = GetPositionsForDirection(row, col, direction, XMAS_PATTERN.Length);

        if (!ArePositionsValid(positions))
            return false;

        return positions.Select((pos, index) => _grid[pos.x, pos.y] == XMAS_PATTERN[index]).All(match => match);
    }

    private bool CheckDiagonalMASPattern(int row, int col, Direction direction)
    {
        var diagonalPositions = GetDiagonalPositions(row, col, direction);

        var isValid = true;
        foreach (var positions in diagonalPositions)
        {
            if (positions.All(pos => IsValidPosition(pos.x, pos.y)) &&
                positions.Select((pos, index) => _grid[pos.x, pos.y] == MAS_PATTERN[index]).All(match => match))
            {
                isValid = true;
            }
            else
            {
                isValid = false;
                break;
            }
        }

        return isValid;
    }

    private bool IsValidPosition(int row, int col)
    {
        return row >= 0 && row < _rows && col >= 0 && col < _cols;
    }

    private bool ArePositionsValid(List<(int x, int y)> positions)
    {
        return positions.All(pos => IsValidPosition(pos.x, pos.y));
    }

    private List<(int x, int y)> GetPositionsForDirection(int row, int col, Direction direction, int length)
    {
        return direction switch
        {
            Direction.Up => Enumerable.Range(0, length).Select(i => (row - i, col)).ToList(),
            Direction.Down => Enumerable.Range(0, length).Select(i => (row + i, col)).ToList(),
            Direction.Left => Enumerable.Range(0, length).Select(i => (row, col - i)).ToList(),
            Direction.Right => Enumerable.Range(0, length).Select(i => (row, col + i)).ToList(),
            Direction.UpLeft => Enumerable.Range(0, length).Select(i => (row - i, col - i)).ToList(),
            Direction.UpRight => Enumerable.Range(0, length).Select(i => (row - i, col + i)).ToList(),
            Direction.DownLeft => Enumerable.Range(0, length).Select(i => (row + i, col - i)).ToList(),
            Direction.DownRight => Enumerable.Range(0, length).Select(i => (row + i, col + i)).ToList(),
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }

    private (int x, int y)[][] GetDiagonalPositions(int row, int col, Direction direction)
    {
        return direction switch
        {
            Direction.Right => new[]
            {
                new[] { (row - 1, col - 1), (row, col), (row + 1, col + 1) },
                new[] { (row + 1, col - 1), (row, col), (row - 1, col + 1) }
            },
            Direction.Left => new[]
            {
                new[] { (row - 1, col + 1), (row, col), (row + 1, col - 1) },
                new[] { (row + 1, col + 1), (row, col), (row - 1, col - 1) }
            },
            Direction.Up => new[]
            {
                new[] { (row + 1, col + 1), (row, col), (row - 1, col - 1) },
                new[] { (row + 1, col - 1), (row, col), (row - 1, col + 1) }
            },
            Direction.Down => new[]
            {
                new[] { (row - 1, col + 1), (row, col), (row + 1, col - 1) },
                new[] { (row - 1, col - 1), (row, col), (row + 1, col + 1) }
            },
            _ => throw new ArgumentOutOfRangeException(nameof(direction))
        };
    }
}

enum Direction
{
    Up,
    Down,
    Left,
    Right,
    UpLeft,
    UpRight,
    DownLeft,
    DownRight
}