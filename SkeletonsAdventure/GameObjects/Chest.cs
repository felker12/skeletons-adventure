using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.GameWorld;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Controls;

namespace SkeletonsAdventure.GameObjects
{
    public class Chest
    {
        public LootList Loot { get; set; } = new();
        public Vector2 Position { get; set; } = new();
        public int ID { get; set; } = -1;
        public Rectangle DetectionArea { get; set; }
        public Label Info { get; set; }

        public Chest()
        {
        }

        public Chest(LootList loot)
        {
            Loot = loot;
        }

        public Chest(ChestData chestData)
        {
            Loot.Loots = GameManager.LoadGameItemsFromItemData(chestData.ItemDatas);
        }

        public void Initialize()
        {
            Info = new()
            {

            };
        }

        public Chest Clone()
        {
            return new(Loot) 
            { 
                Position = Position,
                DetectionArea = DetectionArea,
                ID = ID
            };
        }

        public ChestData GetChestData()
        {
            return new()
            {
                ItemDatas = Loot.GetLootListItemData()
            };
        }
    }
}
