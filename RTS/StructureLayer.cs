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
        }
        
        public void StartAddingBuilding(Building building)
        {
            addingBuilding = new BuildingSprite(building, 0, 0, this);
            addingBuilding.Z = int.MaxValue;
            addingBuilding.AddTileHighlights();
            AddChild(addingBuilding);

            UpdateAddBuilding();
        }

        public bool HasStructure(int cellX, int cellY)
        {
            return buildings.Any(b => b.ContainsCell(cellX, cellY));
        }

        public bool HasStructure(Rectangle rect)
        {
            return buildings.Any(b => b.InsersectsRect(rect));
        }

        private void UpdateAddBuilding()
        {
            if (!IsPlacing) return;
            MouseState mouse = Mouse.GetState();
            Vector2 mousePos = new Vector2(mouse.X, mouse.Y);
            Vector2 relativePos = Vector2.Transform(mousePos, Matrix.Invert(GetFullTransform()));
            int tileX = (int)Math.Floor(relativePos.X / TileSize);
            int tileY = (int)Math.Floor(relativePos.Y / TileSize);
            tileX -= addingBuilding.CellWidth / 2;
            tileY -= addingBuilding.CellHeight / 2;

            addingBuilding.CellX = tileX;
            addingBuilding.CellY = tileY;
        }

        private void CheckToPlaceBuilding()
        {
            if (!IsPlacing) return;
            if (Input.LeftMouseState != InputState.Triggered) return;
            if (!addingBuilding.CanBePlaced()) return;

            addingBuilding.Z = 0;
            buildings.Add(addingBuilding);
            addingBuilding.RemoveTileHighights();
            AddChild(addingBuilding);
            addingBuilding = null;
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
            readonly StructureLayer layer;

            public int TileSize { get { return layer.TileSize; } }

            public int CellX { get { return (int)(X / TileSize); } set { X = value * TileSize; } }
            public int CellY { get { return (int)(Y / TileSize); } set { Y = value * TileSize; } }
            
            public int CellWidth { get { return building.cellWidth; } }
            public int CellHeight { get { return building.cellHeight; } }

            Sprite[,] tileHighlights;

            public BuildingSprite(Building building, int cellX, int cellY, StructureLayer layer)
            {
                this.building = building;
                this.layer = layer;
                CellX = cellX;
                CellY = cellY;

                Bitmap = Assets.LoadBuilding(building.sprite);
                //OX = bitmap.Width / 2;
                //OY = bitmap.Height / 2;

                // Should probably use padding to ensure zoom is proportional
                ScaleX = ((float) CellWidth) * layer.TileSize / Bitmap.Width;
                ScaleY = ((float) CellHeight) * layer.TileSize / Bitmap.Height;
            }

            public Rectangle GetBoundingRect()
            {
                return new Rectangle(CellX, CellY, CellWidth, CellHeight);
            }

            public bool ContainsCell(int cellX, int cellY)
            {
                return GetBoundingRect().Contains(cellX, cellY);
            }

            public bool InsersectsRect(Rectangle rect)
            {
                return GetBoundingRect().Intersects(rect);
            }

            public bool CanBePlaced()
            {
                return !layer.HasStructure(GetBoundingRect());
            }

            public void AddTileHighlights()
            {
                Alpha = 150;
                tileHighlights = new Sprite[CellWidth, CellHeight];
                for (int i = 0; i < CellWidth; i++)
                {
                    for (int j = 0; j < CellHeight; j++)
                    {
                        Sprite tile = new Sprite(new Bitmap(layer.TileSize, layer.TileSize));
                        tile.Bitmap.Clear(Colors.White);
                        tile.X = i * tile.Bitmap.Width / ScaleX;
                        tile.Y = j * tile.Bitmap.Height / ScaleY;
                        tile.Z = -1;
                        tile.ScaleX = 1 / ScaleX;
                        tile.ScaleY = 1 / ScaleY;
                        AddChild(tile);
                        tileHighlights[i, j] = tile;
                        Console.WriteLine(i);
                    }
                }
            }

            public void RemoveTileHighights()
            {
                Alpha = 255;
                for (int i = 0; i < CellWidth; i++)
                {
                    for (int j = 0; j < CellHeight; j++)
                    {
                        RemoveChild(tileHighlights[i,j]);
                    }
                }
                tileHighlights = null;
            }
            
            public override void Update()
            {
                base.Update();
                UpdateTiles();
            }

            private void UpdateTiles()
            {
                if (tileHighlights == null) return;
                for (int i = 0; i < CellWidth; i++)
                {
                    for (int j = 0; j < CellHeight; j++)
                    {
                        bool blocked = layer.HasStructure(CellX + i, CellY + j);
                        if (blocked)
                        {
                            tileHighlights[i, j].Color = new Game_Player.Color(255, 0, 0, 100);
                        }
                        else
                        {
                            tileHighlights[i, j].Color = new Game_Player.Color(0, 255, 0, 100);
                        }
                    }
                }
            }
        }
    }
}
