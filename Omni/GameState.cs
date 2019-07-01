using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni
{
    class GameState
    {
        public GameMap gameMap;
        public Player Player1;
        private Pathfinder pathfinder;
        public GameState(Point MapDimensions)
        {
            Player1 = new Player();

            gameMap = new GameMap(MapDimensions);
            gameMap.GenerateMapArray();
            gameMap.SetAllTileNeighbors();
            pathfinder = new Pathfinder(gameMap);
            gameMap.PrimitiveMapGen();
        }
    }
}
