using System.Runtime.CompilerServices;

namespace SpaceNav.Domain.Models;

/// <summary>
/// A record struct that defines a coordinate with X corresponding to rows, Y to columns
/// </summary>
/// <param name="X"> row coordinate</param>
/// <param name="Y"> column coordinate</param>
public readonly record struct Coordinate(int X, int Y)
{
    /// <summary>
    /// Method to move the coordinate by row and column
    /// </summary>
    /// <param name="deltaX"> how much by row</param>
    /// <param name="deltaY"> how much by column</param>
    /// <returns> Returns a new <see cref="Coordinate"/> with new X and Y</returns>
    public Coordinate Translate(int deltaX, int deltaY)
    {
        return new Coordinate(this.X + deltaX, this.Y + deltaY);
    }
}