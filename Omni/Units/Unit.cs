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

        public Unit(Vector2 coordinates, string name) : base(coordinates)
        {
            this.name = name;
        }
    }
}
