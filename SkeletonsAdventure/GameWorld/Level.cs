using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.EntitySpawners;
using SkeletonsAdventure.Controls;
using System.Collections.Generic;
using RpgLibrary.DataClasses;
using RpgLibrary.EntityClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.ItemClasses;
using System;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.GameObjects;

namespace SkeletonsAdventure.GameWorld
{
    public class Level
    {
        public Label title;
        protected ControlManager ControlManager { get; set; }
        public Player Player { get; set; }
        public Camera Camera { get; set; }
        public EntityManager EntityManager { get; set; }
        public TiledMap TiledMap { get; private set; }
        public MinMaxPair EnemyLevels { get; set; }
        private GraphicsDevice GraphicsDevice { get; }
        public GameTime TotalTimeInWorld { get; set; }
        public Vector2 PlayerStartPosition { get; set; }
        public Vector2 PlayerRespawnPosition { get; set; }
        public ChestManager ChestManager { get; set; }


        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _mapCollisionLayer, _mapSpawnerLayer;
        private readonly Dictionary<string, Enemy> Enemies = [];

        public Level(GraphicsDevice graphics, TiledMap tiledMap, Dictionary<string, Enemy> Enemies, MinMaxPair enemyLevels)
        {
            TiledMap = tiledMap;
            _tiledMapRenderer = new(graphics);
            _tiledMapRenderer.LoadMap(TiledMap);
            _mapCollisionLayer = TiledMap.GetLayer<TiledMapTileLayer>("CollisionLayer");
            _mapSpawnerLayer = tiledMap.GetLayer<TiledMapTileLayer>("SpawnerLayer");
            ChestManager = new(tiledMap.GetLayer<TiledMapTileLayer>("ChestLayer"));

            ChestManager.Chests = ChestManager.GetChestsFromTiledMapTileLayer(new Chest() { ID = 8 });

            this.Enemies = Enemies;

            EntityManager = new();
            EnemyLevels = enemyLevels;
            AddEnemys();

            GraphicsDevice = graphics;

            //TODO controls are temporary and used for debugging
            title = new Label
            {
                Text = "Game Screen",
                Color = Color.Orange
            };
            title.Position = new Vector2(Game1.ScreenWidth / 2 - (title.SpriteFont.MeasureString(title.Text)).X / 2, 20);

            ControlManager = new(ControlManager.SpriteFont)
            {
                title
            };
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.SamplerStates[0] = SamplerState.PointClamp; //prevents wierd yellow lines between tiles
            _tiledMapRenderer.Draw(Camera.Transformation);

            spriteBatch.Begin(
                SpriteSortMode.Immediate,
                BlendState.AlphaBlend,
                SamplerState.PointClamp,
                null,
                null,
                null,
                Camera.Transformation);

            ChestManager.Draw(spriteBatch);
            EntityManager.Draw(spriteBatch);
            ControlManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, GameTime totalTimeInWorld) 
        {
            EntityManager.Update(gameTime, totalTimeInWorld);
            EntityManager.CheckEntityBoundaryCollisions(TiledMap, _mapCollisionLayer);

            Player = EntityManager.Player;
            Camera.Update(Player.Position);

            _tiledMapRenderer.Update(gameTime);

            TotalTimeInWorld = totalTimeInWorld;

            ChestManager.Update();
        }

        public void LoadLevelDataFromLevelData(LevelData levelData)
        {
            EntityManager.RemoveAll();

            EntityManager.Add(Player);
            LoadEnemies(levelData.EntityManagerData);
            LoadDroppedItemsFromLevelData(levelData.DroppedItemDatas);
        }

        public LevelData GetLevelData()
        {
            return new()
            {
                MinMaxPair = EnemyLevels,
                EntityManagerData = EntityManager.GetEnemyData(),
                DroppedItemDatas = EntityManager.DroppedLootManager.GetDroppedItemData(),
            };
        }

        private void LoadEnemies(EntityManagerData entityManagerData)
        {
            foreach(Enemy enemy in Enemies.Values)
            {
                foreach(EntityData entityData in entityManagerData.EntityData)
                {
                    if(enemy.GetType().Name == entityData.type)
                    {
                        dynamic en = Activator.CreateInstance(enemy.GetType(), entityData);
                        en.SetEnemyLevel(EnemyLevels);
                        en.UpdateEntityData(entityData);
                        LoadEnemyGameItemsFromGameData(entityData.Items, en);

                        EntityManager.Add(en);
                    }
                }
            }
        }

        public void LoadEnemyGameItemsFromGameData(List<ItemData> itemDatas, Enemy enemy)
        {
            GameItem temp;

            foreach (ItemData item in itemDatas)
            {
                foreach (GameItem gameItem in GameManager.GetItemsClone().Values)
                {
                    if (item.Name == gameItem.BaseItem.Name)
                    {
                        temp = gameItem.Clone();
                        temp.Quantity = item.Quantity;
                        temp.BaseItem.Quantity = item.Quantity;

                        enemy.LootList.Add(temp);
                    }
                }
            }
        }

        public void LoadDroppedItemsFromLevelData(List<ItemData> items)
        {
            List<GameItem> gameItems = GameManager.LoadGameItemsFromItemData(items); 

            foreach (GameItem gameItem in gameItems)
            {
                EntityManager.DroppedLootManager.Items.Add(gameItem);
            }

        }

        private void AddEnemys()
        {
            Spawner spawner = new(_mapSpawnerLayer);

            foreach (Enemy enemy in Enemies.Values)
            {
                foreach (Enemy spawnerEnemy in spawner.CreateEnemiesForSpawners(enemy))
                {
                    spawnerEnemy.SetEnemyLevel(EnemyLevels);
                    EntityManager.Add(spawnerEnemy);
                }
            }

            //TODO this is just for testing
            EliteSkeleton eliteSkeleton = (EliteSkeleton)Enemies["Elite Skeleton"].Clone();
            eliteSkeleton.Position = new Vector2(500,500);
            eliteSkeleton.LevelRange = EnemyLevels;
            eliteSkeleton.SetEnemyLevel(EnemyLevels);
            //EntityManager.Add(eliteSkeleton);

            Spider spider = (Spider)Enemies["Spider"];
            spider.Position = new Vector2(300,300);
            spider.DefaultColor = Color.Orange;
            spider.SpriteColor = Color.Orange;
            //EntityManager.Add(spider);

            Skeleton skeleton = (Skeleton)Enemies["Skeleton"];

            EliteSkeleton skeleton1 = new(skeleton.GetEntityData())
            {
                Position = new Vector2(100, 100),
                LevelRange = EnemyLevels
            };
            skeleton1.SetEnemyLevel(76);
            skeleton1.LootList = skeleton.LootList;
            //EntityManager.Add(skeleton1);
        }
    }
}
