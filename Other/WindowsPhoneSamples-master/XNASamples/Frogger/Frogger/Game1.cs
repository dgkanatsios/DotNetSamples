/*
 * Videogames Laboratory
 * Project Frogger
 * Developed by Kostas Anagnostou
 * 
 * You are free to use and modify the code in any way for any educational (non-commercial) purpose.
 * 
 * For more game development tutorials (in Greek) visit http://videogameslab.wordpress.com
*/

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

namespace Frogger
{
    public enum GameState
    {
        Intro,
        Playing,
        Paused,
        Win,
        Lose
    }

    public enum FrogState
    {
        OffRoad,
        OnRoad
    }

    enum TrafficDirection
    {
        Left,
        Right
    }
    
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        Texture2D whiteTile;
        Texture2D frog;
        Texture2D road;
        SpriteFont croobieFont;
        SpriteFont croobieFontXL;
        Texture2D squashedFrog;

        Rectangle roadRectagle;
        Rectangle frogRectangle;

        KeyboardState previousState;

        GameState gameState;
        FrogState frogState;

        List<Lane> lanes;
        int numLanes;

        int width;
        int height;
        Random rand = new Random(DateTime.Now.Second);
        int lives;

        int frogHeight;
        int frogWidth;
        int roadHeight;

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
            width = graphics.GraphicsDevice.Viewport.Width;
            height = graphics.GraphicsDevice.Viewport.Height;
            frogHeight = 40;
            frogWidth = 40;
            numLanes = 10;
            roadHeight = numLanes * frogHeight;
            roadRectagle = new Rectangle(0, (height - roadHeight) / 2, width, roadHeight);

            lanes = new List<Lane>();

            gameState = GameState.Intro;

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

            whiteTile = Content.Load<Texture2D>("white_tile");
            frog = Content.Load<Texture2D>("frog");
            road = Content.Load<Texture2D>("road");

            squashedFrog = Content.Load<Texture2D>("squashed_frog");

            croobieFont = Content.Load<SpriteFont>("croobie");
            croobieFontXL = Content.Load<SpriteFont>("croobieXL");

            resetGame();

        }

        private void initLanes()
        {
            lanes.Clear();
            int speed = 11; 
            for (int i = 0; i < numLanes; i++)
            {
                //short of hacky way to determine car spawn rate and car speed.
                float delay = 0.1f + 0.3f * i;
                speed -= 1;
                TrafficDirection direction = rand.Next(2) == 0 ? TrafficDirection.Left : TrafficDirection.Right;

                Lane lane = new Lane(new Vector2(0, roadRectagle.Y + i * frogRectangle.Height), frogRectangle.Height, roadRectagle, direction, delay, speed, whiteTile);
                lanes.Add(lane);
            }
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
            switch (gameState)
            {
                case GameState.Intro:
                    if ( keyClicked(Keys.Enter) )
                    {
                        resetGame();
                        gameState = GameState.Playing;
                    }
                    else if (keyClicked(Keys.Escape))
                    {
                        this.Exit();
                    }

                    break;
                case GameState.Playing:
                    if (keyClicked(Keys.P))
                    {
                        gameState = GameState.Paused;
                    }
                    else if (keyClicked(Keys.Escape))
                    {
                        gameState = GameState.Intro;
                    }

                    updateWorld(gameTime);

                    break;

                case GameState.Win:
                    if (keyClicked(Keys.Enter))
                    {
                        gameState = GameState.Playing;
                        resetGame();
                    }
                    else if (keyClicked(Keys.Escape))
                    {
                        gameState = GameState.Intro;
                    }

                    break;

                case GameState.Lose:
                    if (keyClicked(Keys.Enter))
                    {
                        gameState = GameState.Playing;
                        resetGame();
                    }
                    else if (keyClicked(Keys.Escape))
                    {
                        gameState = GameState.Intro;
                    }

                    break;

                case GameState.Paused:
                    if (keyClicked(Keys.P))
                    {
                        gameState = GameState.Playing;
                    }

                    break;

            }

            previousState = Keyboard.GetState();

            base.Update(gameTime);
        }

        private void resetGame()
        {
            frogRectangle = new Rectangle(width / 2, roadRectagle.Bottom, frogWidth, frogHeight);
            lives=3;
            frogState = FrogState.OffRoad;

            initLanes();
        }

        private void updateWorld(GameTime gameTime)
        {
            //update frog position according to user input
            updateFrogPosition();

            //update lanes (car positions)
            updateLanes(gameTime);

            //check frog/car collisions
            checkCollisions();

            if (lives <= 0)
            {
                gameState = GameState.Lose;
            }
        }

        private void updateFrogPosition()
        {
            if (keyClicked(Keys.Up))
            {
                frogRectangle.Y -= frogRectangle.Height;
                frogState = FrogState.OnRoad;
            }

            if (keyClicked(Keys.Down) && frogState == FrogState.OnRoad)
            {
                frogRectangle.Y += frogRectangle.Height;
            }            
            
            if (keyClicked(Keys.Left))
            {
                frogRectangle.X -= frogRectangle.Width;
            }

            if (keyClicked(Keys.Right))
            {
                frogRectangle.X += frogRectangle.Width;
            }

            if (frogRectangle.Left < 0)
            {
                frogRectangle.X = 0;
            }
            else if (frogRectangle.Right > roadRectagle.Right)
            {
                frogRectangle.X = roadRectagle.Right - frogRectangle.Width;
            }

            if (frogRectangle.Bottom > roadRectagle.Bottom && frogState == FrogState.OnRoad)
            {
                frogRectangle.Y = roadRectagle.Bottom - frogRectangle.Height;
            }
            else if (frogRectangle.Top < roadRectagle.Top)
            {
                gameState = GameState.Win;
            }

        }

        private void checkCollisions()
        {
            foreach (Lane lane in lanes)
            {
                if (lane.CheckCollision(frogRectangle))
                {
                    frogRectangle = new Rectangle(width / 2, roadRectagle.Bottom, frogWidth, frogHeight);
                    lives--;
                    frogState = FrogState.OffRoad;
                }
            }
        }

        private void updateLanes(GameTime gameTime)
        {
            foreach (Lane lane in lanes)
            {
                lane.Update(gameTime);
            }
        }

        protected bool keyClicked(Keys key)
        {
            return previousState.IsKeyUp(key) && Keyboard.GetState().IsKeyDown(key);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Green);

            spriteBatch.Begin();

           spriteBatch.Draw(road, roadRectagle, Color.White);

           switch (gameState)
            {
                case GameState.Intro:
                    renderStringCentered(croobieFontXL, "Frogger",  170, Color.Yellow);
                    spriteBatch.Draw(squashedFrog,new Rectangle(500, 250, 250,250), Color.White);
                    renderStringCentered(croobieFont, "Press ENTER to start",  400, Color.White);

                    break;
                case GameState.Playing:
                    renderWorld();

                    break;
                case GameState.Win:
                    renderWorld();
                    renderStringCentered(croobieFontXL, "You WIN", 170, Color.Yellow);
                    renderStringCentered(croobieFont, "Press ENTER to try again",  350, Color.White);

                    break;

                case GameState.Lose:
                    renderWorld();
                    renderStringCentered(croobieFontXL, "You LOSE", 170, Color.Yellow);
                    renderStringCentered(croobieFont, "Press ENTER to try again", 350, Color.White);

                    break;

                case GameState.Paused:
                    renderWorld();
                    renderStringCentered(croobieFontXL, "PAUSED", 170, Color.Yellow);
                    break;
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void renderWorld()
        {
            foreach (Lane lane in lanes)
            {
                lane.Draw(spriteBatch);
            }

            spriteBatch.Draw(frog, frogRectangle, Color.White);
            spriteBatch.DrawString(croobieFont, "Lives : " + lives, new Vector2(10, 10), Color.White);
        }

        private void renderStringCentered(SpriteFont font, string message, int Y, Color color)
        {
            Vector2 msgSize = font.MeasureString(message);
            spriteBatch.DrawString(font, message, new Vector2((width - msgSize.X) / 2, Y), color);
        }

    }
}
