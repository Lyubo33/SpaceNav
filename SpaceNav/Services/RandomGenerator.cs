using SpaceNav.Domain.Models;

namespace SpaceNav.Services;


/// <summary>
/// This class is used as a service that provides functionality to generate a random grid
/// </summary>
public class RandomGenerator
{
    private static readonly Random _random = new();
    
    /// <summary>
    /// Generates a random map/grid
    /// </summary>
    /// <param name="rows"> The rows of the map</param>
    /// <param name="columns">The columns of the map</param>
    /// <param name="asteroidCount">The number of the asteroids on it</param>
    /// <returns><see cref="List"/> of <see cref="string"/> which represents the grid</returns>
    public List<string> GenerateRandomMapLines(int rows, int columns, int asteroidCount)
    {
        int astronautCount = _random.Next(1, 4);
        bool includeDebris = _random.Next(0, 2) == 0;
        string[,] grid = new string[rows, columns];

        HashSet<Coordinate> occupied = new HashSet<Coordinate>();
        Coordinate spaceStation = GetUniqueCoordinate(rows, columns, occupied);
        grid[spaceStation.X, spaceStation.Y] = "F";

        for (int i = 0; i < astronautCount; i++)
        {
            Coordinate astronautPos = GetUniqueCoordinate(rows, columns, occupied);
            grid[astronautPos.X, astronautPos.Y] = $"S{i+1}";
        }

        for (int i = 0; i < asteroidCount; i++)
        {
            Coordinate asteroidPos = GetUniqueCoordinate(rows, columns, occupied);
            grid[asteroidPos.X, asteroidPos.Y] = "X";
        }

        if (includeDebris)
        {
            int totalTiles = rows * columns;
            int debrisCount = Math.Max(1, totalTiles / 10);
            int maxPossibleDebris = totalTiles - occupied.Count;
            int finalDebrisCount = Math.Min(debrisCount, maxPossibleDebris);

            for (int i = 0; i < finalDebrisCount; i++)
            {
                Coordinate debrisPos = GetUniqueCoordinate(rows, columns, occupied);
                grid[debrisPos.X, debrisPos.Y] = "D";
            }
        }

        List<string> lines = new List<string>();
        for (int r = 0; r < rows; r++)
        {
            List<string> rowElements = new List<string>();
            for (int c = 0; c < columns; c++)
            {
                if (grid[r, c] == null)
                {
                    grid[r, c] = "0";
                }
                rowElements.Add(grid[r, c]);
            }
            lines.Add(string.Join(" ", rowElements));
        }
        return lines;
    }

    /// <summary>
    /// Explicitly handles the generation of unique coordinates on the map grid.
    /// Passes the occupied set by reference to track global map state.
    /// </summary>
    private static Coordinate GetUniqueCoordinate(int maxRows, int maxCols, HashSet<Coordinate> occupied)
    {
        while (true)
        {
            int r = _random.Next(0, maxRows);
            int c = _random.Next(0, maxCols);
            Coordinate coord = new Coordinate(r, c);

            if (!occupied.Contains(coord))
            {
                occupied.Add(coord);
                return coord;
            }
        }
    }

}