﻿using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using RpgLibrary.DataClasses;
using RpgLibrary.ItemClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.ItemClasses;

namespace SkeletonsAdventure.GameWorld
{
    internal class World
    {
        public static Dictionary<string, Level> Levels { get; set; }
        public static Level CurrentLevel { get; set; }
        public static Player Player { get; set; }
        public static Camera Camera { get; set; } = new(Game1.ScreenWidth, Game1.ScreenHeight);
        public static GameTime TotalTimeInWorld { get; set; } = new();
        public static List<string> MessagesToAdd { get; private set; } = [];

        public World(ContentManager content, GraphicsDevice graphics)
        {
            Player = new();
            //Clear the levels dictionary because the levels are static and will persist between game instances
            Levels = [];
            CreateLevels(content, graphics);

            foreach (var lvl in Levels)
                Debug.WriteLine(lvl.Key);

            //TODO
            //SetCurrentLevel(Levels["Level0"], Levels["Level0"].PlayerStartPosition);
            //SetCurrentLevel(Levels["TestLevel"], Levels["TestLevel"].PlayerStartPosition);
            //SetCurrentLevel(Levels["Dungeon/Dungeon"], new(100,100));
            SetCurrentLevel(Levels["Catacombs"], new(100, 100));
            SetCurrentLevel(Levels["Catacombs1"], new(100, 100));
        }

        public static void Update(GameTime gameTime)
        {
            TotalTimeInWorld.TotalGameTime += gameTime.ElapsedGameTime;
            CurrentLevel.Update(gameTime, TotalTimeInWorld);

            //TODO delete this after adding a way to move from level to level to the game
            if (InputHandler.KeyReleased(Keys.NumPad0))
            {
                SetCurrentLevel(Levels["Level0_Old"], Levels["Level0_Old"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad1))
            {
                SetCurrentLevel(Levels["Level1_Old"], Levels["Level1_Old"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad9))
            {
                SetCurrentLevel(Levels["TestLevel"], Levels["TestLevel"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad8))
            {
                SetCurrentLevel(Levels["Testing"], Levels["Testing"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad7))
            {
                SetCurrentLevel(Levels["Level0"], Levels["Level0"].PlayerStartPosition);
            }
            //=======================================================================
        }

        public static void HandleInput(PlayerIndex playerIndex)
        {
            CurrentLevel.HandleInput(playerIndex);
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentLevel.Draw(spriteBatch);
        }

        public static void LoadWorldDataIntoLevels(WorldData worldData)
        {
            Player.UpdatePlayerWithData(worldData.PlayerData);
            LoadPlayerGameItemsFromGameData(worldData.PlayerData.backpack);
            TotalTimeInWorld.TotalGameTime = worldData.TotalTimeInWorld;

            foreach (string key in Levels.Keys)
            {
                foreach (string dataKey in worldData.Levels.Keys)
                {
                    if (key == dataKey)
                    {
                        Levels[key].Player = Player;
                        Levels[key].LoadLevelDataFromLevelData(worldData.Levels[dataKey]);
                    }
                }
            }

            SetCurrentLevel(Levels[worldData.CurrentLevel], Player.Position);
        }

        public static void LoadPlayerGameItemsFromGameData(List<ItemData> itemDatas)
        {
            GameItem temp;
            int index;

            foreach (ItemData item in itemDatas)
            {
                temp = GameManager.LoadGameItemFromItemData(item);

                Player.Backpack.Add(temp);
                if (item.Equipped)
                {
                    index = Player.Backpack.Items.Count - 1;
                    Player.EquippedItems.TryEquipItem(Player.Backpack.Items[index]);
                }
            }
        }

        public static WorldData GetWorldData()
        {
            Dictionary<string, LevelData> levels = [];
            string name = string.Empty;

            foreach (var level in Levels)
            {
                levels.Add(level.Key, level.Value.GetLevelData());

                if (level.Value == CurrentLevel)
                    name = level.Key;
            }

            return new()
            {
                TotalTimeInWorld = TotalTimeInWorld.TotalGameTime,
                PlayerData = Player.GetPlayerData(),
                Levels = levels,
                CurrentLevel = name
            };
        }

        public static void SetCurrentLevel(Level level, Vector2 playerPosition)
        {
            Player.Position = playerPosition;
            Player.RespawnPosition = level.PlayerRespawnPosition;

            CurrentLevel = level;
            Camera.SetBounds(CurrentLevel.TiledMap.WidthInPixels, CurrentLevel.TiledMap.HeightInPixels);
            CurrentLevel.Player = Player;
            CurrentLevel.Camera = Camera;
            CurrentLevel.EntityManager.Player = Player;
        }

        private static void AddLevel(Level level)
        {
            Levels.Add(level.Name, level);
        }

        public static void CreateLevels(ContentManager content, GraphicsDevice graphics)
        {
            TiledMap tiledMap;

            //Test Level
            tiledMap = content.Load<TiledMap>(@"TiledFiles/TestLevel");
            Level level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(76, 76));
            AddLevel(level);

            //Test Level
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Testing");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(76, 76));
            AddLevel(level);

            //Level 1_Old
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Level1_Old");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 100));
            AddLevel(level);

            //Level 0_Old
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Level0_Old");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 100));
            AddLevel(level);

            //Level 0
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Level0");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 1));
            AddLevel(level);

            //Level Dungeon
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Dungeon/Dungeon");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 100));
            AddLevel(level);

            //Level Catacombs
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Catacombs");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 1));
            AddLevel(level);

            //Level Catacombs1
            tiledMap = content.Load<TiledMap>(@"TiledFiles/Catacombs1");
            level = new(graphics, tiledMap, GameManager.EnemiesClone, new MinMaxPair(0, 1));
            AddLevel(level);

            //Initialize Levels
            foreach (Level lvl in Levels.Values)
                InitializeLevel(lvl);
        }

        private static void InitializeLevel(Level level)
        {
            if (level.EnterExitLayer == null)
                return;

            //TODO just used to temporarily provide a way to see where the hitboxes are for the exits
            Rectangle rec;

            foreach (TiledMapObject obj in level.EnterExitLayer.Objects)
            {
                if (obj.Name == "Entrance")
                {
                    if(obj.Properties.TryGetValue("ToLocation", out TiledMapPropertyValue value))
                        level.LevelEntrance = new(obj, Levels[value]);

                    level.PlayerStartPosition = new((int)obj.Position.X, (int)obj.Position.Y);
                    level.PlayerRespawnPosition = level.PlayerStartPosition;
                }
                if (obj.Name == "Exit")
                {
                    level.LevelExit = new(obj, Levels[obj.Properties["ToLocation"]]);
                    level.PlayerEndPosition = new((int)obj.Position.X, (int)obj.Position.Y);
                }

                rec = new((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height);
                level.EnterExitLayerObjectRectangles.Add(rec);
            }

            //if there is no level exit positin set it to the level entrance position
            if(level.LevelExit is null) 
            {
                level.PlayerEndPosition = level.PlayerStartPosition;
            }
        }

        public static void AddMessage(string message)
        {
            MessagesToAdd.Add(message);
        }

        public static void FillPlayerBackback() //TODO this is for testing
        {
            for (int i = 0; i < 3; i++)
            {
                foreach (GameItem item in GameManager.ItemsClone.Values)
                {
                    Player.Backpack.Add(item);
                }
            }

            GameItem Coins = GameManager.ItemsClone["Coins"];
            Coins.SetQuantity(20);

            Player.Backpack.Add(Coins);

            Player.EquippedItems.TryEquipItem(Player.Backpack.Items[0]);
            Player.EquippedItems.TryEquipItem(Player.Backpack.Items[3]);
        }
    }
}
