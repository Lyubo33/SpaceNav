using SpaceNav.Domain.Abstractions;
using SpaceNav.Domain.Models;

namespace SpaceNav.Services;
/// <summary>
/// Implements Dijkstra's algorithm to find the shortest path for a given starting position in a map.
/// </summary>
public class DijkstraPathfinder : IPathfinder
{
    public PathResult FindPath(CosmicMap map, Coordinate start)
    {
        PriorityQueue<Coordinate, int> pQueue = new PriorityQueue<Coordinate, int>();
        Dictionary<Coordinate, int> costSoFar = new Dictionary<Coordinate, int>();
        Dictionary<Coordinate, Coordinate> parentTrail = new Dictionary<Coordinate, Coordinate>();
        
        pQueue.Enqueue(start, 0);
        costSoFar[start] = 0;
        bool isFound = false;

        while (pQueue.Count > 0)
        {
            Coordinate current = pQueue.Dequeue();
            if (current.Equals(map.SpaceStationPosition))
            {
                isFound = true;
                break;
            }

            foreach (Coordinate dir in CosmicMap.Directions)
            {
                Coordinate neighbour = current.Translate(dir.X, dir.Y);
                if (map.IsValidMove(neighbour))
                {
                    int stepCost = map.GetCost(neighbour);
                    int newCost = costSoFar[current] + stepCost;
                    if (!costSoFar.ContainsKey(neighbour) || newCost < costSoFar[neighbour])
                    {
                        costSoFar[neighbour] = newCost;
                        parentTrail[neighbour] = current;  
                        pQueue.Enqueue(neighbour, newCost);
                    }
                }
            }
        }

        if (!isFound) return PathResult.Failed();
        PathReconstructor reconstructor = new PathReconstructor();
        List<Coordinate> steps = reconstructor.ReconstructPath(parentTrail,map.SpaceStationPosition);
        return new PathResult(steps, costSoFar[map.SpaceStationPosition]);
    }
}