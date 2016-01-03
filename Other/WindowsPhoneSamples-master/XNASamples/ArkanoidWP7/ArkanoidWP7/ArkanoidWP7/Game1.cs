using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System.Diagnostics;

namespace ArkanoidWP7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {


        private enum GameState
        {
            Initial,
            Playing,
            Won,
            Lost

        }

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        int score = 0;
        int lives;
        Texture2D whiteTile;
        Rectangle paddle;
        int paddleSpeed;
        Rectangle ball;
        Vector2 ballDirection;
        float ballSpeed;

        int brickWidth;
        int brickHeight;
        int bricksPerRow;
        int numOfRows;
        int brickSpacing;
        int rowStart;
        Brick[] bricks;
        int viewWidth;
        int viewHeight;
        GameState gameState;
        SpriteFont font;
        int numOfVisibleBricks = 0;
        short previousPaddleDirectionSign;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);

            // Pre-autoscale settings.
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
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

            viewHeight = graphics.GraphicsDevice.Viewport.Height;
            viewWidth = graphics.GraphicsDevice.Viewport.Width;
            paddle = new Rectangle(150, 700, 90, 15);
            ball = new Rectangle(150, 400, 15, 15);
            bricksPerRow = 13;
            numOfRows = 5;
            brickSpacing = 10;
            bricks = new Brick[bricksPerRow * numOfRows];
            brickWidth = (viewWidth - (bricksPerRow - 1) * brickSpacing) / bricksPerRow;
            brickHeight = 20;
            paddleSpeed = 10;
            rowStart = 3 * brickHeight;


            paddle = new Rectangle(150, 700, 90, 15);
            ball = new Rectangle(150, 400, 15, 15);
            ballDirection = new Vector2(1, -1);
            ballDirection.Normalize();
            ballSpeed = 5;

            lives = 3;
            gameState = GameState.Initial;

            base.Initialize();
        }

        Random r = new Random(DateTime.Now.Millisecond);
        private Color GetRandomColor()
        {
            return new Color(r.Next(20, 255), r.Next(20, 255), r.Next(20, 255));
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.

            spriteBatch = new SpriteBatch(GraphicsDevice);
            whiteTile = Content.Load<Texture2D>("white_tile");
            font = Content.Load<SpriteFont>("arial");
            for (int j = 0; j < numOfRows; j++)
            {
                for (int i = 0; i < bricksPerRow; i++)
                {
                    Rectangle rect = new Rectangle(i * (brickWidth + brickSpacing), rowStart + j * (brickHeight + brickSpacing), brickWidth,
                    brickHeight);
                    bricks[j * bricksPerRow + i] = new Brick(rect, GetRandomColor(), whiteTile);
                    numOfVisibleBricks++;
                }
            }

            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
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
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();

            // TODO: Add your update logic here


            switch (gameState)
            {
                    
                case GameState.Initial:
                    {
                        TouchCollection tc = TouchPanel.GetState();
                        if (tc.Count > 0)
                            gameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    {
                        UpdateWorld();
                    }
                    break;
                case GameState.Won:
                    {
                        TouchCollection tc = TouchPanel.GetState();
                        if (tc.Count > 0)
                            gameState = GameState.Playing;
                    }
                    break;
                case GameState.Lost:
                    {
                        TouchCollection tc = TouchPanel.GetState();
                        if (tc.Count > 0)
                            gameState = GameState.Playing;
                    }
                    break;

            }

            base.Update(gameTime);


        }

        private void UpdateWorld()
        {
            if (lives <= 0)
            {
                gameState = GameState.Lost;
            }
            else if (numOfVisibleBricks <= 0)
            {
                gameState = GameState.Won;
            }


            ball.X += (int)(ballDirection.X * ballSpeed);
            ball.Y += (int)(ballDirection.Y * ballSpeed);


            paddleSpeed = 10;
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count != 0)
            {
                TouchLocation tl = tc[0];

                int distance = (int)tl.Position.X - (int)(paddle.X + paddle.Width / 2);

                paddleSpeed *= Math.Sign(distance);

                if (Math.Sign(distance) == previousPaddleDirectionSign)
                {
                    paddleSpeed += Math.Sign(distance) * 5;
                }
                previousPaddleDirectionSign = (short)Math.Sign(distance);
                
                Debug.WriteLine(previousPaddleDirectionSign);
                
                if (Math.Abs(distance) >= 3)
                    paddle.X += paddleSpeed;
            }


            //check paddle-wall collision
            if (paddle.Left < 0)
            {
                paddle.X = 0;
            }
            else if (paddle.Right > viewWidth)
            {
                paddle.X = viewWidth - paddle.Width;
            }

            //check ball-wall collision
            if (ball.Left <= 0 || ball.Right >= viewWidth)
            {
                ballDirection.X = -ballDirection.X;
            }
            else if (ball.Top <= 0 || ball.Bottom >= viewHeight)
            {
                ballDirection.Y = -ballDirection.Y;
                if (ball.Bottom >= viewHeight)
                {
                    lives--;
                }
            }




            //check ball-paddle collision
            if (ballDirection.Y > 0 &&
            ball.Bottom >= paddle.Top &&
            ((ball.Left >= paddle.Left && ball.Left <= paddle.Right) ||
            (ball.Right >= paddle.Left && ball.Right <=
            paddle.Right))
            )
            {
                ballDirection.Y = -ballDirection.Y;
            }

            foreach (Brick brick in bricks)
            {
                if (brick.CheckHit(ball))
                {
                    score += 10;
                    ballDirection.Y = -ballDirection.Y;
                    numOfVisibleBricks--;
                    break;
                }
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            GraphicsDevice.Clear(Color.Black);
            spriteBatch.Begin();

            switch (gameState)
            {

                case GameState.Initial:
                    {
                        RenderStringCentered("Arkanoid WP7", 400, Color.Red);
                    }
                    break;
                case GameState.Playing:
                    {
                        foreach (Brick brick in bricks)
                        {
                            brick.Draw(spriteBatch);
                        }
                        spriteBatch.Draw(whiteTile, paddle, Color.White);
                        spriteBatch.Draw(whiteTile, ball, Color.Yellow);
                        RenderStringCentered(String.Format("Score = {0}, Lives Left = {1}", this.score, this.lives), 750, Color.White);
                    }
                    break;
                case GameState.Won:
                    {
                        RenderStringCentered("Congratulations, you won!", 400, Color.Red);
                    }
                    break;
                case GameState.Lost:
                    {
                        RenderStringCentered("Sorry, you lost! But you can try again!", 400, Color.Red);
                    }
                    break;
            }
            spriteBatch.End();

            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }



        private void RenderStringCentered(string message, int Y, Color color)
        {
            Vector2 msgSize = font.MeasureString(message);
            spriteBatch.DrawString(font, message, new Vector2((viewWidth - msgSize.X) / 2, Y), color);
        }
    }
}
