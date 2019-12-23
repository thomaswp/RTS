using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Game_Player;
using System;
using System.Collections.Generic;

namespace RTS
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        int FPS = 60;

        GraphicsDeviceManager graphics;
        int frames = 0;
        int miliseconds = 0;
        int lastToggle = 0;
        const int TOGGLE_TIME = 3000;
        MouseState currentMouseState;
        MouseState lastMouseState;
        int spriteSpeed = 3;
        int rotationDirection = 1;

        Game_Player.Viewport viewport;
        Sprite sprite;

        public Game1()
        {

            graphics = new GraphicsDeviceManager(this);
            Graphics.DeviceManager = graphics;

            Graphics.ScreenRect = new Rect(0, 0, 640, 480);
            graphics.PreferredBackBufferWidth = 640;
            graphics.PreferredBackBufferHeight = 480;
            graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            Paths.Root = "Content/";

            IsMouseVisible = true;

            viewport = new Game_Player.Viewport(0, 0, Graphics.ScreenWidth, Graphics.ScreenHeight);
            sprite = new Sprite(viewport, new Bitmap("elmo"));
            sprite.ZoomX = sprite.ZoomY = 0.5;
            sprite.OX = sprite.Bitmap.Width / 2;
            sprite.OY = sprite.Bitmap.Height / 2;

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            Graphics.SpriteBatch = new SpriteBatch(GraphicsDevice);
            Graphics.Effects = Content.Load<Effect>("Effects");
            Graphics.TransEffect = Content.Load<Effect>("Trans");
            // TODO: use this.Content to load your game content here
        }


        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            //Globals.Audio.Dispose();
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // TODO: Add your update logic here
            if (Graphics.FPS != FPS)
            {
                FPS = Graphics.FPS;
                this.TargetElapsedTime = new TimeSpan((long)Math.Pow(10, 7) / FPS);
            }

            // Fullscreen logic
            if (lastToggle < TOGGLE_TIME)
                lastToggle += gameTime.ElapsedGameTime.Milliseconds;
            if ((Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.LeftAlt) ||
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.RightAlt)) &&
                Keyboard.GetState().IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) &&
                lastToggle >= TOGGLE_TIME)
            {
                graphics.ToggleFullScreen();
                lastToggle = 0;
            }

            // Elmo
            sprite.X += spriteSpeed * (3/2);
            sprite.Y += spriteSpeed;
            sprite.Z = viewport.Sprites.Count;
            sprite.Rotation += 0.01 * rotationDirection;
            if (sprite.X > Graphics.ScreenWidth) sprite.X = 0;
            if (sprite.Y > Graphics.ScreenHeight) sprite.Y = 0;


            // Click detection
            lastMouseState = currentMouseState;
            currentMouseState = Mouse.GetState();

            if (lastMouseState.LeftButton == ButtonState.Released && currentMouseState.LeftButton == ButtonState.Pressed)
            {
                rotationDirection *= -1;
            }


            // Snake
            if (currentMouseState.LeftButton == ButtonState.Pressed)
            {
                WeirdBall ball = new WeirdBall(viewport);
                ball.X = Mouse.GetState().X;
                ball.Y = Mouse.GetState().Y;
                ball.Z = viewport.Sprites.Count;
            }

            viewport.Sprites.ForEach(sprite => sprite.Update());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //UsageData.StartUsage("mydraw");
            Graphics.Draw();
            //Console.WriteLine(UsageData.EndUsage("mydraw"));

            base.Draw(gameTime);
            UpdateFPS(gameTime);
        }

        void UpdateFPS(GameTime gameTime)
        {
            frames++;
            miliseconds += gameTime.ElapsedGameTime.Milliseconds;
            if (miliseconds >= 1000)
            {
                this.Window.Title = Paths.Title + " - " + (frames * 1000 / miliseconds).ToString() + " FPS";
                frames = 0; miliseconds -= 1000;
            }
        }
    }
}

