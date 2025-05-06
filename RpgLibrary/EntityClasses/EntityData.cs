using Microsoft.Xna.Framework;
using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class EntityData
    {
        public string type = string.Empty;
        public int baseHealth, baseDefence, baseAttack, entityLevel, baseXP, currentHealth;
        public Vector2? position, respawnPosition;
        public bool isDead;
        public TimeSpan? LastDeathTime = new();
        public List<ItemData> Items { get; set; } = new();

        public EntityData() { }

        public EntityData(string type, int baseHealth, int baseDefence, int baseAttack, int entityLevel, int baseXP, 
            Vector2? position, Vector2? respawnPosition, int currentHealth, bool isDead, TimeSpan? lastDeathTime)
        {
            this.type = type;
            this.baseHealth = baseHealth;
            this.baseDefence = baseDefence;
            this.baseAttack = baseAttack;
            this.entityLevel = entityLevel;
            this.baseXP = baseXP;
            this.position = position;
            this.respawnPosition = respawnPosition;
            this.currentHealth = currentHealth;
            this.isDead = isDead;
            LastDeathTime = lastDeathTime;
        }

        public EntityData(EntityData entityData)
        {
            SetEntityData(entityData);
        }

        public void SetEntityData(EntityData entityData)
        {
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
            LastDeathTime = entityData.LastDeathTime;
        }

        public virtual EntityData Clone()
        {
            EntityData data = new(type, baseHealth, baseDefence, baseAttack, entityLevel, baseXP,
                position, respawnPosition, currentHealth, isDead, LastDeathTime)
            {
                Items = Items
            };
            return data;
        }

        public override string ToString()
        {
            string toString = string.Empty;
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
            toString += LastDeathTime;

            return toString;
        }
    }
}
