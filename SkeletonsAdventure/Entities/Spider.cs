using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.Entities
{
    public class Spider : Enemy
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
