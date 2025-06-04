using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Timers;
using RpgLibrary.DataClasses;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Attacks;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.ItemLoot;
using System;
using System.Collections.Generic;

namespace SkeletonsAdventure.Entities
{
    internal class EntityManager
    {
        public List<Entity> Entities { get; }
        public DroppedLootManager DroppedLootManager { get; }
        public Player Player { get; set; }
        public MinMaxPair EnemyLevelRange { get; set; } = new(0, 0); //used to set the level of enemies when they are created or respawned

        public EntityManager()
        {
            Entities = [];
            DroppedLootManager = new();
        }

        public void Add(Entity entity)
        {
            Entities.Add(entity);

            if (entity is Player player)
                Player = player;
        }
        public void Remove(Entity entity)
        {
            Entities.Remove(entity);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            DroppedLootManager.Draw(spriteBatch);

            foreach (Entity entity in Entities)
            {
                if(entity.IsDead == false)
                    entity.Draw(spriteBatch);
            }

            //draw the attacks after the entities so the attacks are always on top
            foreach (Entity entity in Entities)
                entity.AttackManager.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime, GameTime totalTimeInWorld)
        {
            foreach (var entity in Entities)
            {
                if (entity.IsDead == false)
                {
                    entity.Update(gameTime);

                    entity.AttackManager.CheckAttackHit(Entities, gameTime); //TODO: this might have to be changed to totalTimeInWorld instead later
                    AttackManager.ClearOldAttacks(Entities);

                    if (entity == Player)
                        PickUpLoot();

                    if (entity.Health < 1)
                    {
                        entity.EntityDied(totalTimeInWorld);
                        entity.IsDead = true;
                        DroppedLootManager.Add(entity.LootList, entity.Position);
                    }
                }
                else if (entity.IsDead && totalTimeInWorld.TotalGameTime - entity.lastDeathTime > new TimeSpan(0, 0, entity.RespawnTime))
                {
                    entity.Respawn();
                }

                List<EntityAttack> toRemove = [];
                foreach (Entity otherEntity in Entities)
                {
                    if (entity != otherEntity) //make sure we are not checking the same entity
                        foreach (EntityAttack atk in entity.AttacksHitBy)
                        {
                            if (atk.AttackTimedOut())
                                toRemove.Add(atk); //remove any timed out attacks
                            else if (otherEntity.IsDead && atk.Source == otherEntity)
                                toRemove.Add(atk); //remove any attacks that were caused by an enemy that is now dead
                        }
                }

                //remove the attacks that need to be removed
                foreach (EntityAttack atk in toRemove)
                {
                    entity.AttacksHitBy.Remove(atk);
                }
            }

            CheckIfEnemyDetectPlayer(gameTime);
            DroppedLootManager.Update();
        }

        public void RemoveAll()
        {
            Entities?.Clear();
        }

        public void PickUpLoot()
        {
            foreach(GameItem item in DroppedLootManager.Items)
            {
                if (Player.GetRectangle.Intersects(item.ItemRectangle) && Player.Backpack.AddItem(item) == true)
                    DroppedLootManager.ItemToRemove.Add(item);
            }
        }

        public EntityManagerData GetEnemyData()
        {
            EntityManagerData entityManagerData = new();

            foreach (Entity entity in Entities)
            {
                if (entity is Enemy)
                    entityManagerData.EntityData.Add(entity.GetEntityData());
            }

            return entityManagerData;
        }

        private void CheckIfEnemyDetectPlayer(GameTime gameTime)
        {
            foreach (Entity entity in Entities)
            {
                if (entity is Enemy enemy && enemy.IsDead == false)
                {
                    //if the player is close then go to the player
                    if (CheckForPlayer(enemy))
                    {
                        enemy.AutoAttack(Player, gameTime);
                    }
                    //if the enemy has attacked by an out of sight enemy then move to the point the enemy was at when  
                    else if(enemy.LastTimeAttacked.TotalMilliseconds != 0 
                        && gameTime.TotalGameTime.TotalMilliseconds - enemy.LastTimeAttacked.TotalMilliseconds < 6000
                        && enemy.CheckedLastAtackArea == false)
                    {
                        //if the enemy detects player on way then mark area checked so the enemy doesn't keep moving towards that point if the player gets away
                        if (CheckForPlayer(enemy)) 
                            enemy.CheckedLastAtackArea = true;

                        if (enemy.GetRectangle.Intersects(new((int)enemy.PositionLastAttackedFrom.X, (int)enemy.PositionLastAttackedFrom.Y, 1, 1)))
                        {
                            enemy.CheckedLastAtackArea = true;
                            if (CheckForPlayer(enemy))
                                enemy.AutoAttack(Player, gameTime);
                            else
                                enemy.WalkInSquare();
                        }
                        else
                            enemy.PathToPoint(enemy.PositionLastAttackedFrom);
                    }
                    else //if no player in sight do something
                        enemy.WalkInSquare();
                }
            }
        }

        public bool CheckForPlayer(Enemy enemy)
        {
            bool playerInRange = false;

            if (enemy.DetectionArea.Intersects(Player.GetRectangle))
            {
                playerInRange = true;
            }

            enemy.Motion = Vector2.Zero; //Stops any motion caused by another method

            //if the enemy detects the player then move towerds the player
            if (!enemy.GetRectangle.Intersects(Player.GetRectangle))
                enemy.PathToPoint(Player.Position);
            else
                enemy.FaceTarget(Player);

            return playerInRange;
        }




        public void CheckEntityBoundaryCollisions(TiledMap tiledMap, TiledMapTileLayer mapCollisionLayer)
        {
            foreach (Entity entity in Entities)
            {
                if(entity.CanMove)
                    CheckCollision(entity, tiledMap, mapCollisionLayer);

                foreach(EntityAttack entityAttack in entity.AttackManager.Attacks)
                {
                    if (entityAttack.CanMove)
                        CheckCollision(entityAttack, tiledMap, mapCollisionLayer);
                }
            }
        }

        private static void CheckCollision(AnimatedSprite entity, TiledMap tiledMap, TiledMapTileLayer mapCollisionLayer)
        {
            Vector2 pos = entity.Position;
            Rectangle rec = entity.GetRectangle;
            int width = tiledMap.TileWidth;
            int height = tiledMap.TileHeight;

            if (entity.Motion != Vector2.Zero)
            {
                Vector2 motion = entity.Motion;

                List<ushort> collisionPointsX = [(ushort)(pos.X / width), 
                    (ushort)((pos.X + rec.Width) / width),
                    (ushort)((pos.X + rec.Width/2) / width)];

                List<ushort> collisionPointsY = [(ushort)(pos.Y / height),
                    (ushort)((pos.Y + rec.Height) / height), 
                    (ushort)((pos.Y + rec.Height/2) / height)];

                pos += entity.Motion * entity.Speed * Game1.DeltaTime * Game1.BaseSpeedMultiplier;
                List<ushort> collisionPointsX2 = [(ushort)(pos.X / width),
                    (ushort)((pos.X + rec.Width) / width),
                    (ushort)((pos.X + rec.Width/2) / width)];

                List<ushort> collisionPointsY2 = [(ushort)(pos.Y / height),
                    (ushort)((pos.Y + rec.Height) / height),
                    (ushort)((pos.Y + rec.Height/2) / height)];

                foreach (ushort pointX in collisionPointsX)
                {
                    foreach (ushort pointY in collisionPointsY2)
                    {
                        if (mapCollisionLayer.TryGetTile(pointX, pointY, out TiledMapTile? tile))
                        {
                            if (!tile.Value.IsBlank)
                            {
                                motion.Y = 0;
                                break;
                            }
                        }
                    }
                }
                foreach (ushort pointX in collisionPointsX2)
                {
                    foreach (ushort pointY in collisionPointsY)
                    {
                        if (mapCollisionLayer.TryGetTile(pointX, pointY, out TiledMapTile? tile))
                        {
                            if (!tile.Value.IsBlank)
                            {
                                motion.X = 0;
                                break;
                            }
                        }
                    }
                }

                //apply the motion to the sprite if its not colliding
                entity.Motion = motion;
                entity.Position += entity.Motion * entity.Speed * Game1.DeltaTime * Game1.BaseSpeedMultiplier;
            }
        }
    }
}
