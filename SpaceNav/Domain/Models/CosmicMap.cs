namespace SpaceNav.Domain.Models;

/// <summary>
/// This class defines the Space that contains all <see cref="Astronaut"/>s,
/// the End position and possible directions to move in as <see cref="Coordinate"/>.
/// It stores the underlying grid and calculates the cost of cells or validates potential moves
/// </summary>
public class CosmicMap
{
    public string[,] Grid { get; }
    public int Rows { get; }
    public int Columns { get; }
    public Coordinate SpaceStationPosition { get; }
    
    /// <summary>
    /// All possible directions to move in
    /// </summary>
    public static readonly Coordinate[] Directions = new[]
    {
        new Coordinate(0, -1),
        new Coordinate(0, 1),
        new Coordinate(-1, 0),
        new Coordinate(1, 0)
    };

    public CosmicMap(string[,] grid, Coordinate spaceStationPosition)
    {
        Grid = grid ?? throw new ArgumentNullException(nameof(grid));
        Rows = grid.GetLength(0);
        Columns = grid.GetLength(1);
        SpaceStationPosition = spaceStationPosition;
    }
    /// <summary>
    /// Validate if a move is correct or not
    /// </summary>
    /// <param name="target"> the target coordinate</param>
    /// <returns> A boolean value representing whether the move to the given <see cref="Coordinate"/> is valid</returns>
    public bool IsValidMove(Coordinate target)
    {
        if (target.X < 0 || target.X >= Rows || target.Y < 0 || target.Y >= Columns)
        {
            return false;
        }
        string symbol = Grid[target.X, target.Y];
        if (symbol == "X")
        {
            return false;    
        }
        return true;
    }
    
    /// <summary>
    /// Get the cost of a cell by a given <see cref="Coordinate"/>
    /// </summary>
    /// <param name="target"></param>
    /// <returns>Return the value of the cost as an integer, either 1 or 2</returns>
    public int GetCost(Coordinate target)
    {
        string symbol = Grid[target.X,target.Y];
        return symbol == "D" ? 2 : 1;
    }
    /// <summary>
    /// Check if map has debris or not
    /// </summary>
    /// <returns>True if map has debris, False if not</returns>
    public bool HasDebris()
    {
        for (int r = 0; r < Rows; r++)
        {
            for (int c = 0; c < Columns; c++)
            {
                if (Grid[r, c] == "D")
                {
                    return true;
                }
            }
        }
        return false;
    }
}