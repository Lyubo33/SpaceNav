using SpaceNav.Domain.Models;

namespace SpaceNav.Services;

public class PathReconstructor
{
    /// <summary>
    /// Given a dictionary and an end coordinate get the path and reverse it and return the list of steps
    /// from start to end
    /// </summary>
    /// <param name="parentTrail"> The trail of steps see <see cref="Dictionary{TKey,TValue}"/></param>
    /// <param name="end"> The <see cref="Coordinate"/> of the destination</param>
    /// <returns>Returns a list of <see cref="Coordinate"/> that comprise the path</returns>
    public List<Coordinate> ReconstructPath(Dictionary<Coordinate, Coordinate> parentTrail, Coordinate end)
    {
        List<Coordinate> steps = new List<Coordinate>();
        Coordinate current = end;
        while (true)
        {
            steps.Add(current);
            if (!parentTrail.TryGetValue(current, out Coordinate previous))
            {
                break;
            }
            current = previous;
        }
        if (steps.Count > 0)
        {
            steps.RemoveAt(steps.Count - 1);
        }
        steps.Reverse();
        return steps;
    }
}