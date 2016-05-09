using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Endless_Nameless
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        const int SPEED = 6;

        //Extra attribute creation
        Texture2D image;
        Texture2D playerImage;
        Texture2D groundImg;
        Vector2 coord;
        Player player1;
        List<Platform> platformList;
        double timer;
        double timeLived;
        ScoreKeeper keeper;

        // Jake's stuff
        // Initializations
        Button start;
        Texture2D startButton;
        Texture2D startSelect;
        int sX, sY;
        int sW = 200;
        int sH = 50;

        Button option;
        Texture2D optButton;
        Texture2D optSelect;
        int oX, oY;
        int oW = 200;
        int oH = 50;

        Button back;
        Texture2D backButton;
        Texture2D backSelect;
        int bX, bY;
        int bW = 200;
        int bH = 50;

        Button exit;
        Texture2D exitButton;
        Texture2D exitSelect;
        int eX, eY;
        int eW = 200;
        int eH = 50;

        MouseState mStatePrev;
        MouseState mouseState;

        SpriteFont font;
        Vector2 fontLoc;

        // chunk generation sources
        string[] chunkSource = new string[16];
        Chunk currChunk;
        Chunk prevChunk;
        Chunk[] chunk;
        const int CHUNK_SOURCE = 7;

        Random rando = new Random();

        // Menu States
        enum MenuMode
        {
            Main,
            Options
        }
        MenuMode menuMode;

        enum GameMode
        {
            Menu,
            Game,
            Gameover
        }
        GameMode gameMode;


        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferWidth = 1280;
            graphics.PreferredBackBufferHeight = 768;
            graphics.ApplyChanges();
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // Original menu mode is "main"
            menuMode = MenuMode.Main;
            // Original game mode is "menu..." you start at the menu
            gameMode = GameMode.Menu;

            // Make mouse visible
            this.IsMouseVisible = true;

            // Original menu mode is "main"
            menuMode = MenuMode.Main;
            // Original game mode is "menu..." you start at the menu
            gameMode = GameMode.Menu;

            // Initial button placement
            sX = GraphicsDevice.Viewport.Width / 2 - 100;
            sY = GraphicsDevice.Viewport.Height / 2 - 100;
            start = new Button(sX, sY, sW, sH);

            oX = GraphicsDevice.Viewport.Width / 2 - 100;
            oY = GraphicsDevice.Viewport.Height / 2;
            option = new Button(oX, oY, oW, oH);

            eX = GraphicsDevice.Viewport.Width / 2 - 100;
            eY = GraphicsDevice.Viewport.Height / 2 + 100;
            exit = new Button(eX, eY, eW, eH);

            bX = GraphicsDevice.Viewport.Width / 2 - 100;
            bY = GraphicsDevice.Viewport.Height / 2;
            back = new Button(bX, bY, bW, bH);

            // Font location
            fontLoc = new Vector2((GraphicsDevice.Viewport.Width / 2) - 100, 25);

            //Initialization of the player and platforms
            coord = new Vector2(100, 100);
            player1 = new Player(coord, image, new Rectangle((int)coord.X, (int)coord.Y, 64, 128));
            /*
            platforms = new List<Platform>();
            platforms.Add(new Platform(new Rectangle((int)coord.X, (int)coord.Y + 300, 300, 50), groundImg));
            platforms.Add(new Platform(new Rectangle(700, 400, 300, 50), groundImg));
            platforms.Add(new Platform(new Rectangle(700, 100, 300, 50), groundImg));
            platforms.Add(new Platform(new Rectangle(1700, 400, 300, 50), groundImg));
            platforms.Add(new Platform(new Rectangle(2200, 300, 300, 50), groundImg));
            platforms.Add(new Platform(new Rectangle(2600, 100, 300, 50), groundImg));
            */
            
            platformList = new List<Platform>();

            //Initialization of the ScoreKeeper
            keeper = new ScoreKeeper();

            // load in chunks
            /*
            int count = 0; // temp variable to assits with initialization of chunks
            while(chunkSource[count] != null)
            {
                count++;
            }
            chunk = new Chunk[count]; 

            for(int x = 0; x < count; x++)
            {
                chunk[x] = new Chunk(chunkSource[x], groundImg);
            }
            */

            base.Initialize();

        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            // TODO: use this.Content to load your game content here
            image = Content.Load<Texture2D>("char_music_1");
            playerImage = Content.Load<Texture2D>("SpriteSheet");
            groundImg = Content.Load<Texture2D>("platform");

            // JAKE
            startButton = Content.Load<Texture2D>("startbutton");
            startSelect = Content.Load<Texture2D>("startSelect");
            optButton = Content.Load<Texture2D>("optionbutton");
            optSelect = Content.Load<Texture2D>("optionSelect");
            exitButton = Content.Load<Texture2D>("exitbutton");
            exitSelect = Content.Load<Texture2D>("exitSelect");
            backButton = Content.Load<Texture2D>("backbutton");
            backSelect = Content.Load<Texture2D>("backSelect");
            font = Content.Load<SpriteFont>("mainFont");

            // Chunk generation sources
            chunkSource[0] = "start.txt";
            chunkSource[1] = "source1.txt";
            chunkSource[2] = "source2.txt";
            chunkSource[3] = "source3.txt";
            chunkSource[4] = "source4.txt";
            chunkSource[5] = "source5.txt";
            chunkSource[6] = "source6.txt";

        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            switch (gameMode)
            {
                case GameMode.Menu:
                    // Button placement scaling
                    sX = GraphicsDevice.Viewport.Width / 2 - 100;
                    sY = GraphicsDevice.Viewport.Height / 2 - 100;
                    start.Texture = startButton;
                    start.Rect = new Rectangle(sX, sY, sW, sH);

                    oX = GraphicsDevice.Viewport.Width / 2 - 100;
                    oY = GraphicsDevice.Viewport.Height / 2;
                    option.Texture = optButton;
                    option.Rect = new Rectangle(oX, oY, oW, oH);

                    eX = GraphicsDevice.Viewport.Width / 2 - 100;
                    eY = GraphicsDevice.Viewport.Height / 2 + 100;
                    exit.Texture = exitButton;
                    exit.Rect = new Rectangle(eX, eY, eW, eH);

                    bX = GraphicsDevice.Viewport.Width / 2 - 100;
                    bY = GraphicsDevice.Viewport.Height / 2;
                    back.Texture = backButton;
                    back.Rect = new Rectangle(bX, bY, bW, bH);

                    // Get mouse state
                    mouseState = Mouse.GetState();

                    // Hovering over buttons highlights them
                    if (mouseState.X <= start.Rect.X + start.Rect.Width
                                && mouseState.X >= start.Rect.X
                                && mouseState.Y <= start.Rect.Y + start.Rect.Height
                                && mouseState.Y >= start.Rect.Y)
                    {
                        start.Texture = startSelect;
                    }
                    if (mouseState.X <= option.Rect.X + option.Rect.Width
                                && mouseState.X >= option.Rect.X
                                && mouseState.Y <= option.Rect.Y + option.Rect.Height
                                && mouseState.Y >= option.Rect.Y)
                    {
                        option.Texture = optSelect;
                    }
                    if (mouseState.X <= exit.Rect.X + exit.Rect.Width
                                && mouseState.X >= exit.Rect.X
                                && mouseState.Y <= exit.Rect.Y + exit.Rect.Height
                                && mouseState.Y >= exit.Rect.Y)
                    {
                        exit.Texture = exitSelect;
                    }
                    if (mouseState.X <= back.Rect.X + back.Rect.Width
                                && mouseState.X >= back.Rect.X
                                && mouseState.Y <= back.Rect.Y + back.Rect.Height
                                && mouseState.Y >= back.Rect.Y)
                    {
                        back.Texture = backSelect;
                    }


                    // If mouse clicks on buttons on main
                    if (menuMode == MenuMode.Main)
                    {
                        if (mouseState.LeftButton == ButtonState.Pressed && mStatePrev.LeftButton == ButtonState.Released)
                        {
                            if (mouseState.X <= start.Rect.X + start.Rect.Width
                                && mouseState.X >= start.Rect.X
                                && mouseState.Y <= start.Rect.Y + start.Rect.Height
                                && mouseState.Y >= start.Rect.Y)
                            {
                                // generates starting chunks
                                platformList.Clear();

                                currChunk = new Chunk("source1.txt", groundImg);
                                currChunk.ChunkStart();
                                prevChunk = new Chunk("start.txt", groundImg);

                                for (int x = 0; x < currChunk.Platforms.Length; x++)
                                {
                                    platformList.Add(currChunk.Platforms[x]);
                                }
                                for (int x = 0; x < prevChunk.Platforms.Length; x++)
                                {
                                    platformList.Add(prevChunk.Platforms[x]);
                                }

                                gameMode = GameMode.Game;
                            }

                            // Options
                            if (mouseState.X <= option.Rect.X + option.Rect.Width
                                && mouseState.X >= option.Rect.X
                                && mouseState.Y <= option.Rect.Y + option.Rect.Height
                                && mouseState.Y >= option.Rect.Y)
                            {
                                menuMode = MenuMode.Options;
                            }

                            // Exit
                            if (mouseState.X <= exit.Rect.X + exit.Rect.Width
                                && mouseState.X >= exit.Rect.X
                                && mouseState.Y <= exit.Rect.Y + exit.Rect.Height
                                && mouseState.Y >= exit.Rect.Y)
                            {
                                this.Exit();
                            }
                        }

                        mStatePrev = mouseState;
                    }

                    // Mouse clicks on Options
                    if (menuMode == MenuMode.Options)
                    {
                        bool clicked;


                        // Options button was clicked, mouse released
                        if (mouseState.LeftButton == ButtonState.Released && mouseState.LeftButton == ButtonState.Pressed)
                        {
                            clicked = true;
                        }
                        else if (mouseState.LeftButton == ButtonState.Pressed && mStatePrev.LeftButton == ButtonState.Released)
                        {
                            clicked = false;
                        }
                        else
                        {
                            clicked = true;
                        }

                        // Back
                        if (mouseState.X <= back.Rect.X + back.Rect.Width
                            && mouseState.X >= back.Rect.X
                            && mouseState.Y <= back.Rect.Y + back.Rect.Height
                            && mouseState.Y >= back.Rect.Y
                            && mouseState.LeftButton == ButtonState.Pressed
                            && clicked == false)
                        {
                            menuMode = MenuMode.Main;
                        }
                        mStatePrev = mouseState;
                    }
                    break;

                case GameMode.Game:
                    
                    //Gives the player a velocity downwards
                    player1.Fall();

                    //Check for collisions and updates player position if necessary
                    player1.CheckCollisions(platformList);

                    //Handles player input and position updates for the player
                    player1.Update(gameTime);

                    //Platform position updates
                    /*
                    foreach (Platform plat in platformList)
                    {
                        plat.Update(gameTime, 2);
                    }
                    */

                    // chunk position updates
                    /*
                    foreach (Chunk c in chunk)
                    {
                        c.Update(gameTime, 2);
                    }
                    */
                   
                    // chunk position updates
                    currChunk.Update(gameTime, SPEED);
                    prevChunk.Update(gameTime, SPEED);

                    // check if new chunk needs to be generated
                    if (currChunk.CheckChunk() == true)
                    {
                        Chunk tempChunk = currChunk;       

                        currChunk = new Chunk(chunkSource[rando.Next(CHUNK_SOURCE)], groundImg); // needs to get stitch values and possible connections
                        currChunk.ChunkPlace(); // sets chunks farther along after they are created
                        prevChunk = tempChunk; // sets chunk the player is currently in a new chunk that continues to function

                        // reset platform list
                        platformList.Clear();

                        for (int x = 0; x < currChunk.Platforms.Length; x++)
                        {
                            platformList.Add(currChunk.Platforms[x]);
                        }
                        for (int x = 0; x < prevChunk.Platforms.Length; x++)
                        {
                            platformList.Add(prevChunk.Platforms[x]);
                        }
                    }

                    //Temporary code for the event of a game over
                    //Incrementation of the timer to show the correct amount of time the player has been
                    //alive for
                    timer += gameTime.ElapsedGameTime.TotalSeconds;

                    //Temporary code for the event of a game over
                    if (player1.CollisionRect.Y >= GraphicsDevice.Viewport.Height)
                    {
                        gameMode = GameMode.Gameover;
                        player1 = new Player(coord, image, new Rectangle((int)coord.X, (int)coord.Y, 64, 128));

                        /*
                        platforms[0] = (new Platform(new Rectangle((int)coord.X, (int)coord.Y + 300, 300, 50), groundImg));
                        platforms[1] = (new Platform(new Rectangle(700, 400, 300, 50), groundImg));
                        platforms[2] = (new Platform(new Rectangle(700, 100, 300, 50), groundImg));
                        platforms[3] = (new Platform(new Rectangle(1700, 400, 300, 50), groundImg));
                        platforms[4] = (new Platform(new Rectangle(2200, 300, 300, 50), groundImg));
                        platforms[5] = (new Platform(new Rectangle(2600, 100, 300, 50), groundImg));
                        */

                        //Assigns the survived time to a new value and updates the score list if need be
                        timeLived = timer;
                        keeper.UpdateScores(timeLived);

                        //When the player recieves a gameover the timer is reset
                        timer = 0;
                    }
                    break;
                //Case for when the player reaches the game over screen
                case GameMode.Gameover:
                    timer += gameTime.ElapsedGameTime.TotalSeconds;

                    //Holds on the screen for an x amount of time before returning to the main menu
                    if (timer >= 3)
                    {
                        gameMode = GameMode.Menu;
                        timer = 0;
                    }
                    break;
            }

            

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            switch(gameMode)
            {
                case GameMode.Menu:
                    GraphicsDevice.Clear(Color.LightGreen);

                    // TODO: Add your drawing code here

                    // Menu modes       
                    switch (menuMode)
                    {
                        case MenuMode.Main:
                            Main();
                            break;
                        case MenuMode.Options:
                            Options();
                            break;
                    }
                    break;
                case GameMode.Game:
                    GraphicsDevice.Clear(Color.White);

                    // Draws the content to the screen
                    spriteBatch.Begin();

                    //Loops through the platforms list and draws each platform onto the screen
                    foreach (Platform plat in platformList)
                    {
                        spriteBatch.Draw(groundImg, plat.CollisionBox, Color.White);
                    }

                    // draws out chunks
                    currChunk.Draw(spriteBatch);
                    prevChunk.Draw(spriteBatch);

                    player1.Draw(spriteBatch, playerImage);                      // animated character
                    // spriteBatch.Draw(image, player1.CollisionRect, Color.White);    // static character

                    spriteBatch.End();
                    break;

                    //Case for when the player loses the game
                case GameMode.Gameover:
                    GraphicsDevice.Clear(Color.LightGreen);

                    // Begins the spriteBach
                    spriteBatch.Begin();

                    spriteBatch.DrawString(font, "You have lasted " + String.Format("{0 : 0.00}", timeLived) + " seconds.", new Vector2(GraphicsDevice.Viewport.Width / 2 - 200, 0), Color.Black);
                    spriteBatch.DrawString(font, "Best Times:", new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, 50), Color.Black);

                    //Prints out each of the top high scores for the player
                    foreach(double time in keeper.HighScores)
                    {
                        spriteBatch.DrawString(font, keeper.HighScores.IndexOf(time) + 1 + ":  " + String.Format("{0 : 0.00}", time), new Vector2(GraphicsDevice.Viewport.Width / 2 - 100, 100 + (keeper.HighScores.IndexOf(time) * 50)), Color.Black);
                    }

                    spriteBatch.End();
                    break;
            }
            
            base.Draw(gameTime);
        }
        public void Main()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Endless, Nameless", fontLoc, Color.Black);
            start.Draw(spriteBatch);
            option.Draw(spriteBatch);
            exit.Draw(spriteBatch);
            spriteBatch.End();
        }

        public void Options()
        {
            spriteBatch.Begin();
            spriteBatch.DrawString(font, "Options", fontLoc, Color.Black);
            back.Draw(spriteBatch);
            spriteBatch.End();
        }

    }
}
