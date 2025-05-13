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

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var attack in Attacks)
                attack.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            List<EntityAttack> toRemove = [];
            foreach (var attack in Attacks)
            {
                attack.Update(gameTime);
                if (attack.Duration.TotalMilliseconds > attack.AttackLength)
                    toRemove.Add(attack);
            }

            foreach (var atk in toRemove)
            {
                RemoveAttack(atk);
            }

            if (_attacked)
            {
                LastAttackTime = gameTime.TotalGameTime;
                _attacked = false;
            }
        }

        public void AddAttack(EntityAttack atk)
        {
            Attacks.Add(atk);
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
        
        public void CheckAttackHit(List<Entity> entities)
        {
            //check to see if the attack hit an entity
            foreach (var attack in Attacks)
            {
                foreach (var entity in entities)
                {
                    if (SourceEntity != entity) //makes sure the entity cannot attack itself
                    {
                        if (entity is Enemy && SourceEntity is Enemy) { } //This line prevents enemies from attacking other enemies
                        else if (entity.GetRectangle.Intersects(attack.GetRectangle))
                        {
                            entity.CollisionCount = 1;
                            if (attack.CanHit == true)
                            {
                                attack.HasHit = true;
                                int dmg = (int)(DamageEngine.CalculateDamage(SourceEntity, entity) * attack.DamageModifier);
                                attack.Info.Text += dmg;
                                entity.Health -= dmg;
                                if (entity.Health < 1 && SourceEntity is Player player) //If the etity dies give xp to the player that killed it
                                {
                                    player.GainXp(entity.XP);
                                }
                            }
                        }
                    }
                }
            }

            foreach (var attack in Attacks) //prevents the same attack from hitting an enemy multiple times
            {
                if (attack.HasHit)
                { 
                    attack.CanHit = false; 
                }
            }
        }
    }
}
