using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Entities;

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
                    attack.AttackVisible = false;
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
            if (atk is ShootingAttack shootingAtk)
                shootingAtk.PathPoints = [];

            atk.InitialMotion = atk.Motion;

            Attacks.Add(atk);
            LastAttackTime = gameTime.TotalGameTime;
            _attacked = true;
        }

        public void RemoveAttack(EntityAttack atk)
        {
            atk.Position = Vector2.Zero;
            atk.AttackVisible = true;
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
                if(attack.AttackVisible)
                {
                    foreach (var entity in entities)
                    {
                        if (entity.IsDead)
                            continue;//prevents attacking dead enemies
                        if (entity.AttacksHitBy.Contains(attack) is false) //prevents an attack from hitting an opponent multiple times
                        {
                            if (SourceEntity != entity) //makes sure the entity cannot attack itself
                            {
                                if (entity is Enemy && SourceEntity is Enemy) { } //This line prevents enemies from attacking other enemies
                                else if (entity.GetRectangle.Intersects(attack.DamageHitBox))
                                {
                                    entity.GetHitByAttack(attack, gameTime);
                                }
                            }
                        }
                    }
                }
                
            }
        }

        public static void ClearOldAttacks(List<Entity> entities)
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
