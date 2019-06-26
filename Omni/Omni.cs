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
        private Texture2D grass_tile;
        private Texture2D laborer;
        private Texture2D tree;
        private Texture2D lumber_camp;
        private GameTile[,] game_tiles;
        private List<Entity> entities = new List<Entity>();


        public Point MapDimensions = new Point(50, 50);
        public Vector2 TileDimensions = new Vector2(40, 20);
        public Vector2 MousePos;
        public Vector2 DisplayShift;



        public Omni()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            graphics.PreferredBackBufferWidth = 1600;
            graphics.PreferredBackBufferHeight = 900;
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
            coordinateConverter = new CoordinateConverter(TileDimensions);
            this.IsMouseVisible = true;
            MousePos = new Vector2(graphics.GraphicsDevice.Viewport.Width / 2,
                        graphics.GraphicsDevice.Viewport.Height / 2);
            DisplayShift.X = TileDimensions.X * (MapDimensions.X / 2) - TileDimensions.Y / 2;
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
            int num_trees = random.Next(10, 100);
            for (int t = 0; t < num_trees; t++)
            {
                int rand_x = random.Next(0, MapDimensions.X - 1);
                int rand_y = random.Next(0, MapDimensions.Y - 1);
                Tree newTree = new Tree(new Vector2(rand_x, rand_y));
                entities.Add(newTree);
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
            grass_tile = Content.Load<Texture2D>("art/tiles/grass_1");
            laborer = Content.Load<Texture2D>("art/units/laborer");
            tree = Content.Load<Texture2D>("art/terrain/tree_1");
            lumber_camp = Content.Load<Texture2D>("art/buildings/lumber_camp");

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

            if (KeyState.IsKeyDown(Keys.Right))
                DisplayShift.X -= 5;
            if (KeyState.IsKeyDown(Keys.Left))
                DisplayShift.X += 5;
            if (KeyState.IsKeyDown(Keys.Up))
                DisplayShift.Y += 5;
            if (KeyState.IsKeyDown(Keys.Down))
                DisplayShift.Y -= 5;
            if (KeyState.IsKeyDown(Keys.Escape))
                Exit();
    
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
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            foreach (GameTile tileObject in game_tiles)
            {
                (float x2, float y2) = coordinateConverter.MapToScreen(tileObject.x, tileObject.y);
                spriteBatch.Draw(grass_tile, new Vector2(x2 + DisplayShift.X, y2 + DisplayShift.Y), Color.White);
            }

            foreach (Entity entityObject in entities)
            {
                (float x3, float y3) = coordinateConverter.MapToScreen(entityObject.Get_X(), entityObject.Get_Y());
                spriteBatch.Draw(tree, new Vector2(x3 + DisplayShift.X, y3 + DisplayShift.Y - 60), Color.White);
            }

            (float x4, float y4) = coordinateConverter.MapToScreen(49, 49);
            spriteBatch.Draw(lumber_camp, new Vector2(x4 + DisplayShift.X, y4 + DisplayShift.Y - 60), Color.White);
            (float x5, float y5) = coordinateConverter.MapToScreen(25, 25);
            spriteBatch.Draw(laborer, new Vector2(x5 + DisplayShift.X, y5 + DisplayShift.Y - 60), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
