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
        public string name;
        private int moveCounterBase;
        private int moveCounter;
        protected Vector2? target;
        protected List<Vector2> path;

        public Unit(Vector2 coordinates, string name, int moveCounterBase) : base(coordinates)
        {
            this.name = name;
            this.moveCounterBase = moveCounterBase;
            moveCounter = moveCounterBase;
            pathable = true;
            target = new Vector2(0, 0);
        }
        private void moveIncrement()
        {
            moveCounter -= 1;
        }
        private void move()
        {
            moveIncrement();
            if (moveCounter == 0)
            {
                moveCounter = moveCounterBase;
                coordinates = path[0];
                path = path.GetRange(1, path.Count() - 1);
                if (coordinates == target)
                {
                    path = null;
                }
            }
        }
        public void SetTarget(List<Terrain> targetsList)
        {
            Random random = new Random();
            Entity choice = targetsList[random.Next(targetsList.Count)];
            target = new Vector2(choice.Get_X(), choice.Get_Y());
        }
        public Vector2? GetTarget()
        {
            return target;
        }
        public void SetPath(List<Vector2> Path)
        {
            path = Path;
        }
        public void Tick(GameMap gameMap)
        {
            if (target.HasValue)
            {
                if (path != null)
                {
                    move();
                }
                else if (gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                {
                    target = null;
                }
            }
        }
    }
}
