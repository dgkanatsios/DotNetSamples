using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;

namespace PuzzleGameWP7
{
    class PuzzleGame : DrawableGameComponent
    {
        GameState gameState;

        SpriteFont font;
        Texture2D fullPicture;
        SpriteBatch spriteBatch;

        const int Columns = 3;
        const int Rows = 5;

        const int TileWidth = 160;
        const int TileHeight = 160;

        Tile[,] tilesArray;
        Tile[,] tilesArrayTemp;
        Random random = new Random();
        public PuzzleGame(Game game) : base(game) { }

        public override void Initialize()
        {
            gameState = GameState.StartScreen;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(Game.GraphicsDevice);
            fullPicture = this.Game.Content.Load<Texture2D>("picture");
            font = this.Game.Content.Load<SpriteFont>("font");
            Extensions.LineTexture = this.Game.Content.Load<Texture2D>("pixel");
            LoadTilesToTheirInitialLocation();

            base.LoadContent();
        }

        private void LoadTilesToTheirInitialLocation()
        {
            tilesArray = new Tile[Columns, Rows];
            for (int column = 0; column < Columns; column++)
                for (int row = 0; row < Rows; row++)
                {
                    Tile t = new Tile();
                    t.TextureLocationInSpriteSheet.X = TileWidth * column;
                    t.TextureLocationInSpriteSheet.Y = TileHeight * row;
                    t.OriginalLocationInPuzzle = row * Columns + column + 1;
                    tilesArray[column, row] = t;
                }
        }


        public override void Update(GameTime gameTime)
        {
            TouchLocation? tl = null;
            TouchCollection tc = TouchPanel.GetState();
            if (tc.Count > 0 && tc[0].State == TouchLocationState.Released)
            {
                tl = tc[0];
            }

            switch (gameState)
            {
                case GameState.StartScreen:
                    if (tl != null)
                    {
                        LoadTilesIntoRandomLocations();
                        gameState = GameState.Playing;
                    }
                    break;
                case GameState.Playing:
                    if (tl != null)
                    {
                        int hitTileColumn = (int)(tl.Value.Position.X / TileWidth);
                        int hitTileRow = (int)(tl.Value.Position.Y / TileHeight);
                        CheckAndSwap(hitTileColumn, hitTileRow);
                        if (CheckIfPlayerWins())
                            gameState = GameState.Winner;
                    }
                    else if (tl == null) //user has not touched the screen, so don't bother drawing
                        Game.SuppressDraw();
                    break;
                case GameState.Winner:
                    if (tl != null)
                        gameState = GameState.StartScreen;
                    break;
            }

            base.Update(gameTime);
        }

        private bool CheckIfPlayerWins()
        {
            bool playerWins = true;

            for (int column = 0; column < Columns; column++)
                for (int row = 0; row < Rows; row++)
                {
                    if (tilesArray[column, row] == null) continue; //if we are at the empty tile, just continue
                    if (tilesArray[column, row].OriginalLocationInPuzzle != row * Columns + column + 1)
                    {
                        playerWins = false;
                        break;
                    }
                }

            return playerWins;
        }

        private void CheckAndSwap(int column, int row)
        {
            if (row > 0 && tilesArray[column, row - 1] == null)
            {
                tilesArray[column, row - 1] = tilesArray[column, row];
                tilesArray[column, row] = null;
            }
            else if (column > 0 && tilesArray[column - 1, row] == null)
            {
                tilesArray[column - 1, row] = tilesArray[column, row];
                tilesArray[column, row] = null;
            }
            else if (row < Rows - 1 && tilesArray[column, row + 1] == null)
            {
                tilesArray[column, row + 1] = tilesArray[column, row];
                tilesArray[column, row] = null;
            }
            else if (column < Columns - 1 && tilesArray[column + 1, row] == null)
            {
                tilesArray[column + 1, row] = tilesArray[column, row];
                tilesArray[column, row] = null;
            }
        }

        private void LoadTilesIntoRandomLocations()
        {
            tilesArrayTemp = new Tile[Columns, Rows];
            for (int column = 0; column < Columns; column++)
                for (int row = 0; row < Rows; row++)
                {
                    if (column == Columns - 1 && row == Rows - 1) break;

                    int newColumn; // = random.Next(0, Columns);
                    int newRow; // = random.Next(0, Rows);
                    do
                    {
                        newColumn = random.Next(0, Columns);
                        newRow = random.Next(0, Rows);
                    } while (tilesArrayTemp[newColumn, newRow] != null || (newColumn == Columns - 1 && newRow == Rows - 1));
                    tilesArrayTemp[newColumn, newRow] = tilesArray[column, row];
                }
            tilesArray = tilesArrayTemp;
        }

        public override void Draw(GameTime gameTime)
        {
            spriteBatch.Begin();
            
            switch(gameState)
            {
                case GameState.StartScreen:
                
                    DrawTiles();
                    break;
                case GameState.Playing:
                    DrawTiles();
                    DrawLines();
                    break;
                case GameState.Winner:
                    DrawWinnerMessage();
                    break;

            }
            spriteBatch.End();
            base.Draw(gameTime);
        }

        private void DrawWinnerMessage()
        {
            string congrats = "Congratulations!";
            string tryagain = "Tap to play again";

            Vector2 v = font.MeasureString(congrats);
            Vector2 v2 = font.MeasureString(tryagain);

            spriteBatch.DrawString(font, congrats, new Vector2((Game.GraphicsDevice.Viewport.Width / 2) - (v.X / 2), 200), Color.White);
            spriteBatch.DrawString(font, tryagain, new Vector2((Game.GraphicsDevice.Viewport.Width / 2) - (v2.X / 2), 400), Color.White);
        }

        private void DrawLines()
        {
            //draw first vertical line
            spriteBatch.DrawLine(new Vector2(TileWidth - 1, 0), 2, Game.GraphicsDevice.Viewport.Height,
                Color.White, 100, 0, SpriteEffects.None, 0);
            //draw second vertical line
            spriteBatch.DrawLine(new Vector2(2 * TileWidth - 1, 0), 2, Game.GraphicsDevice.Viewport.Height,
                Color.White, 100, 0, SpriteEffects.None, 0);
            //draw first horizontal line
            spriteBatch.DrawLine(new Vector2(0,TileHeight - 1),  Game.GraphicsDevice.Viewport.Width, 2,
                Color.White, 100, 0, SpriteEffects.None, 0);
            //draw second horizontal line
            spriteBatch.DrawLine(new Vector2(0, 2* TileHeight - 1), Game.GraphicsDevice.Viewport.Width, 2,
               Color.White, 100, 0, SpriteEffects.None, 0);
            //draw third horizontal line
            spriteBatch.DrawLine(new Vector2(0, 3 * TileHeight - 1), Game.GraphicsDevice.Viewport.Width, 2,
               Color.White, 100, 0, SpriteEffects.None, 0);
            //draw fourth horizontal line
            spriteBatch.DrawLine(new Vector2(0, 4 * TileHeight - 1), Game.GraphicsDevice.Viewport.Width, 2,
               Color.White, 100, 0, SpriteEffects.None, 0);
        }

        private void DrawTiles()
        {
            for (int column = 0; column < Columns; column++)
            {
                for (int row = 0; row < Rows; row++)
                {
                    if (tilesArray[column, row] == null) continue;

                    spriteBatch.Draw(fullPicture,
                        new Vector2(column * TileWidth, row * TileHeight),
                        new Rectangle((int)tilesArray[column, row].TextureLocationInSpriteSheet.X,
                            (int)tilesArray[column, row].TextureLocationInSpriteSheet.Y,
                            TileWidth, TileHeight),
                        Color.White,
                        0f, //rotation
                        Vector2.Zero,//origin
                        1,//scale
                        SpriteEffects.None,
                        0);
                }
            }
        }
    }


    enum GameState
    {
        StartScreen,
        Playing,
        Winner
    }
}
