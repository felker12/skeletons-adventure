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

        public ItemData()
        {
        }

        public virtual ItemData Clone()
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
                Consumable = Consumable,
                Position = Position,
                Quantity = Quantity
            };

            return itemData;
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
            toString += Quantity;
            return toString;
        }
    }
}
