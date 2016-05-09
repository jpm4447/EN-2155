// Jake Roth

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endless_Nameless
{
    class Button
    {
        private Texture2D texture;
        private Rectangle rect;

        public Texture2D Texture
        {
            get { return texture; }
            set { texture = value; }
        }

        public Rectangle Rect
        {
            get { return rect; }
            set { rect = value; }
        }

        public Button(int xPosition, int yPosition, int width, int height)
        {
            rect = new Rectangle(xPosition, yPosition, width, height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, rect, Color.White);
        }
    }
}
