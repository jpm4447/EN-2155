using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Endless_Nameless
{
    // Author(s):  JP Meeks
    // a class that acts as an object for each new level section

    class Chunk
    {
        // attributes 
        Generation generate = new Generation();
        string source = null; // the file location to read from
        int lStitch; // stitch values are the lowest points at the start and end of a chunk to connect up to other chunks 
        int rStitch;
        int chance;
        int[,] layout = new int[8 , 6];  // the layout of each chunk and where to place chunks for each part // 6 rows 8 columns // top row is always empty
        Platform[] platform;
        Texture2D texture; // set texture upon creation in game

        // default constructor
        public Chunk(Texture2D image)
        {
            source = "start.txt";
            chance = 1;
            texture = image;

            // runs through and creates layout using a generation method
            layout = generate.GenerateLayout(source);

            // gets left and right stitch points
            rStitch = generate.GetRightStitch(layout);
            lStitch = generate.GetLeftStitch(layout);

            // get number of platforms and create platform array
            int platformAmount = generate.GetPlatformNumber(layout);
            platform = new Platform[platformAmount];
            int count = 0; // keeps track of number of platforms in following for loops

            // generate platforms
            for(int x = 0; x < 8; x ++)
            {
                for(int y = 0; y < 6; y++)
                {
                    if(layout[x,y] == 1 && count < platformAmount)
                    {
                        platform[count] = generate.SetPlatform((2560 + (x * 320)), (768 - ((y + 1) * 128)), texture);
                        count++;
                    }
                }
            }
        }

        // parameterized constructor given source location
        public Chunk(string src, Texture2D image)
        {
            source = src;
            chance = 1;
            texture = image;

            // runs through and creates layout using a generation method
            layout = generate.GenerateLayout(source);

            // gets left and right stitch points
            rStitch = generate.GetRightStitch(layout);
            lStitch = generate.GetLeftStitch(layout);

            // get number of platforms and create platform array
            int platformAmount = generate.GetPlatformNumber(layout);
            platform = new Platform[platformAmount];
            int count = 0; // keeps track of number of platforms in following for loops

            // generate platforms
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (layout[x, y] == 1 && count < platformAmount)
                    {
                        platform[count] = generate.SetPlatform((2560 + (x * 320)), (768 - ((y + 1) * 128)), texture);
                        count++;
                    }
                }
            }
        }

        // parameterized contructor given source location and possibility of spawning // currently unused
        public Chunk(string src, int chn, Texture2D image)
        {
            source = src;
            chance = chn;
            texture = image;

            // runs through and creates layout using a generation method
            layout = generate.GenerateLayout(source);

            // gets left and right stitch points
            rStitch = generate.GetRightStitch(layout);
            lStitch = generate.GetLeftStitch(layout);

            // get number of platforms and create platform array
            int platformAmount = generate.GetPlatformNumber(layout);
            platform = new Platform[platformAmount];
            int count = 0; // keeps track of number of platforms in following for loops

            // generate platforms
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 6; y++)
                {
                    if (layout[x, y] == 1 && count < platformAmount)
                    {
                        platform[count] = generate.SetPlatform((2560 + (x * 320)), (768 - ((y + 1) * 128)), texture);
                        count++;
                    }
                }
            }
        }


        // properties

        public int[,] Layout
        {
            get { return layout; }
        }
        public int RStitch
        {
            get { return rStitch; }
        }
        public int LStitch
        {
            get { return lStitch; }
        }
        public Platform[] Platforms
        {
            get { return platform; }
        }

        // method
        public void Update(GameTime gameTime, int speed)
        {
            foreach(Platform p in platform)
            {
                p.Update(gameTime, speed);
            }
        }

        // check if new chunk needs to be generated
        public bool CheckChunk()
        {
            if (platform[platform.Length - 1].CollisionBox.X <= 1280)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // set platform x location on creation
        public void ChunkPlace()
        {
            for(int x = 0; x < platform.Length; x++)
            {
                platform[x].PlatformPlace();
            }
        }

        // set platform x location on creation
        public void ChunkStart()
        {
            for (int x = 0; x < platform.Length; x++)
            {
                platform[x].PlatformStart();
            }
        }
    }
}
