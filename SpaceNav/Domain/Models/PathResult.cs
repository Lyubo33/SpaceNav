namespace SpaceNav.Domain.Models;

/// <summary>
/// Defines the object that holds the result from a path calculation
/// Holds the total cost of a path and a <see cref="List"/>
/// of <see cref="Coordinate"/>s which represents the steps in the path.
/// Also holds a boolean value to indicate if there even is a path at all.
/// </summary>
public class PathResult
{
    public bool Success { get; }
    public List<Coordinate> Steps { get; }
    public int TotalCost { get; }

    public PathResult(List<Coordinate> steps,int totalCost)
    {
        Success = true;
        Steps = steps ?? new List<Coordinate>();
        TotalCost = totalCost;
    }

    public static PathResult Failed() => 
        new PathResult(new List<Coordinate>(), int.MaxValue, false);
    private PathResult(List<Coordinate> steps, int totalCost, bool success)
    {
        Success = success;
        Steps = steps;
        TotalCost = totalCost;
    }
}