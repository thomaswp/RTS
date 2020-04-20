using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public class Plane : OldSprite
    {
        public Plane(Viewport viewport) : base(viewport) 
        {
            this.Rect = Graphics.ScreenRect;
        }

        public Plane() : this(Viewport.Default) { }
    }
}
