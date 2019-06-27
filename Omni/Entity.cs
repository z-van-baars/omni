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
