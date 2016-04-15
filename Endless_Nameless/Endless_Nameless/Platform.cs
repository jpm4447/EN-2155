using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Endless_Nameless
{
    //Author(s):  Logan Lesoine
    //Class to allow the creation of platforms
    class Platform
    {
        //Attribute creation
        private Rectangle collisionBox;
        private Texture2D image;

        //Constructor to initialize the attributes
        public Platform(Rectangle rect, Texture2D img)
        {
            collisionBox = rect;
            image = img;
        }

        //Property for collisionBox to allow it's value to be obtained and changed if need be
        public Rectangle CollisionBox
        {
            get { return collisionBox; }
            set { collisionBox = new Rectangle(); }

        }

        //Update method to allow for scrolling:  Accepts a speed integer for adjusting how fast the platforms scroll
        public void Update(GameTime gameTime, int speed)
        {
            collisionBox.X -= speed;
        }

        //Method to allow the platforms to draw themselves
        //Does not currently work
        public void Draw(SpriteBatch spriteBach)
        {
            spriteBach.Draw(image, collisionBox, Color.White);
        }
    }
}
