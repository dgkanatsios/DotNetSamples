using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace PuzzleGameWP7
{
    class Tile
    {
        public Vector2 TextureLocationInSpriteSheet;
        /// <summary>
        /// This contains a number from 1-15 containing the 
        /// original location of the tile tile, 
        /// in order to determine of the player has won
        /// </summary>
        public int OriginalLocationInPuzzle;
    }
}
