using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.Entities
{
    internal class Skeleton(EntityData entityData) : Enemy(entityData)
    {
        public override Skeleton Clone()
        {
            Skeleton skeleton = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor,
           };
            return skeleton;
        }
    }
}
