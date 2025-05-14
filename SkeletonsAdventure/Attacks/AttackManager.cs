using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.Engines;

namespace SkeletonsAdventure.Attacks
{
    public class AttackManager(Entity entity)
    {
        public Entity SourceEntity { get; set; } = entity;
        public List<EntityAttack> Attacks { get; private set; } = [];
        public TimeSpan LastAttackTime { get; set; } = new(0, 0, 0, 0);

        private bool _attacked = false;

        public List<EntityAttack> ToRemove = [];

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var attack in Attacks)
                attack.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            ToRemove = [];
            foreach (var attack in Attacks)
            {
                attack.Update(gameTime);
                if (attack.AttackTimedOut())
                {
                    ToRemove.Add(attack);
                }
            }

            foreach (var atk in ToRemove)
            {
                RemoveAttack(atk);
            }

            if (_attacked)
            {
                LastAttackTime = gameTime.TotalGameTime;
                _attacked = false;

                if(SourceEntity is Player player)
                {
                    player.AimVisible = false;
                }
            }
        }

        public void AddAttack(EntityAttack atk, GameTime gameTime)
        {
            Attacks.Add(atk);
            LastAttackTime = gameTime.TotalGameTime;
            //atk.LastAttackTime = gameTime.TotalGameTime;
            _attacked = true;
        }

        public void RemoveAttack(EntityAttack atk)
        {
            Attacks.Remove(atk);
        }

        public void ClearAttacks()
        {
            Attacks.Clear();
        }
        
        public void CheckAttackHit(List<Entity> entities, GameTime gameTime)
        {
            //check to see if the attack hit an entity
            foreach (var attack in Attacks)
            {
                foreach (var entity in entities)
                {
                    if(entity.IsDead)
                        continue;//prevents attacking dead enemies
                    if (entity.AttacksHitBy.Contains(attack) is false) //prevents an attack from hitting an opponent multiple times
                    {
                        if (SourceEntity != entity) //makes sure the entity cannot attack itself
                        {
                            if (entity is Enemy && SourceEntity is Enemy) { } //This line prevents enemies from attacking other enemies
                            else if (entity.GetRectangle.Intersects(attack.GetRectangle))
                            {
                                if (entity.AttacksHitBy.Contains(attack) is false)
                                {
                                    entity.AttacksHitBy.Add(attack);
                                }

                                int dmg = (int)(DamageEngine.CalculateDamage(SourceEntity, entity) * attack.DamageModifier);
                                attack.Info.Text += dmg;
                                entity.Health -= dmg;
                                entity.LastTimeAttacked = gameTime.TotalGameTime;

                                if (entity.Health < 1 && SourceEntity is Player player) //If the entity dies give xp to the player that killed it
                                {
                                    player.GainXp(entity.XP);
                                }
                            }
                        }
                    }
                }
            }
        }

        public void ClearOldAttacks(List<Entity> entities)
        {
            foreach (var attack in Attacks)
            {
                foreach (var entity in entities)
                {
                    foreach (var atk in entity.AttacksHitBy)
                    {
                        if (atk.AttackTimedOut())
                        {
                            entity.AttacksHitBy.Remove(atk);
                            break;
                        }
                    }
                }
            }
        }
    }
}
