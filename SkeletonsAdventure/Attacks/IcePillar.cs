using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Entities;
using RpgLibrary.AttackData;

namespace SkeletonsAdventure.Attacks
{
    public class IcePillar : ShootingAttack
    {
        public IcePillar(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Initialize();
        }

        public IcePillar(IcePillar attack) : base(attack)
        {
            Initialize();
        }

        private void Initialize()
        {
            Width = Texture.Width;
            Height = Texture.Height;

            Frame = new(0, 0, Texture.Width, Texture.Height);

            DamageHitBox = new((int)Position.X + 40, (int)Position.Y, 32, Texture.Height);
        }

        public override IcePillar Clone()
        {
            return new IcePillar(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            DamageHitBox = new((int)Position.X + 40, (int)Position.Y, 32, Texture.Height);
        }

        public override void Offset()
        {
            AttackOffset = new Vector2(-Width / 2, -Height / 2);
        }
    }
}
