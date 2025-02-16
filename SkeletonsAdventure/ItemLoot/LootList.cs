using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemLoot
{
    public class LootList()
    {
        public List<GameItem> Loots = [];

        public void Add(GameItem item) { Loots.Add(item.Clone()); } 
        public void Remove(GameItem item) { Loots.Remove(item); }
        public void Clear() { Loots.Clear(); }

        public override string ToString()
        {
            string toString = string.Empty;
            
            foreach(GameItem loot in Loots)
                toString += loot.ToString() + "\n";

            return toString;
        }

        public List<ItemData> GetLootListItemData()
        {
            List<ItemData> data = [];

            foreach (GameItem item in Loots)
            {
                data.Add(item.GetItemData());
            }

            return data;
        }
    }
}
