using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemLoot
{
    public class DroppedLootManager()
    {
        public List<GameItem> Items { get; set; } = [];
        public List<GameItem> ItemToRemove { get; set; } = [];

        public void Update()
        {
            foreach(GameItem item in ItemToRemove)
            {
                Items.Remove(item);
            }

            foreach (GameItem item in Items)
                item.Update();

            ItemToRemove.Clear();
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (GameItem item in Items)
                item.Draw(spriteBatch);
        }

        public void Add(GameItem item, Vector2 pos)
        {
            item.Position = pos + new Vector2(20,20);
            Items.Add(item.Clone());
        }

        public void Add(List<GameItem> items, Vector2 pos)
        {
            foreach (GameItem item in items)
                Add(item, pos);
        }

        public void Add(LootList lootList, Vector2 pos)
        {
            Vector2 offset = Vector2.Zero;
            foreach(GameItem item in lootList.Loots)
            { 
                Add(item, pos + offset);
                offset += new Vector2(15, 15);
            }
        }

        public void Remove(GameItem item)
        {
            Items.Remove(item);
        }

        public void Clear()
        {
            Items.Clear();
        }

        public List<ItemData> GetDroppedItemData()
        {
            List<ItemData> droppedItemData = [];

            foreach (var gameItem in Items)
            {
                droppedItemData.Add(gameItem.GetItemData());
            }

            return droppedItemData;
        }
    }
}
