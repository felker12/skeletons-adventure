using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.EntitySpawners;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameObjects
{
    internal class ChestManager(TiledMapTileLayer mapChestLayer)
    {
        public List<Chest> Chests { get; set; } = [];
        public TiledMapTileLayer TiledMapTileLayer { get; set; } = mapChestLayer;

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach(Chest chest in Chests)
            {
                chest.Draw(spriteBatch);
            }    
        }

        public List<Chest> GetChestsFromTiledMapTileLayer(Chest chest)
        {
            List<Chest> chests = [];
            int width = TiledMapTileLayer.TileWidth;
            int height = TiledMapTileLayer.TileHeight;

            foreach (TiledMapTile tile in GameManager.TileLocations(chest.ID, TiledMapTileLayer.Tiles))
            {
                chest.Position = new(tile.X * width, tile.Y * height);
                chest.DetectionArea = new Rectangle((int)chest.Position.X - 25, (int)chest.Position.Y - 25,
                    TiledMapTileLayer.TileWidth + 50, TiledMapTileLayer.TileHeight + 50);

                chests.Add(chest.Clone());
            }

            return chests;
        }

        public List<ChestData> GetChestDatas()
        {
            List<ChestData> chestDatas = [];

            foreach (Chest chest in Chests)
                chestDatas.Add(chest.GetChestData());

            return chestDatas;
        }

        public void UpdateFromSave(List<ChestData> chestDatas)
        {
            foreach (Chest chest in Chests)
            {
                foreach(ChestData data in chestDatas)
                {
                    if(chest.Position == data.Position)
                    {
                        chest.Loot.Loots = GameManager.LoadGameItemsFromItemData(data.ItemDatas);
                    }
                }
            }
        }
    }
}
