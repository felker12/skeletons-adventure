using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using MonoGame.Extended;
using RpgLibrary.EntityClasses;
using RpgLibrary.DataClasses;
using System;

namespace SkeletonsAdventure.Entities
{
    public class Enemy : Entity
    {
        private int x, y, x2, y2, walkDistance, detectionWidth, detectionHeight;
        public Rectangle DetectionArea, AttackArea;
        public bool IsElite { get; set; } = false;

        private MinMaxPair LevelRange { get; set; } = new(0, 0);

        public Enemy(EntityData entityData) : base(entityData)
        {
            Initialize();
        }

        private void Initialize()
        {
            ResetSquare();
            walkDistance = 200;
            detectionWidth = 300;
            detectionHeight = 300;
        }

        public void SetEnemyLevel(MinMaxPair levels)
        {
            LevelRange = levels;
            EntityLevel = levels.GetRandomNumberInRange();
            EnemyStatAdjustmentForLevel();
        }

        public void SetEnemyLevel(int level) //TODO
        {
            EntityLevel = level;
            EnemyStatAdjustmentForLevel();
        }

        private void EnemyStatAdjustmentForLevel()
        {
            MaxHealth = baseHealth + EntityLevel * 2;
            Health = MaxHealth;
            Defence = baseDefence + EntityLevel * 2;
            Attack = baseAttack + EntityLevel * 2;
            XP = baseXP + EntityLevel * 2;

            if(IsElite)
            {
                int healthBonus =(int)(Health * 1.5);
                int Bonus = (int)Math.Ceiling((double)Defence / 4);

                Health += healthBonus;
                MaxHealth += healthBonus;

                Defence += Bonus;
                Attack += Bonus;
                DefaultColor = new Color(Color.Black, 255);
                SpriteColor = DefaultColor;
                XP *= 2;
            }

        }
        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
            //TODO delete this
            Info.Text += "\nXP = " + XP;

            int detectX = (int)Position.X - (detectionWidth - Width) /2;
            int detectY = (int)Position.Y - (detectionHeight - Height) / 2;
            DetectionArea = new(detectX, detectY, detectionWidth, detectionHeight);

            detectX = (int)Position.X - Width;
            detectY = (int)Position.Y - Width;
            AttackArea = new(detectX, detectY, Width * 3, Height + Width * 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            //spriteBatch.DrawRectangle(DetectionArea, Color.Aqua, 1, 0); //TODO
            //spriteBatch.DrawRectangle(AttackArea, Color.Red, 1, 0); //TODO
        }

        public override Enemy Clone()
        {
            Enemy enemy = new(GetEntityData())
            {
                Position = Position,
                LootList = LootList
            };
            return enemy;
        }

        public override void Respawn()
        { 
            ResetSquare();
            Motion = Vector2.Zero;
            SetEnemyLevel(LevelRange);
            base.Respawn();
        }

        public void WalkInSquare()
        {
            Vector2 movement = Vector2.Zero;

            if (x < walkDistance)
            {
                movement.Y = 0;
                movement.X = 1;
                x++;
            }
            else if (x == walkDistance)
            {
                if (y < walkDistance)
                {
                    movement.X = 0;
                    movement.Y = 1;
                    y++;
                }
                else
                {
                    if (x2 == walkDistance)
                    {
                        movement.X = 0;
                        movement.Y = -1;
                        y2++;

                        if (y2 == walkDistance)
                            ResetSquare();
                    }
                    else
                    {
                        movement.X = -1;
                        movement.Y = 0;
                        x2++;
                    }
                }
            }

            Motion = movement;
        }

        public void ResetSquare()
        {
            x = 0;
            y = 0;
            x2 = 0;
            y2 = 0;
        }
    }
}
