using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.EntitySpawners;
using RpgLibrary.GameObjectClasses;

namespace SkeletonsAdventure.GameObjects
{
    public class ChestManager
    {
        public List<Chest> Chests { get; set; } = [];
        public TiledMapTileLayer TiledMapTileLayer { get; set; } 

        public ChestManager(TiledMapTileLayer mapChestLayer)
        {
            TiledMapTileLayer = mapChestLayer;
        }

        public void Update()
        {
            foreach(Chest chest in Chests)
            {
                chest.DetectionArea = new Rectangle((int)chest.Position.X - 25, (int)chest.Position.Y - 25,
                    TiledMapTileLayer.TileWidth + 50, TiledMapTileLayer.TileHeight + 50);
            }

        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //TODO
            foreach(Chest chest in Chests)
            {
                spriteBatch.DrawRectangle(chest.DetectionArea, Color.White, 1, 0); //TODO
            }    
        }

        public List<Chest> GetChestsFromTiledMapTileLayer(Chest chest)
        {
            List<Chest> chests = [];
            int width = TiledMapTileLayer.TileWidth;
            int height = TiledMapTileLayer.TileHeight;


            foreach (TiledMapTile tile in Spawner.TileLocations(chest.ID, TiledMapTileLayer.Tiles))
            {
                chest.Position = new Vector2(tile.X * width, tile.Y * height);
                chests.Add(chest.Clone());
            }

            return chests;
        }

        public List<ChestData> GetChestDatas()
        {
            List<ChestData> chestDatas = [];

            foreach (Chest chest in Chests)
            {
                chestDatas.Add(chest.GetChestData());
            }

            return chestDatas;
        }
    }
}
