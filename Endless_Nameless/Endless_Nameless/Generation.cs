using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endless_Nameless
{
    // Author(s):  JP Meeks
    // Class that contains the methods for chunk generation and for stitching chunks together

    // Each column of 2D arrays reads up from the bottom of the screen for platforms while rows read across
    // each platform should ideally take up 320 pixels across and 64 pixels vertically // each cell that can contain a platform is 320 x 128
        // each chunk is then 2x the size of the screen at any given time
        // and each platform is half the size of a platform cell

    class Generation
    {
        // attributes

        // import in new level chunks given a source file
        public int[,] GenerateLayout(string src)
        {
            // variable
            int[,] layout = new int[8, 6]; // layout to be returned

            // open text reader to get source file
            TextReader read = new StreamReader(src);
            string text = null;
            int count = 0;

            while((text = read.ReadLine()) != null)
            {
                // split source document into multiple parts
                string[] tempText = text.Split(' ');
                int tempInt = 0;    // will store parsed value of tempText or give 0

                for (int x = 0; x < tempText.Length; x++)
                {
                    int.TryParse(tempText[x], out tempInt);
                    layout[count, x] = tempInt;
                }
                layout[count, 5] = 0;
                count++;
            }

            read.Close();

            return layout;
        }

        // export stitch values for a given layout
        public int GetRightStitch(int[,] source)
        {
            // variable
            int stitch = -1;

            // get lowest platform value
            for (int x = 0; x < 5; x++)
            {
                if (source[7, x] == 1)
                {
                    stitch = x;
                    break;
                }
            }

            return stitch;
        }
        public int GetLeftStitch(int[,] source)
        {
            // variable
            int stitch = -1;

            // get lowest platform value
            for (int x = 0; x < 5; x++)
            {
                if(source[0 , x] == 1)
                {
                    stitch = x;
                    break;
                }
            }

            return stitch;
        }

        // get the number of plaforms in chunk.layout
        public int GetPlatformNumber(int[,] source)
        {
            int count = 0;

            for(int x = 0; x < 8; x++)
            {
                for(int y = 0; y < 6; y++)
                {
                    if(source[x,y] == 1)
                    {
                        count++;
                    }
                }
            }

            return count;
        }

        // generate new platforms in chunk
        public Platform SetPlatform(int xPos, int yPos, Texture2D image)
        {
            Rectangle rect = new Rectangle(xPos, yPos, 320, 64);

            Platform temp = new Platform(rect, image);

            return temp;
        }

        // check if new chunk needs to be generated
        public bool CheckChunk(Platform[] platforms)
        {
            if(platforms.Length < 1280)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
