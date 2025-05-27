using RpgLibrary.ItemClasses;
using RpgLibrary.QuestClasses;

namespace RpgLibrary.EntityClasses
{
    public class PlayerData : EntityData
    {
        public int totalXP = 0;
        public int baseMana = 0;
        public int mana = 0;
        public int maxMana = 0;
        public int statusPoints = 0;
        public List<ItemData> backpack = new();
        public List<QuestData> activeQuests = new();
        public List<QuestData> completedQuests = new();

        public PlayerData() { }

        public PlayerData(PlayerData entityData) : base(entityData)
        {
            totalXP = entityData.totalXP;
            baseMana = entityData.baseMana;
            mana = entityData.mana;
            maxMana = entityData.maxMana;
            statusPoints = entityData.statusPoints;
            backpack = entityData.backpack;
            activeQuests = entityData.activeQuests;
            completedQuests = entityData.completedQuests;
        }

        public PlayerData(EntityData entityData) : base(entityData)
        {
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
