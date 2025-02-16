
namespace RpgLibrary.ItemClasses
{
    public class WeaponData : ItemData
    {
        public Hands NumberHands {  get; set; }
        public int AttackValue {  get; set; }

        public WeaponData() : base()
        {
        }
        public override WeaponData Clone()
        {
            return new WeaponData()
            {
                AttackValue = AttackValue,
                NumberHands = NumberHands,
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
            toString += NumberHands.ToString() + ", ";
            toString += AttackValue.ToString();
            return toString;
        }
    }
}
