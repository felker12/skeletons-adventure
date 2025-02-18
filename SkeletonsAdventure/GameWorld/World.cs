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
        public Dictionary<string, Level> Levels { get; set; } = [];
        public Level CurrentLevel { get; set; }
        public Player Player { get; set; }
        public Camera Camera { get; set; }
        public SpriteFont ToolTipFont { get; private set; }
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

            CreateLevels(_content, _graphics);

            //TODO
            //SetCurrentLevel(Levels["level0"]);
            SetCurrentLevel(Levels["testLevel"], true);

            TotalTimeInWorld = new();
        }


        public void Update(GameTime gameTime)
        {
            TotalTimeInWorld.TotalGameTime += gameTime.ElapsedGameTime;

            CurrentLevel.Update(gameTime, TotalTimeInWorld);

            Player = CurrentLevel.Player;

            //TODO
            CurrentLevel.title.Text = "\n" + gameTime.TotalGameTime + 
                "\n" + TotalTimeInWorld.TotalGameTime;

            //TODO
            if (InputHandler.KeyReleased(Keys.NumPad0))
            {
                SetCurrentLevel(Levels["level0"], true);
            }
            if (InputHandler.KeyReleased(Keys.NumPad1))
            {
                SetCurrentLevel(Levels["testLevel"], true);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
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

            SetCurrentLevel(Levels[worldData.CurrentLevel], false);
        }

        public void LoadPlayerGameItemsFromGameData(List<ItemData> itemDatas)
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

        public void SetCurrentLevel(Level level, bool resetPlayerPosition)
        {
            if(resetPlayerPosition)
                Player.Position = level.PlayerStartPosition;

            Player.RespawnPosition = level.PlayerRespawnPosition;

            CurrentLevel = level;
            Camera.SetBounds(CurrentLevel.TiledMap.WidthInPixels, CurrentLevel.TiledMap.HeightInPixels);
            CurrentLevel.Player = Player;
            CurrentLevel.Camera = Camera;
            CurrentLevel.EntityManager.Player = Player;
        }

        public void CreateLevels(ContentManager content, GraphicsDevice graphics)
        {
            _tiledMap = content.Load<TiledMap>("TiledFiles/TestLevel");
            Level level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(76,76))
            {
                PlayerStartPosition = new(80, 80),
                PlayerRespawnPosition = new(80, 80)
            };
            Levels.Add("testLevel", level);

            _tiledMap = content.Load<TiledMap>(@"TiledFiles\Level0");
            level = new(graphics, _tiledMap, GameManager.GetEnemiesClone(), new MinMaxPair(0, 100))
            {
                PlayerStartPosition = new(80, 80),
                PlayerRespawnPosition = new(80, 80)
            };
            Levels.Add("level0", level);

            foreach (Level lvl in Levels.Values)
            {
                InitializeLevel(lvl);
            }
        }

        private void InitializeLevel(Level level)
        {
            level.Player = Player;
            level.Camera = Camera;
            if(level.EntityManager.Player != null)
            {
                level.EntityManager.Remove(Player);
            }
            level.EntityManager.Add(Player);
        }

        public void FillPlayerBackback() //TODO
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
