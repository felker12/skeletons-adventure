
namespace RpgLibrary.ItemClasses
{
    public class Armor : BaseItem
    {
        public ArmorLocation ArmorLocation { get; set; }
        public int DefenseValue { get; set; }

        public Armor(ArmorData armorData)
            : base((ItemData)armorData)
        {
            DefenseValue = armorData.DefenseValue;
            ArmorLocation = armorData.ArmorLocation;
        }

        protected Armor(Armor armor) : base(armor)
        {
            DefenseValue = armor.DefenseValue;
            ArmorLocation = armor.ArmorLocation;
        }

        public override string ToString()
        {
            string weaponString = base.ToString() + ", ";
            weaponString += DefenseValue.ToString();
            return weaponString;
        }

        public override Armor Clone()
        {
            Armor armor = new(this);
            return armor;
        }
    }
}
