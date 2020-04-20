using System;
using System.Collections.Generic;
using System.Text;

namespace Game_Player
{
    /// <summary>
    /// Defines a Sprite for displaying Bitmaps and adding effects.
    /// </summary>
    public class Sprite : Renderable
    {

        private Rect _bmpSourceRect = new Rect();
        /// <summary>
        /// Gets or sets he Rect of the bitmap that is rendered. If null, this property returns
        /// the Bitmap's Rect property.
        /// </summary>
        public override Rect BmpSourceRect
        {
            get 
            {
                if (_bmpSourceRect.Empty())
                {
                    if (Bitmap != null)
                        return Bitmap.Rect;
                    else
                        return new Rect(0, 0);
                }
                else
                { return _bmpSourceRect; }

            }
            protected set { _bmpSourceRect = value; }
        }

        public Sprite()
        {

        }

        public Sprite(Bitmap bitmap)
        {
            Bitmap = bitmap;
        }
    }
}
