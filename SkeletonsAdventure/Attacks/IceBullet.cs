using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.AttackData;
using SkeletonsAdventure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Attacks
{
    public class IceBullet : ShootingAttack
    {
        public float RotationAngle { get; set; } = 0f;
        public IceBullet(AttackData attackData, Texture2D texture, Entity source) : base(attackData, texture, source)
        {
            Initialize();
        }

        public IceBullet(IceBullet attack) : base(attack)
        {
            Initialize();
        }

        private void Initialize()
        {
            Width = 32;
            Height = 32;

            SetFrames(3, Width, Height, 0, Height);
        }

        public override IceBullet Clone()
        {
            return new IceBullet(this);
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
            //start the attack at the center of the entity
            AttackOffset = new(Source.Width / 2 - Width / 2, Source.Height / 2 - Height / 2);
        }
    }
}
