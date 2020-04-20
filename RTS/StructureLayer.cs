using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Game_Player;
using HearthData;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;

namespace RTS
{
    public class StructureLayer : Transformable
    {
        public readonly int TileSize = 40;

        readonly HearthData.Game game;

        List<BuildingSprite> buildings = new List<BuildingSprite>();

        BuildingSprite addingBuilding;

        public bool IsPlacing { get { return addingBuilding != null; } }

        public StructureLayer(HearthData.Game game)
        {
            this.game = game;

            game.buildings.ForEach(b => AddBuilding(b, 1, 0));
        }

        public void AddBuilding(Building building, int cellX, int cellY)
        {
            BuildingSprite sprite = new BuildingSprite(building, cellX, cellY, this);
            buildings.Add(sprite);
            AddChild(sprite);
            sprite.X = cellX * TileSize;
            sprite.Y = cellY * TileSize;
            
        }
        
        public void StartAddingBuilding(Building building)
        {
            addingBuilding = new BuildingSprite(building, 0, 0, this);
            addingBuilding.Z = int.MaxValue;
            AddChild(addingBuilding);
            UpdateAddBuilding();
        }

        private void UpdateAddBuilding()
        {
            if (!IsPlacing) return;
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
            Vector2 relativePos = Vector2.Transform(mousePos, Matrix.Invert(GetFullTransform()));
            int tileX = (int)relativePos.X / TileSize;
            int tileY = (int)relativePos.Y / TileSize;
            tileX -= addingBuilding.CellWidth / 2;
            tileY -= addingBuilding.CellHeight / 2;

            addingBuilding.X = tileX * TileSize;
            addingBuilding.Y = tileY * TileSize;
        }

        private void CheckToPlaceBuilding()
        {
            if (!IsPlacing) return;
            if (Input.LeftMouseState == InputState.Triggered)
            {
                addingBuilding.Z = 0;
                buildings.Add(addingBuilding);
                AddChild(addingBuilding);
                addingBuilding = null;
            }
        }

        private void CheckToStartAddingBuilding()
        {
            if (IsPlacing) return;
            if (Input.RightMouseState == InputState.Triggered)
            {
                StartAddingBuilding(game.buildings[0]);
            }
        }

        public override void Update()
        {
            base.Update();

            CheckToStartAddingBuilding();
            UpdateAddBuilding();
            CheckToPlaceBuilding();

            
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

                Bitmap = Assets.LoadBuilding(building.sprite);
                //OX = bitmap.Width / 2;
                //OY = bitmap.Height / 2;

                // Should probably use padding to ensure zoom is proportional
                ScaleX = ((float) CellWidth) * layer.TileSize / Bitmap.Width;
                ScaleY = ((float) CellHeight) * layer.TileSize / Bitmap.Height;
            }

            public override void Update()
            {
                base.Update();
            }
        }
    }
}
