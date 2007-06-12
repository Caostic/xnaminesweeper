using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Storage;

namespace Minesweeper.Presentation
{
    class NumberFont
    {
        /// <summary>
        /// The table of digit textures.
        /// </summary>
        private Texture2D[] digitTable = new Texture2D[10];
        /// <summary>
        /// The SpriteBatch that will do the drawing.
        /// </summary>
        private SpriteBatch batch;

        public NumberFont()
        {
        }

        /// <summary>
        /// Initialize this number font to use a given graphics device.
        /// </summary>
        /// <param name="g">The graphics device to attach the SpriteBatch to.</param>
        /// <param name="content">The content manager for loading the sprites.</param>
        public void Initialize(GraphicsDevice g, ContentManager content)
        {
            batch = new SpriteBatch(g);
            for (int i = 0; i < digitTable.Length; i++)
            {
                digitTable[i] = content.Load<Texture2D>(@"Content\Sprites\NumberFont\" + i);
            }
        }
        /// <summary>
        /// Draw a given number onto the screen at the given coordinates.
        /// </summary>
        /// <param name="num">The number to draw.</param>
        /// <param name="digits">The maximum number of digits to use.</param>
        /// <param name="x">The x-coordinate to draw from</param>
        /// <param name="y">The y-coordinate to draw from</param>
        public void Draw(int num, int digits, int x, int y)
        {
            int[] indices = convertToDigitList(num, digits);
            batch.Begin(SpriteBlendMode.AlphaBlend);
            for (int i = 0; i < indices.Length; i++)
            {
                batch.Draw(digitTable[indices[i]], new Vector2(x + i * 16, y),
                                Color.White);
            }
            batch.End();
        }
        /// <summary>
        /// Draw a given number onto the screen at the given coordinates, scaling by a certain amount.
        /// </summary>
        /// <param name="num">The number to draw.</param>
        /// <param name="digits">The maximum number of digits to use.</param>
        /// <param name="x">The x-coordinate to draw from</param>
        /// <param name="y">The y-coordinate to draw from</param>
        /// <param name="scale">The amount to scale the size of the font by.</param>
        public void Draw(int num, int digits, int x, int y, double scale)
        {
            int[] indices = convertToDigitList(num, digits);
            int w = (int)(digitTable[indices[0]].Width * scale);
            int h = (int)(digitTable[indices[0]].Height * scale);
            batch.Begin(SpriteBlendMode.AlphaBlend);
            for (int i = 0; i < indices.Length; i++)
            {
                batch.Draw(digitTable[indices[i]],
                                new Rectangle(x + i * w, y, w, h),
                                Color.White);
            }
            batch.End();
        }

        /// <summary>
        /// Converts a number to a list of digits.
        /// </summary>
        /// <param name="num">The number to convert</param>
        /// <param name="digitCount">The maximum number of digits</param>
        /// <returns>A list of digits</returns>
        private int[] convertToDigitList(int num, int digitCount)
        {
            int[] result = new int[digitCount];
            for (int i = digitCount - 1; i >= 0; i--)
            {
                result[i] = num % 10;
                num /= 10;
            }
            return result;
        }
    }
}
