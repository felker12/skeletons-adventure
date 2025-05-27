using Microsoft.Xna.Framework;

namespace RpgLibrary.ItemClasses
{
    public enum Hands { Main, Off, Both }
    public enum ArmorLocation { Body, Head, Hands, Feet }
    public class BaseItem
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Equipped { get; set; } = false;
        public float Weight { get; set; }
        public int Price { get; set; }
        public bool Stackable { get; set; }
        public int Quantity {  get; set; }
        public Rectangle SourceRectangle { get; set; }
        public string TexturePath { get; set; }

        public BaseItem(ItemData item) 
        {
            Name = item.Name;
            Type = item.Type;
            Description = item.Description;
            Weight = item.Weight;
            Price = item.Price;
            Stackable = item.Stackable;
            SourceRectangle = item.SourceRectangle;
            TexturePath = item.TexturePath;
        }

        protected BaseItem(BaseItem item)
        {
            Name = item.Name;
            Type = item.Type;
            Description = item.Description;
            Weight = item.Weight;
            Price = item.Price;
            Stackable = item.Stackable;
            SourceRectangle = item.SourceRectangle;
            TexturePath = item.TexturePath;
        }

        public virtual BaseItem Clone()
        {
            return new(this);
        }
        public override string ToString()
        {
            return $"Name: {Name}, " +
                   $"Description: {Description}, " +
                   $"Type: {Type}, " +
                   $"Equipped: {Equipped}, " +
                   $"Weight: {Weight}, " +
                   $"Price: {Price}, " +
                   $"Stackable: {Stackable}, " +
                   $"Quantity: {Quantity}, " +
                   $"SourceRectangle: {SourceRectangle}, " +
                   $"TexturePath: {TexturePath}";
        }
    }
}
