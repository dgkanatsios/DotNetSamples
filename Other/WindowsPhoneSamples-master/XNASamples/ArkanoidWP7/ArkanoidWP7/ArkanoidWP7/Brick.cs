using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ArkanoidWP7
{
    class Brick
    {
        Rectangle rectangle;
        Texture2D texture;
        Color color;
        bool visible;
        public Brick(Rectangle rect, Color col, Texture2D text)
        {
            rectangle = rect;
            color = col;
            texture = text;
            visible = true;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (visible)
            {
                spriteBatch.Draw(texture, rectangle, color);
            }
        }

        public bool CheckHit(Rectangle ball)
        {
            if (visible && Intersects(ball))
            {
                visible = false;
                return true;
            }
            return false;
        }




        private bool Intersects(Rectangle ball)
        {
            if (rectangle.Right < ball.Left ||
            rectangle.Left > ball.Right ||
            rectangle.Top > ball.Bottom ||
            rectangle.Bottom < ball.Top)
            {
                return false;
            }
            return true;
        }
    }
}
