using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omni
{
    public class PriorityQueue<T>
    {
        // I'm using an unsorted array for this example, but ideally this
        // would be a binary heap. There's an open issue for adding a binary
        // heap to the standard C# library: https://github.com/dotnet/corefx/issues/574
        //
        // Until then, find a binary heap class:
        // * https://github.com/BlueRaja/High-Speed-Priority-Queue-for-C-Sharp
        // * http://visualstudiomagazine.com/articles/2012/11/01/priority-queues-with-c.aspx
        // * http://xfleury.github.io/graphsearch.html
        // * http://stackoverflow.com/questions/102398/priority-queue-in-net

        private List<Tuple<T, double>> elements = new List<Tuple<T, double>>();

        public int Count
        {
            get { return elements.Count; }
        }

        public void Enqueue(T item, double priority)
        {
            elements.Add(Tuple.Create(item, priority));
        }

        public T Dequeue()
        {
            int bestIndex = 0;

            for (int i = 0; i < elements.Count; i++)
            {
                if (elements[i].Item2 < elements[bestIndex].Item2)
                {
                    bestIndex = i;
                }
            }

            T bestItem = elements[bestIndex].Item1;
            elements.RemoveAt(bestIndex);
            return bestItem;
        }
    }
    class Pathfinder
    {
        private GameMap gameMap;
        private Point mapDimensions;
        public Pathfinder(GameMap gameMap)
        {
            this.gameMap = gameMap;
            this.mapDimensions = gameMap.MapDimensions;
        }
        private Vector2 exploreFrontier(PriorityQueue<Vector2> frontier, Vector2 target, Dictionary<Vector2, Vector2> cameFrom, Dictionary<Vector2, double> costSoFar)
        {
            Vector2 closestTile = new Vector2();
            Vector2 currentTile;
            double closestDistance = 99999;
            while (frontier.Count > 0)
            {
                currentTile = frontier.Dequeue();
                double currentDistance = Math.Sqrt(Math.Abs(currentTile.X - target.X) + Math.Abs(currentTile.Y - target.Y));
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTile = currentTile;
                }

                List < Vector2 > neighbors = gameMap.game_tiles[(int)currentTile.Y, (int)currentTile.X].NeighborsCoords;
                foreach (Vector2 neighborTile in neighbors)
                {
                    GameTile tileObject = gameMap.game_tiles[(int)neighborTile.Y, (int)neighborTile.X];
                    if (tileObject.IsPathable())
                    {
                        double tileModifier = 1.0;
                        double distanceFromTarget = Math.Sqrt(Math.Abs(tileObject.X - target.X) + Math.Abs(tileObject.Y - target.Y));
                        double newCost = (costSoFar[currentTile] + tileModifier) + distanceFromTarget;
                        if (!cameFrom.ContainsKey(neighborTile) || newCost < costSoFar[currentTile])
                        {
                            cameFrom[neighborTile] = currentTile;
                            costSoFar[neighborTile] = newCost;
                            frontier.Enqueue(neighborTile, newCost);
                        }
                    }
                }
                if (closestTile == target)
                {
                    break;
                }
            }
            return closestTile;
        }
        public List<Vector2> GetPath(Vector2 start, Vector2 target)
        {
            List<Vector2> path = new List<Vector2>();
            Dictionary<Vector2, Vector2> cameFrom = new Dictionary<Vector2, Vector2>();
            Dictionary<Vector2, double> costSoFar = new Dictionary<Vector2, double>();
            Vector2 closestTile = new Vector2();
            cameFrom[start] = start;
            costSoFar[start] = 0;
            PriorityQueue<Vector2> frontier = new PriorityQueue<Vector2>();
            List<Vector2> neighbors = gameMap.game_tiles[(int)start.Y, (int)start.X].NeighborsCoords;
            frontier.Enqueue(start, 0);
            foreach (Vector2 tile in neighbors)
            {
                GameTile tileObject = gameMap.game_tiles[(int)tile.Y, (int)tile.X];
                if (tileObject.IsPathable())
                {
                    frontier.Enqueue(tile, 0);
                }

            }
            closestTile = exploreFrontier(
                frontier, target, cameFrom, costSoFar);
            List<Vector2> newPathSteps = new List<Vector2>();
            newPathSteps.Add(closestTile);

            while (true)
            {
                Vector2 nextTile = newPathSteps[0];
                if (nextTile == start)
                {
                    break;
                }
                newPathSteps.Insert(0, cameFrom[nextTile]);
            }
            return newPathSteps;
        }
    }
}
