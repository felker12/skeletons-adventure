using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.Entities
{
    internal class Skeleton : Enemy
    {
        public Skeleton(EntityData entityData) : base(entityData)
        {
        }

        public override Skeleton Clone()
        {
            Skeleton skeleton = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor
            };
            return skeleton;
        }
    }
}
