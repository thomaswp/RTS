using Game_Player;
using HearthData;
using Microsoft.Xna.Framework;
using System;

namespace RTS
{
    public class MapRoot : Transformable
    {
        readonly StructureLayer structureLayer;
        readonly GroundLayer groundLayer;

        public readonly HearthGame game;
        public readonly Map map;

        public const int TILE_SIZE = 40;
        const float MIN_SCALE = 0.2f, MAX_SCALE = 5;

        private float targetScale = 1;
        private Vector2 targetCenter;

        private Vector2 dragStartScreen, dragStartCenter;
        bool dragging;

        public MapRoot(HearthGame game)
        {
            this.game = game;
            structureLayer = new StructureLayer(game);
            AddChild(structureLayer);

            map = game.startingMap.Get();
            groundLayer = new GroundLayer(game, map);
            AddChild(groundLayer);
            groundLayer.Z = -1;

            Sprite s = new Sprite(new Bitmap(20, 20));
            s.Bitmap.FillRect(0, 0, 19, 19, Colors.White);
            AddChild(s);

            CenterX = map.tileWidth * TILE_SIZE / 2;
            CenterY = map.tileHeight * TILE_SIZE / 2;
            targetCenter = Center;
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
            ScaleX = ScaleY = Lerp(ScaleX, targetScale, 0.1f);

            Vector2 arrowDir = Input.GetWASDDir();
            targetCenter += arrowDir * 5 / ScaleX;

            if (Input.LeftMouseState == InputState.Triggered)
            {
                dragging = true;
                dragStartScreen = Input.GetMousePosition();
                dragStartCenter = Center;
            }
            else if (Input.LeftMouseState == InputState.Lifted)
            {
                dragging = false;
            }
            if (dragging)
            {
                // TODO: Still doesn't really work with zooming out.
                targetCenter = TransformScreenToLocalPoint(dragStartScreen) - TransformScreenToLocalPoint(Input.GetMousePosition()) + dragStartCenter;
            }

            Center = Lerp(Center, targetCenter, 0.3f);
        }

        private static float Lerp(float from, float to, float by)
        {
            return from * (1 - by) + to * by;
        }

        private static Vector2 Lerp(Vector2 from, Vector2 to, float by)
        {
            return from * (1 - by) + to * by;
        }
    }
}
