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
            this.displayDimensions = displayDimensions;
        }

        /// <summary>
        /// Convert map coordinates into pixel coordinates.
        /// </summary>
        /// <param name="p">Map coordinates</param>
        /// <returns>Pixel coordinates</returns>
        public Point MapToScreen(Point p)
        {
            var x2 = ((((p.X + 1) - (p.Y + 1)) * (tileDimensions.X / 2)) - (tileDimensions.Y / 2));
            var y2 = (((p.Y + p.X) * (tileDimensions.Y / 2)));
            return new Point(x2, y2);
        }
        public Point ScreenToMap(Point coordinates, Point displayShift)
        {
            /// screen pixel coordinates to tile translation math - Don't fuck with this it works right now
            /// still a bit squishy though
            int background_center = tileDimensions.X / 2 + (displayShift.X + displayDimensions.X / 2);

            /// strips out the display shift camera offset
            int xt = (coordinates.X - displayShift.X) - (background_center - displayShift.X);
            int yt = coordinates.Y - displayShift.Y;

            /// converts raw pixel coordinate data into canonical map coordinates
            int column = ((xt / (tileDimensions.X / 2) + yt / (tileDimensions.Y / 2)) / 2);
            int row = ((yt / (tileDimensions.Y / 2) - xt / (tileDimensions.X / 2)) / 2);

            return new Point(column, row);
        }

    }
}
