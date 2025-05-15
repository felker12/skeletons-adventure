using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.AttackData;
using SkeletonsAdventure.Entities;
using System;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Animations;
using MonoGame.Extended;

namespace SkeletonsAdventure.Attacks
{
    public class ShootingAttack : EntityAttack
    {
        public ShootingAttack(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Initialize();
        }

        public ShootingAttack(ShootingAttack attack) : base(attack)
        {

        }

        private void Initialize()
        {
            //TODO
            Frame = new Rectangle(0, 0, 32, 32);
        }

        public override ShootingAttack Clone()
        {
            return new ShootingAttack(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.DrawLine(StartPosition + new Vector2(Width / 2, Height / 2), GetCenter(), Color.Aquamarine, 1);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        //TODO Overide this with the corret offset parameters based on the type of the entity calling the method 
        public override void Offset()
        {
            Width = 32;
            Height = 32;
            Frame = new(0, 0, Width, Height);

            //start the attack at the center of the entity
            AttackOffset = new(Source.Width / 2 - Width / 2, Source.Height / 2 - Height / 2);
        }
    }
}
