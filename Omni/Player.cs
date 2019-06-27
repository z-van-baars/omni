using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class Player
    {
        private double wood = 0;

        public double GetWood()
        {
            return wood;
        }
        public void SetWood(double newWood)
        {
            wood = newWood;
        }
        public void AddWood(double incomingWood)
        {
            wood += incomingWood;
        }
    }
}
