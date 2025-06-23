using Microsoft.Xna.Framework;
using RpgLibrary.ItemClasses;

namespace RpgLibrary.EntityClasses
{
    public class EntityData
    {
        public List<ItemData> Items { get; set; } = new();
        public DropTableData DropTableData { get; set; } = new DropTableData();

        public int id = 0;
        public string type = string.Empty;
        public int baseHealth, baseDefence, baseAttack, entityLevel, baseXP, currentHealth;
        public Vector2? position, respawnPosition;
        public bool isDead;
        public TimeSpan? lastDeathTime = new();

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
            toString += lastDeathTime;

            return toString;
        }
    }
}
