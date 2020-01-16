using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Player;

namespace RTS
{
    class WeirdBall : Sprite
    {
        int lifeTick;
        const int LIFE = 5000;

        public WeirdBall(Viewport viewport) : base(viewport)
        {
            Bitmap bmp = new Bitmap(50, 50);
            bmp.FillEllipse(25, 25, 50, 50, new Game_Player.Color(255, 255, 255));
            bmp.FillEllipse(8, 8, 16, 16, new Game_Player.Color(255, 255, 255));
            bmp.FillEllipse(8, 42, 16, 16, new Game_Player.Color(255, 255, 255));
            bmp.FillEllipse(42, 8, 16, 16, new Game_Player.Color(255, 255, 255));
            bmp.FillEllipse(42, 42, 16, 16, new Game_Player.Color(255, 255, 255));
            this.Bitmap = bmp;
            ZoomX = ZoomY = 2;
            OX = Bitmap.Width / 2;
            OY = Bitmap.Height / 2;
            Rotation = Rand.NextDouble();
        }

        public override void Update()
        {
            base.Update();
            Opacity -= 2;
            Rotation += 0.01;
            // Set the color to a hue-saturation-lightness color, based on the
            // number of frames that have passed since creation
            lifeTick++;
            Color = new HSLColor((lifeTick++ % 240), 240.0, 120.0);
            // degrades opacity from 1 to 0 over the course of LIFE
            //if (lifeTick < LIFE)
            //{
            //    this.Opacity = (LIFE - lifeTick) / 255;
            //}
            //else
            //{
            //    this.Dispose();
            //}
            if (Opacity == 0) Dispose();
        }
    }
}
