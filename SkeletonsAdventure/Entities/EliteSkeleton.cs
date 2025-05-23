using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.Entities
{
    internal class EliteSkeleton : Skeleton
    {
        public EliteSkeleton(EntityData entityData) : base(entityData)
        {
            ID = 842;
            IsElite = true;
        }

        public override EliteSkeleton Clone()
        {
            EliteSkeleton skeleton = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList,
                EntityLevel = this.EntityLevel,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor
            };
            return skeleton;
        }
    }
}
