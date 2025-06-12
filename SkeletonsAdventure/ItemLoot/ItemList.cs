using SkeletonsAdventure.ItemClasses;
using System.Collections.Generic;
using RpgLibrary.ItemClasses;

namespace SkeletonsAdventure.ItemLoot
{
    internal class ItemList()
    {
        public List<GameItem> Items { get; set; } = [];
        public int Count => Items.Count;

        public void Update()
        {
            foreach (GameItem item in Items)
                item.Update();
        }

        public virtual bool Add(GameItem item)
        {
            if (item is null)
                return false;

            if (item.BaseItem.Stackable && ContainsBaseItem(item)) //if the item is stackable and already exists in the backpack, increase the quantity of that item
            {
                return AddItemToStack(item); //returns true if the item was added to an existing stack
            }
            else
            {
                Items.Add(item.Clone());
                return true;
            }
        }

        protected virtual bool AddItemToStack(GameItem item)
        {
            foreach (var gameItem in Items)
            {
                if (item.BaseItem == gameItem.BaseItem)
                {
                    gameItem.Quantity += item.Quantity;
                    gameItem.BaseItem.Quantity = gameItem.Quantity; //update the base item's quantity as well //TODO idk if this is necessary
                    return true;
                }
            }

            return false; //if no existing stack was found, return false
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
    }
}
