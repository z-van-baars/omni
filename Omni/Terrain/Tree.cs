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

        public Tree(Vector2 coordinates) : base(coordinates, "Tree")
        {
        }
        public void ChangeWood(double amountToChange)
        {
            wood += amountToChange;
        }
        public double GetWood()
        {
            return wood;
        }
        public void Expire()
        {
            name = "Stump";
        }
        public void Tick()
        {
            if (wood == 0)
            {
                Expire();
            }
            else if (wood < 100)
            {
                name = "Chopped Tree";
            }
        }
    }
}
