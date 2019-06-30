using Microsoft.Xna.Framework;

namespace Omni
{
    abstract class Entity
    {
        public string name;
        public bool pathable = false;

        public Entity(Point coordinates, string name)
        {
            Coordinates = coordinates;
            this.name = name;
        }

        public Point Coordinates
        {
            get;
            protected set;
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
