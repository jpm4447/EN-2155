using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Endless_Nameless
{
    //Author(s):  Logan Lesoine
    //Class that can read and write data to the file holding high scores

    class ScoreKeeper
    {
        /********************|
        |*****Attributes*****|
        |********************/

        private List<double> highScores;
        private StreamReader timeReader;
        private StreamWriter timeWriter;

        //Constructor to initialize the streams and highScores list
        public ScoreKeeper()
        {
            highScores = new List<double>();
            LoadScores();
        }

        //Property to obtain the highScores list
        public List<double> HighScores
        {
            get { return highScores; }
        }

        //Method for updating the scores if need be
        public void UpdateScores(double time)
        { 
            //Creation of a new stream
            timeWriter = new StreamWriter("Best_Times.txt");

            //Checks to see if there are any scores in the list
            if (highScores.Count > 0)
            {
                //If there are it loops through each item in the list
                foreach(double highScore in highScores)
                {
                    //Checks to see if the new time is higher than the one in the list
                    if(time >= highScore)
                    {
                        //Checks to see if there are under 5 high scores
                        if(HighScores.Count < 5)
                        {
                            //Inserts the new score into the list
                            highScores.Insert(highScores.IndexOf(highScore), time);
                        }
                        //Else there are not enough spaces for the old score
                        else
                        {
                            //Therefore the new score replaces the old score
                            int pos = highScores.IndexOf(highScore);
                            highScores[pos] = time;
                        }
                        break;
                    }
                }
                //Writes each new score out to the file
                foreach (double score in highScores)
                {
                    timeWriter.WriteLine((String.Format("{0 : 0.00}", score)));
                    timeWriter.Flush();
                }
            }
            //If there were no scores in the list it adds the time to the list
            else
            {
                highScores.Add(time);
                timeWriter.WriteLine((String.Format("{0 : 0.00}", time)));
                timeWriter.Flush();
            }

            //Closes the stream
            timeWriter.Close();
        }

        //Method to allow for the reseting of highscores
        public void ResetScores()
        {
            //Opens the file for writing
            timeWriter = new StreamWriter("Best_Times.txt");

            //Loops through 5 times to create stub scores
            for (int x = 0; x < 5; x++)
            {
                timeWriter.WriteLine("-- : -.--");
                timeWriter.Flush();
            }

            timeWriter.Close();
        }

        /********************|
        |***Helper Methods***|
        |********************/

        //Method to load the scores from a file
        private void LoadScores()
        {
            //Try block to load the file
            try
            {
                //Creates the stream
                timeReader = new StreamReader("Best_Times.txt");

                //Creation of a string to hold the inputed lines and a double for parsing
                string line = timeReader.ReadLine();
                double num;

                //While loop to read through the file until there are no lines left
                while (line != null)
                {
                    //TryParses the line to a double
                    double.TryParse(line, out num);
                    if (num != -1 && num != 0)
                    {
                        //Adds the score to the list
                        highScores.Add(num);
                    }
                    //Reads in a new line
                    line = timeReader.ReadLine();
                }
            }
            //Catch for if the file does not exist
            catch (FileNotFoundException)
            {
                //Creates the file
                timeWriter = new StreamWriter("Best_Times.txt");

                //Loops through 5 times to create stub scores
                for (int x = 0; x < 5; x++)
                {
                    timeWriter.WriteLine("-- : -.--");
                    timeWriter.Flush();
                }

                //Closes the writer stream
                timeWriter.Close();
            }
            //Closes the reader stream
            timeReader.Close();
        }
    }
}
