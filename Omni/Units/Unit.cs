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
        protected Vector2? target;
        protected List<Vector2> path;

        public Unit(Vector2 coordinates, string name, int moveCounterBase) : base(coordinates, name)
        {
            this.moveCounterBase = moveCounterBase;
            moveCounter = moveCounterBase;
            pathable = true;
            target = null;
        }
        protected void MoveIncrement()
        {
            moveCounter -= 1;
        }
        protected void Move()
        {
            MoveIncrement();
            if (moveCounter == 0)
            {
                /// reset the move counter, and move IMMEDIATELY to the next position in the path
                moveCounter = moveCounterBase;
                coordinates = path[0];
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
        public void SetTargetClosest(List<Entity> targetsList)
        {
            double closestDistance = 99999;
            Entity closestChoice = new Entity(new Vector2());
            foreach (Entity possibleChoice in targetsList)
            {
                double distanceToChoice = Math.Sqrt(Math.Abs(coordinates.X - possibleChoice.Get_X()) + Math.Abs(coordinates.Y - possibleChoice.Get_Y()));
                if (distanceToChoice < closestDistance)
                {
                    closestDistance = distanceToChoice;
                    closestChoice = possibleChoice;
                }
            }
            target = new Vector2(closestChoice.Get_X(), closestChoice.Get_Y());
        }
        public Vector2? GetTarget()
        {
            return target;
        }
        public void SetPath(List<Vector2> Path)
        {
            path = Path;
        }
        public List<Vector2> GetPath()
        {
            return path;
        }
        public virtual void Tick(GameMap gameMap, Player Player1, Pathfinder pathfinder)
        {
            if (target.HasValue)
            {
                if (path != null)
                {
                    Move();
                }

                else if (gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                {
                    target = null;
                }
            }
            else
            {
                SetTargetClosest(gameMap.GetTerrain());
            }
        }
    }
}
