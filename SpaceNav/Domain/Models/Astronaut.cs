namespace SpaceNav.Domain.Models;

/// <summary>
/// This class defines the Astronaut, stores their start position as a <see cref="Coordinate"/>
/// as well as a <see cref="List"/> of such <see cref="Coordinate"/>s that is their path to the
/// destination which is the position of the space station managed by the <see cref="CosmicMap"/> class 
/// </summary>
public class Astronaut
{
    public string Id { get; }
    public Coordinate StartPosition { get;}
    public int PathCost { get; private set; }
    public List<Coordinate> PathSteps { get; private set; }
    public bool IsPathFound { get; private set; }
    
    public Astronaut(string id, Coordinate startPosition)
    {
        if(string.IsNullOrEmpty(id))
            throw new ArgumentException("Astronaut id cannot be null or empty", nameof(id));
        Id = id;
        StartPosition = startPosition;
        PathCost = 0;
        IsPathFound = false;
    }
    
    /// <summary>
    /// Sets the path of Astronaut
    /// </summary>
    /// <param name="steps"><see cref="List"/> of <see cref="Coordinate"/>s representing the steps in a path</param>
    /// <param name="pathCost">The cost of the path</param>
    /// <exception cref="ArgumentNullException"></exception>
    public void UpdateCalculatedPath(List<Coordinate> steps, int pathCost)
    {
        PathSteps = steps ?? throw new ArgumentNullException(nameof(steps));
        PathCost = pathCost;
        IsPathFound = true;
    }
    
    /// <summary>
    /// Sets an Astronaut as lost
    /// </summary>
    public void SetAsLost()
    {
        PathSteps = new List<Coordinate>();
        PathCost = int.MaxValue;
        IsPathFound = false;
    }

}