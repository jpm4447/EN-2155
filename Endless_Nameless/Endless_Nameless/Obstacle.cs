using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Endless_Nameless
{
    //Author(s):  Logan Lesoine
    //Class for obstacle creation

    class Obstacle
    {
        /********************|
        |*****Attributes*****|
        |********************/
        private Texture2D image;
        private Platform platform;
        private Rectangle collisionBox;
        private double timer;

        //Constructor
        public Obstacle(Platform plat, Texture2D img)
        {
            platform = plat;
            image = img;
            timer = 0;
            collisionBox = new Rectangle(plat.CollisionBox.X + (plat.CollisionBox.Width / 2), plat.CollisionBox.Y - (plat.CollisionBox.Height) - 100, 50, 100);
        }

        //Property for collisionBox
        public Rectangle CollisionBox
        {
            get { return collisionBox; }
            set { collisionBox = value; }
        }

        //Update method
        public void Update(GameTime gameTime, int speed)
        {
            collisionBox.X -= speed;

            timer += gameTime.ElapsedGameTime.Seconds;
        }

        //Sets the spawn
        public void ObstaclePlace()
        {
            collisionBox.X += 1280;
        }

        public void ObstacleStart()
        {
            collisionBox.X += 2560;
        }
    }
}