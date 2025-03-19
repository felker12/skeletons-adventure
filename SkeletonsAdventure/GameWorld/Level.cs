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
using SkeletonsAdventure.GameObjects;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.Engines;
using MonoGame.Extended;

namespace SkeletonsAdventure.GameWorld
{
    public class Level
    {
        public string Name { get; set; } = string.Empty;
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
        public Vector2 PlayerEndPosition { get; set; } //location of the exit so if the player comes back to the level this is where they will be placed
        public Vector2 PlayerRespawnPosition { get; set; }
        public ChestManager ChestManager { get; set; }
        public Menu ChestMenu { get; set; }
        public TiledMapObjectLayer EnterExitLayer { get; set; } = null;
        public LevelExit LevelExit { get; set; } = null;
        public LevelExit LevelEntrance { get; set; } = null;

        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _mapCollisionLayer, _mapSpawnerLayer;
        private readonly Dictionary<string, Enemy> Enemies = [];

        public List<Rectangle> Recs { get; set; } = []; //TODO

        public Level(GraphicsDevice graphics, TiledMap tiledMap, Dictionary<string, Enemy> Enemies, MinMaxPair enemyLevels)
        {
            TiledMap = tiledMap;
            _tiledMapRenderer = new(graphics);
            _tiledMapRenderer.LoadMap(TiledMap);
            _mapCollisionLayer = TiledMap.GetLayer<TiledMapTileLayer>("CollisionLayer");
            _mapSpawnerLayer = tiledMap.GetLayer<TiledMapTileLayer>("SpawnerLayer");
            ChestManager = new(tiledMap.GetLayer<TiledMapTileLayer>("ChestLayer"));
            EnterExitLayer = TiledMap.GetLayer<TiledMapObjectLayer>("EnterExitLayer");

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

            foreach(Rectangle rec in Recs) //TODO delete this 
            {
                spriteBatch.DrawRectangle(rec, Color.White, 1, 0);
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

            ChestManager.Update();//TODO is this needed?
            CheckIfPlayerNearChest();
            ChestMenu.Update(true, Camera.Transformation);

            if(LevelExit != null)
                CheckIfPlayerIsNearExit(LevelExit);

            if (LevelEntrance != null)
                CheckIfPlayerIsNearExit(LevelEntrance);
        }

        public void LoadLevelDataFromLevelData(LevelData levelData)
        {
            EntityManager.RemoveAll();

            EntityManager.Add(Player);
            EntityManager.DroppedLootManager.Items = GameManager.LoadGameItemsFromItemData(levelData.DroppedItemDatas);
            LoadEnemies(levelData.EntityManagerData);

            ChestManager.UpdateFromSave(levelData.Chests);
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
            Chest chestToOpen = null;
            foreach (Chest chest in ChestManager.Chests)
            {
                if (Player.GetRectangle.Intersects(chest.DetectionArea))
                {
                    if(chest.Loot.Loots.Count > 0)
                    {
                        chest.Info.Text = "Press R to open";

                        if (InputHandler.KeyReleased(Keys.R) ||
                            InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
                        {
                            chestToOpen = chest;
                        }
                    }
                    else
                        chest.Info.Text = "Chest Empty";

                    chest.Info.Visible = true;
                    count++;
                }
                else
                    chest.Info.Visible = false;
            }

            if (count < 1) //the player isn't near a chest anymore hide menu
                ChestMenu.Visible = false;
            else
            {
                //having the chestToOpen variable prevents the menu from not appearing if multiple chests are detected
                if (chestToOpen is not null) 
                    ChestOpened(chestToOpen);
            }
        }
        
        private void ChestOpened(Chest chest)
        {
            if (ChestMenu.Visible == false && chest.Info.Visible == true)
            {
                ChestMenu.Visible = true;
                ChestMenu.Buttons.Clear();

                Dictionary<string, Button> buttons = [];
                foreach (GameItem gameItem in chest.Loot.Loots)
                {
                    GameButton btn = new(GameManager.DefaultButtonTexture, GameManager.ToolTipFont);

                    #pragma warning disable IDE0039 // Use local function
                    EventHandler handler = (object sender, EventArgs e) =>
                    {
                        btn.Visible = false;
                        Player.Backpack.AddItem(gameItem);
                        chest.Loot.Remove(gameItem);
                    };
                    #pragma warning restore IDE0039 // Use local function
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

        private bool CheckIfPlayerIsNearLevelExit(LevelExit exit)
        {
            bool nearExit = false;

            if (exit.ExitArea.Intersects(Player.GetRectangle))
                nearExit = true;

            return nearExit;
        }

        public void CheckIfPlayerIsNearExit(LevelExit exit)
        {
            if(CheckIfPlayerIsNearLevelExit(exit))
            {
                Player.Info.Text += "\n" + CheckIfPlayerIsNearLevelExit(LevelExit); //TODO delete this

                if (InputHandler.KeyReleased(Keys.R) ||
                            InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
                {
                    //TODO change location when have better method to determine where the player will go
                    World.SetCurrentLevel(exit.NextLevel, exit.NextLevel.PlayerStartPosition); 
                }
            }
        }
    }
}
