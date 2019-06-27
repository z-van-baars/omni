using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class GameMap
    {
        public Point MapDimensions;
        public GameTile[,] game_tiles;

        public GameMap(Point MapDimensions)
        {
            this.MapDimensions = MapDimensions;
        }
        public void GenerateMapArray()
        {
            game_tiles = new GameTile[MapDimensions.Y, MapDimensions.X];
            for (int y = 0; y < MapDimensions.Y; y++)
            {
                for (int x = 0; x < MapDimensions.Y; x++)
                {
                    GameTile gameTile = new GameTile(x, y, "Grass");
                    game_tiles[y, x] = gameTile;
                }
            }
        }
        public bool IsPointInside(Vector2 mapCoordinates)
        {
            if (mapCoordinates.X >= 0
                && mapCoordinates.X < MapDimensions.X
                && mapCoordinates.Y >= 0
                && mapCoordinates.Y < MapDimensions.Y)
            {
                return true;
            }
            return false;
        }
        public List<Vector2> GetValidNeighbors(Vector2 coordinates)
        {
            List<Vector2> validNeighbors = new List<Vector2>();
            List<Vector2> candidateNeighbors = new List<Vector2>()
            {
                new Vector2(-1, -1),
                new Vector2(0, -1),
                new Vector2(1, -1),
                new Vector2(-1, 0),
                new Vector2(1, 0),
                new Vector2(-1, 1),
                new Vector2(0, 1),
                new Vector2(1, 1)
            };
            foreach (Vector2 altPoint in candidateNeighbors)
            {
                if (coordinates.X + altPoint.X >= 0
                    && coordinates.X + altPoint.X < MapDimensions.X
                    && coordinates.Y + altPoint.Y >= 0
                    && coordinates.Y + altPoint.Y < MapDimensions.Y)
                {
                    validNeighbors.Add(new Vector2(coordinates.X + altPoint.X, coordinates.Y + altPoint.Y));
                }
            }
            return validNeighbors;
        }
    }
}
