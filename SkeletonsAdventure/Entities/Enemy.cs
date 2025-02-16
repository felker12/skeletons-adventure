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
        private Vector2 movement;
        private int x, y, x2, y2, walkDistance, detectionWidth, detectionHeight;
        public Rectangle detectionArea, attackArea;
        public bool IsElite { get; set; } = false;

        public MinMaxPair LevelRange { get; set; } = new(0, 0);
        public int ID { get; set; } = 0;

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
            Health = baseHealth + EntityLevel * 2;
            maxHealth = baseHealth + EntityLevel * 2;
            defence = baseDefence + EntityLevel * 2;
            attack = baseAttack + EntityLevel * 2;
            XP = baseXP + EntityLevel * 2;

            if(IsElite)
            {
                int healthBonus =(int)(Health * 1.5);
                int Bonus = (int)Math.Ceiling((double)defence / 4);

                Health += healthBonus;
                maxHealth += healthBonus;

                defence += Bonus;
                attack += Bonus;
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

            //Info.Text += "\nAttack = " + attack;
            //Info.Text += "\nDefence = " + defence;
            //Info.Text += "\nBase Attack = " + baseAttack;
            //Info.Text += "\nBase Defence = " + baseDefence;

            Motion = movement;

            int detectX = (int)Position.X - (detectionWidth - Width) /2;
            int detectY = (int)Position.Y - (detectionHeight - Height) / 2;
            detectionArea = new(detectX, detectY, detectionWidth, detectionHeight);

            detectX = (int)Position.X - Width;
            detectY = (int)Position.Y - Width;
            attackArea = new(detectX, detectY, Width * 3, Height + Width * 2);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
            spriteBatch.DrawRectangle(detectionArea, Color.Aqua, 1, 0); //TODO
            spriteBatch.DrawRectangle(attackArea, Color.Red, 1, 0); //TODO
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
            movement = Vector2.Zero;
            SetEnemyLevel(LevelRange);
            base.Respawn();
        }

        public void WalkInSquare()
        {
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
