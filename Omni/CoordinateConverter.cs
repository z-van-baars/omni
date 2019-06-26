using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Omni
{
    class CoordinateConverter
    {
        private Vector2 tileDimensions;
        
        public CoordinateConverter(Vector2 tileDimensions)
        {
            this.tileDimensions = tileDimensions;
        }
        public (float, float) MapToScreen(float x, float y)
        {
            float x2 = ((((x + 1) - (y + 1)) * (tileDimensions.X / 2)) - (tileDimensions.Y / 2));
            float y2 = (((y + x) * (tileDimensions.Y / 2)));
            return (x2, y2);
        }
        public Vector2 ScreenToMap(Vector2 coordinates, Vector2 displayShift)
        {
            // ShiftX = TileWidth * (MapWidth / 2) - TileWidth / 2;
            // float xt = (coordinates.X - displayShift.X) - (background_x_middle - displayShift.X);
            float xt = (coordinates.X - displayShift.X);
            float yt = coordinates.Y - displayShift.Y;
            int column = (int)Math.Floor((xt / tileDimensions.X) + yt / (tileDimensions.Y / 2) / 2);
            int row = (int)Math.Floor((yt / (tileDimensions.Y / 2) - xt / tileDimensions.X) / 2);
            Vector2 mapCoordinates = new Vector2(column, row);
            return mapCoordinates;
        }

    }
}
