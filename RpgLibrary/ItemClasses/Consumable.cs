
namespace RpgLibrary.ItemClasses
{
    public enum Effect { Heal, AttackIncrease, DefenceIncrease}
    public class Consumable : BaseItem
    {
        public Effect Effect { get; set; }
        public int EffectDuration { get; set; }
        public int EffectBonus { get; set; }

        public Consumable(ConsumableData consumableData) : base((ItemData)consumableData)
        {
            Effect = consumableData.Effect;
            EffectDuration = consumableData.EffectDuration;
            EffectBonus = consumableData.EffectBonus;
        }

        protected Consumable(Consumable consumable) : base(consumable)
        {
            Effect = consumable.Effect;
            EffectDuration = consumable.EffectDuration;
            EffectBonus = consumable.EffectBonus;
        }

        public override string ToString()
        {
            string toString = base.ToString() + ", ";
            toString += Effect.ToString() + ", ";
            toString += EffectDuration.ToString() + ", ";
            toString += EffectBonus.ToString();
            return toString;
        }

        public override Consumable Clone()
        {
            Consumable consumable = new(this);
            return consumable;
        }
    }
}
