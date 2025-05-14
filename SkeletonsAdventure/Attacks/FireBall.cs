using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Entities;
using System;
using SkeletonsAdventure.Animations;
using RpgLibrary.AttackData;

namespace SkeletonsAdventure.Attacks
{
    public class FireBall : ShootingAttack
    {


        public FireBall(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Initialize();
        }

        public FireBall(FireBall attack) : base(attack)
        {
            Initialize();
        }

        private void Initialize()
        {

        }

        public override FireBall Clone()
        {
            return new FireBall(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }


    }
}
