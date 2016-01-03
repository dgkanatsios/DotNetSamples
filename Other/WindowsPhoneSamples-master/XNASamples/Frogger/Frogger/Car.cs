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
    class Car
    {
        Rectangle rectangle;
        int speed;
        Texture2D texture;
        int viewWidth;
        Color color;
        TrafficDirection direction;

        public Car(Rectangle rect, int viewWidth, int speed, TrafficDirection direction, Texture2D tex)
        {
            this.rectangle = rect;
            this.viewWidth = viewWidth;
            this.texture = tex;
            this.direction = direction;
            this.speed = speed;

            color = Color.White;
        }

        public void Update(GameTime gameTime)
        {
            if (direction == TrafficDirection.Right)
            {
                rectangle.X += speed;
            }
            else
            {
                rectangle.X -= speed;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rectangle, color);
        }

        public bool CheckCollision(Rectangle rect)
        {
            return rectangle.Intersects(rect);
        }

        public void Reset(Rectangle rect, int viewWidth, int speed, TrafficDirection direction)
        {
            this.rectangle = rect;
            this.viewWidth = viewWidth;
            this.direction = direction;
            this.speed = speed;
        }

    }
}
