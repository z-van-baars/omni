using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace Omni
{
    class GameTile
    {
        public string Biome;
        public Unit Unit;
        public Terrain Terrain;
        public Building Building;
        public List<Point> NeighborsCoords;
        public List<GameTile> NeighborsTile;

        public GameTile(Point coordinates, string Biome)
        {
            Coordinates = coordinates;
            this.Biome = Biome;
        }

        public Point Coordinates { get; }

        public bool IsPathable()
        {
            return ((Terrain == null || Terrain.pathable)
                    && (Building == null || Building.pathable)
                    && (Unit == null || Unit.pathable));
        }
        public void SetNeighbors(List<GameTile> neighborTileList, List<Point> neighborCoordList)
        {
            NeighborsCoords = neighborCoordList;
            NeighborsTile = neighborTileList;
        }
    }
}
