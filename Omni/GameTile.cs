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
        private int x;
        private int y;
        private string biome;

        public GameTile(int x, int y, string biome)
        {
            this.x = x;
            this.y = y;
            this.biome = biome;
        }
    }
}
