using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
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
        private Dictionary<string, Texture2D> imageGraphics;
        private Texture2D grass_tile;
        private Texture2D laborer;
        private Texture2D tree;
        private Texture2D lumber_camp;
        private Texture2D white_selection_box;


        private GameTile[,] game_tiles;
        private List<Entity> entities = new List<Entity>();
        private List<Terrain> terrain = new List<Terrain>();
        private List<Building> buildings = new List<Building>();
        private List<Unit> units = new List<Unit>();
        private Player Player1;


        public Point MapDimensions = new Point(500, 500);
        public Vector2 TileDimensions = new Vector2(40, 20);
        public Vector2 MousePos;
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

            game_tiles = new GameTile[MapDimensions.Y, MapDimensions.X];
            for (int y = 0; y < MapDimensions.Y; y++)
            {                
                for (int x = 0; x < MapDimensions.Y; x++)
                {
                    GameTile gameTile = new GameTile(x, y, "Grass");
                    game_tiles[y, x] = gameTile;
                }
            }
            Random random = new Random();
            int num_trees = random.Next(50, 100);
            for (int t = 0; t < num_trees; t++)
            {
                int tries = 0;
                int rand_x = random.Next(0, MapDimensions.X - 1);
                int rand_y = random.Next(0, MapDimensions.Y - 1);
                while (game_tiles[rand_y, rand_x].terrain != null || tries <= 5)
                {
                    rand_x = random.Next(0, MapDimensions.X - 1);
                    rand_y = random.Next(0, MapDimensions.Y - 1);
                    tries += 1;
                }
                Tree newTree = new Tree(new Vector2(rand_x, rand_y));

                terrain.Add(newTree);
                game_tiles[rand_y, rand_x].terrain = newTree;

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
            KeyboardState KeyState = Keyboard.GetState();
            MouseState MouseState = Mouse.GetState();
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

    
            if (MouseState.LeftButton == ButtonState.Pressed)
            {
                Vector2 mapCoords = coordinateConverter.ScreenToMap(MousePos, DisplayShift);
                System.Diagnostics.Debug.WriteLine(mapCoords.X.ToString() + "," + mapCoords.Y.ToString());
            }


            // TODO: Add your update logic here


            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.DarkSlateBlue);

            spriteBatch.Begin();
            foreach (GameTile tileObject in game_tiles)
            {
                (float x2, float y2) = coordinateConverter.MapToScreen(tileObject.x, tileObject.y);
                spriteBatch.Draw(grass_tile, new Vector2(x2 + DisplayShift.X, y2 + DisplayShift.Y), Color.White);
            }
            if (CursorMapPos.X >= 0
                && CursorMapPos.X < MapDimensions.X
                && CursorMapPos.Y >= 0
                && CursorMapPos.Y < MapDimensions.Y)
            {
                (float stx, float sty) = coordinateConverter.MapToScreen(CursorMapPos.X, CursorMapPos.Y);
                Vector2 selectedTilePosition = new Vector2(stx, sty);
                spriteBatch.Draw(white_selection_box, new Vector2(selectedTilePosition.X + DisplayShift.X, selectedTilePosition.Y + DisplayShift.Y), Color.White);
            }

            foreach (Terrain terrainObject in terrain)
            {
                (float x3, float y3) = coordinateConverter.MapToScreen(terrainObject.Get_X(), terrainObject.Get_Y());
                int imageFileHeightOffset = (int)(imageGraphics[terrainObject.name].Height - TileDimensions.Y);
                spriteBatch.Draw(imageGraphics[terrainObject.name],
                    new Vector2(x3 + DisplayShift.X, y3 + DisplayShift.Y - (imageFileHeightOffset)), Color.White);
            }
            foreach (Building buildingObject in buildings)
            {
                (float x4, float y4) = coordinateConverter.MapToScreen(49, 49);
                spriteBatch.Draw(imageGraphics[buildingObject.name], new Vector2(x4 + DisplayShift.X, y4 + DisplayShift.Y - 60), Color.White);
            }

            foreach (Unit unitObject in units)
            {
                (float x5, float y5) = coordinateConverter.MapToScreen(25, 25);
                spriteBatch.Draw(imageGraphics[unitObject.name], new Vector2(x5 + DisplayShift.X, y5 + DisplayShift.Y - 60), Color.White);
            }

            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
