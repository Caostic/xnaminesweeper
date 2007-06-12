using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Minesweeper.GameLogic
{
    class GameBoard
    {

        #region Private Instance Variables

        /// <summary>
        /// Whether the game has been lost or not
        /// </summary>
        private bool gameOver;

        /// <summary>
        /// Whether the game has been won or not
        /// </summary>
        private bool winner;

        /// <summary>
        /// Width (in number of squares) of the game board
        /// </summary>
        private int width;

        /// <summary>
        /// Height (in number of squares) of the game board
        /// </summary>
        private int height;

        /// <summary>
        /// Length of time the current game has been going for (timer counting up)
        /// </summary>
        private int playTime;

        /// <summary>
        /// Number of mines on the board
        /// </summary>
        private int numMines;

        /// <summary>
        /// The game board, 2D array of game squares
        /// </summary>
        private GameSquare[ , ] board;

        #endregion


        #region Properties

        public bool GameOver
        {
            get
            {
                return gameOver;
            }
            set
            {
                gameOver = value;
            }
        }

        public bool Winner
        {
            get
            {
                return winner;
            }
            set
            {
                winner = value;
            }
        }

        public int Width
        {
            get
            {
                return width;
            }
            set
            {
                width = value;
            }
        }

        public int Height
        {
            get
            {
                return height;
            }
            set
            {
                height = value;
            }
        }

        public int PlayTime
        {
            get
            {
                return playTime;
            }
            set
            {
                playTime = value;
            }
        }

        public int NumMines
        {
            get
            {
                return numMines;
            }
            set
            {
                numMines = value;
            }
        }

        public void SetGameSquare(int x, int y, GameSquare gs)
        {
            board[x,y] = gs;
        }

        public GameSquare GetGameSquare(int x, int y)
        {
            return board[x,y];
        }

        #endregion


        public GameBoard(int w, int h, int mines)
        {
            gameOver = false;
            width = w;
            height = h;
            playTime = 0;
            numMines = mines;
            board = new GameSquare[width,height];

            GenerateGameBoard();
        }

        public void RestartGame()
        {
            gameOver = false;
            winner = false;
            int x;
            int y;

            //May have to initialize all of the GameSquares on the board
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    board[x, y].Clicked = GameLogic.GameSquare.UNCLICKED;
                    board[x, y].Searched = false;
                    board[x, y].SquareValue = GameLogic.GameSquare.BLANK;
                }
            }

            //Random number generator
            Random rand = new Random();

            //Add mines to the board
            int index;
            int randomX;
            int randomY;
            for (index = 0; index < numMines; index++)
            {
                //Generate two random numbers
                //One between 0 and Width - 1, the other between 0 and Height - 1
                randomX = rand.Next(0, width - 1);
                randomY = rand.Next(0, height - 1);

                //while this square is a bomb, generate two new random numbers
                while (board[randomX, randomY].SquareValue == GameLogic.GameSquare.BOMB)
                {
                    randomX = rand.Next(0, width - 1);
                    randomY = rand.Next(0, height - 1);
                }

                //Have got a new square
                //Set it to be a bomb
                board[randomX, randomY].SquareValue = GameLogic.GameSquare.BOMB;

            }

            //Iterate through each square of the board
            //If it is not a mine, check the squares around it to calculate its value
            //Set all squares to UNCLICKED
            int count = 0;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    //If this square is not a bomb, calculate its value
                    if (board[x, y].SquareValue != GameLogic.GameSquare.BOMB)
                    {
                        //Count the number of bombs around the current square
                        int i;
                        int j;
                        for (i = x - 1; i <= x + 1; i++)
                        {
                            for (j = y - 1; j <= y + 1; j++)
                            {
                                //Don't check squares that are out of bounds
                                if ((i >= 0 && i < width) && (j >= 0 && j < height))
                                {
                                    //Increment count if this is a bomb
                                    if (board[i, j].SquareValue == GameLogic.GameSquare.BOMB)
                                    {
                                        count++;
                                    }
                                }
                            }
                        }

                        //Set the value of this square
                        board[x, y].SquareValue = count;
                        count = 0;
                    }//end if
                }//end inner for loop
            }//end outer for loop
            
        }

        public void GenerateGameBoard()
        {
            int x;
            int y;

            //May have to initialize all of the GameSquares on the board
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    board[x, y] = new GameSquare(x, y);
                }
            }


            //Random number generator
            Random rand = new Random();

            //Add mines to the board
            int index;
            int randomX;
            int randomY;
            for (index = 0; index < numMines; index++)
            {
                //Generate two random numbers
                //One between 0 and Width - 1, the other between 0 and Height - 1
                randomX = rand.Next(0, width - 1);
                randomY = rand.Next(0, height - 1);

                //while this square is a bomb, generate two new random numbers
                while (board[randomX, randomY].SquareValue == GameLogic.GameSquare.BOMB)
                {
                    randomX = rand.Next(0, width - 1);
                    randomY = rand.Next(0, height - 1);
                }

                //Have got a new square
                //Set it to be a bomb
                board[randomX, randomY].SquareValue = GameLogic.GameSquare.BOMB;

            }

            //Iterate through each square of the board
            //If it is not a mine, check the squares around it to calculate its value
            //Set all squares to UNCLICKED
            int count = 0;
            for (x = 0; x < width; x++)
            {
                for (y = 0; y < height; y++)
                {
                    //If this square is not a bomb, calculate its value
                    if (board[x, y].SquareValue != GameLogic.GameSquare.BOMB)
                    {
                        //Count the number of bombs around the current square
                        int i;
                        int j;
                        for (i = x - 1; i <= x + 1; i++)
                        {
                            for (j = y - 1; j <= y + 1; j++)
                            {
                                    //Don't check squares that are out of bounds
                                    if ((i >= 0 && i < width) && (j >= 0 && j < height))
                                    {
                                        //Increment count if this is a bomb
                                        if (board[i, j].SquareValue == GameLogic.GameSquare.BOMB)
                                        {
                                            count++;
                                        }
                                    }
                            }
                        }

                        //Set the value of this square
                        board[x, y].SquareValue = count;
                        count = 0;
                    }//end if
                }//end inner for loop
            }//end outer for loop

        }
    }
}
