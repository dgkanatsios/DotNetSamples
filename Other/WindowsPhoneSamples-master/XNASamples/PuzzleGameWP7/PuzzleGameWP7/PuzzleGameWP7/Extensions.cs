using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace PuzzleGameWP7
{
    static class Extensions
    {

        public static Texture2D LineTexture { get; set; }

        public static void DrawLine(this SpriteBatch spriteBatch, Vector2 position, int width, int height,
            Color color, byte opacity, float rotation, SpriteEffects effects, float layerDepth)
        {
            color.A = opacity;
            spriteBatch.Draw(LineTexture, 
                new Rectangle((int)position.X,(int)position.Y,width,height),
                null,
                color,
                rotation,
                Vector2.Zero,
                effects,
                layerDepth);
        }

                
    }
}
