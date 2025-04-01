using Assimp;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using System.Xml.Linq;

namespace SkeletonsAdventure.ItemClasses
{
    public class GameItem
    {
        public Vector2 Position { get; set; }
        public Texture2D Image { get; }
        public Rectangle SourceRectangle { get; }
        public BaseItem BaseItem { get; set; }
        public string Type { get; }
        public Rectangle ItemRectangle { get; set; }
        public static int Width { get; } = 32;
        public static int Height { get; } = 32;
        public int Quantity { get; set; }
        public Label ToolTip { get; set; }
        public string Name => GetItemData().Name;

        public GameItem(BaseItem item, int quantity, Texture2D texture)
        {
            BaseItem = item.Clone();
            Image = texture;
            SourceRectangle = item.SourceRectangle;
            Type = item.Type;

            Quantity = quantity;
            ToolTip = new()
            {
                Visible = false,
                SpriteFont = GameManager.ToolTipFont,
                Color = Color.Aqua
            };

            if (BaseItem.Stackable)
                ToolTip.Text = Quantity + " " + BaseItem.Name;
            else
                ToolTip.Text = BaseItem.Name;
        }

        public GameItem(GameItem gameItem)
        {
            if(gameItem.BaseItem.Stackable)
                BaseItem = gameItem.BaseItem;
            else
                BaseItem = gameItem.BaseItem.Clone();

            BaseItem.Equipped = gameItem.BaseItem.Equipped;
            Image = gameItem.Image;
            SourceRectangle = gameItem.SourceRectangle;
            Type = gameItem.Type;
            ToolTip = new()
            {
                Visible = false,
                SpriteFont = gameItem.ToolTip.SpriteFont,
                Color = gameItem.ToolTip.Color,
                Text = gameItem.ToolTip.Text,
                Position = gameItem.ToolTip.Position
            };
            Quantity = gameItem.Quantity;
            Position = gameItem.Position;
        }

        public GameItem Clone()
        {
            GameItem item = new(this);
            return item;
        }

        public void Update()
        {
            ItemRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            ToolTip.Position = Position + new Vector2(2, 0);

            if (BaseItem.Stackable)
            {
                ToolTip.Text = Quantity + " " + BaseItem.Name;
                BaseItem.Quantity = Quantity;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Image, Position, SourceRectangle, Color.White);

            //TODO
            if(BaseItem.Equipped == true)
                spriteBatch.DrawRectangle(ItemRectangle, Color.MediumVioletRed, 2, 0);
            else
                spriteBatch.DrawRectangle(ItemRectangle, Color.WhiteSmoke, 1, 0);

            if (ToolTip.Visible)
                ToolTip.Draw(spriteBatch);
        }

        public override string ToString()
        {
            return BaseItem.ToString();
        }

        public void SetEquipped(bool equipped)
        {
            BaseItem.Equipped = equipped;
        }

        public ItemData GetItemData()
        {
            ItemData itemData = new()
            {
                Name = BaseItem.Name,
                Type = BaseItem.Type,
                Description = BaseItem.Description,
                Price = BaseItem.Price,
                Weight = BaseItem.Weight,
                Equipped = BaseItem.Equipped,
                Stackable = BaseItem.Stackable,
                Position = Position,
                Quantity = Quantity
            };

            if (BaseItem is Consumable)
                itemData.Consumable = true;
            else 
                itemData.Consumable = false;

            return itemData;
        }
    }
}
