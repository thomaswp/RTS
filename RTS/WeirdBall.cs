﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Player;

namespace RTS
{
    class WeirdBall : Sprite
    {
        public WeirdBall(Viewport viewport) : base(viewport)
        {
            Bitmap bmp = new Bitmap(50, 50);
            bmp.FillEllipse(25, 25, 50, 50, new Game_Player.Color(
                Rand.Next(0, 255), Rand.Next(0, 255), Rand.Next(0, 255)));
            bmp.FillRect(0, 0, 10, 10, new Game_Player.Color(255, 0, 0));
            this.Bitmap = bmp;
            ZoomX = ZoomY = 2;
            OX = Bitmap.Width / 2;
            OY = Bitmap.Height / 2;
            Rotation = Rand.NextDouble();
        }

        public override void Update()
        {
            base.Update();

            Rotation += 0.01;
        }
    }
}