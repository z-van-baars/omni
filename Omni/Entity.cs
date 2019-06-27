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
        protected Vector2 coordinates;
        public bool pathable = false;

        public Entity(Vector2 coordinates)
        {
            this.coordinates = coordinates;
        }
        public float Get_X()
        {
            return coordinates.X;
        }
        public float Get_Y()
        {
            return coordinates.Y;
        }
    }
}
