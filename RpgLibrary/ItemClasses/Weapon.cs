
namespace RpgLibrary.ItemClasses
{
    public class Weapon : BaseItem
    {
        public Hands NumberOfHands { get; set; }
        public int AttackValue { get; set; }

        public Weapon(WeaponData weaponData)
            : base((ItemData)weaponData)
        {
            NumberOfHands = weaponData.NumberHands;
            AttackValue = weaponData.AttackValue;
        }
        protected Weapon(Weapon weapon) : base(weapon)
        {
            NumberOfHands = weapon.NumberOfHands;
            AttackValue = weapon.AttackValue;
        }

        public override string ToString()
        {
            string weaponString = base.ToString() + ", ";
            weaponString += NumberOfHands.ToString() + ", ";
            weaponString += AttackValue.ToString();
            return weaponString;
        }

        public override Weapon Clone()
        {
            Weapon weapon = new(this);
            return weapon;
        }
    }
}
