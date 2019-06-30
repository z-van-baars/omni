﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Omni.Buildings;
using Omni.Units;
using System;
using System.Collections.Generic;

namespace Omni
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Omni : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private CoordinateConverter coordinateConverter;
        private Pathfinder pathfinder;
        private readonly Dictionary<string, Texture2D> imageGraphics = new Dictionary<string, Texture2D>();
        private Texture2D grass_tile;
        private Texture2D laborer;
        private Texture2D tree;
        private Texture2D tree_chopped;
        private Texture2D tree_stump;
        private Texture2D lumber_camp;
        private Texture2D white_selection_box;
        private Texture2D red_selection_box;
        private SpriteFont wood_font;
        private Texture2D score_widget;

        private GameMap gameMap;

        private Player Player1;


        public Point MapDimensions = new Point(50, 50);
        public Point TileDimensions = new Point(40, 20);
        public bool DisplayPaths = false;
        public bool DisplayScore = true;
        public Point MousePos;
        public MouseState MouseState;
        public KeyboardState KeyboardState;
        public Point CursorMapPos;
        public Point DisplayShift;
        public Point DisplayDimensions;

        RenderTarget2D tileDisplayLayer;



        public Omni()
        {
            graphics = new GraphicsDeviceManager(this);
            DisplayDimensions = new Point(1600, 900);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)DisplayDimensions.X;
            graphics.PreferredBackBufferHeight = (int)DisplayDimensions.Y;
            graphics.IsFullScreen = false;
            TargetElapsedTime = new TimeSpan(0, 0, 0, 0, 17);
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic
            coordinateConverter = new CoordinateConverter(TileDimensions, DisplayDimensions);
            IsMouseVisible = true;
            MousePos = new Point(graphics.GraphicsDevice.Viewport.Width / 2,
                                 graphics.GraphicsDevice.Viewport.Height / 2);
            DisplayShift.X = 0;
            DisplayShift.Y = 0;

            Player1 = new Player();

            gameMap = new GameMap(MapDimensions);
            gameMap.GenerateMapArray();
            gameMap.SetAllTileNeighbors();
            pathfinder = new Pathfinder(gameMap);
            gameMap.PrimitiveMapGen();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            white_selection_box = Content.Load<Texture2D>("art/ui/white_sb");
            red_selection_box = Content.Load<Texture2D>("art/ui/red_sb");
            grass_tile = Content.Load<Texture2D>("art/tiles/grass_1");
            laborer = Content.Load<Texture2D>("art/units/laborer");
            tree = Content.Load<Texture2D>("art/terrain/tree_1");
            tree_chopped = Content.Load<Texture2D>("art/terrain/tree_chopped");
            tree_stump = Content.Load<Texture2D>("art/terrain/tree_stump");
            lumber_camp = Content.Load<Texture2D>("art/buildings/lumber_camp");
            wood_font = Content.Load<SpriteFont>("Score");
            score_widget = Content.Load<Texture2D>("art/ui/score_widget");
            imageGraphics["Grass Tile"] = grass_tile;
            imageGraphics["Laborer"] = laborer;
            imageGraphics["Tree"] = tree;
            imageGraphics["Chopped Tree"] = tree_chopped;
            imageGraphics["Stump"] = tree_stump;
            imageGraphics["Lumber Camp"] = lumber_camp;


            int displayLayerWidth = (MapDimensions.X * (TileDimensions.X / 2)) +
                (MapDimensions.Y * (TileDimensions.X / 2));
            int displayLayerHeight = (MapDimensions.X * (TileDimensions.Y / 2)) +
                (MapDimensions.Y * (TileDimensions.Y / 2));

            tileDisplayLayer = new RenderTarget2D(
                GraphicsDevice,
                displayLayerWidth,
                displayLayerHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            GraphicsDevice.SetRenderTarget(tileDisplayLayer);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (GameTile tileObject in gameMap.game_tiles)
            {
                var ti = coordinateConverter.MapToScreen(tileObject.Coordinates);
                int background_center = TileDimensions.X / 2 + (DisplayShift.X + displayLayerWidth / 2);
                ti.X = ti.X + background_center - (TileDimensions.X);
                spriteBatch.Draw(grass_tile, (ti + DisplayShift).ToVector2(), Color.White);
            }
            spriteBatch.End();
            GraphicsDevice.SetRenderTarget(null);

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // IO events start
            var random = new Random();
            var lastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            var lastMouseState = MouseState;
            MouseState = Mouse.GetState();
            MousePos.X = MouseState.X;
            MousePos.Y = MouseState.Y;
            CursorMapPos = coordinateConverter.ScreenToMap(MousePos, DisplayShift);

            if (KeyboardState.IsKeyDown(Keys.Right))
            {
                DisplayShift.X -= 5;
            }
            if (KeyboardState.IsKeyDown(Keys.Left))
            {
                DisplayShift.X += 5;
            }
            if (KeyboardState.IsKeyDown(Keys.Up))
            {
                DisplayShift.Y += 5;
            }
            if (KeyboardState.IsKeyDown(Keys.Down))
            {
                DisplayShift.Y -= 5;
            }
            if (KeyboardState.IsKeyDown(Keys.Escape))
            {
                Exit();
            }
            if (KeyboardState.IsKeyUp(Keys.P)
                && lastKeyboardState.IsKeyDown(Keys.P))
            {
                DisplayPaths = !DisplayPaths;
            }
            if (KeyboardState.IsKeyUp(Keys.S)
                && lastKeyboardState.IsKeyDown(Keys.S))
            {
                DisplayScore = !DisplayScore;
            }

            if (MouseState.LeftButton == ButtonState.Released
                && lastMouseState.LeftButton == ButtonState.Pressed)
            {
                var mapCoords = coordinateConverter.ScreenToMap(MousePos, DisplayShift);
                if (gameMap.IsPointInside(mapCoords)
                    && gameMap.game_tiles[mapCoords.Y, mapCoords.X].IsPathable())
                {
                    var newLumberCamp = new LumberCamp(mapCoords);
                    gameMap.game_tiles[mapCoords.Y, mapCoords.X].Building = newLumberCamp;
                    gameMap.GetBuildings().Add(newLumberCamp);
                    var validNeighbors = gameMap.GetValidNeighbors(mapCoords);
                    var spawnableNeighbors = new List<Point>();
                    foreach (var validNeighbor in validNeighbors)
                    {
                        if (gameMap.game_tiles[(int)validNeighbor.Y, (int)validNeighbor.X].IsPathable())
                        {
                            spawnableNeighbors.Add(validNeighbor);
                        }
                    }
                    for (int x = 0; x < 3; x++)
                    {
                        var spawnTile = spawnableNeighbors[random.Next(spawnableNeighbors.Count)];
                        var newLaborer = new Laborer(spawnTile);
                        gameMap.GetUnits().Add(newLaborer);
                        var theTile = gameMap.game_tiles[(int)spawnTile.Y, (int)spawnTile.X];
                        theTile.Unit = newLaborer;

                    }

                }
                
            }

            // IO events end


            // game logic start
            var expiredEntities = new List<Entity>();
            foreach (var unitObject in gameMap.GetUnits())
            {
                unitObject.Tick(gameMap, Player1, pathfinder);
            }
            foreach (var terrainObject in gameMap.GetTerrain())
            {
                terrainObject.Tick(gameMap, Player1, pathfinder);
                if (terrainObject.IsExpired())
                {
                    expiredEntities.Add(terrainObject);
                }
            }
            foreach (var entityObject in expiredEntities)
            {
                entityObject.OnDeath(gameMap);
                gameMap.GetTerrain().Remove(entityObject);
                gameMap.GetBuildings().Remove(entityObject);

            }


            // game logic ends


            base.Update(gameTime);
        }

        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        /// ???^^^ I don't know what this means ^^^???
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            /// only one spritebatch right now - perhaps more?  dunno if layers will play nice with this
            spriteBatch.Begin();
            /// draw all the gametiles - individual draw operations, not (yet) batched, and not collated into a single layer draw op
            spriteBatch.Draw(tileDisplayLayer, new Vector2(-(tileDisplayLayer.Width / 2) + TileDimensions.X / 2 + DisplayShift.X, DisplayShift.Y), Color.White);
            /// draw the selected tile graphic if the selected tile is within the map bounds
            if (gameMap.IsPointInside(CursorMapPos))
            {
                var st = coordinateConverter.MapToScreen(CursorMapPos);
                spriteBatch.Draw(white_selection_box, (st + DisplayShift).ToVector2(), Color.White);
            }
            if (DisplayPaths)
            {
                foreach (Unit unitObject in gameMap.GetUnits())
                {
                    if (unitObject.GetPath() != null)
                    {
                        foreach (var pathStep in unitObject.GetPath())
                        {
                            var rb = coordinateConverter.MapToScreen(pathStep);
                            spriteBatch.Draw(red_selection_box, (rb + DisplayShift).ToVector2(), Color.White);
                        }
                    }
                }
            }

            Action<Entity> drawEntity = entity =>
            {
                var imageFileHeightOffset = new Point(0, imageGraphics[entity.name].Height - TileDimensions.Y);
                var te = coordinateConverter.MapToScreen(entity.Coordinates);
                spriteBatch.Draw(imageGraphics[entity.name], (te + DisplayShift - imageFileHeightOffset).ToVector2(), Color.White);

            };
            gameMap.GetTerrain().ForEach(drawEntity);
            gameMap.GetBuildings().ForEach(drawEntity);
            gameMap.GetUnits().ForEach(drawEntity);

            if (DisplayScore)
            {
                spriteBatch.Draw(score_widget, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(wood_font, "Wood " + Player1.GetWood(), new Vector2(10, 8), Color.Black);
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
