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
using RpgLibrary.GameObjectClasses;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.Engines;

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
        public Menu ChestMenu { get; set; }

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

            //ChestManager.Chests = ChestManager.GetChestsFromTiledMapTileLayer(new Chest() { ID = 8 }); //TODO
            ChestManager.Chests = ChestManager.GetChestsFromTiledMapTileLayer(GameManager.GetChestsClone()["BasicChest"]);

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

            ChestMenu = new()
            {
                Visible = false,
                Texture = GameManager.GamePopUpBoxTexture
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

            if(ChestMenu.Visible)
            {
                ChestMenu.Draw(spriteBatch);
            }

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
            CheckIfPlayerNearChest();
            ChestMenu.Update(gameTime);
            
        }

        public void LoadLevelDataFromLevelData(LevelData levelData)
        {
            EntityManager.RemoveAll();

            EntityManager.Add(Player);
            LoadEnemies(levelData.EntityManagerData);
            LoadDroppedItemsFromLevelData(levelData.DroppedItemDatas);
            LoadChestFromLevelData(levelData.Chests);
        }

        public LevelData GetLevelData()
        {
            return new()
            {
                MinMaxPair = EnemyLevels,
                EntityManagerData = EntityManager.GetEnemyData(),
                DroppedItemDatas = EntityManager.DroppedLootManager.GetDroppedItemData(),
                Chests = ChestManager.GetChestDatas()
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
                        en.LootList.Add(GameManager.LoadGameItemsFromItemData(entityData.Items));

                        EntityManager.Add(en);
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

        public void LoadChestFromLevelData(List<ChestData> chests)
        {
            //TODO
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
        }

        private void CheckIfPlayerNearChest()
        {
            int count = 0;
            foreach (Chest chest in ChestManager.Chests)
            {
                if (Player.GetRectangle.Intersects(chest.DetectionArea))
                {
                    chest.Info.Visible = true;
                    count++;

                    if (InputHandler.KeyReleased(Keys.R) ||
                        InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
                    {
                        ChestOpened(chest);
                    }
                }
                else
                {
                    chest.Info.Visible = false;
                }
            }

            if(count < 1) //the player isn't near a chest anymore hide menu
            {
                ChestMenu.Visible = false;
            }
        }
        
        private void ChestOpened(Chest chest)
        {
            if (ChestMenu.Visible == false)
            {
                ChestMenu.Visible = true;
                ChestMenu.Buttons.Clear();

                Dictionary<string, Button> buttons = [];
                foreach (GameItem gameItem in chest.Loot.Loots)
                {
                    Button btn = new(GameManager.DefaultButtonTexture, GameManager.ToolTipFont);

                    EventHandler handler = (object sender, EventArgs e) =>
                    {
                        btn.Visible = false;
                        Player.Backpack.AddItem(gameItem);
                        chest.Loot.Remove(gameItem);
                    };
                    btn.Click += (object sender, EventArgs e) =>
                    {
                        handler?.Invoke(sender, e);
                        btn.Click -= handler;
                    };

                    buttons.Add(gameItem.Name, btn);
                }

                ChestMenu.AddButtons(buttons);

                foreach (Button button in ChestMenu.Buttons)
                {
                    button.Visible = true;
                }
            }
            else
                ChestMenu.Visible = false;

            ChestMenu.Position = chest.Position;

        }
    }
}
