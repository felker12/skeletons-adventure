
namespace RpgLibrary.ItemClasses
{
    public class Weapon : BaseItem
    {
        public Hands NumberHands { get; set; }
        public int AttackValue { get; set; }

        public Weapon(WeaponData weaponData)
            : base((ItemData)weaponData)
        {
            NumberHands = weaponData.NumberHands;
            AttackValue = weaponData.AttackValue;
        }
        protected Weapon(Weapon weapon) : base(weapon)
        {
            NumberHands = weapon.NumberHands;
            AttackValue = weapon.AttackValue;
        }

        public override string ToString()
        {
            string weaponString = base.ToString() + ", ";
            weaponString += NumberHands.ToString() + ", ";
            weaponString += AttackValue.ToString();
            return weaponString;
        }

        public override Weapon Clone()
        {
            return new(this);
        }
    }
}
