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
        private float slideTime;
        private float veloc;

        // animation attributes
        int frame;
        double timePerFrame = 500;
        int numFrames = 4;
        int framesElapsed;
        const int PLAYER_HEIGHT = 64;
        const int PLAYER_WIDTH = 32;

        enum Animation
        {
            Jump, Duck, Move
        }

        Animation animations;

        //Player constructor
        public Player(Vector2 coords, Texture2D img, Rectangle rect)
        {
            pos = coords;
            character = img;
            collisionRect = rect;
            time = 0;
            slideTime = 500;
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

        //Method that manages most of the changes for draw time
        public void Update(GameTime gameTime)
        {
            framesElapsed = (int)(gameTime.TotalGameTime.TotalMilliseconds / timePerFrame);
            frame = framesElapsed % numFrames + 1;

            //Detects player input for jumping
            DetectJump();

            //Detects player input for Ducking
            Duck(gameTime);

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
            MoveOnY();
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

        //Method to check for upcomming collisions and fix player positioning accordingly
        public void CheckCollisions(List<Platform> platforms)
        {
            float tempY = PosY;

            //Checks the player against each platform and stops them if they collide with one
            foreach (Platform plat in platforms)
            {
                //Checks for collisions with the platform
                if(IsColliding(plat.CollisionBox))
                {
                    //Checks to see if the player is falling
                    if(veloc >= 0)
                    {
                        //Checks to see if the players height is within a decent proximity to the top of the platform
                        if (PosY + collisionRect.Height < plat.CollisionBox.Y + plat.CollisionBox.Height / 1.5)
                        {
                            //Stops the player and updates their position
                            Stop();
                            PosY = plat.CollisionBox.Y - CollisionRect.Height + 1;
                        }
                        
                    }
                }
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

        //Method to stop the player's velocity
        private void Stop()
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

        //This method detects if the jump key was pressed
        private void DetectJump()
        {
            //detects the keyboard state
            kStateCurr = Keyboard.GetState();

            //If statement to decipher if the key has already been pressed or not
            if ((kStateCurr.IsKeyDown(Keys.W) && kStatePrev.IsKeyUp(Keys.W)) || (kStateCurr.IsKeyDown(Keys.Space) && kStatePrev.IsKeyUp(Keys.Space))
                || (kStateCurr.IsKeyDown(Keys.Up) && kStatePrev.IsKeyUp(Keys.Up)))
            {
                //If it hasn't it gives the player a velocity
                if (veloc == 0)
                {
                    veloc = -80;
                }
            }

            kStatePrev = kStateCurr;
        }

        //Method that allows the player to duck underneath obstacles
        private void Duck(GameTime time)
        {
            //Setting of the current keyboards state
            kStateCurr = Keyboard.GetState();

            //Checks to see if any of the keys required for ducking are pressed and if the duck is available as well as if the player is in the air
            if((kStateCurr.IsKeyDown(Keys.S) || kStateCurr.IsKeyDown(Keys.Down) || kStateCurr.IsKeyDown(Keys.LeftControl)) && slideTime == 500 && veloc == 0)
            {
                //If one of the keys is pressed, the duck is available for use, and the player is not in the air they duck
                collisionRect.Height = collisionRect.Height / 2;

                //Their position is then adjusted accordingly along the y axis
                PosY += collisionRect.Height;

                //Initial time subtraction
                slideTime -= time.ElapsedGameTime.Milliseconds;
            }

            //Checks to see if the duck has run out
            else if(slideTime <= 0)
            {
                //If yes the position is updated accordingly along the y axis and the collision box is fixed
                PosY -= collisionRect.Height;
                collisionRect.Height = collisionRect.Height * 2;

                //An extra amount of time is added so the player cannot duck again right away
                slideTime = 700;
            }

            //Checks to see if the player is in the proccess of ducking
            else if(slideTime < 500 && slideTime > 0)
            {

                //If they jump while they are jumping it cancels out their duck
                if(veloc != 0)
                {
                    slideTime = 0;
                }
                
                //Ducking time left is then adjusted
                slideTime -= time.ElapsedGameTime.Milliseconds;
            }

            //Checks to see if there is a cooldown on the duck
            else if(slideTime > 500)
            {
                //If there is a fair amount of time still left in the cooldown in adjusts the cooldown
                if(slideTime - time.ElapsedGameTime.Milliseconds > 500)
                {
                    slideTime -= time.ElapsedGameTime.Milliseconds;
                }

                //Otherwise the cooldown is removed
                else
                {
                    slideTime = 500;
                }
            }
        }

        public void Draw(SpriteBatch spriteBatch, Texture2D texture)
        {
            spriteBatch.Draw(texture, pos, new Rectangle(32 + frame * 64, 0, 64, 128), Color.White);
        }
    }
}
