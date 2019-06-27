using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class GameTile
    {
        public int x;
        public int y;
        public string Biome;
        public List<Unit> Units = new List<Unit>();
        public Terrain Terrain;
        public Building Building;

        public GameTile(int x, int y, string Biome)
        {
            this.x = x;
            this.y = y;
            this.Biome = Biome;
        }
        public bool IsPathable()
        {
            return ((Terrain == null || Terrain.pathable)
                    && (Building == null || Building.pathable));
        }
    }
}
