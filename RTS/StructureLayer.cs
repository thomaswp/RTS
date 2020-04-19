using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Player;
using HearthData;
using Microsoft.Xna.Framework.Input;

namespace RTS
{
    public class StructureLayer : Viewport
    {
        public readonly int TileSize = 40;

        readonly Game game;

        List<BuildingSprite> buildings = new List<BuildingSprite>();

        BuildingSprite addingBuilding;

        public StructureLayer(Game game) : base(0, 0, Graphics.ScreenWidth, Graphics.ScreenHeight)
        {
            this.game = game;

            game.buildings.ForEach(b => AddBuilding(b, 1, 0));

            StartAddingBuilding(game.buildings[0]);
        }

        public void AddBuilding(Building building, int cellX, int cellY)
        {
            BuildingSprite sprite = new BuildingSprite(building, cellX, cellY, this);
            buildings.Add(sprite);
            AddSprite(sprite);
            sprite.X = cellX * TileSize;
            sprite.Y = cellY * TileSize;
        }
        
        public void StartAddingBuilding(Building building)
        {
            addingBuilding = new BuildingSprite(building, 0, 0, this);
            UpdateAddBuilding();
        }

        private void UpdateAddBuilding()
        {
            if (addingBuilding == null) return;

            MouseState mouse = Mouse.GetState();
            int tileX = mouse.X / TileSize;
            int tileY = mouse.Y / TileSize;
            tileX -= addingBuilding.CellWidth / 2;
            tileY -= addingBuilding.CellHeight / 2;

            addingBuilding.X = tileX * TileSize;
            addingBuilding.Y = tileY * TileSize;
        }

        public override void Update()
        {
            base.Update();
            UpdateAddBuilding();
        }


        class BuildingSprite : Sprite
        {
            readonly Building building;
            int cellX, cellY;
            
            public int CellWidth { get { return building.cellWidth; } }
            public int CellHeight { get { return building.cellHeight; } }

            public BuildingSprite(Building building, int cellX, int cellY, StructureLayer layer)
            {
                this.building = building;
                this.cellX = cellX;
                this.cellY = cellY;

                bitmap = Assets.LoadBuilding(building.sprite);
                //OX = bitmap.Width / 2;
                //OY = bitmap.Height / 2;

                // Should probably use padding to ensure zoom is proportional
                ZoomX = ((float) CellWidth) * layer.TileSize / bitmap.Width;
                ZoomY = ((float) CellHeight) * layer.TileSize / bitmap.Height;
            }

            public override void Update()
            {
                base.Update();
            }
        }
    }
}
