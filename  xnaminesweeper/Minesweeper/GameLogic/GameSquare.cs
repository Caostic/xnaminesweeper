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
    class GameSquare
    {

        #region Constants

        public const int CLICKED = 1;
        public const int UNCLICKED = 0;
        public const int FLAGGED = 2;

        public const int REDBOMB = -2;
        public const int BOMB = -1;
        public const int BLANK = 0;
        public const int ONE = 1;
        public const int TWO = 2;
        public const int THREE = 3;
        public const int FOUR = 4;
        public const int FIVE = 5;
        public const int SIX = 6;
        public const int SEVEN = 7;
        public const int EIGHT = 8;


        #endregion


        #region Private instance Variables

        /// <summary>
        /// Whether the square has been clicked or not
        /// </summary>
        private int clicked;

        /// <summary>
        /// The value of the square (what it would be if clicked)
        /// </summary>
        private int squareValue;

        /// <summary>
        /// Location of the upper left hand corner of this square
        /// </summary>
        private Point location;

        /// <summary>
        /// Height of this square
        /// </summary>
        private int height;

        /// <summary>
        /// Width of this square
        /// </summary>
        private int width;

        /// <summary>
        /// Whether this square has been searched or not (used for expanding empty squares when clicked)
        /// </summary>
        private bool searched;

        #endregion


        #region Properties

        public int Clicked
        {
            get
            {
                return clicked;
            }
            set
            {
                clicked = value;
            }
        }

        public bool Searched
        {
            get
            {
                return searched;
            }
            set
            {
                searched = value;
            }
        }

        public int SquareValue
        {
            get
            {
                return squareValue;
            }
            set
            {
                squareValue = value;
            }
        }

        public Point Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
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


        #endregion

        public GameSquare()
        {
            height = 50;
            width = 50;
            location = new Point(0, 0);
            clicked = UNCLICKED;
            squareValue = BLANK;
            searched = false;
        }

        public GameSquare(int x, int y, int val)
        {
            height = 50;
            width = 50;
            location = new Point(x, y);
            clicked = UNCLICKED;
            squareValue = val;
            searched = false;
        }

        public GameSquare(int x, int y)
        {
            height = 50;
            width = 50;
            location = new Point(x, y);
            clicked = UNCLICKED;
            squareValue = BLANK;
            searched = false;
        }



    }
}
