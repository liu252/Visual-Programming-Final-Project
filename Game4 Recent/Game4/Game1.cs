using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Threading;

namespace Game4
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Texture2D startButton, exitButton, pauseButton, resumeButton, loadingScreen;
        private Vector2 startButtonPosition, exitButtonPosition, resumeButtonPosition;
        private Thread backgroundThread;
        private bool isLoading = false;
        bool isPaused; 
        MouseState mouseState, previousMouseState;
        KeyboardState keyboardState, previousKeyboardState; 

        enum GameState
        {
            StartMenu,
            Loading,
            Playing,
            Paused
        }
        static GameState gameState;

        public static Game1 Instance { get; private set; }
        public static Viewport Viewport { get { return Instance.GraphicsDevice.Viewport; } }
        public static Vector2 ScreenSize { get { return new Vector2(Viewport.Width, Viewport.Height); } }
        public static GameTime GameTime { get; private set; }

        public Game1()
        {
            Instance = this;
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

            startButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 200);
            exitButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - 50, 250);
            gameState = GameState.StartMenu;
            isPaused = false; 
            mouseState = Mouse.GetState();
            keyboardState = Keyboard.GetState();
            previousMouseState = mouseState;
            base.Initialize();
            //SpriteManager.Add(Player.Instance);
            //SpriteManager.Add(Platform.Instance);
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Images.Load(Content);
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            startButton = Content.Load<Texture2D>("start");
            exitButton = Content.Load<Texture2D>("exit");
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
            mouseState = Mouse.GetState();
            if (previousMouseState.LeftButton == ButtonState.Pressed && mouseState.LeftButton == ButtonState.Released)
            {
                MouseClicked(mouseState.X, mouseState.Y);
            }
            previousMouseState = mouseState;
            keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.P) == true && !isPaused)
            {
                gameState = GameState.Paused;
            }
            if (keyboardState.IsKeyDown(Keys.R) == true && isPaused)
            {
                gameState = GameState.Playing;
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            GameTime = gameTime;
            Controls.Update();
            PlayerStatus.Update();
            SpriteManager.Update();
            Spawner.Update();



            if (gameState == GameState.Playing && isLoading)
            {
                LoadGame();
                isLoading = false;
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
            GraphicsDevice.Clear(Color.Black);

            // TODO: Add your drawing code here
            spriteBatch.Begin();
            if (gameState == GameState.StartMenu)
            {
                spriteBatch.Draw(startButton, startButtonPosition, Color.White);
                spriteBatch.Draw(exitButton, exitButtonPosition, Color.White);
                spriteBatch.Draw(Images.Pointer, Controls.mousePosition, Color.White);
            }



            if (gameState == GameState.Playing)
            {
                //spriteBatch.Draw(pauseButton, new Vector2(0, 0), Color.White);
                spriteBatch.DrawString(Images.Font, "Lives: " + PlayerStatus.Lives, new Vector2(5), Color.White);
                DrawRightAlignedString("Score: " + PlayerStatus.Score, 5);
                DrawRightAlignedString("Multiplier: " + PlayerStatus.Multiplier, 35);

                SpriteManager.Draw(this.spriteBatch);
                spriteBatch.Draw(Images.Pointer, Controls.mousePosition, Color.White);
            }


            if(gameState == GameState.Paused)
            {
                spriteBatch.Draw(resumeButton, resumeButtonPosition, Color.White);
                spriteBatch.Draw(Images.Pointer, Controls.mousePosition, Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        private void DrawRightAlignedString(string text, float y)
        {
            var textWidth = Images.Font.MeasureString(text).X;
            spriteBatch.DrawString(Images.Font, text, new Vector2(ScreenSize.X - textWidth - 5, y), Color.White);
        }
        void LoadGame()
        {
            SpriteManager.Add(Player.Instance);
            pauseButton = Content.Load<Texture2D>("Pause");
            resumeButton = Content.Load<Texture2D>("Resume");
            resumeButtonPosition = new Vector2((GraphicsDevice.Viewport.Width / 2) - (resumeButton.Width / 2), (GraphicsDevice.Viewport.Height / 2) - (resumeButton.Height / 2));

        }
        void MouseClicked(int x, int y)
        {
            Rectangle mouseClickRect = new Rectangle(x, y, 10, 10);
            if (gameState == GameState.StartMenu)
            {
                Rectangle startButtonRect = new Rectangle((int)startButtonPosition.X, (int)startButtonPosition.Y, 100, 20);
                Rectangle exitButtonRect = new Rectangle((int)exitButtonPosition.X, (int)exitButtonPosition.Y, 100, 20);

                if (mouseClickRect.Intersects(startButtonRect))
                {
                    gameState = GameState.Playing;
                    isLoading = true;
                }
                else if (mouseClickRect.Intersects(exitButtonRect))
                {
                    Exit();
                }

                if (gameState == GameState.Playing)
                {
                    Rectangle pauseButtonRect = new Rectangle(0, 0, 70, 70);
                    if (mouseClickRect.Intersects(pauseButtonRect))
                    {
                        gameState = GameState.Paused;
                    }
                }

                if (gameState == GameState.Paused)
                {
                    Rectangle resumeButtonRect = new Rectangle((int)resumeButtonPosition.X, (int)resumeButtonPosition.Y, 100, 20);
                    if (mouseClickRect.Intersects(resumeButtonRect))
                    {
                        gameState = GameState.Playing;
                        isLoading = true;
                    }
                }

            }
        }

    }
}
