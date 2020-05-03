using FirstMG.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace FirstMG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        Source.GamePlay.World _world;
        Source.Engine.Asset2D _cursor;
        
        public Main()
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
            Source.Engine.Globals.ScreenWidth = 1600;
            Source.Engine.Globals.ScreenHeight = 900;

            graphics.PreferredBackBufferWidth = Source.Engine.Globals.ScreenWidth;
            graphics.PreferredBackBufferHeight = Source.Engine.Globals.ScreenHeight;

            graphics.ApplyChanges();

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Source.Engine.Globals.MyContent = this.Content;
            Source.Engine.Globals.MySpriteBatch = new SpriteBatch(GraphicsDevice);
            // TODO: use this.Content to load your game content here

            _cursor = new Source.Engine.Asset2D("Assets\\CursorArrow", Vector2.Zero, new Vector2(40,40));

            Source.Engine.Globals.MyKeyboard = new Source.Engine.Input.MyKeyboard();
            Source.Engine.Globals.MyMouse = new Source.Engine.Input.MyMouseControl();

            _world = new Source.GamePlay.World();
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

            Source.Engine.Globals.GlobalGameTime = gameTime;
            Source.Engine.Globals.MyKeyboard.Update();
            Source.Engine.Globals.MyMouse.Update();

            _world.Update();

            Source.Engine.Globals.MyKeyboard.UpdateOld();
            Source.Engine.Globals.MyMouse.UpdateOld();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            Source.Engine.Globals.MySpriteBatch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend);


            _world.Draw(Vector2.Zero);

            _cursor.Draw(Source.Engine.Globals.NewVector(Source.Engine.Globals.MyMouse.NewMousePos), Vector2.Zero);

            Source.Engine.Globals.MySpriteBatch.End();

            base.Draw(gameTime);
        }
    }


    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main()
        {
            using (var game = new Main())
                game.Run();
        }
    }
}
