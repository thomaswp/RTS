using Game_Player;
using HearthData;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTS
{
    public class Map : Transformable
    {
        public StructureLayer structureLayer;

        public readonly Game game;

        const float MIN_SCALE = 0.2f, MAX_SCALE = 5;

        private float targetScale = 1;

        public Map(Game game)
        {
            this.game = game;
            structureLayer = new StructureLayer(game);
            AddChild(structureLayer);

            Sprite s = new Sprite(new Bitmap(20, 20));
            s.Bitmap.FillRect(0, 0, 19, 19, Colors.White);
            AddChild(s);

        }

        public override void Update()
        {
            X = Graphics.ScreenWidth / 2;
            Y = Graphics.ScreenHeight / 2;
            base.Update();

            if (Input.MouseScroll != 0)
            {
                float scaleChange = (float)Math.Pow(1.001, Input.MouseScroll);
                targetScale = Math.Max(Math.Min(targetScale * scaleChange, MAX_SCALE), MIN_SCALE);
            }
            ScaleX = 0.9f * ScaleX + 0.1f * targetScale;
            ScaleY = 0.9f * ScaleY + 0.1f * targetScale;

            if (Input.Held(Keys.X))
            {
                CenterX += 3f / ScaleX;
            }
            if (Input.Held(Keys.Z))
            {
                Console.WriteLine("!");
                CenterX -= 3f / ScaleX;
            }
        }
    }
}
