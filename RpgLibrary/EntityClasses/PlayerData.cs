using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class PlayerData : EntityData
    {
        public int totalXP;
        public int baseMana;
        public int mana;
        public int maxMana;
        public int statusPoints;
        public List<ItemData> backpack = new();
        public PlayerData() { }

        public PlayerData(EntityData entityData)
        {
            SetEntityData(entityData);
        }

        public override string ToString()
        {
            string toString = base.ToString() + ", ";
            toString += totalXP + ", ";
            toString += baseMana + ", ";
            toString += mana + ", ";
            toString += maxMana;
            return toString;
        }
    }
}
