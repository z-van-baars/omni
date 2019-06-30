using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Priority_Queue;

namespace Omni
{

    class FastPoint : FastPriorityQueueNode
    {
        public Point P { get; set; }
        public FastPoint(Point p) { P = p; }
    }

    class Pathfinder
    {
        private GameMap gameMap;
        private Point mapDimensions;

        public Pathfinder(GameMap gameMap)
        {
            this.gameMap = gameMap;
            mapDimensions = gameMap.MapDimensions;
        }

        private Point exploreFrontier(
            FastPriorityQueue<FastPoint> frontier,
            Point target,
            Dictionary<Point, Point> cameFrom,
            Dictionary<Point, float> costSoFar)
        {
            FastPoint closestTile = null;
            FastPoint currentTile;
            double closestDistance = double.PositiveInfinity;
            while (frontier.Count > 0)
            {
                currentTile = frontier.Dequeue();
                double currentDistance = Math.Sqrt(
                    Math.Abs(currentTile.P.X - target.X) +
                    Math.Abs(currentTile.P.Y - target.Y));
                if (currentDistance < closestDistance)
                {
                    closestDistance = currentDistance;
                    closestTile = currentTile;
                }

                foreach (var neighborTile in gameMap.game_tiles[currentTile.P.Y, currentTile.P.X].NeighborsCoords)
                {
                    var tileObject = gameMap.game_tiles[neighborTile.Y, neighborTile.X];
                    if (tileObject.IsPathable())
                    {
                        var tileModifier = 1.0f;
                        var distanceFromTarget = (float)Math.Sqrt(
                            Math.Abs(tileObject.Coordinates.X - target.X) +
                            Math.Abs(tileObject.Coordinates.Y - target.Y));
                        float newCost = (costSoFar[currentTile.P] + tileModifier) + distanceFromTarget;
                        if (!cameFrom.ContainsKey(neighborTile) || newCost < costSoFar[currentTile.P])
                        {
                            cameFrom[neighborTile] = currentTile.P;
                            costSoFar[neighborTile] = newCost;
                            frontier.Enqueue(new FastPoint(neighborTile), newCost);
                        }
                    }
                }
                if (closestTile?.P == target)
                {
                    break;
                }
            }
            return closestTile.P;
        }
        public List<Point> GetPath(Point start, Point target)
        {
            var path = new List<Point>();
            var cameFrom = new Dictionary<Point, Point>();
            var costSoFar = new Dictionary<Point, float>();
            var closestTile = new Point();
            cameFrom[start] = start;
            costSoFar[start] = 0f;
            var frontier = new FastPriorityQueue<FastPoint>(1000);
            var neighbors = gameMap.game_tiles[start.Y, start.X].NeighborsCoords;
            frontier.Enqueue(new FastPoint(start), 0);
            foreach (var tile in neighbors)
            {
                var tileObject = gameMap.game_tiles[tile.Y, tile.X];
                if (tileObject.IsPathable())
                {
                    frontier.Enqueue(new FastPoint(tile), 0);
                }

            }
            closestTile = exploreFrontier(frontier, target, cameFrom, costSoFar);
            var newPathSteps = new List<Point>();
            newPathSteps.Add(closestTile);

            while (true)
            {
                var nextTile = newPathSteps[0];
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
