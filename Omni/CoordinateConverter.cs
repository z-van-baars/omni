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
        private Vector2 displayDimensions;
        
        public CoordinateConverter(Vector2 tileDimensions, Vector2 displayDimensions)
        {
            this.tileDimensions = tileDimensions;
        }
        public (float, float) MapToScreen(float x, float y)
        {
            /// converts coordinates into raw pixel output coordinates
            float x2 = ((((x + 1) - (y + 1)) * (tileDimensions.X / 2)) - (tileDimensions.Y / 2));
            float y2 = (((y + x) * (tileDimensions.Y / 2)));
            return (x2, y2);
        }
        public Vector2 ScreenToMap(Vector2 coordinates, Vector2 displayShift)
        {
            /// screen pixel coordinates to tile translation math - Don't fuck with this it works right now
            /// still a bit squishy though
            float background_center = tileDimensions.X / 2 + (displayShift.X + displayDimensions.X / 2);

            /// strips out the display shift camera offset
            float xt = (coordinates.X - displayShift.X) - (background_center - displayShift.X);
            float yt = coordinates.Y - displayShift.Y;

            /// converts raw pixel coordinate data into canonical map coordinates
            int column = (int)((xt / (tileDimensions.X / 2) + yt / (tileDimensions.Y / 2)) / 2);
            int row = (int)((yt / (tileDimensions.Y / 2) - xt / (tileDimensions.X / 2)) / 2);

            Vector2 mapCoordinates = new Vector2(column, row);
            return mapCoordinates;
        }

    }
}
