using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.GameWorld;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;

namespace SkeletonsAdventure.GameObjects
{
    public class Chest
    {
        public ChestType ChestType {get; set; } //TODO add different types of chests
        public LootList Loot { get; set; } = new();
        public Vector2 Position { get; set; } = new();
        public int ID { get; set; } = -1;
        public Rectangle DetectionArea { get; set; }
        public Label Info { get; set; } = new()
        {
            Text = "Press R to open",
            Visible = false,
            SpriteFont = GameManager.Arial12
        };

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle(DetectionArea, Color.White, 1, 0); //TODO

            if (Info.Visible)
            {
                Info.Draw(spriteBatch); 
            }
        }

        public Chest()
        {
        }

        public Chest(Chest chest)
        {
            Position = chest.Position;
            DetectionArea = chest.DetectionArea;
            ID = chest.ID;
            ChestType = chest.ChestType;
            Loot = chest.Loot.Clone();
            Info.Position = chest.Position;
        }

        public Chest(LootList loot)
        {
            Loot = loot;
        }

        public Chest Clone()
        {
            return new(this);
        }

        public ChestData GetChestData()
        {
            return new()
            {
                ItemDatas = Loot.GetLootListItemData(),
                ID = ID,
                ChestType = ChestType,
                Position = Position
            };
        }
    }
}
