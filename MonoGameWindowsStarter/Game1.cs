using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using MapLibrary;

namespace MonoGameWindowsStarter
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        Random r = new Random();
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        SpriteSheet sheet;

        Tileset tileset;
        Tilemap tilemap;

        Player player;
        List<Enemy> enemies = new List<Enemy>();
        AxisList world;

        Texture2D background;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
#if VISUAL_DEBUG
            VisualDebugging.LoadContent(Content);
#endif
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            background = Content.Load<Texture2D>("background");

            var t = Content.Load<Texture2D>("spritesheet");
            sheet = new SpriteSheet(t, 34, 34, 0, 0);

            //var playerFrames = from index in Enumerable.Range(8, 14) select sheet[index];
            var playerFrames = new Sprite[] { sheet[8], sheet[9], sheet[10], sheet[11], sheet[12], sheet[13], sheet[14], sheet[15] };
            player = new Player(playerFrames);

            var enemyFrames = from index in Enumerable.Range(0, 7) select sheet[index];
            enemies.Add(new Enemy(new BoundingRectangle(50, 50, 34, 34), sheet[1], new Vector2((float)r.NextDouble(), (float)r.NextDouble()), 0, 200));
            enemies.Add(new Enemy(new BoundingRectangle(50, 100, 34, 34), sheet[2], new Vector2((float)r.NextDouble(), (float)r.NextDouble()), 1, 200));
            enemies.Add(new Enemy(new BoundingRectangle(50, 200, 34, 34), sheet[3], new Vector2((float)r.NextDouble(), (float)r.NextDouble()), 0, 200));
            enemies.Add(new Enemy(new BoundingRectangle(50, 400, 34, 34), sheet[4], new Vector2((float)r.NextDouble(), (float)r.NextDouble()), 1, 200));
            enemies.Add(new Enemy(new BoundingRectangle(50, 500, 34, 34), sheet[5], new Vector2((float)r.NextDouble(), (float)r.NextDouble()), 1, 200));
            
            world = new AxisList();
            foreach (Enemy enemy in enemies)
            {
                world.AddGameObject(enemy);
            }
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

            // TODO: Add your update logic here
            player.Update(gameTime);

            //update logic for enemies here
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(gameTime);
            }

            var enemyQuery = world.QueryRange(player.Bounds.X, player.Bounds.X + player.Bounds.Width);
            player.CheckForEnemyCollision(enemyQuery);

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            // Calculate and apply the world/view transform
            var offset = new Vector2(200, 300) - player.Position;
            var t = Matrix.CreateTranslation(offset.X, offset.Y, 0);
            spriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, t);

            //spriteBatch.Draw(background, new Rectangle(0, 0, 800, 800), Color.Black);
            tilemap.Draw(spriteBatch);

            // TODO: Add your drawing code here
            var enemyQuery = world.QueryRange(player.Position.X - 221, player.Position.X + 400);
            foreach (Enemy enemy in enemyQuery)
            {
                enemy.Draw(spriteBatch);
            }
            Debug.WriteLine($"{enemyQuery.Count()} Platforms rendered");

            player.Draw(spriteBatch);

            //draw certain amount of sprites in spritesheet on screen
            for (var i = 0; i < 15; i++)
            {
                sheet[i].Draw(spriteBatch, new Vector2(i * 25, 25), Color.White);
            }


            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
