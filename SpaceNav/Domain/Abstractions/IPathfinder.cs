using SpaceNav.Domain.Models;

namespace SpaceNav.Domain.Abstractions;

public interface IPathfinder
{
    /// <summary>
    /// Finds the shortest path in a <see cref="CosmicMap"/> to the position of its SpaceStation
    /// from a given start position <see cref="Coordinate"/>
    /// </summary>
    /// <param name="map"></param>
    /// <param name="start"></param>
    /// <returns><see cref="PathResult"/> object</returns>
    PathResult FindPath(CosmicMap map, Coordinate start);
}