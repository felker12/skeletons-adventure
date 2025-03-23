using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using MonoGame.Extended.Tiled;
using SkeletonsAdventure.Entities;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.ItemClasses;
using RpgLibrary.DataClasses;
using RpgLibrary.WorldClasses;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.Engines;

namespace SkeletonsAdventure.GameWorld
{
    public class World
    {
        public static Dictionary<string, Level> Levels { get; set; } = [];
        public static Level CurrentLevel { get; set; }
        public static Player Player { get; set; }
        public static Camera Camera { get; set; }
        public GameTime TotalTimeInWorld { get; set; }

        private TiledMap _tiledMap;
        private readonly ContentManager _content;
        private readonly GraphicsDevice _graphics;
        private Viewport _gameWindow;

        public World(ContentManager content, GraphicsDevice graphics, Viewport gameWindow)
        {
            _content = content;
            _graphics = graphics;
            _gameWindow = gameWindow;
            Initialilze();
        }

        public void Initialilze()
        {
            Camera = new(_gameWindow.Width, _gameWindow.Height);
            Player = new()
            {
                Position = new(80, 80),
                RespawnPosition = new(80, 80)
            };

            Levels = []; //Clear the levels dictionary 
            CreateLevels(_content, _graphics);

            //TODO
            SetCurrentLevel(Levels["level0"], Levels["level0"].PlayerStartPosition);
            //SetCurrentLevel(Levels["testLevel"], true);

            if(CurrentLevel.LevelExit is not null)
                Player.Position = CurrentLevel.LevelExit.ExitPosition - new Vector2(0,20); //TODO

            TotalTimeInWorld = new();
        }


        public void Update(GameTime gameTime)
        {
            TotalTimeInWorld.TotalGameTime += gameTime.ElapsedGameTime;

            CurrentLevel.Update(gameTime, TotalTimeInWorld);

            Player = CurrentLevel.Player;

            //TODO
            CurrentLevel.title.Text = "\n" + gameTime.TotalGameTime + 
                "\n" + TotalTimeInWorld.TotalGameTime + "\n" + CurrentLevel.Width + "\n" + CurrentLevel.Height;

            //TODO delete this after adding a way to move from level to level to the game
            if (InputHandler.KeyReleased(Keys.NumPad0))
            {
                SetCurrentLevel(Levels["level0"], Levels["level0"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad1))
            {
                SetCurrentLevel(Levels["level1"], Levels["level1"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad9))
            {
                SetCurrentLevel(Levels["testLevel"], Levels["testLevel"].PlayerStartPosition);
            }
            if (InputHandler.KeyReleased(Keys.NumPad8))
            {
                SetCurrentLevel(Levels["testing"], Levels["testing"].PlayerStartPosition);
            }
            //=======================================================================
        }

        public static void Draw(SpriteBatch spriteBatch)
        {
            CurrentLevel.Draw(spriteBatch);
        }

        public void LoadWorldDataIntoLevels(WorldData worldData)
        {
            Player.UpdatePlayerData(worldData.PlayerData);
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

                Player.Backpack.AddItem(temp);
                if (item.Equipped)
                {
                    index = Player.Backpack.Items.Count - 1;
                    Player.EquippedItems.TryEquipItem(Player.Backpack.Items[index]);
                }
            }
        }

        public WorldData GetWorldData()
        {
            Dictionary<string, LevelData> levels = [];
            string name = string.Empty;

            foreach (var level in Levels)
            {
                levels.Add(level.Key, level.Value.GetLevelData());

                if (level.Value == CurrentLevel)
                {
                    name = level.Key;
                }
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

        public void CreateLevels(ContentManager content, GraphicsDevice graphics)
        {
            //Test Level
            _tiledMap = content.Load<TiledMap>("TiledFiles/TestLevel");
            Level level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(76,76))
            {
                PlayerStartPosition = new(80, 80),
                PlayerRespawnPosition = new(80, 80),
                PlayerEndPosition = new(80,80),
                Name = "testLevel"
            };
            Levels.Add("testLevel", level);

            //Test Level
            _tiledMap = content.Load<TiledMap>("TiledFiles/Testing");
            level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(76, 76))
            {
                PlayerStartPosition = new(80, 80),
                PlayerRespawnPosition = new(80, 80),
                PlayerEndPosition = new(80, 80),
                Name = "testing"
            };
            Levels.Add("testing", level);

            //Level 1
            _tiledMap = content.Load<TiledMap>(@"TiledFiles\Level1");
            level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(0, 100))
            {
                Name = "level1"
            };
            Levels.Add("level1", level);

            //Level 0
            _tiledMap = content.Load<TiledMap>(@"TiledFiles\Level0");
            level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(0, 100))
            {
                Name = "level0"
            };
            Levels.Add("level0", level);

            //Initialize Levels
            foreach (Level lvl in Levels.Values)
            {
                InitializeLevel(lvl);
            }
        }

        private static void InitializeLevel(Level level)
        {
            level.Player = Player;
            level.Camera = Camera;
            if(level.EntityManager.Player != null)
            {
                level.EntityManager.Remove(Player);
            }
            level.EntityManager.Add(Player);

            //TODO just used to temporarily provide a way to see where the hitboxes are for the exits
            Rectangle rec;

            foreach (TiledMapObject obj in level.EnterExitLayer.Objects)
            {
                if (obj.Name == "Exit")
                {
                    level.LevelExit = new(obj, World.Levels[obj.Properties["ToLocation"]]);

                    level.PlayerEndPosition = new((int)obj.Position.X, (int)obj.Position.Y);
                }
                if (obj.Name == "Entrance")
                {
                    System.Diagnostics.Debug.WriteLine(level.Name);

                    if(obj.Properties.TryGetValue("ToLocation", out TiledMapPropertyValue value))
                        level.LevelEntrance = new(obj, World.Levels[value]);

                    level.PlayerStartPosition = new((int)obj.Position.X, (int)obj.Position.Y);
                    level.PlayerRespawnPosition = level.PlayerStartPosition;
                }

                rec = new((int)obj.Position.X, (int)obj.Position.Y, (int)obj.Size.Width, (int)obj.Size.Height);
                level.Recs.Add(rec);
            }
        }

        public static void FillPlayerBackback() //TODO this is for testing
        {
            for (int i = 0; i < 6; i++)
            {
                foreach (GameItem item in GameManager.GetItemsClone().Values)
                {
                    Player.Backpack.AddItem(item);
                }
            }

            Player.EquippedItems.TryEquipItem(Player.Backpack.Items[1]);
            Player.EquippedItems.TryEquipItem(Player.Backpack.Items[2]);
        }
    }
}
