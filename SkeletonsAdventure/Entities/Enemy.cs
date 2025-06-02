using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RpgLibrary.DataClasses;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Attacks;
using System;

namespace SkeletonsAdventure.Entities
{
    internal class Enemy : Entity
    {
        private int x, y, x2, y2, walkDistance, detectionWidth, detectionHeight;

        public bool IsElite { get; set; } = false;
        private MinMaxPair LevelRange { get; set; } = new(0, 0);
        public bool CheckedLastAtackArea { get; set; } = false;
        public Rectangle DetectionArea => new((int)Position.X - (detectionWidth - Width) / 2,
            (int)Position.Y - (detectionHeight - Height) / 2, detectionWidth, detectionHeight);
        public Rectangle AttackArea => new((int)Position.X - Width,
            (int)Position.Y - Width, Width * 3, Height + Width * 2);

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
            Level = levels.GetRandomNumberInRange();
            EnemyStatAdjustmentForLevel();
        }

        public void SetEnemyLevel(int level) //TODO
        {
            Level = level;
            EnemyStatAdjustmentForLevel();
        }

        private void EnemyStatAdjustmentForLevel()
        {
            MaxHealth = baseHealth + Level * 2;
            Health = MaxHealth;
            Defence = baseDefence + Level * 2;
            Attack = baseAttack + Level * 2;
            XP = baseXP + Level * 2;

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

            UpdateEntityData(GetEntityData()); //TODO: check if this is needed
        }
        public override void Update(GameTime gameTime)
        {
            //Hide health bar when entity hasn't been attacked in a while
            if (LastTimeAttacked.TotalMilliseconds != 0 && gameTime.TotalGameTime.TotalMilliseconds - LastTimeAttacked.TotalMilliseconds < 6000)
                HealthBarVisible = true;
            else
                HealthBarVisible = false;

            base.Update(gameTime);

            //TODO delete this
            Info.Text += "\nXP = " + XP;
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

        public override void GetHitByAttack(EntityAttack attack, GameTime gameTime)
        {
            base.GetHitByAttack(attack, gameTime);

            if (gameTime.TotalGameTime.TotalMilliseconds - LastTimeAttacked.TotalMilliseconds > 6000)
            {
                ResetSquare();
                CheckedLastAtackArea = false;
            }

            if (Health < 1 && attack.Source is Player player) //If the entity dies give xp to the player that killed it
            {
                player.GainXp(XP);
            }
        }

        public override void Respawn()
        { 
            ResetSquare();
            Motion = Vector2.Zero;
            SetEnemyLevel(LevelRange);
            base.Respawn();
        }

        public void AutoAttack(Player player, GameTime gameTime)
        {
            //attack the player if the player is close enough
            if (AttackArea.Intersects(player.GetRectangle))
            {
                //TODO: add logic for other types of attacks (probably move this logic to the enemy)
                //so there can be attacks based on what the enemy is
                PerformAttack(gameTime, EntityAttack);
            }
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
