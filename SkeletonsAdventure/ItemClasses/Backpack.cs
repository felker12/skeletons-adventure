using System.Collections.Generic;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemClasses
{
    public class Backpack()
    {
        public List<GameItem> Items { get; } = [];
        public static int Capacity { get; } = 76;

        public List<GameItem> Clone()
        {
            List<GameItem> items = [];

            foreach (var item in Items)
                items.Add(item.Clone());

            return items;
        }

        public void Update()
        {
            foreach (GameItem item in Items)
                item.Update();
        }

        public bool AddItem(GameItem item)
        {
            bool added = false;
            if (Items.Count < Capacity)
            {
                if (item.BaseItem.Stackable && ContainsBaseItem(item))
                {
                    foreach (var gameItem in Items)
                    {
                        if (item.BaseItem == gameItem.BaseItem)
                        {
                            gameItem.Quantity += item.Quantity;
                        }
                    }
                }
                else
                    Items.Add(item.Clone());

                added = true;
            }
            else if (Items.Count == Capacity)
            {
                if (item.BaseItem.Stackable && ContainsBaseItem(item))
                {
                    foreach (var gameItem in Items)
                    {
                        if (item.BaseItem == gameItem.BaseItem)
                        {
                            gameItem.Quantity += item.Quantity;
                        }
                    }
                    added = true;
                }
            }

            return added;
        }

        public void RemoveItem(GameItem item)
        {
            Items.Remove(item);
        }
        public bool ContainsBaseItem(GameItem item)
        {
            foreach (var gameItem in Items)
            {
                if (item.BaseItem == gameItem.BaseItem)
                    return true;
            }
            return false;
        }

        public bool ContainsItem(GameItem item)
        {
            foreach (var gameItem in Items)
            {
                if (item == gameItem)
                    return true;
            }
            return false;
        }

        public override string ToString()
        {
            string output = string.Empty;

            foreach (GameItem item in Items)
                output += item.ToString() + "\n";

            return output;
        }

        public List<ItemData> GetBackpackData()
        {
            List<ItemData> backpackData = [];

            foreach (var gameItem in Items)
            {
                backpackData.Add(gameItem.GetItemData()); //TODO?
            }

            return backpackData;
        }
    }
}
