using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Game_Player
{
    public abstract class Renderable : Transformable
    {
        public virtual Bitmap Bitmap { get;  protected set; }
        public virtual Rect BmpSourceRect { get; protected set; }
        public bool Visible = true;

        //public int Width { get { return Bitmap == null ? 0 : Bitmap.Width; } }
        //public int Height { get { return Bitmap == null ? 0 : Bitmap.Height; } }
    }
}
