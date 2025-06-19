using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.Entities
{
    internal class EliteSkeleton : Skeleton
    {
        public EliteSkeleton(EntityData entityData) : base(entityData)
        {
            IsElite = true;
        }

        public override EliteSkeleton Clone()
        {
            EliteSkeleton skeleton = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList,
                Level = this.Level,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor
            };
            return skeleton;
        }
    }
}
