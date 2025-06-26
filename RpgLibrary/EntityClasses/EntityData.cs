using Microsoft.Xna.Framework;
using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class EntityData
    {
        public List<ItemData> Items { get; set; } = new();
        public int id { get; set; } = 0;
        public string type { get; set; } = string.Empty;
        public int baseHealth { get; set; }
        public int baseDefence { get; set; }
        public int baseAttack { get; set; }
        public int entityLevel { get; set; }
        public int baseXP { get; set; }
        public int currentHealth { get; set; }
        public Vector2? position { get; set; }
        public Vector2? respawnPosition { get; set; }
        public bool isDead { get; set; }
        public TimeSpan? lastDeathTime { get; set; } = new();
        public string dropTableName { get; set; } = string.Empty;

        public EntityData() { }

        public EntityData(EntityData entityData)
        {
            id = entityData.id;
            type = entityData.type;
            baseHealth = entityData.baseHealth;
            baseDefence = entityData.baseDefence;
            baseAttack = entityData.baseAttack;
            entityLevel = entityData.entityLevel;
            baseXP = entityData.baseXP;
            position = entityData.position;
            respawnPosition = entityData.respawnPosition;
            currentHealth = entityData.currentHealth;
            isDead = entityData.isDead;
            lastDeathTime = entityData.lastDeathTime;
            Items = entityData.Items;
            dropTableName = entityData.dropTableName;
        }

        public virtual EntityData Clone()
        {
            return new(this);
        }

        public override string ToString()
        {
            string toString = string.Empty;
            toString += id + ", ";
            toString += type + ", ";
            toString += baseHealth + ", ";
            toString += baseDefence + ", ";
            toString += baseAttack + ", ";
            toString += entityLevel + ", ";
            toString += baseXP + ", ";
            toString += position + ", ";
            toString += respawnPosition + ", ";
            toString += currentHealth + ", ";
            toString += isDead + ", ";
            toString += lastDeathTime + ", ";
            toString += dropTableName;

            return toString;
        }
    }
}
