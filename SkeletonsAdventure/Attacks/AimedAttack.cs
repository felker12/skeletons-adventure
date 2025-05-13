using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.AttackData;
using SkeletonsAdventure.Entities;
using System;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Animations;

namespace SkeletonsAdventure.Attacks
{
    public class AimedAttack : EntityAttack
    {

        public AimedAttack(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Initialize();
        }

        public AimedAttack(AimedAttack attack) : base(attack)
        {

        }

        private void Initialize()
        {
            //TODO
            Frame = new Rectangle(0, 0, 32, 32);
        }

        public override AimedAttack Clone()
        {
            return new AimedAttack(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
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
            AttackOffset = new(-6, -Height);
            Frame = new(0, 0, Width, Height);


            if (Source.CurrentAnimation == AnimationKey.Up)
            {
                AttackOffset = new(-6, -Height);
            }
            else if (Source.CurrentAnimation == AnimationKey.Down)
            {
                AttackOffset = new(-6, Source.Height);
            }
            if (Source.CurrentAnimation == AnimationKey.Left)
            {
                AttackOffset = new(-Source.Width, 0);
            }
            else if (Source.CurrentAnimation == AnimationKey.Right)
            {
                AttackOffset = new(Source.Width, 0);
            }
        }
    }
}
