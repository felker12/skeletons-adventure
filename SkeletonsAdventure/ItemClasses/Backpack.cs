using System.Collections.Generic;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.ItemLoot;

namespace SkeletonsAdventure.ItemClasses
{
    internal class Backpack() : ItemList
    {
        public int Capacity { get; } = 76;

        public void Update()
        {
            foreach (GameItem item in Items)
                item.Update();
        }

        public new bool Add(GameItem item)
        {
            bool added = false;

            if (item is null) 
                return added;

            if (Count <= Capacity)
            {
                if (item.BaseItem.Stackable && ContainsBaseItem(item))
                {
                    foreach (var gameItem in Items)
                    {
                        if (item.BaseItem == gameItem.BaseItem)
                        {
                            gameItem.Quantity += item.Quantity;
                            added = true;
                            break;
                        }
                    }
                }
                else if (Count < Capacity)
                {
                    Items.Add(item.Clone());
                    added = true;
                }
            }

            return added;
        }

        public override void Add(List<GameItem> items)
        {
            foreach (GameItem item in items)
                Add(item);
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
