using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class PlayerData : EntityData
    {
        public int totalXP;
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
            return toString;
        }
    }
}
