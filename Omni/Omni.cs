using Microsoft.Xna.Framework;
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
        private Dictionary<string, Texture2D> imageGraphics = new Dictionary<string, Texture2D>();
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


        public Point MapDimensions = new Point(100, 100);
        public Vector2 TileDimensions = new Vector2(40, 20);
        public bool DisplayPaths = false;
        public bool DisplayScore = true;
        public Vector2 MousePos;
        public MouseState MouseState;
        public KeyboardState KeyboardState;
        public Vector2 CursorMapPos;
        public Vector2 DisplayShift;
        public Vector2 DisplayDimensions;

        RenderTarget2D tileDisplayLayer;



        public Omni()
        {
            graphics = new GraphicsDeviceManager(this);
            DisplayDimensions = new Vector2(1600, 900);
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
            this.IsMouseVisible = true;
            MousePos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                        graphics.GraphicsDevice.Viewport.Height / 2);
            DisplayShift.X = 0;
            DisplayShift.Y = 0;

            Player1 = new Player();

            gameMap = new GameMap(MapDimensions);
            gameMap.GenerateMapArray();
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


            float displayLayerWidth = (MapDimensions.X * (TileDimensions.X / 2)) +
                (MapDimensions.Y * (TileDimensions.X / 2));
            float displayLayerHeight = (MapDimensions.X * (TileDimensions.Y / 2)) +
                (MapDimensions.Y * (TileDimensions.Y / 2));

            tileDisplayLayer = new RenderTarget2D(
                GraphicsDevice,
                (int)displayLayerWidth,
                (int)displayLayerHeight,
                false,
                SurfaceFormat.Color,
                DepthFormat.None);

            GraphicsDevice.SetRenderTarget(tileDisplayLayer);
            GraphicsDevice.Clear(Color.Transparent);
            spriteBatch.Begin();
            foreach (GameTile tileObject in gameMap.game_tiles)
            {
                (float tiX, float tiY) = coordinateConverter.MapToScreen(tileObject.X, tileObject.Y);
                float background_center = TileDimensions.X / 2 + (DisplayShift.X + displayLayerWidth / 2);
                tiX = tiX + background_center - (TileDimensions.X);
                spriteBatch.Draw(grass_tile, new Vector2(tiX + DisplayShift.X, tiY + DisplayShift.Y), Color.White);
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
            Random random = new Random();
            KeyboardState lastKeyboardState = KeyboardState;
            KeyboardState = Keyboard.GetState();
            MouseState lastMouseState = MouseState;
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
                Vector2 mapCoords = coordinateConverter.ScreenToMap(MousePos, DisplayShift);
                if (gameMap.IsPointInside(mapCoords)
                    && gameMap.game_tiles[(int)mapCoords.Y, (int)mapCoords.X].IsPathable())
                {
                    LumberCamp newLumberCamp = new LumberCamp(mapCoords);
                    gameMap.game_tiles[(int)mapCoords.Y, (int)mapCoords.X].Building = newLumberCamp;
                    gameMap.GetBuildings().Add(newLumberCamp);
                    List<Vector2> validNeighbors = gameMap.GetValidNeighbors(mapCoords);
                    List<Vector2> spawnableNeighbors = new List<Vector2>();
                    foreach (Vector2 validNeighbor in validNeighbors)
                    {
                        if (gameMap.game_tiles[(int)validNeighbor.Y, (int)validNeighbor.X].IsPathable())
                        {
                            spawnableNeighbors.Add(validNeighbor);
                        }
                    }
                    for (int x = 0; x < 3; x++)
                    {
                        Vector2 spawnTile = spawnableNeighbors[random.Next(spawnableNeighbors.Count)];
                        Laborer newLaborer = new Laborer(spawnTile);
                        gameMap.GetUnits().Add(newLaborer);
                        GameTile theTile = gameMap.game_tiles[(int)spawnTile.Y, (int)spawnTile.X];
                        theTile.Unit = newLaborer;

                    }

                }
                
            }

            // IO events end


            // game logic start
            List<Entity> expiredEntities = new List<Entity>();
            foreach (Unit unitObject in gameMap.GetUnits())
            {
                unitObject.Tick(gameMap, Player1, pathfinder);
            }
            foreach (Terrain terrainObject in gameMap.GetTerrain())
            {
                terrainObject.Tick(gameMap, Player1, pathfinder);
                if (terrainObject.IsExpired())
                {
                    expiredEntities.Add(terrainObject);
                }
            }
            foreach (Entity entityObject in expiredEntities)
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
                (float stx, float sty) = coordinateConverter.MapToScreen(CursorMapPos.X, CursorMapPos.Y);
                Vector2 selectedTilePosition = new Vector2(stx, sty);
                spriteBatch.Draw(white_selection_box, new Vector2(selectedTilePosition.X + DisplayShift.X, selectedTilePosition.Y + DisplayShift.Y), Color.White);
            }
            if (DisplayPaths)
            {
                foreach (Unit unitObject in gameMap.GetUnits())
                {
                    if (unitObject.GetPath() != null)
                    {
                        foreach (Vector2 pathStep in unitObject.GetPath())
                        {
                            (float rbx, float rby) = coordinateConverter.MapToScreen(pathStep.X, pathStep.Y);
                            spriteBatch.Draw(red_selection_box, new Vector2(rbx + DisplayShift.X, rby + DisplayShift.Y), Color.White);
                        }
                    }
                }
            }

            /// draw the terrain objects next, resources, trees, rocks, etc
            foreach (Terrain terrainObject in gameMap.GetTerrain())
            {
                int imageFileHeightOffset = (int)(imageGraphics[terrainObject.name].Height - TileDimensions.Y);
                (float teX, float teY) = coordinateConverter.MapToScreen(terrainObject.Get_X(), terrainObject.Get_Y());
                spriteBatch.Draw(imageGraphics[terrainObject.name],
                    new Vector2(teX + DisplayShift.X, teY + DisplayShift.Y - (imageFileHeightOffset)), Color.White);
            }
            /// draw buildings
            foreach (Building buildingObject in gameMap.GetBuildings())
            {
                int imageFileHeightOffset = (int)(imageGraphics[buildingObject.name].Height - TileDimensions.Y);
                (float bX, float bY) = coordinateConverter.MapToScreen(buildingObject.Get_X(), buildingObject.Get_Y());
                spriteBatch.Draw(imageGraphics[buildingObject.name], new Vector2(bX + DisplayShift.X, bY + DisplayShift.Y - imageFileHeightOffset), Color.White);
            }
            /// draw (moving) units
            foreach (Unit unitObject in gameMap.GetUnits())
            {
                int imageFileHeightOffset = (int)(imageGraphics[unitObject.name].Height - TileDimensions.Y);
                (float x5, float y5) = coordinateConverter.MapToScreen(unitObject.Get_X(), unitObject.Get_Y());
                spriteBatch.Draw(imageGraphics[unitObject.name], new Vector2(x5 + DisplayShift.X, y5 + DisplayShift.Y - imageFileHeightOffset), Color.White);
            }
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
