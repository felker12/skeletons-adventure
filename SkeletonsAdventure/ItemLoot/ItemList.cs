using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemLoot
{
    internal class ItemList()
    {
        public List<GameItem> Items { get; set; } = [];
        public int Count => Items.Count;

        public virtual void Add(GameItem item) 
        {
            Items.Add(item.Clone());
        }

        public virtual void Add(List<GameItem> items)
        {
            foreach (GameItem item in items)
                Add(item);
        }

        public virtual void Remove(GameItem item) 
        {
            Items.Remove(item); 
        }

        public void Clear() 
        { 
            Items.Clear(); 
        }

        public override string ToString()
        {
            string toString = string.Empty;
            
            foreach(GameItem item in Items)
                toString += item.ToString() + "\n";

            return toString;
        }

        public ItemList Clone()
        {
            ItemList items = new();

            foreach (GameItem item in Items)
                items.Add(item);

            return items;
        }

        public List<ItemData> GetItemListItemData()
        {
            List<ItemData> data = [];

            foreach (GameItem item in Items)
                data.Add(item.GetItemData());

            return data;
        }
    }
}
