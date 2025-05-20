﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Entities;
using RpgLibrary.AttackData;

namespace SkeletonsAdventure.Attacks
{
    public class IcePillar : ShootingAttack
    {
        private int AnimationFrames => _animations.Count;

        public IcePillar(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Width = Texture.Width;
            Height = Texture.Height;
            Frame = new(0, 0, Width, Height);

            AnimatedAttack = true;
        }

        public IcePillar(IcePillar attack) : base(attack)
        {
            Width = attack.Width;
            Height = attack.Height;
            AnimatedAttack = attack.AnimatedAttack;
            Frame = attack.Frame;
            DamageHitBox = attack.DamageHitBox;

            if(AnimatedAttack)
                SetFrames(4, 62, 62, 0, 62);
        }

        public IcePillar(AttackData attackData, Texture2D texture, Entity source, int width, int height) : base(attackData, texture, source)
        {
            Width = width;
            Height = height;
            Frame = new(0, 0, Width, Height);
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
            
            if (AnimatedAttack)
            {
                if(Duration.TotalMilliseconds > AttackDelay)
                    AnimatePillar();
                else
                    DamageHitBox = new((int)Position.X + Width / 4, (int)(Position.Y), Width / 2, Height);
            }
            else
                DamageHitBox = new((int)Position.X + Width / 4, (int)(Position.Y), Width / 2, Height);
        }

        public override void Offset()
        {
            AttackOffset = new Vector2(-Width / 2, -Height / 2);
        }

        private void AnimatePillar()
        {
            int timePerFrame = (AttackLength - AttackDelay) / AnimationFrames;
            int currentFrame = (int)(Duration.TotalMilliseconds - AttackDelay) / timePerFrame;
            double progressPercent = (currentFrame * (100 / (AnimationFrames + 2))) / 100.0; //the +2 is to make it shrink less per tick

            Frame = new Rectangle(0 + (currentFrame * Width), 0, Width, Height);
            DamageHitBox = new((int)Position.X + Width / 4, (int)(Position.Y + Height * progressPercent), Width / 2, (int)(Height * (1.0 - progressPercent)));
        }
    }
}
