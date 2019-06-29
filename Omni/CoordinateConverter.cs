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
        private Point tileDimensions;
        private Point displayDimensions;
        
        public CoordinateConverter(Point tileDimensions, Point displayDimensions)
        {
            this.tileDimensions = tileDimensions;
        }

        public Point MapToScreen(Point v)
        {
            /// converts coordinates into raw pixel output coordinates
            var x2 = ((((v.X + 1) - (v.Y + 1)) * (tileDimensions.X / 2)) - (tileDimensions.Y / 2));
            var y2 = (((v.X + v.Y) * (tileDimensions.Y / 2)));
            return new Point(x2, y2);
        }

        public Point ScreenToMap(Point coordinates, Point displayShift)
        {
            /// screen pixel coordinates to tile translation math - Don't fuck with this it works right now
            /// still a bit squishy though
            var background_center = tileDimensions.X / 2 + (displayShift.X + displayDimensions.X / 2);

            /// strips out the display shift camera offset
            var xt = (coordinates.X - displayShift.X) - (background_center - displayShift.X);
            var yt = coordinates.Y - displayShift.Y;

            /// converts raw pixel coordinate data into canonical map coordinates
            int column = (int)((xt / (tileDimensions.X / 2) + yt / (tileDimensions.Y / 2)) / 2);
            int row = (int)((yt / (tileDimensions.Y / 2) - xt / (tileDimensions.X / 2)) / 2);

            return new Point(column, row);
        }

    }
}
