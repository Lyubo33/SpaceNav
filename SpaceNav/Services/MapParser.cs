using SpaceNav.Domain.Models;

namespace SpaceNav.Services;

public class MapParser
{
    private static readonly HashSet<string> validSymbolSet = new() { "X", "0", "D", "F", "S1", "S2", "S3" };

    /// <summary>
    /// Transforms the raw text grid that is input into Domain objects such as Astronaut and
    /// Cosmic Map. Returns the generated CosmicMap object and a list of all astronauts
    /// </summary>
    /// <param name="lines"> The <see cref="List"/> of rows as <see cref="string"/></param>
    /// <param name="rows"> The expected row count</param>
    /// <param name="columns">The expected column count</param>
    /// <returns> Returns the <see cref="CosmicMap"/> object and a <see cref="List"/> of all <see cref="Astronaut"/>
    /// objects</returns>

    public static (CosmicMap Map, List<Astronaut> Astronauts ) ParseMapData(List<string> lines, int rows, int columns)
    {
        if (lines == null || lines.Count != rows)
        {
            throw new FormatException($"Dimesnion mismatch: Expected {rows} rows of data but recieved " +
                                      $"{lines?.Count ?? 0} instead");
        }

        string[,] grid = new string[rows, columns];
        List<Astronaut> astronauts = new List<Astronaut>();
        Coordinate spaceStationPos = default;
        bool isSpaceStationFound = false;

        for (int r = 0; r < rows; r++)
        {
            string[] tokens = lines[r].Split(' ',StringSplitOptions.RemoveEmptyEntries);
            if (tokens.Length != columns)
            {
                throw new FormatException($"Dimension mismatch:Row{r+1} contains {tokens.Length} columns," +
                                          $" but exactly {columns} were specified");
            }

            for (int c = 0; c < columns; c++)
            {
                string token = tokens[c];
                Coordinate coord = new Coordinate(r, c);
                if (!validSymbolSet.Contains(token))
                {
                    throw new FormatException($"Invalid symbol:Symbol {token} is not allowed,");
                }
                grid[r, c] = token;
                if (token == "F")
                {
                    spaceStationPos = coord;
                    isSpaceStationFound = true;
                }
                else if (token == "S1" || token == "S2" || token == "S3")
                {
                    astronauts.Add(new Astronaut(token,coord));
                }
            }
        }
        if (!isSpaceStationFound)
        {
            throw new FormatException("Invalid Map Structure: Missing a Space Station target point ('F').");
        }
        if (astronauts.Count == 0 || astronauts.Count > 3)
        {
            throw new FormatException("Invalid Map Structure: The layout must contain at least one astronaut launch pad (S1, S2, or S3) and up to three.");
        }
        return (new CosmicMap(grid, spaceStationPos), astronauts);
    }
}