using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class PlayerData : EntityData
    {
        public int totalXP, xPSinceLastLevel;
        public List<ItemData> backpack = new(); //TODO add backpack to constructor with parameters
        public PlayerData() { }

        /*
        public PlayerData(string type, int baseHealth, int baseDefence, int baseAttack, int entityLevel, int baseXP, Vector2? position,
            Vector2? respawnPosition, int currentHealth, bool isDead, TimeSpan? lastDeathTime, int totalXP, int xpSinceLastLevel, List<ItemData> backpackData) 
            : base(type, baseHealth, baseDefence, baseAttack, entityLevel, baseXP, position, respawnPosition, currentHealth, isDead, lastDeathTime)
        {
            this.totalXP = totalXP;
            this.xPSinceLastLevel = xpSinceLastLevel;
            this.backpack = backpackData;
        }
        public override PlayerData Clone()
        {
            PlayerData data = new(type, baseHealth, baseDefence, baseAttack, entityLevel, baseXP, 
                position, respawnPosition, currentHealth, isDead, LastDeathTime ,totalXP, xPSinceLastLevel, backpack);
            return data;
        }
        
        */

        public override string ToString()
        {
            string toString = base.ToString() + ", ";
            toString += totalXP + ", ";
            toString += xPSinceLastLevel + ", ";
            return toString;
        }
    }
}
