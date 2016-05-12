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
        private Obstacle obst;
        private Random rand;
        private double timer;
        int obstacleSpawn;

        //Constructor to initialize the attributes
        public Platform(Rectangle rect, Texture2D img)
        {
            collisionBox = rect;
            image = img;
            rand = new Random();
            obstacleSpawn = rand.Next(2);
            timer = 0;

            if (obstacleSpawn == 0)
            {
                obst = new Obstacle(this, img);
            }
        }

        //Property for collisionBox to allow it's value to be obtained and changed if need be
        public Rectangle CollisionBox
        {
            get { return collisionBox; }
            set { collisionBox = new Rectangle(); }

        }

        //Property to get and set the Obstacle object
        public Obstacle Obst
        {
            get { return obst; }
            set { obst = value; }
        }

        //Update method to allow for scrolling:  Accepts a speed integer for adjusting how fast the platforms scroll
        public void Update(GameTime gameTime, int speed, double currTime)
        {
            collisionBox.X -= speed;

            timer = currTime;

            if (timer < 15F)
            {
                obst = null;
            }

            if (obst != null)
            {
                obst.Update(gameTime, speed);
            }
        }

        // set platfrom location
        public void PlatformPlace()
        {
            collisionBox.X += 1280;
        }

        public void PlatformStart()
        {
            collisionBox.X += 2560;
        }

        //Method to allow the platforms to draw themselves
        //Does not currently work
        public void Draw(SpriteBatch spriteBach)
        {
            spriteBach.Draw(image, collisionBox, Color.White);
        }
    }
}
