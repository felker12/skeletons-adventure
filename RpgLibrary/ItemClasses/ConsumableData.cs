
namespace RpgLibrary.ItemClasses
{
    public class ConsumableData : ItemData
    {
        public Effect Effect { get; set; }
        public int EffectDuration { get; set; }
        public int EffectBonus { get; set; }
        public ConsumableData() : base()
        {
        }

        public override ConsumableData Clone()
        {
            return new ConsumableData()
            {
                Effect = Effect,
                EffectBonus = EffectBonus,
                EffectDuration = EffectBonus,
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
            toString += Effect.ToString() + ", ";
            toString += EffectDuration.ToString() + ", ";
            toString += EffectBonus.ToString();
            return toString;
        }
    }
}
