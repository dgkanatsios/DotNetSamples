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

namespace BubbleBreakerWP7
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class BubbleBreaker : Microsoft.Xna.Framework.Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Random r = new Random(DateTime.Now.Millisecond);
        Ellipse[,] ellipses;


        public const int EllipseHeight = 45;
        public const int EllipseWidth = 45;
        public const int EllipsesColumns = 10;
        public const int EllipsesRows = 16;

        private bool AreEllipsesSelected = false;
        private bool IsGameOver = false;

        private int score = 0;
        private const string messageScore = "Score = {0}";
        private const string messageGameOver = " Game Over";

        private SpriteFont font;

        private List<Ellipse> SelectedEllipses;
        private const int minEllipsesToRemove = 2;

        private Color selectedEllipseColor = Color.White;

        SoundEffect selectedSoundEffect;
        SoundEffect deletedSoundEffect;

        private Color GetRandomEllipseColor()
        {
            return Ellipse.AvailableColors[r.Next(0, Ellipse.AvailableColors.Count)];
        }

        public BubbleBreaker()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            graphics.SupportedOrientations = DisplayOrientation.Portrait;
            graphics.PreferredBackBufferHeight = 800;
            graphics.PreferredBackBufferWidth = 480;

            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
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

            InitializeGame();

            base.Initialize();
        }

        private void InitializeGame()
        {
            AreEllipsesSelected = false;
            IsGameOver = false;
            score = 0;
            ellipses = new Ellipse[EllipsesColumns, EllipsesRows];
            for (int column = 0; column < EllipsesColumns; column++)
            {
                for (int row = 0; row < EllipsesRows; row++)
                {
                    Ellipse el = new Ellipse(this.Content.Load<Texture2D>("ellipse"), GetRandomEllipseColor());
                    el.Location = new Vector2(column * EllipseWidth, row * EllipseHeight);
                    ellipses[column, row] = el;
                }
            }
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            selectedSoundEffect = Content.Load<SoundEffect>("sound1");
            deletedSoundEffect = Content.Load<SoundEffect>("sound2");

            font = Content.Load<SpriteFont>("Arial");


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

            HandleTouchInput();

            IsGameOver = CheckIsGameOver();

            base.Update(gameTime);
        }

        private void HandleTouchInput()
        {
            TouchCollection touchCollection = TouchPanel.GetState();
            if (touchCollection.Count > 0)
            {
                TouchLocation touchLocation = touchCollection[0];


                if (touchLocation.State != TouchLocationState.Released)
                {
                    return;
                }


                //check if the user wants to reset the game
                if (touchLocation.Position.Y > 750)
                {
                    InitializeGame();
                    return;
                }


                int column = (int)(touchLocation.Position.X / EllipseWidth);
                int row = (int)(touchLocation.Position.Y / EllipseHeight);

                //out of ellipses bounds
                if (column >= EllipsesColumns || row >= EllipsesRows) return;

                Ellipse selectedEllipse = ellipses[column, row];

                if (!AreEllipsesSelected)//user selects an ellipse
                {
                    if (selectedEllipse != null)
                    {
                        SelectedEllipses = new List<Ellipse>();
                        MarkEllipses(selectedEllipse, column, row, selectedEllipse.EllipseColor);
                        if (SelectedEllipses.Count < minEllipsesToRemove) //not enough selected ellipses
                        {
                            //reset the selected
                            foreach (Ellipse el in SelectedEllipses)
                                el.EllipseColor = el.OriginalEllipseColor;
                          
                            return;
                        }
                        AreEllipsesSelected = true;
                        selectedSoundEffect.Play();
                    }
                }
                else if (AreEllipsesSelected) //ellipses are already selected
                {
                    if (SelectedEllipses.Contains(selectedEllipse) && SelectedEllipses.Count >= minEllipsesToRemove)//let's disappear them!
                    {
                        foreach (Ellipse el in SelectedEllipses)
                        {
                            ellipses[(int)(el.Location.X / EllipseWidth), (int)(el.Location.Y / EllipseHeight)] = null;
                        }

                        score += SelectedEllipses.Count * (SelectedEllipses.Count - 1);

                        //let's deorganize the rest of the ellipses
                        ReallocateEllipses();
                        deletedSoundEffect.Play();
                    }
                    else
                    {
                        foreach (Ellipse el in SelectedEllipses)
                        {
                            el.EllipseColor = el.OriginalEllipseColor;
                        }
                    }
                    AreEllipsesSelected = false;
                }
            }
        }

        private void ReallocateEllipses()
        {
            //first, let's clear the empty spaces in the rows
            for (int column = 0; column < EllipsesColumns; column++)
                for (int row = EllipsesRows - 1; row >= 0; row--)
                {
                    //
                    for (int l = 1; l <= row; l++)
                    {
                        if (ellipses[column, l] == null && ellipses[column, l - 1] != null)
                        {
                            ellipses[column, l] = ellipses[column, l - 1];
                            ellipses[column, l - 1] = null;
                            ellipses[column, l].Location += new Vector2(0, EllipseHeight);
                        }
                    }

                }

            //now, we'll check for empty columns
            for (int column = EllipsesColumns - 1; column >= 0; column--)
            {
                for (int row = 1; row <= column; row++)
                {
                    //we'll check the bottom element
                    //if it's null, then the whole row is null
                    if (ellipses[row, EllipsesRows - 1] == null && ellipses[row - 1, EllipsesRows - 1] != null)
                    {
                        //copy entire column...
                        for (int k = 0; k < EllipsesRows; k++)
                        {
                            if (ellipses[row - 1, k] == null) continue;
                            ellipses[row, k] = ellipses[row - 1, k];
                            ellipses[row - 1, k] = null;
                            ellipses[row, k].Location += new Vector2(EllipseWidth, 0);
                        }
                    }
                }
            }
        }

        private void MarkEllipses(Ellipse ellipse, int column, int row, Color colorToCompare)
        {
            if (ellipse != null)
            {
                if (ellipse.EllipseColor == colorToCompare)
                {
                    ellipse.EllipseColor = selectedEllipseColor;
                    SelectedEllipses.Add(ellipse);

                    //check left
                    if (column != 0)
                        MarkEllipses(ellipses[column - 1, row], column - 1, row, colorToCompare);
                    if (row != 0) //check top
                        MarkEllipses(ellipses[column, row - 1], column, row - 1, colorToCompare);
                    if (row != EllipsesRows - 1) //check bottom
                        MarkEllipses(ellipses[column, row + 1], column, row + 1, colorToCompare);
                    if (column != EllipsesColumns - 1) //check top
                        MarkEllipses(ellipses[column + 1, row], column + 1, row, colorToCompare);
                }
                else
                    return;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            //get a black background
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            //draw all the ellipses
            foreach (Ellipse e in ellipses)
            {
                if (e != null)
                    e.Draw(spriteBatch, graphics.GraphicsDevice.Viewport.Bounds);
            }


            DrawScore(spriteBatch);

            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawScore(SpriteBatch spriteBatch)
        {
            Vector2 textLocation = new Vector2(10, 750);

            spriteBatch.DrawString(font, string.Format(messageScore, score), textLocation, Color.White);

            if (IsGameOver)
            {
                Vector2 msgSize = font.MeasureString(messageScore);
                spriteBatch.DrawString(font, string.Format(messageGameOver, score), new Vector2(textLocation.X + msgSize.X + 10, textLocation.Y), Color.White);
            }
        }


        private bool CheckIsGameOver()
        {
            //if there are any ellipses selected, there's no point in checking as it's definitely not game over
            if (AreEllipsesSelected) return false;

            for (int column = 0; column <= EllipsesColumns - 1; column++)
            {
                for (int row = 0; row < EllipsesRows - 1; row++)
                {
                    //we are comparing each ellipse with the ones located below and right from it
                    if (ellipses[column, row] == null) continue;


                    if (ellipses[column, row].EllipseColor == ellipses[column, row + 1].EllipseColor)
                        return false;

                    if (column < EllipsesColumns - 1)
                    {
                        if (ellipses[column + 1, row] == null) continue;

                        if (ellipses[column, row].EllipseColor == ellipses[column + 1, row].EllipseColor)
                            return false;
                    }
                }
            }

            return true;

        }
    }
}
