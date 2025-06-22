using MonoGame.Extended;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.ItemClasses
{
    internal class GameItem
    {
        public Vector2 Position { get; set; } = Vector2.Zero;
        public Texture2D Image { get; }
        public Rectangle SourceRectangle { get; }
        public BaseItem BaseItem { get; set; }
        public string Type { get; } = string.Empty;
        public Rectangle ItemRectangle { get; set; }
        public static int Width { get; } = 32;
        public static int Height { get; } = 32;
        public int Quantity { get; set; } = 1;
        public int Price { get; set; } = 0;
        public float Weight { get; set;} = 0f;
        public bool Stackable { get; set; } = false;
        public bool Equipped { get; set; } = false;
        public Label ToolTip { get; set; } = new()
        {
            Visible = false,
            SpriteFont = GameManager.Arial10,
            TextColor = Color.Aqua
        };
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string TexturePath { get; set; } = string.Empty;

        public GameItem(BaseItem item, int quantity, Texture2D texture) : this(item)
        {
            BaseItem = item.Clone();
            Image = texture;
            Quantity = quantity;
        }

        public GameItem(BaseItem item)
        {
            BaseItem = item.Clone();
            Image = GameManager.Content.Load<Texture2D>(@$"{item.TexturePath}");
            SourceRectangle = item.SourceRectangle;
            Type = item.Type;
            Quantity = item.Quantity;
            Name = item.Name;
            Stackable = item.Stackable;
            Description = item.Description;
            TexturePath = item.TexturePath;
            Price = item.Price;
            Weight = item.Weight;
            Equipped = item.Equipped;

            if (Stackable)
                ToolTip.Text = Quantity + " " + Name;
            else
                ToolTip.Text = Name;
        }

        public GameItem(GameItem gameItem)
        {
            if(gameItem.Stackable)
                BaseItem = gameItem.BaseItem;
            else
                BaseItem = gameItem.BaseItem.Clone();

            BaseItem.Equipped = gameItem.BaseItem.Equipped;
            Image = gameItem.Image;
            SourceRectangle = gameItem.SourceRectangle;
            Type = gameItem.Type;
            Name = gameItem.Name;
            Stackable = gameItem.Stackable;
            Description = gameItem.Description;
            TexturePath = gameItem.TexturePath;
            Price = gameItem.Price;
            Weight = gameItem.Weight;
            Equipped = gameItem.Equipped;

            ToolTip = new()
            {
                Visible = false,
                SpriteFont = gameItem.ToolTip.SpriteFont,
                TextColor = gameItem.ToolTip.TextColor,
                Text = gameItem.ToolTip.Text,
                Position = gameItem.ToolTip.Position
            };
            Quantity = gameItem.Quantity;
            Position = gameItem.Position;
        }

        public GameItem Clone()
        {
            return new GameItem(this);
        }

        public void Update()
        {
            ItemRectangle = new Rectangle((int)Position.X, (int)Position.Y, Width, Height);

            ToolTip.Position = Position + new Vector2(2, 0);

            if (Stackable)
            {
                ToolTip.Text = Quantity + " " + Name;
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
            Equipped = equipped;
        }

        public ItemData GetItemData()
        {
            ItemData itemData = new()
            {
                Name = Name,
                Type = Type,
                Description = Description,
                Price = Price,
                Weight = Weight,
                Equipped = Equipped,
                Stackable = Stackable,
                Position = Position,
                Quantity = Quantity,
                TexturePath = TexturePath,
            };

            if (BaseItem is Consumable)
                itemData.Consumable = true;
            else 
                itemData.Consumable = false;

            if(BaseItem is Weapon weapon)
            {
                WeaponData weaponData = new(itemData)
                {
                    NumberHands = weapon.NumberHands,
                    AttackValue = weapon.AttackValue
                };
                return weaponData;
            }
            else if (BaseItem is Armor armor)
            {
                ArmorData armorData = new(itemData)
                {
                    ArmorLocation = armor.ArmorLocation,
                    DefenseValue = armor.DefenseValue
                };

                return armorData;
            }
            else if (BaseItem is Consumable consumable)
            {
                ConsumableData consumableData = new(itemData)
                {
                    Effect = consumable.Effect,
                    EffectBonus = consumable.EffectBonus,
                    EffectDuration = consumable.EffectDuration
                };
                return consumableData;
            }

            return itemData;
        }
    }
}
