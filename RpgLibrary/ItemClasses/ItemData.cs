using Microsoft.Xna.Framework;

namespace RpgLibrary.ItemClasses
{
    public class ItemData
    {
        public string Name = string.Empty;
        public string Type = string.Empty;
        public string Description = string.Empty;
        public int Price;
        public float Weight;
        public bool Equipped;
        public bool Stackable;
        public bool Consumable;
        public Vector2 Position = Vector2.Zero;
        public int Quantity;
        public Rectangle SourceRectangle;
        public string TexturePath = string.Empty;

        public ItemData()
        {
        }

        protected ItemData(ItemData itemData)
        {
            Name = itemData.Name;
            Type = itemData.Type;
            Description = itemData.Description;
            Price = itemData.Price;
            Weight = itemData.Weight;
            Equipped = itemData.Equipped;
            Stackable = itemData.Stackable;
            Consumable = itemData.Consumable;
            Position = itemData.Position;
            Quantity = itemData.Quantity;
            SourceRectangle = itemData.SourceRectangle;
            TexturePath = itemData.TexturePath;
        }

        public virtual ItemData Clone()
        {
            return new(this);
        }

        public override string ToString()
        {
            string toString = Name + ", ";
            toString += Type + ", ";
            toString += Description + ", ";
            toString += Price + ", ";
            toString += Weight + ", ";
            toString += Stackable + ", ";
            toString += Consumable + ", ";
            toString += Position.ToString() + ", ";
            toString += Quantity + ", ";
            toString += SourceRectangle.ToString();
            return toString;
        }
    }
}
