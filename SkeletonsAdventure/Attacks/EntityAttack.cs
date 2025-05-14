using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using SkeletonsAdventure.Entities;
using System;
using SkeletonsAdventure.Animations;
using RpgLibrary.AttackData;
using MonoGame.Extended;

namespace SkeletonsAdventure.Attacks
{
    public class EntityAttack : AnimatedSprite
    {
        public int AttackLength { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan Duration { get; set; }
        public TimeSpan LastAttackTime { get; set; }
        public Vector2 AttackOffset { get; set; }
        public Entity Source { get; protected set; }
        public int AttackCoolDownLength { get; protected set; } = 800;  //length of the delay between attacks in milliseconds
        public float DamageModifier { get; set; }
        public int ManaCost { get; set; }
        public bool AnimatedAttack { get; set; } = false; //TODO

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
            LastAttackTime = attack.LastAttackTime;
            AttackCoolDownLength = attack.AttackCoolDownLength;
            Motion = attack.Motion;
            Speed = attack.Speed;
            DamageModifier = attack.DamageModifier;
            ManaCost = attack.ManaCost;
        }

        public EntityAttack(AttackData attackData, Texture2D texture, Entity source) : base()
        {
            AttackLength = attackData.AttackLength;
            StartTime = attackData.StartTime;
            Duration = attackData.Duration;
            AttackOffset = attackData.AttackOffset;
            LastAttackTime = attackData.LastAttackTime;
            AttackCoolDownLength = attackData.AttackCoolDownLength;
            Speed = attackData.Speed;
            DamageModifier = attackData.DamageModifier;
            ManaCost = attackData.ManaCost;

            Texture = texture;
            Source = source;
        }

        public virtual EntityAttack Clone()
        {
            return new EntityAttack(this);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(GetRectangle, SpriteColor, 1, 0); //TODO
            spriteBatch.Draw(Texture, Position, Frame, SpriteColor);

            Info.Draw(spriteBatch);
        }

        public override void Update(GameTime gameTime)
        {
            if (AnimatedAttack)
                base.Update(gameTime);

            if (StartTime > TimeSpan.Zero)
                Duration = gameTime.TotalGameTime - StartTime;

            Info.Position = Position + new Vector2(1, 1);

            //TODO draw the info with a different color for the player
            if (Source is Player)
                Info.Color = Color.Cyan;
            else
                Info.Color = new Color(255, 81, 89, 255);

            Position += Motion;
        }

        //TODO delete
        public virtual EntityAttack PerformAttack(GameTime gameTime, Color attackColor)
        {
            EntityAttack attack = Clone();
            attack.StartTime = gameTime.TotalGameTime;
            //attack.Offset();
            attack.Position = Source.Position + attack.AttackOffset;
            attack.DefaultColor = attackColor;
            attack.SpriteColor = attack.DefaultColor;
            attack.Motion = Motion;
            attack.Speed = Speed;
            attack.DamageModifier = DamageModifier;
            attack.ManaCost = ManaCost;

            return attack;
        }

        public virtual void SetUpAttack(GameTime gameTime, Color attackColor)
        {
            StartTime = gameTime.TotalGameTime;
            Offset();
            Position = Source.Position + AttackOffset;
            DefaultColor = attackColor;
            SpriteColor = DefaultColor;
            LastAttackTime = gameTime.TotalGameTime;
            Info.Text = string.Empty;
        }

        public bool IsOnCooldown(GameTime gameTime)
        {
            return ((gameTime.TotalGameTime - LastAttackTime).TotalMilliseconds < AttackCoolDownLength);
        }


        public virtual AttackData GetAttackData()
        {
            return new()
            {
                AttackLength = AttackLength,
                StartTime = StartTime,
                Duration = Duration,
                AttackOffset = AttackOffset,
                LastAttackTime = LastAttackTime,
                AttackCoolDownLength = AttackCoolDownLength,
                Speed = Speed,
                DamageModifier = DamageModifier,
                ManaCost = ManaCost,
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

        public bool AttackTimedOut()
        {
            if(Duration.TotalMilliseconds > AttackLength)
                return true;
            return false;
        }

        //TODO
        public void MoveToPosition(Vector2 target)
        {
            Motion = Vector2.Normalize(target - Position) * Speed;
            System.Diagnostics.Debug.WriteLine(Motion);
        }
    }
}
