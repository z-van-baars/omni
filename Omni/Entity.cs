using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class Entity
    {
        public string name;
        public bool pathable = false;
        protected Vector2 coordinates;

        public Entity(Vector2 coordinates, string name)
        {
            this.coordinates = coordinates;
            this.name = name;
        }

        public float X
        {
            get { return coordinates.X; }
        }

        public float Y
        {
            get { return coordinates.Y; }
        }

        public virtual double GetRemaining()
        {
            return 0;
        }
        public virtual void ChangeRemaining(double amountToChange)
        {
        }
        public virtual void Tick(GameMap gameMap, Player Player1, Pathfinder pathfinder)
        {
        }
        public virtual void OnDeath(GameMap gameMap)
        {
        }
        public virtual bool IsExpired()
        {
            return false;
        }
    }
}
