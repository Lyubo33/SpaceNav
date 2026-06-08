using SpaceNav.Domain.Abstractions;
using SpaceNav.Domain.Models;

namespace SpaceNav.Services;

/// <summary>
/// Implements Breadth First Search to find the shortest path for a given starting position in a map.
/// </summary>
public class BfsPathfinder : IPathfinder
{
    
    public PathResult FindPath(CosmicMap map, Coordinate start)
    {
        Queue<Coordinate> queue = new Queue<Coordinate>();
        HashSet<Coordinate> visited = new HashSet<Coordinate>();
        Dictionary<Coordinate, Coordinate> parentTrail = new Dictionary<Coordinate, Coordinate>();
        
        if (start.Equals(map.SpaceStationPosition))
        {
            return new PathResult(new List<Coordinate>{start}, 0);
        }
        queue.Enqueue(start);
        visited.Add(start);
        bool isFound = false;
        while (queue.Count > 0)
        {
            Coordinate current = queue.Dequeue();

            foreach (Coordinate dir in CosmicMap.Directions)
            {
                Coordinate neighbour = current.Translate(dir.X, dir.Y);
                if (map.IsValidMove(neighbour) && !visited.Contains(neighbour))
                {
                    visited.Add(neighbour);
                    parentTrail[neighbour] = current;
                    if (neighbour.Equals(map.SpaceStationPosition))
                    {
                        isFound = true;
                        break;
                    }
                    queue.Enqueue(neighbour);
                }
            }

            if (isFound) break;
        }

        if (!isFound) return PathResult.Failed();
        PathReconstructor reconstructor = new PathReconstructor();
        List<Coordinate> path = reconstructor.ReconstructPath(parentTrail, map.SpaceStationPosition);
        return new PathResult(path, path.Count - 1);
    }
}