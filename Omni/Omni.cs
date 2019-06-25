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
        private List<Tree> trees = new List<Tree>();


        public int MapWidth = 50;
        public int MapHeight = 50;
        public int TileWidth = 40;
        public int TileHeight = 20;
        public int ShiftX;
        public int ShiftY;



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
            coordinateConverter = new CoordinateConverter(TileWidth, TileHeight);
            this.IsMouseVisible = true;
            ShiftX = TileWidth * (MapWidth / 2) - TileWidth / 2;
            game_tiles = new GameTile[MapHeight, MapWidth];
            for (int y = 0; y < MapHeight; y++)
            {                
                for (int x = 0; x < MapWidth; x++)
                {
                    GameTile gameTile = new GameTile(x, y, "Grass");
                    game_tiles[y, x] = gameTile;
                }
            }
            Random random = new Random();
            int num_trees = random.Next(10, 100);
            for (int t = 0; t < num_trees; t++)
            {
                int rand_x = random.Next(0, MapWidth - 1);
                int rand_y = random.Next(0, MapHeight - 1);
                Tree newTree = new Tree(rand_x, rand_y);
                trees.Add(newTree);
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            KeyboardState KeyState = Keyboard.GetState();
            if (KeyState.IsKeyDown(Keys.Right))
                ShiftX -= 5;
            if (KeyState.IsKeyDown(Keys.Left))
                ShiftX += 5;
            if (KeyState.IsKeyDown(Keys.Up))
                ShiftY += 5;
            if (KeyState.IsKeyDown(Keys.Down))
                ShiftY -= 5;

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
                int x2 = coordinateConverter.MapToScreenX(tileObject.x, tileObject.y);
                int y2 = coordinateConverter.MapToScreenY(tileObject.x, tileObject.y);
                spriteBatch.Draw(grass_tile, new Vector2(x2 + ShiftX, y2 + ShiftY), Color.White);
            }

            foreach (Tree treeObject in trees)
            {
                int x3 = coordinateConverter.MapToScreenX(treeObject.x, treeObject.y);
                int y3 = coordinateConverter.MapToScreenY(treeObject.x, treeObject.y);
                spriteBatch.Draw(tree, new Vector2(x3 + ShiftX, y3 + ShiftY - 60), Color.White);
            }

            int x4 = coordinateConverter.MapToScreenX(49, 49);
            int y4 = coordinateConverter.MapToScreenY(49, 49);
            spriteBatch.Draw(lumber_camp, new Vector2(x4 + ShiftX, y4 + ShiftY - 60), Color.White);
            int x5 = coordinateConverter.MapToScreenX(25, 25);
            int y5 = coordinateConverter.MapToScreenY(25, 25);
            spriteBatch.Draw(laborer, new Vector2(x5 + ShiftX, y5 + ShiftY - 60), Color.White);
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
