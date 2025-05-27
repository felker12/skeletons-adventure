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
using MonoGame.Extended.Graphics.Effects;
using SkeletonsAdventure.Quests;

namespace SkeletonsAdventure.GameWorld
{
    internal class Level
    {
        public Label title;
        public int Width { get; set; }
        public int Height { get; set; }
        public string Name { get; set; } = string.Empty;
        protected ControlManager ControlManager { get; set; }
        public Player Player { get; set; }
        public Camera Camera { get; set; }
        public EntityManager EntityManager { get; set; }
        public TiledMap TiledMap { get; private set; }
        public MinMaxPair EnemyLevels { get; set; }
        private GraphicsDevice GraphicsDevice { get; }
        public GameTime TotalTimeInWorld { get; set; }
        public Vector2 PlayerStartPosition { get; set; } = new(80, 80);
        public Vector2 PlayerEndPosition { get; set; } = new(80, 80);//location of the exit so if the player comes back to the level this is where they will be placed
        public Vector2 PlayerRespawnPosition { get; set; } = new(80, 80);
        public ChestManager ChestManager { get; set; }
        public PopUpBox ChestMenu { get; set; }
        public TiledMapObjectLayer EnterExitLayer { get; set; } = null;
        public TiledMapObjectLayer InteractableObjectLayer { get; set; } = null;
        public LevelExit LevelExit { get; set; } = null;
        public LevelExit LevelEntrance { get; set; } = null;
        internal InteractableObjectManager InteractableObjectManager { get; set; } = new();

        private readonly TiledMapRenderer _tiledMapRenderer;
        private readonly TiledMapTileLayer _mapCollisionLayer, _mapSpawnerLayer;
        private readonly Dictionary<string, Enemy> Enemies = [];

        public List<Rectangle> EnterExitLayerObjectRectangles { get; set; } = []; //TODO used to temporarily see where hitboxes are for exits

        public Level(GraphicsDevice graphics, TiledMap tiledMap, Dictionary<string, Enemy> Enemies, MinMaxPair enemyLevels)
        {
            GraphicsDevice = graphics;
            this.Enemies = Enemies;
            TiledMap = tiledMap;
            _tiledMapRenderer = new(graphics);
            _tiledMapRenderer.LoadMap(TiledMap);
            _mapCollisionLayer = TiledMap.GetLayer<TiledMapTileLayer>("CollisionLayer");
            _mapSpawnerLayer = tiledMap.GetLayer<TiledMapTileLayer>("SpawnerLayer");
            ChestManager = new(tiledMap.GetLayer<TiledMapTileLayer>("ChestLayer"));
            EnterExitLayer = TiledMap.GetLayer<TiledMapObjectLayer>("EnterExitLayer");
            InteractableObjectLayer = TiledMap.GetLayer<TiledMapObjectLayer>("InteractableObjectLayerObjects");

            Width = tiledMap.WidthInPixels;
            Height = tiledMap.HeightInPixels;
            Name = tiledMap.Name[11..]; //trim "TiledFiles/" from the tiledmap name to use as the level name

            ChestManager.Chests = ChestManager.GetChestsFromTiledMapTileLayer(GameManager.ChestsClone["BasicChest"]);

            EntityManager = new();
            EnemyLevels = enemyLevels;
            AddEnemys();

            LoadInteractableObjects();

            //TODO controls are temporary and used for debugging
            title = new Label
            {
                Text = Name,
                Color = Color.Orange
            };
            title.Position = new Vector2(Game1.ScreenWidth / 2 - (title.SpriteFont.MeasureString(title.Text)).X / 2, 20);

            ControlManager = new(ControlManager.SpriteFont)
            {
                title
            };
            //=================================================================

            ChestMenu = new()
            {
                Visible = false,
                Texture = GameManager.PopUpBoxTexture
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
            InteractableObjectManager.Draw(spriteBatch);

            if (ChestMenu.Visible)
                ChestMenu.Draw(spriteBatch);

            foreach(Rectangle rec in EnterExitLayerObjectRectangles) //TODO delete this 
                spriteBatch.DrawRectangle(rec, Color.White, 1, 0); //used to see where the hitboxes are for the exits

            if (LevelEntrance is not null && LevelEntrance.ExitTextVisible)
                spriteBatch.DrawString(GameManager.Arial12, LevelEntrance.ExitText, LevelEntrance.ExitPosition, Color.White);
            if (LevelExit is not null && LevelExit.ExitTextVisible)
                spriteBatch.DrawString(GameManager.Arial12, LevelExit.ExitText, LevelExit.ExitPosition, Color.White);

            spriteBatch.End();
        }

        public void Update(GameTime gameTime, GameTime totalTimeInWorld) 
        {
            EntityManager.Update(gameTime, totalTimeInWorld);
            EntityManager.CheckEntityBoundaryCollisions(TiledMap, _mapCollisionLayer);

            Player = EntityManager.Player; //TODO look into this
            Camera.Update(Player.Position);

            _tiledMapRenderer.Update(gameTime);

            TotalTimeInWorld = totalTimeInWorld;

            CheckIfPlayerNearChest();
            ChestMenu.Update(true, Camera.Transformation);

            InteractableObjectManager.Update(gameTime, Player);

            if (LevelExit != null)
                CheckIfPlayerIsNearExit(LevelExit, LevelExit.NextLevel.PlayerStartPosition);

            if (LevelEntrance != null)
                CheckIfPlayerIsNearExit(LevelEntrance, LevelEntrance.NextLevel.PlayerEndPosition);
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
                        en.SetEnemyLevel(entityData.entityLevel);
                        en.UpdateEntityData(entityData);
                        en.LootList.Add(GameManager.LoadGameItemsFromItemData(entityData.Items));

                        EntityManager.Add(en);
                    }
                }
            }
        }

        private void LoadInteractableObjects()
        {
            if (InteractableObjectLayer != null) //TODO 
            {
                foreach (TiledMapObject obj in InteractableObjectLayer.Objects)
                {
                    if (obj.Properties.TryGetValue("TypeOfObject", out TiledMapPropertyValue value))
                    {
                        if (value == "Quest")
                        {
                            if (obj.Properties.TryGetValue("Quests", out TiledMapPropertyValue quests))
                            {
                                System.Diagnostics.Debug.WriteLine($"Quests: {quests}");

                                QuestNode questNode = new(obj);
                                string[] Quests = quests.ToString().Split(',', StringSplitOptions.TrimEntries);

                                foreach (string questName in Quests)
                                {
                                    System.Diagnostics.Debug.WriteLine($"Quest: {questName}");
                                    if (GameManager.QuestsClone.TryGetValue(questName, out Quest quest))
                                    {
                                        questNode.Quests.Add(quest.Clone()); //Clone the quest to prevent modifying the original quest
                                        continue;
                                    }
                                    else
                                        System.Diagnostics.Debug.WriteLine($"Quest {questName} not found in GameManager.QuestsClone");
                                }

                                InteractableObjectManager.Add(questNode);
                                break;
                            }
                        }
                        else if (value == "Resource")
                        {
                            InteractableObjectManager.Add(new ResourceNode(obj));
                            break;
                        }
                    }

                    InteractableObjectManager.Add(new InteractableObject(obj));
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
                    Button btn = new(GameManager.DefaultButtonTexture, GameManager.Arial10);

                    #pragma warning disable IDE0039 // Use local function
                    EventHandler handler = (object sender, EventArgs e) =>
                    {
                        if(Player.Backpack.AddItem(gameItem))
                        {
                            btn.Visible = false;
                            chest.Loot.Remove(gameItem);
                        }
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

        public void CheckIfPlayerIsNearExit(LevelExit exit, Vector2 targetPosition)
        {
            if (exit.ExitArea.Intersects(Player.GetRectangle))
            {
                exit.ExitTextVisible = true;

                if (InputHandler.KeyReleased(Keys.R) 
                    || InputHandler.ButtonDown(Buttons.A, PlayerIndex.One))
                {
                    World.SetCurrentLevel(exit.NextLevel, targetPosition);
                }
            }
            else
                exit.ExitTextVisible = false;
        }
    }
}
