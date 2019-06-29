using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class GameTile
    {
        public int X;
        public int Y;
        public string Biome;
        public Unit Unit;
        public Terrain Terrain;
        public Building Building;
        public List<Vector2> NeighborsCoords;
        public List<GameTile> NeighborsTile;

        public GameTile(int X, int Y, string Biome)
        {
            this.X = X;
            this.Y = Y;
            this.Biome = Biome;
        }
        public bool IsPathable()
        {
            return ((Terrain == null || Terrain.pathable)
                    && (Building == null || Building.pathable)
                    && (Unit == null || Unit.pathable));
        }
        public void SetNeighbors(List<GameTile> neighborTileList, List<Vector2> neighborCoordList)
        {
            NeighborsCoords = neighborCoordList;
            NeighborsTile = neighborTileList;
        }
    }
}
