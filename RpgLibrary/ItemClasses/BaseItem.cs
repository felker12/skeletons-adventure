
namespace RpgLibrary.ItemClasses
{
    public enum Hands { Main, Off, Both }
    public enum ArmorLocation { Body, Head, Hands, Feet }
    public class BaseItem
    {
        public string Name { get; set; }
        public string Description { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public bool Equipped { get; set; } = false;
        public float Weight { get; set; }
        public int Price { get; set; }
        public bool Stackable { get; set; }
        public int Quantity {  get; set; }

        public BaseItem(ItemData item) 
        {
            Name = item.Name;
            Type = item.Type;
            Description = item.Description;
            Weight = item.Weight;
            Price = item.Price;
            Stackable = item.Stackable;
        }

        protected BaseItem(BaseItem item)
        {
            Name = item.Name;
            Type = item.Type;
            Description = item.Description;
            Weight = item.Weight;
            Price = item.Price;
            Stackable = item.Stackable;
        }

        public virtual BaseItem Clone()
        {
            BaseItem item = new(this);
            return item;
        }
        public override string ToString()
        {
            string itemString = Name + ", ";
            itemString += Type + ", ";
            itemString += Description + ", ";
            itemString += Price.ToString() + ", ";
            itemString += Weight.ToString();
            return itemString;
        }
    }
}
