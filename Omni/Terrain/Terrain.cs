﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omni
{
    class Terrain : Entity
    {
        public string name;
        public Terrain(Vector2 coordinates, string name) : base(coordinates)
        {
            this.name = name;
        }
    }
}