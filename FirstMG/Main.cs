using FirstMG.Source;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Xml.Linq;

namespace FirstMG
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Main : Game
    {
        GraphicsDeviceManager graphics;
        //SpriteBatch spriteBatch;
        Source.GamePlay.GamePlay _gameplay;
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
            XDocument settingsXml = XDocument.Load("XML\\Settings\\Settings.xml");
            int sWidth = 800;
            int sHeight = 600;

            if (settingsXml.Element("Root").Element("Resolution") != null)
            {
                sWidth = Convert.ToInt32(settingsXml.Element("Root").Element("Resolution").Element("width").Value);
                sHeight = Convert.ToInt32(settingsXml.Element("Root").Element("Resolution").Element("height").Value);
            }
            Source.Engine.Globals.ScreenWidth = sWidth;
            Source.Engine.Globals.ScreenHeight = sHeight;

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

            _cursor = new Source.Engine.Asset2D("Assets\\cursor_arrow", Vector2.Zero, new Vector2(40,40));

            Source.Engine.Globals.NormalEffect = Source.Engine.Globals.MyContent.Load<Effect>("Shaders\\basic_shader");

            Source.Engine.Globals.MyKeyboard = new Source.Engine.Input.MyKeyboard();
            Source.Engine.Globals.MyMouse = new Source.Engine.Input.MyMouseControl();

            _gameplay = new Source.GamePlay.GamePlay();
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
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                Exit();

            Source.Engine.Globals.GlobalGameTime = gameTime;
            Source.Engine.Globals.MyKeyboard.Update();
            Source.Engine.Globals.MyMouse.Update();

            _gameplay.Update();

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

            Source.Engine.Globals.MySpriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);


            _gameplay.Draw();

                
            _cursor.Draw(Source.Engine.Globals.NewVector(Source.Engine.Globals.MyMouse.NewMousePos), Vector2.Zero, Color.White);
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
