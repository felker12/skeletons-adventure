using Microsoft.Xna.Framework;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    internal class Spider : Enemy
    {
        public Spider(EntityData entityData) : base(entityData)
        {
            Texture = GameManager.SpiderTexture;
            SetFrames(6, 32, 32, 0, 32);
            BasicAttackColor = Color.Gray;
        }

        public override Spider Clone()
        {
            Spider spider = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList,
                SpriteColor = this.SpriteColor,
                DefaultColor = this.DefaultColor
            };
            return spider;
        }
    }
}
