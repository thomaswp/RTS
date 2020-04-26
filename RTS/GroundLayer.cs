using Game_Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HearthData;

namespace RTS
{
    public class GroundLayer : Transformable
    {
        public readonly int TileSize = MapRoot.TILE_SIZE;

        private readonly HearthGame game;
        private readonly Map map;

        //private Sprite[,] tiles;
        private Bitmap[,] tiles;
        private Sprite sprite;

        public int CellWidth { get { return map.tileWidth; } }
        public int CellHeight { get { return map.tileHeight; } }

        public GroundLayer(HearthGame game, Map map)
        {
            this.game = game;
            this.map = map;

            Bitmap defaultTile = Assets.LoadTile(map.defaultTile.Get().sprite);
            // TODO: Why is this needed?
            defaultTile.Resize(TileSize + 1, TileSize + 1);

            sprite = new Sprite(new Bitmap(CellWidth * TileSize, CellHeight * TileSize));
            AddChild(sprite);

            System.Drawing.Graphics graphics = sprite.Bitmap.Graphics;
            tiles = new Bitmap[CellWidth, CellHeight];
            for (int i = 0; i < CellWidth; i++)
            {
                for (int j = 0; j < CellHeight; j++)
                {
                    //Sprite tile = new Sprite(defaultTile);
                    int tileX = i * TileSize;
                    int tileY = j * TileSize;
                    
                    //tile.ScaleX = (float) TileSize / tile.Bitmap.Width;
                    //tile.ScaleY = (float) TileSize / tile.Bitmap.Height;
                    tiles[i, j] = defaultTile;
                    graphics.DrawImage(defaultTile.SystemBitmap, tileX, tileY);
                }
            }
        }
    }
}
