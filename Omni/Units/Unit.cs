using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class Unit : Entity
    {
        private int moveCounterBase;
        private int moveCounter;
        protected Entity target;
        protected List<Point> path;

        public Unit(Point coordinates, string name, int moveCounterBase) : base(coordinates, name)
        {
            this.moveCounterBase = moveCounterBase;
            moveCounter = moveCounterBase;
            pathable = false;
            target = null;
        }
        protected void MoveIncrement()
        {
            moveCounter -= 1;
        }
        protected bool CheckPath(GameMap gameMap)
        {
            return gameMap.game_tiles[path[0].Y, path[0].X].IsPathable();
        }
        
        protected void Move(GameMap gameMap)
        {
            MoveIncrement();
            if (moveCounter == 0)
            {
                /// reset the move counter, and move IMMEDIATELY to the next position in the path
                moveCounter = moveCounterBase;
                gameMap.game_tiles[Coordinates.Y, Coordinates.X].Unit = null;
                Coordinates = path[0];
                gameMap.game_tiles[Coordinates.Y, Coordinates.X].Unit = this;
                /// !Important Note!: this statement doesn't test if it has arrived at the target!
                /// It only tests to see if the path list it was given by the pathfinder object
                /// has any more steps in it!
                if (path.Count() == 1)
                {
                    path = null;
                }
                else
                {
                    path = path.GetRange(1, path.Count() - 1);
                }
            }
        }
        public void SetTargetClosest(List<Entity> targetsList, Type type)
        {
            double closestDistance = double.PositiveInfinity;
            var closestChoice = targetsList.First();
            foreach (var possibleChoice in targetsList.Skip(1))
            {
                if (possibleChoice.GetType() == type)
                {
                    double distanceToChoice = Math.Sqrt(
                        Math.Abs(Coordinates.X - possibleChoice.Coordinates.X) +
                        Math.Abs(Coordinates.Y - possibleChoice.Coordinates.Y));

                    if (distanceToChoice < closestDistance)
                    {
                        closestDistance = distanceToChoice;
                        closestChoice = possibleChoice;
                    }
                }

            }
            target = closestChoice;
        }
        public Entity GetTarget()
        {
            return target;
        }
        public void SetPath(List<Point> Path)
        {
            path = Path;
        }
        public List<Point> GetPath()
        {
            return path;
        }
    }
}
