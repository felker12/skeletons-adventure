using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using System;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.ItemClasses;
using RpgLibrary.EntityClasses;
using SkeletonsAdventure.Attacks;

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

                    entity.AttackManager.CheckAttackHit(Entities);
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
                CheckCollision(entity, tiledMap, mapCollisionLayer);
        }

        private static void CheckCollision(Entity entity, TiledMap tiledMap, TiledMapTileLayer mapCollisionLayer)
        {
            Vector2 pos = entity.Position;
            Rectangle rec = entity.GetRectangle;

            if (entity.Motion != Vector2.Zero)
            {
                pos += entity.Motion;
                entity.IsCollidingBoundary = false;

                ushort tx = (ushort)(pos.X / tiledMap.TileWidth),
                    ty = (ushort)(pos.Y / tiledMap.TileHeight),
                    tx2 = (ushort)((pos.X + rec.Width) / tiledMap.TileWidth),
                    ty2 = (ushort)((pos.Y + rec.Height) / tiledMap.TileHeight),
                    //tx3 = (ushort)((pos.X + rec.Height/2) / tiledMap.TileHeight),
                    ty3 = (ushort)((pos.Y + rec.Height/2) / tiledMap.TileHeight);


                //Check if any corner or half way point of the entity is colliding with the collision layer
                if (mapCollisionLayer.TryGetTile(tx, ty, out TiledMapTile? tile))
                {
                    if (!tile.Value.IsBlank)
                        entity.IsCollidingBoundary = true;

                    else if (entity.IsCollidingBoundary == false)
                    {
                        if (mapCollisionLayer.TryGetTile(tx2, ty2, out tile) && (!tile.Value.IsBlank))
                            entity.IsCollidingBoundary = true;
                        else if (entity.IsCollidingBoundary == false)
                            if (mapCollisionLayer.TryGetTile(tx, ty2, out tile) && (!tile.Value.IsBlank))
                                entity.IsCollidingBoundary = true;
                            else if (entity.IsCollidingBoundary == false)
                                if (mapCollisionLayer.TryGetTile(tx2, ty, out tile) && (!tile.Value.IsBlank))
                                    entity.IsCollidingBoundary = true;
                                else if (entity.IsCollidingBoundary == false)
                                    if (mapCollisionLayer.TryGetTile(tx, ty3, out tile) && (!tile.Value.IsBlank))
                                        entity.IsCollidingBoundary = true;
                                    else if (entity.IsCollidingBoundary == false)
                                        if (mapCollisionLayer.TryGetTile(tx2, ty3, out tile) && (!tile.Value.IsBlank))
                                            entity.IsCollidingBoundary = true;
                    }
                }
                if (entity.IsCollidingBoundary == false)
                    entity.Position = pos;
            }
        }
    }
}
