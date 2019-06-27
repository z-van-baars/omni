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
        private Texture2D lumber_camp;
        private Texture2D white_selection_box;

        private GameMap gameMap;
        private List<Entity> entities = new List<Entity>();
        private List<Terrain> terrain = new List<Terrain>();
        private List<Building> buildings = new List<Building>();
        private List<Unit> units = new List<Unit>();
        private Player Player1;


        public Point MapDimensions = new Point(50, 50);
        public Vector2 TileDimensions = new Vector2(40, 20);
        public Vector2 MousePos;
        public MouseState MouseState;
        public Vector2 CursorMapPos;
        public Vector2 DisplayShift;
        public Vector2 DisplayDimensions;



        public Omni()
        {
            graphics = new GraphicsDeviceManager(this);
            DisplayDimensions = new Vector2(1600, 900);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = (int)DisplayDimensions.X;
            graphics.PreferredBackBufferHeight = (int)DisplayDimensions.Y;
            graphics.IsFullScreen = false;
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
            ///(DisplayShift.X, DisplayShift.Y) = coordinateConverter.MapToScreen(MapDimensions.X / 2, MapDimensions.Y / 2);
            ///DisplayShift.X -= graphics.GraphicsDevice.Viewport.Width / 2;
            ///DisplayShift.Y -= graphics.GraphicsDevice.Viewport.Width / 2;

            Player1 = new Player();

            gameMap = new GameMap(MapDimensions);
            gameMap.GenerateMapArray();
            pathfinder = new Pathfinder(gameMap);


            Random random = new Random();
            int num_trees = random.Next(100, 110);
            for (int t = 0; t < num_trees; t++)
            {
                int tries = 0;
                int rand_x = random.Next(0, MapDimensions.X - 1);
                int rand_y = random.Next(0, MapDimensions.Y - 1);

                while (gameMap.game_tiles[rand_y, rand_x].Terrain != null)
                {
                    rand_x = random.Next(0, MapDimensions.X - 1);
                    rand_y = random.Next(0, MapDimensions.Y - 1);
                    tries += 1;
                }
                Tree newTree = new Tree(new Vector2(rand_x, rand_y));

                terrain.Add(newTree);
                gameMap.game_tiles[rand_y, rand_x].Terrain = newTree;

            }
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
            grass_tile = Content.Load<Texture2D>("art/tiles/grass_1");
            laborer = Content.Load<Texture2D>("art/units/laborer");
            tree = Content.Load<Texture2D>("art/terrain/tree_1");
            lumber_camp = Content.Load<Texture2D>("art/buildings/lumber_camp");
            imageGraphics["Grass Tile"] = grass_tile;
            imageGraphics["Laborer"] = laborer;
            imageGraphics["Tree"] = tree;
            imageGraphics["Lumber Camp"] = lumber_camp;

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
            Random random = new Random();
            KeyboardState KeyState = Keyboard.GetState();
            MouseState lastMouseState = MouseState;
            MouseState = Mouse.GetState();
            MousePos.X = MouseState.X;
            MousePos.Y = MouseState.Y;
            CursorMapPos = coordinateConverter.ScreenToMap(MousePos, DisplayShift);

            if (KeyState.IsKeyDown(Keys.Right))
            {
                DisplayShift.X -= 5;
            }
            if (KeyState.IsKeyDown(Keys.Left))
            {
                DisplayShift.X += 5;
            }
            if (KeyState.IsKeyDown(Keys.Up))
            {
                DisplayShift.Y += 5;
            }
            if (KeyState.IsKeyDown(Keys.Down))
            {
                DisplayShift.Y -= 5;
            }
            if (KeyState.IsKeyDown(Keys.Escape))
            {
                Exit();
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
                    buildings.Add(newLumberCamp);
                    List<Vector2> validNeighbors = gameMap.GetValidNeighbors(mapCoords);
                    List<Vector2> spawnableNeighbors = new List<Vector2>();
                    foreach (Vector2 validNeighbor in validNeighbors)
                    {
                        if (gameMap.game_tiles[(int)validNeighbor.Y, (int)validNeighbor.X].IsPathable())
                        {
                            spawnableNeighbors.Add(validNeighbor);
                        }
                    }
                    for (int x = 0; x < 1; x++)
                    {
                        Vector2 spawnTile = spawnableNeighbors[random.Next(spawnableNeighbors.Count)];
                        Laborer newLaborer = new Laborer(spawnTile);
                        units.Add(newLaborer);
                        GameTile theTile = gameMap.game_tiles[(int)spawnTile.Y, (int)spawnTile.X];
                        theTile.Units.Add(newLaborer);
                        newLaborer.SetTarget(terrain);
                        List<Vector2> newPath = pathfinder.GetPath(spawnTile, newLaborer.GetTarget().Value);
                        newLaborer.SetPath(newPath);

                    }

                }
                
            }

            foreach (Unit unitObject in units)
            {
                unitObject.Tick(gameMap);
            }


            // TODO: Add your update logic here


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
            foreach (GameTile tileObject in gameMap.game_tiles)
            {
                (float tiX, float tiY) = coordinateConverter.MapToScreen(tileObject.x, tileObject.y);
                spriteBatch.Draw(grass_tile, new Vector2(tiX + DisplayShift.X, tiY + DisplayShift.Y), Color.White);
            }
            /// draw the selected tile graphic if the selected tile is within the map bounds
            if (gameMap.IsPointInside(CursorMapPos))
            {
                (float stx, float sty) = coordinateConverter.MapToScreen(CursorMapPos.X, CursorMapPos.Y);
                Vector2 selectedTilePosition = new Vector2(stx, sty);
                spriteBatch.Draw(white_selection_box, new Vector2(selectedTilePosition.X + DisplayShift.X, selectedTilePosition.Y + DisplayShift.Y), Color.White);
            }
            /// draw the terrain objects next, resources, trees, rocks, etc
            foreach (Terrain terrainObject in terrain)
            {
                int imageFileHeightOffset = (int)(imageGraphics[terrainObject.name].Height - TileDimensions.Y);
                (float teX, float teY) = coordinateConverter.MapToScreen(terrainObject.Get_X(), terrainObject.Get_Y());
                spriteBatch.Draw(imageGraphics[terrainObject.name],
                    new Vector2(teX + DisplayShift.X, teY + DisplayShift.Y - (imageFileHeightOffset)), Color.White);
            }
            /// draw buildings
            foreach (Building buildingObject in buildings)
            {
                int imageFileHeightOffset = (int)(imageGraphics[buildingObject.name].Height - TileDimensions.Y);
                (float bX, float bY) = coordinateConverter.MapToScreen(buildingObject.Get_X(), buildingObject.Get_Y());
                spriteBatch.Draw(imageGraphics[buildingObject.name], new Vector2(bX + DisplayShift.X, bY + DisplayShift.Y - imageFileHeightOffset), Color.White);
            }
            /// draw (moving) units
            foreach (Unit unitObject in units)
            {
                int imageFileHeightOffset = (int)(imageGraphics[unitObject.name].Height - TileDimensions.Y);
                (float x5, float y5) = coordinateConverter.MapToScreen(unitObject.Get_X(), unitObject.Get_Y());
                spriteBatch.Draw(imageGraphics[unitObject.name], new Vector2(x5 + DisplayShift.X, y5 + DisplayShift.Y - imageFileHeightOffset), Color.White);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
