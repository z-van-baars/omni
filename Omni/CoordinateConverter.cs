using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class CoordinateConverter
    {
        private int tileWidth;
        private int tileHeight;
        
        public CoordinateConverter(int tileWidth, int tileHeight)
        {
            this.tileWidth = tileWidth;
            this.tileHeight = tileHeight;
        }
        public (int, int) MapToScreen(int x, int y)
        {
            int x2 = ((((x + 1) - (y + 1)) * (tileWidth / 2)) - (tileHeight / 2));
            int y2 = (((y + x) * (tileHeight / 2)));
            return (x2, y2);
        }
    }
}
