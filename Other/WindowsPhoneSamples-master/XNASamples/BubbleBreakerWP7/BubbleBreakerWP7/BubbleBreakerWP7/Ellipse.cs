using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace BubbleBreakerWP7
{
    class Ellipse
    {
        public Texture2D Texture {get;set;}
        public Color EllipseColor { get; set; }
        public Vector2 Location { get; set; }
        public Color OriginalEllipseColor { get; private set; }

        

        public Ellipse(Texture2D texture, Color color)
        {
            this.Texture = texture;
            Location = Vector2.Zero;
            OriginalEllipseColor = EllipseColor = color;
        }

        public void Update()
        { }


        public void Draw(SpriteBatch spriteBatch, Rectangle bounds)
        {
            spriteBatch.Draw(Texture, Location, EllipseColor);
        }


        public static List<Color> AvailableColors { get; private set; }
        static Ellipse()
        {
            AvailableColors = new List<Color>();
            AvailableColors.Add(Color.Blue);
            AvailableColors.Add(Color.Purple);
            AvailableColors.Add(Color.Green);
            AvailableColors.Add(Color.Yellow);
            AvailableColors.Add(Color.Red);
        
        }
    }

    
}
