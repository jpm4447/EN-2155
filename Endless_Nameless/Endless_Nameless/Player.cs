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
    //Class to allow the creation of a player object
    class Player
    {
        /********************|
        |*****Attributes*****|
        |********************/
        private KeyboardState kStateCurr;
        private KeyboardState kStatePrev;
        
        private Vector2 pos;
        private Texture2D character;
        private Rectangle collisionRect;
        private float time;
        private float veloc;

        //Player constructor
        public Player(Vector2 coords, Texture2D img, Rectangle rect)
        {
            pos = coords;
            character = img;
            collisionRect = rect;
            time = 0;
            veloc = 0;
        }

        //Property for the bounding box to allow for updating with movement
        public Rectangle CollisionRect
        {
            get { return collisionRect; }
        }

        //Property to allow the changing of the player's Y coordinate
        public float PosY
        {
            get { return pos.Y; }
            set { pos.Y = value; }
        }

        //This method detects if the jump key was pressed
        public void DetectJump()
        {
            //detects the keyboard state
            kStateCurr = Keyboard.GetState();

            //If statement to decipher if the key has already been pressed or not
            if(kStateCurr.IsKeyDown(Keys.W) && kStatePrev.IsKeyUp(Keys.W))
            {
                //If it hasn't it gives the player a velocity
                if(veloc == 0)
                {
                    veloc = -80;
                }
            }
            //After the if statement it adjusts the position accordingly along the Y axis
            MoveOnY();
            kStatePrev = kStateCurr;
        }

        //Method that manages most of the changes for draw time
        public void Update(GameTime gameTime)
        {
            //Adds the elapsed time in milliseconds to time
            time += gameTime.ElapsedGameTime.Milliseconds;

            //Adjusts the velocity value based upon if it is already in motion
            if(veloc != 0)
            {
                //If it is about to reach peak height it resets the time and changes velocity to make the drop velocity more reasonable
                if(veloc < 0 && (veloc + (9.8F * (time/1000)) > 0))
                {
                    time = 0;
                    veloc = 0.1F;
                }
                //It then continues with it's usual velocity increaes
                veloc += 9.8F * (time / 1000);
            }

            //Otherwise if the player is on a surface with no velocity, the time consistently resets
            else
            {
                time = 0;
            }

            //Temporary left &right movement until scrolling is added
            if (kStateCurr.IsKeyDown(Keys.D) == true)
            {
                pos.X += 5;
            }
            else if (kStateCurr.IsKeyDown(Keys.A) == true)
            {
                pos.X -= 5;
            }

            //Updating of collisionRect
            collisionRect = new Rectangle((int)pos.X, (int)pos.Y, collisionRect.Width, collisionRect.Height);
        }

        //Method to have the player fall initially
        public void Fall()
        {
            if(veloc == 0)
            {
                veloc += 0.1F;
            }
        }

        //Method to stop the player's velocity
        public void Stop()
        {
            if (veloc > 0)
            {
                veloc = 0;
            }
        }

        //Simple method to return true or false depending on whether the player is colliding with the object
        public Boolean IsColliding(Rectangle rect)
        {
            if (collisionRect.Intersects(rect))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /********************|
        |***Helper Methods***|
        |********************/

        //This method moves the player along the Y axis depending on their velocity and how long they have been in the air
        private void MoveOnY()
        {
            pos.Y += veloc * (time / 1000);
        }

        //Skeleton method for a duck feature
        private void Duck()
        {

        }
    }
}
