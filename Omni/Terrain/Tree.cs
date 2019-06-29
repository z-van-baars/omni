using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class Tree : Terrain
    {
        private double wood = 100;

        public Tree(Point coordinates) : base(coordinates, "Tree")
        {
        }

        public override void ChangeRemaining(double amountToChange)
        {
            wood += amountToChange;
        }

        public override double GetRemaining()
        {
            return wood;
        }

        public override void OnDeath(GameMap gameMap)
        {
            Stump newStump = new Stump(Coordinates);
            gameMap.GetTerrain().Add(newStump);
        }

        public override void Tick(GameMap gameMap, Player Player1, Pathfinder pathfinder)
        {
            if (wood < 100 && name != "Chopped Tree")
            {
                name = "Chopped Tree";
            }
        }

        public override bool IsExpired()
        {
            return wood <= 0;
        }
    }
}
