using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Omni.Units
{
    public enum State { Idle, Chop, Dump, Move}

    class Laborer : Unit
    {
        private State currentState;
        private int chopTimerBase = 5;
        private int chopTimer = 15;
        private double wood = 0;
        private int maxWood = 10;
        public Laborer(Vector2 coordinates) : base(coordinates, "Laborer", 5)
        {
            currentState = State.Idle;
        }
        public override void Tick(GameMap gameMap, Player Player1, Pathfinder pathfinder)
        {
            switch (currentState)
            {
                /// idle state
                case State.Idle:
                    /// if I'm idling, look for nearby trees to chopum
                    if (gameMap.GetTerrain().Count() != 0)
                    {
                        currentState = State.Chop;
                    }
                    break;
                /// chop state
                case State.Chop:
                    if (target.HasValue)
                    {
                        /// if I have a target, do I have a path?
                        if (path != null)
                        {
                            Move();
                        }
                        /// if I have a null path, is my target in a neighboring tile?
                        else if (path == null
                            && gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                        {
                            chopTimer -= 1;
                            if (chopTimer == 0
                                && target.GetWood() > 0)
                            {
                                chopTimer = chopTimerBase;
                                target.ChangeWood(-1);
                                wood += 1;
                                if (wood == maxWood)
                                {
                                    target = null;
                                    currentState = State.Dump;
                                }
                                if (target.GetWood() = 0)
                                {
                                    target = null;
                                    currentState = State.Dump;
                                }

                            }

                        }
                        /// If I have a null path and I have a target but it's not 
                        /// In my neighbors, path to it
                        /// if they've reached the end of their path they will get stuck
                        /// possibly will throw an error
                        else if (path == null
                            && !gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                        {
                            SetPath(pathfinder.GetPath(coordinates, (Vector2)target));
                            /// some thing
                        }
                    }
                    else
                    {
                        SetTargetClosest(gameMap.GetTerrain());
                    }
                    break;
                /// dump state
                case State.Dump:
                    if (target.HasValue)
                    {
                        /// if I have a target, do I have a path?
                        if (path != null)
                        {
                            Move();
                        }
                        /// if I have a null path, is my target in a neighboring tile?
                        else if (path == null
                            && gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                        {

                            target = null;
                            Player1.AddWood(wood);
                            wood = 0;
                            currentState = State.Idle;
                        }
                        /// If I have a null path and I have a target but it's not 
                        /// In my neighbors, path to it
                        /// if they've reached the end of their path they will get stuck
                        /// possibly will throw an error
                        else if (path == null
                            && !gameMap.GetValidNeighbors(coordinates).Contains(target.Value))
                        {
                            SetPath(pathfinder.GetPath(coordinates, (Vector2)target));
                        }
                    }
                    else
                    {
                        SetTargetClosest(gameMap.GetBuildings());
                    }
                    break;
                /// move state - does not exist outside chop / dump states.
                case State.Move:
                    break;
            }

        }
    }
}
