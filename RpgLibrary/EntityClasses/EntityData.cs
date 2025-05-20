using Microsoft.Xna.Framework;
using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class EntityData
    {
        public List<ItemData> Items { get; set; } = new();

        public int id = 0;
        public string type = string.Empty;
        public int baseHealth, baseDefence, baseAttack, entityLevel, baseXP, currentHealth;
        public Vector2? position, respawnPosition;
        public bool isDead;
        public TimeSpan? lastDeathTime = new();

        public EntityData() { }

        public EntityData(int id, string type, int baseHealth, int baseDefence, int baseAttack, int entityLevel, int baseXP, 
            Vector2? position, Vector2? respawnPosition, int currentHealth, bool isDead, TimeSpan? lastDeathTime)
        {
            this.id = id;
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
            this.lastDeathTime = lastDeathTime;
        }

        public EntityData(EntityData entityData)
        {
            SetEntityData(entityData);
        }

        public void SetEntityData(EntityData entityData)
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
        }

        public virtual EntityData Clone()
        {
            EntityData data = new(id, type, baseHealth, baseDefence, baseAttack, entityLevel, baseXP,
                position, respawnPosition, currentHealth, isDead, lastDeathTime)
            {
            };
            return data;
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
            toString += lastDeathTime;

            return toString;
        }
    }
}
