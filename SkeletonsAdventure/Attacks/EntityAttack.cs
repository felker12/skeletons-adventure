using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Entities;
using System;
using SkeletonsAdventure.Animations;
using MonoGame.Extended;
using RpgLibrary.AttackData;
using CppNet;

namespace SkeletonsAdventure.Attacks
{
    public class EntityAttack : Sprite
    {
        public int AttackLength { get; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public Vector2 AttackOffset { get; set; }
        public bool HasHit { get; set; } = false;
        public bool CanHit { get; set; } = true;
        public Entity Source { get; }

        public EntityAttack(Texture2D texture, Entity source) : base()
        {
            Texture = texture;
            AttackLength = 300; //length the attack animation is drawn on the screen in milliseconds
            AttackOffset = new();
            Source = source;
        }

        public EntityAttack(Texture2D texture, Entity source, int attackLength) : base()
        {
            Texture = texture;
            AttackLength = attackLength; //length the attack animation is drawn on the screen in milliseconds
            AttackOffset = new();
            Frame = new();
            Source = source;
        }

        public EntityAttack(EntityAttack attack) : base()
        {
            Texture = attack.Texture;
            AttackLength = attack.AttackLength;
            AttackOffset = attack.AttackOffset;
            Frame = attack.Frame;
            Source = attack.Source;
            Position = attack.Position;
            SpriteColor = attack.SpriteColor;
            Info.Color = attack.Info.Color;
        }

        public EntityAttack(AttackData attackData, Texture2D texture, Entity source) : base()
        {
            AttackLength = attackData.AttackLength;
            StartTime = attackData.StartTime;
            Duration = attackData.Duration;
            AttackOffset = attackData.AttackOffset;

            Texture = texture;
            Source = source;
        }

        public EntityAttack Clone()
        {
            EntityAttack attack = new(Texture, Source);
            return attack;
        }

        public EntityAttack Clone(EntityAttack attack)
        {
            return new EntityAttack(attack);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
            spriteBatch.Draw(Texture, Position, Frame, SpriteColor);

            Info.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (StartTime > TimeSpan.Zero)
                Duration = gameTime.TotalGameTime - StartTime;

            Info.Position = Position + new Vector2(1,1);

            //TODO draw the info with a different color for the player
            if (Source is Player)
                Info.Color = Color.Cyan;
            else
                Info.Color = new Color(255, 81, 89, 255);
        }

        public virtual AttackData GetAttackData() 
        {
            return new()
            {
                AttackLength = AttackLength,
                StartTime = StartTime,
                Duration = Duration,
                AttackOffset = AttackOffset,
            };
        }

        //TODO Overide this with the corret offset parameters based on the type of the entity calling the method 
        public virtual void Offset()
        {
            if (Source.CurrentAnimation == AnimationKey.Up)
            {
                Width = 48;
                Height = 32;
                AttackOffset = new(-6, -Height);
                Frame = new(10, 160, Width, Height);
            }
            else if (Source.CurrentAnimation == AnimationKey.Down)
            {
                Width = 48;
                Height = 32;
                AttackOffset = new(-6, Source.Height);
                Frame = new(10, 230, Width, Height);
            }
            if (Source.CurrentAnimation == AnimationKey.Left)
            {
                Width = 32;
                Height = 60;
                AttackOffset = new(-Source.Width, 0);
                Frame = new(15, 5, Width, Height);
            }
            else if (Source.CurrentAnimation == AnimationKey.Right)
            {
                Width = 32;
                Height = 60;
                AttackOffset = new(Source.Width, 0);
                Frame = new(20, 80, Width, Height);
            }
        }
    }
}
