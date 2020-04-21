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

        private Sprite[,] tiles;

        public int CellWidth { get { return map.tileWidth; } }
        public int CellHeight { get { return map.tileHeight; } }

        public GroundLayer(HearthGame game, Map map)
        {
            this.game = game;
            this.map = map;

            Bitmap defaultTile = Assets.LoadTile(map.defaultTile.Get().sprite);
            tiles = new Sprite[CellWidth, CellHeight];
            for (int i = 0; i < CellWidth; i++)
            {
                for (int j = 0; j < CellHeight; j++)
                {
                    Sprite tile = new Sprite(defaultTile);
                    tile.X = i * TileSize;
                    tile.Y = j * TileSize;
                    tile.ScaleX = (float) TileSize / tile.Bitmap.Width;
                    tile.ScaleY = (float) TileSize / tile.Bitmap.Height;
                    AddChild(tile);
                    tiles[i, j] = tile;
                }
            }
        }
    }
}
