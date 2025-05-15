using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using System;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.ItemClasses;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Attacks;
using System.Security.Cryptography.X509Certificates;

namespace SkeletonsAdventure.Entities
{
    public class EntityManager
    {
        public List<Entity> Entities { get; }
        public DroppedLootManager DroppedLootManager { get; }
        public Player Player { get; set; }

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
                    entity.AttackManager.ClearOldAttacks(Entities);

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
                    if (enemy.DetectionArea.Intersects(Player.GetRectangle))
                    {
                        enemy.Motion = Vector2.Zero; //Stops any motion caused by another method

                        //if the enemy detects the player then move towerds the player
                        if (!enemy.GetRectangle.Intersects(Player.GetRectangle))
                            enemy.PathToPoint(Player);
                        else
                            enemy.FaceTarget(Player);

                        //attack the player if the player is close enough
                        if (enemy.AttackArea.Intersects(Player.GetRectangle))
                        {
                            //TODO: add logic for other types of attacks (probably move this logic to the enemy)
                            //so there can be attacks based on what the enemy is
                            enemy.PerformAttack(gameTime, enemy.EntityAttack); 
                        }
                    }
                    else //if no player in sight do something
                        enemy.WalkInSquare();
                }
            }
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
                //Fixes animation bug where the player walks wrong way when colliding with a wall to the north or south
                if (entity is Entity)
                {
                    //entity.UpdateCurrentAnimation(motion);
                }

                //apply the motion to the sprite if its not colliding
                entity.Motion = motion;
                entity.Position += entity.Motion * entity.Speed * Game1.DeltaTime * Game1.BaseSpeedMultiplier;

            }
        }
    }
}
