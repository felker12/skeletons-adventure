using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemLoot
{
    internal class LootList()
    {
        public List<GameItem> Loots { get; set; } = [];
        public int Count => Loots.Count;

        public void Add(GameItem item) 
        {
            Loots.Add(item.Clone());
        }

        public void Add(List<GameItem> items)
        {
            foreach (GameItem item in items)
                Add(item);
        }

        public void Remove(GameItem item) 
        {
            Loots.Remove(item); 
        }

        public void Clear() 
        { 
            Loots.Clear(); 
        }

        public override string ToString()
        {
            string toString = string.Empty;
            
            foreach(GameItem loot in Loots)
                toString += loot.ToString() + "\n";

            return toString;
        }

        public LootList Clone()
        {
            LootList loot = new();

            foreach (GameItem item in Loots)
                loot.Add(item);

            return loot;
        }

        public List<ItemData> GetLootListItemData()
        {
            List<ItemData> data = [];

            foreach (GameItem item in Loots)
                data.Add(item.GetItemData());

            return data;
        }
    }
}
