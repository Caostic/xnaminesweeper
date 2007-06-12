
#region Using Statements
using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;
#endregion

using Minesweeper.Presentation;
using Minesweeper.GameLogic;

namespace Minesweeper
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class MinesweeperGame : Microsoft.Xna.Framework.Game
    {
        private GraphicsDeviceManager graphics;
        public GraphicsDeviceManager Graphics
        {
            get
            {
                return graphics;
            }
        }
        ContentManager content;

        private Matrix projectionMatrix;
        public Matrix ProjectionMatrix
        {
            get
            {
                return projectionMatrix;
            }
        }
        private Matrix viewMatrix;
        public Matrix ViewMatrix
        {
            get
            {
                return viewMatrix;
            }
        }

        private NumberFont nf;

        private const int PAUSED = 0;
        private const int PLAYING = 1;
        private const int INTRO = 2;

        private const int BOARDHEIGHT = 12;
        private const int BOARDWIDTH = 16;
        private const int BOARDMINES = 40;

        private int gameState;
        private int flashCountdown;
        private bool drawFlash;
        KeyboardState oldState;
        MouseState oldMouseState;
        private GameBoard mineBoard;

        SpriteBatch mySpriteBatch;
        Texture2D bomb;
        Texture2D clickedBlank;
        Texture2D unclicked;
        Texture2D flagged;
        Texture2D clickedOne;
        Texture2D clickedTwo;
        Texture2D clickedThree;
        Texture2D clickedFour;
        Texture2D clickedFive;
        Texture2D clickedSix;
        Texture2D clickedSeven;
        Texture2D clickedEight;
        Texture2D introSplash;
        Texture2D introSplashFlash;
        Texture2D pauseScreen;
        Texture2D redbomb;
        Texture2D winnerSplash;

        public MinesweeperGame()
        {
            graphics = new GraphicsDeviceManager(this);
            content = new ContentManager(Services);
            nf = new NumberFont();
        }


        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            InitializeMatrices();
            nf.Initialize(graphics.GraphicsDevice, content);

            ContentManager myLoader = new ContentManager(this.Services);
            mySpriteBatch = new SpriteBatch(this.graphics.GraphicsDevice);
            bomb = myLoader.Load<Texture2D>("./Content/Sprites/bomb");
            clickedBlank = myLoader.Load<Texture2D>("./Content/Sprites/clickedBlank");
            unclicked = myLoader.Load<Texture2D>("./Content/Sprites/unclicked");
            flagged = myLoader.Load<Texture2D>("./Content/Sprites/flagged");
            clickedOne = myLoader.Load<Texture2D>("./Content/Sprites/clickedOne");
            clickedTwo = myLoader.Load<Texture2D>("./Content/Sprites/clickedTwo");
            clickedThree = myLoader.Load<Texture2D>("./Content/Sprites/clickedThree");
            clickedFour = myLoader.Load<Texture2D>("./Content/Sprites/clickedFour");
            clickedFive = myLoader.Load<Texture2D>("./Content/Sprites/clickedFive");
            clickedSix = myLoader.Load<Texture2D>("./Content/Sprites/clickedSix");
            clickedSeven = myLoader.Load<Texture2D>("./Content/Sprites/clickedSeven");
            clickedEight = myLoader.Load<Texture2D>("./Content/Sprites/clickedEight");
            introSplash = myLoader.Load<Texture2D>("./Content/Sprites/introSplash");
            introSplashFlash = myLoader.Load<Texture2D>("./Content/Sprites/introSplashFlash");
            pauseScreen = myLoader.Load<Texture2D>("./Content/Sprites/pauseScreen");
            redbomb = myLoader.Load<Texture2D>("./Content/Sprites/redbomb");
            winnerSplash = myLoader.Load<Texture2D>("./Content/Sprites/winnerSplash");

            drawFlash = true;
            flashCountdown = 15;
            gameState = INTRO;
            mineBoard = new GameBoard(BOARDWIDTH, BOARDHEIGHT, BOARDMINES);
            oldMouseState = Mouse.GetState();

            base.Initialize();
        }

        private void InitializeMatrices()
        {
            viewMatrix = Matrix.CreateLookAt(new Vector3(0, 0, 5), Vector3.Zero, Vector3.Up);

            projectionMatrix = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.ToRadians(45),
                (float)graphics.GraphicsDevice.Viewport.Width /
                (float)graphics.GraphicsDevice.Viewport.Height,
                1.0f, 100.0f);
        }


        /// <summary>
        /// Load your graphics content.  If loadAllContent is true, you should
        /// load content from both ResourceManagementMode pools.  Otherwise, just
        /// load ResourceManagementMode.Manual content.
        /// </summary>
        /// <param name="loadAllContent">Which type of content to load.</param>
        protected override void LoadGraphicsContent(bool loadAllContent)
        {
            if (loadAllContent)
            {
                // TODO: Load any ResourceManagementMode.Automatic content
            }

            // TODO: Load any ResourceManagementMode.Manual content
        }


        /// <summary>
        /// Unload your graphics content.  If unloadAllContent is true, you should
        /// unload content from both ResourceManagementMode pools.  Otherwise, just
        /// unload ResourceManagementMode.Manual content.  Manual content will get
        /// Disposed by the GraphicsDevice during a Reset.
        /// </summary>
        /// <param name="unloadAllContent">Which type of content to unload.</param>
        protected override void UnloadGraphicsContent(bool unloadAllContent)
        {
            if (unloadAllContent == true)
            {
                content.Unload();
            }
        }


        //Check for mouse input from the user
        private void CheckUserInput()
        {
            const int RIGHT = 1;
            const int LEFT = 2;

            //Check for mouse click
            int mouseX;
            int mouseY;
            int button = 0;
            
            //Get x and y value of mouse click
            MouseState current_mouse = Mouse.GetState();
            mouseX = current_mouse.X;
            mouseY = current_mouse.Y;

            if (current_mouse.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released)
            {
                button = LEFT;
            }
            else if (current_mouse.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released)
            {
                button = RIGHT;
            }

            //Get the square on the board that has been clicked
            mouseX = mouseX / mineBoard.GetGameSquare(0, 0).Width;
            mouseY = mouseY / mineBoard.GetGameSquare(0, 0).Height;

            oldMouseState = current_mouse;

            if (button != 0)
            {
                ProcessUserInput(mouseX, mouseY, button);
            }
        }

        //Check if this square can be clicked, update square
        //Given x and y indexes in the board array and what mouse button was pressed
        private void ProcessUserInput(int x, int y, int button)
        {
            const int RIGHT = 1;
            const int LEFT = 2;

            GameSquare current = mineBoard.GetGameSquare(x, y);

            //No action if square is already clicked
            if (current.Clicked != GameLogic.GameSquare.CLICKED)
            {
                //Action if square is already flagged, and right clicked
                if (button == RIGHT)
                {
                    if (current.Clicked == GameLogic.GameSquare.FLAGGED)
                    {
                        current.Clicked = GameLogic.GameSquare.UNCLICKED;
                        mineBoard.SetGameSquare(x, y, current);
                    }
                    else
                    {
                        current.Clicked = GameLogic.GameSquare.FLAGGED;
                        mineBoard.SetGameSquare(x, y, current);
                    }
                }

                //Left mouse button, no action if square is flagged
                if (button == LEFT)
                {
                    if (current.Clicked == GameLogic.GameSquare.UNCLICKED)
                    {
                        current.Clicked = GameLogic.GameSquare.CLICKED;
                        mineBoard.SetGameSquare(x, y, current);


                        //CHECK IF CLICKED SQUARE IS A BOMB
                        //End game if necessary
                        if (current.SquareValue == GameLogic.GameSquare.BOMB)
                        {
                            current.SquareValue = GameLogic.GameSquare.REDBOMB;
                            mineBoard.GameOver = true;
                        }


                        //CHECK IF CLICKED SQUARE IS BLANK
                        //Reveal other blank squares
                        if (current.SquareValue == GameLogic.GameSquare.BLANK)
                        {
                           CheckBlank(x,y);
                        }

                    }
                }
            }


        }


        //Recursive method to reveal blank squares
        public void CheckBlank(int x, int y)
        {
            if (x >= 0 && x < mineBoard.Width && y >= 0 && y <= mineBoard.Height)
            {
                GameSquare current = mineBoard.GetGameSquare(x, y);
                current.Clicked = GameLogic.GameSquare.CLICKED;

                //All squares around this one are also clicked
                if (x - 1 >= 0)
                {
                    mineBoard.GetGameSquare(x - 1, y).Clicked = GameLogic.GameSquare.CLICKED;

                    if (y - 1 >= 0)
                    {
                        mineBoard.GetGameSquare(x-1, y-1).Clicked = GameLogic.GameSquare.CLICKED;
                    }
                }
                if (y - 1 >= 0)
                {
                    mineBoard.GetGameSquare(x, y - 1).Clicked = GameLogic.GameSquare.CLICKED;

                    if (x + 1 < mineBoard.Width)
                    {
                        mineBoard.GetGameSquare(x + 1, y - 1).Clicked = GameLogic.GameSquare.CLICKED;
                    }
                }
                if (x + 1 < mineBoard.Width)
                {
                    mineBoard.GetGameSquare(x + 1, y).Clicked = GameLogic.GameSquare.CLICKED;

                    if (y + 1 < mineBoard.Height)
                    {
                        mineBoard.GetGameSquare(x + 1, y + 1).Clicked = GameLogic.GameSquare.CLICKED;
                    }
                }
                if (y + 1 < mineBoard.Height)
                {
                    mineBoard.GetGameSquare(x, y+1).Clicked = GameLogic.GameSquare.CLICKED;

                    if (x- 1 >= 0)
                    {
                        mineBoard.GetGameSquare(x - 1, y + 1).Clicked = GameLogic.GameSquare.CLICKED;
                    }
                }

                mineBoard.SetGameSquare(x, y, current);

                if ((y - 1) >= 0)
                {
                    if (mineBoard.GetGameSquare(x, y - 1).SquareValue == GameLogic.GameSquare.BLANK && mineBoard.GetGameSquare(x, y - 1).Searched == false)
                    {
                        mineBoard.GetGameSquare(x, y - 1).Searched = true;
                        CheckBlank(x, y - 1);
                    }
                }
                if ((y + 1) < mineBoard.Height)
                {
                    if (mineBoard.GetGameSquare(x, y + 1).SquareValue == GameLogic.GameSquare.BLANK && mineBoard.GetGameSquare(x, y + 1).Searched == false)
                    {
                        mineBoard.GetGameSquare(x, y + 1).Searched = true;
                        CheckBlank(x, y + 1);
                    }
                }
                if ((x - 1) >= 0)
                {
                    if (mineBoard.GetGameSquare(x - 1, y).SquareValue == GameLogic.GameSquare.BLANK && mineBoard.GetGameSquare(x - 1, y).Searched == false)
                    {
                        mineBoard.GetGameSquare(x - 1, y).Searched = true;
                        CheckBlank(x - 1, y);
                    }
                }
                if ((x + 1) < mineBoard.Width)
                {
                    if (mineBoard.GetGameSquare(x + 1, y).SquareValue == GameLogic.GameSquare.BLANK && mineBoard.GetGameSquare(x + 1, y).Searched == false)
                    {
                        mineBoard.GetGameSquare(x + 1, y).Searched = true;
                        CheckBlank(x + 1, y);
                    }
                }
            }

            return;
        }

        //Check if the game has been won
        public bool CheckWin()
        {
            bool winner = true;
            for (int i = 0; i < mineBoard.Width; i++)
            {
                for (int j = 0; j < mineBoard.Height; j++)
                {
                    //Check to see if square is either a bomb or clicked
                    if (mineBoard.GetGameSquare(i, j).SquareValue != GameLogic.GameSquare.BOMB && mineBoard.GetGameSquare(i, j).Clicked != GameLogic.GameSquare.CLICKED)
                    {
                        winner = false;
                    }

                }
            }

            return winner;
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            KeyboardState newState = Keyboard.GetState();

            switch (gameState)
            {
                case PLAYING:
                    // Use Escape to exit to menu
                    
                    //Set mouse to visible
                    IsMouseVisible = true;

                    if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
                    {
                        gameState = PAUSED;
                    }
                    else if (newState.IsKeyDown(Keys.N) && oldState.IsKeyUp(Keys.N))
                    {
                        mineBoard.RestartGame();
                    }
                    else
                    {
                        if (mineBoard.GameOver == false && mineBoard.Winner == false)
                        {
                            //Check for user input, mouse click on the board
                            CheckUserInput();

                            //Update playing time
                            //May have to adjust this, may update too quickly

                            //Check for winner
                            if (CheckWin())
                            {
                                mineBoard.Winner = true;
                            }

                        }

                    }
                    break;

                case INTRO:
                    //Just shows intro splash screen, press enter to play

                    flashCountdown--;
                    if (flashCountdown <= 0)
                    {
                        //Reverse value of drawFlash
                        if (drawFlash == true)
                        {
                            drawFlash = false;
                        }
                        else
                        {
                            drawFlash = true;
                        }
                        //Reset countdown
                        flashCountdown = 15;
                    }

                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        gameState = PLAYING;
                    }
                    break;
                case PAUSED:
                    IsMouseVisible = false;

                    if (newState.IsKeyDown(Keys.Escape) && oldState.IsKeyUp(Keys.Escape))
                    {
                        //Do nothing while paused, just show paused screen and wait for player to continue
                        gameState = PLAYING;
                    }
                    break;
            }

            //Switch keyboard states
            oldState = newState;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {

            if (gameState == PLAYING)
            {
                graphics.GraphicsDevice.Clear(Color.White);

                int startX = 0;
                int startY = 0;

                int drawX = 0;
                int drawY = 0;

                mySpriteBatch.Begin(SpriteBlendMode.AlphaBlend);

                //Draw game board
                //If game over, stop timer and show entire board (highlight bomb hit)
                for (int x = 0; x < mineBoard.Width; x++)
                {
                    for (int y = 0; y < mineBoard.Height; y++)
                    {
                        switch (mineBoard.GetGameSquare(x, y).SquareValue)
                        {
                            case GameLogic.GameSquare.BLANK:
                                mySpriteBatch.Draw(clickedBlank, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case  GameLogic.GameSquare.BOMB:
                                mySpriteBatch.Draw(bomb, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.REDBOMB:
                                mySpriteBatch.Draw(redbomb, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.ONE:
                                mySpriteBatch.Draw(clickedOne, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.TWO:
                                mySpriteBatch.Draw(clickedTwo, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.THREE:
                                mySpriteBatch.Draw(clickedThree, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.FOUR:
                                mySpriteBatch.Draw(clickedFour, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.FIVE:
                                mySpriteBatch.Draw(clickedFive, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.SIX:
                                mySpriteBatch.Draw(clickedSix, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.SEVEN:
                                mySpriteBatch.Draw(clickedSeven, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;

                            case GameLogic.GameSquare.EIGHT:
                                mySpriteBatch.Draw(clickedEight, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                                break;
                        }

                        if (mineBoard.GetGameSquare(x, y).Clicked == GameLogic.GameSquare.UNCLICKED)
                        {
                            mySpriteBatch.Draw(unclicked, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                        }

                        //Layer flag sprite
                        if (mineBoard.GetGameSquare(x, y).Clicked == GameLogic.GameSquare.FLAGGED)
                        {
                            mySpriteBatch.Draw(flagged, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                        }
                        
                        if (mineBoard.GameOver == true)
                        {
                            if (mineBoard.GetGameSquare(x, y).SquareValue == GameLogic.GameSquare.BOMB)
                            {
                                mySpriteBatch.Draw(bomb, new Rectangle(drawX, drawY, mineBoard.GetGameSquare(0, 0).Width, mineBoard.GetGameSquare(0, 0).Height), Color.White);
                            }

                        }
                        if (mineBoard.Winner == true)
                        {
                            //Display winning message
                            mySpriteBatch.Draw(winnerSplash, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, 200), Color.White);
                        }

                        //Move down to draw next below square
                        drawY = drawY + mineBoard.GetGameSquare(0, 0).Height;
                    }

                    //Move over to draw next column
                    drawY = startY;
                    drawX = drawX + mineBoard.GetGameSquare(0, 0).Width;
                }
                mySpriteBatch.End();


                //Draw Time (Number font)



                base.Draw(gameTime);
            }
            else if (gameState == INTRO)
            {
                if (drawFlash == false)
                {
                    //Display opening menu without Resume option
                    mySpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    mySpriteBatch.Draw(introSplashFlash, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                    mySpriteBatch.End();
                }
                else
                {
                    //Display opening menu without Resume option
                    mySpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                    mySpriteBatch.Draw(introSplash, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                    mySpriteBatch.End();
                }
            }
            else if (gameState == PAUSED)
            {
                mySpriteBatch.Begin(SpriteBlendMode.AlphaBlend);
                mySpriteBatch.Draw(pauseScreen, new Rectangle(0, 0, graphics.GraphicsDevice.Viewport.Width, graphics.GraphicsDevice.Viewport.Height), Color.White);
                mySpriteBatch.End();
            }
        }
    }
}