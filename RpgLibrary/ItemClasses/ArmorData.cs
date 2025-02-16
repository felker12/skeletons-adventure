
namespace RpgLibrary.ItemClasses
{
    public class ArmorData : ItemData
    {
        public ArmorLocation ArmorLocation {  get; set; }
        public int DefenseValue {  get; set; }

        public ArmorData() : base()
        {
        }


        public override ArmorData Clone()
        {
            return new ArmorData()
            {
                ArmorLocation = ArmorLocation,
                DefenseValue = DefenseValue,
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
        }

        public override string ToString()
        {
            string toString = base.ToString() + ", ";
            toString += ArmorLocation.ToString() + ", ";
            toString += DefenseValue.ToString();
            return toString;
        }
    }
}
