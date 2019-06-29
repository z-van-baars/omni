using Microsoft.Xna.Framework;
using Omni.Buildings;
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
            pathable = false;
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
                    if (target != null)
                    {
                        /// if I have a target, do I have a path?
                        if (path != null)
                        {
                            if (!CheckPath(gameMap))
                            {
                                SetPath(pathfinder.GetPath(coordinates, target.Coordinates));
                            }
                            if (path != null)
                            {
                                Move(gameMap);
                            }
                        }
                        /// if I have a null path, is my target in a neighboring tile?
                        else if (path == null
                            && gameMap.GetValidNeighbors(coordinates).Contains(target.Coordinates))
                        {
                            chopTimer -= 1;
                            if (chopTimer == 0)
                            {
                                chopTimer = chopTimerBase;
                                if (target.GetRemaining() > 0)
                                {
                                    chopTimer = chopTimerBase;
                                    target.ChangeRemaining(-1);
                                    wood += 1;
                                }
                                else
                                {
                                    target = null;
                                    currentState = State.Dump;
                                }
                                if (wood == maxWood)
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
                            && !gameMap.GetValidNeighbors(coordinates).Contains(target.Coordinates))
                        {
                            SetPath(pathfinder.GetPath(coordinates, target.Coordinates));
                            /// some thing
                        }
                    }
                    else
                    {
                        SetTargetClosest(gameMap.GetTerrain(), typeof(Tree));
                    }
                    break;
                /// dump state
                case State.Dump:
                    if (target != null)
                    {
                        /// if I have a target, do I have a path?
                        if (path != null)
                        {
                            if (!CheckPath(gameMap))
                            {
                                SetPath(pathfinder.GetPath(coordinates, target.Coordinates));
                            }
                            Move(gameMap);
                        }
                        /// if I have a null path, is my target in a neighboring tile?
                        else if (path == null
                            && gameMap.GetValidNeighbors(coordinates).Contains(target.Coordinates))
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
                            && !gameMap.GetValidNeighbors(coordinates).Contains(target.Coordinates))
                        {
                            SetPath(pathfinder.GetPath(coordinates, target.Coordinates));
                        }
                    }
                    else
                    {
                        SetTargetClosest(gameMap.GetBuildings(), typeof(LumberCamp));
                    }
                    break;
                /// move state - does not exist outside chop / dump states.
                case State.Move:
                    break;
            }

        }
    }
}
