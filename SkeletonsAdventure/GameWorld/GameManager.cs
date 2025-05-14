using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.EntityClasses;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameObjects;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.ItemLoot;
using System;
using System.Collections.Generic;
using RpgLibrary.GameObjectClasses;
using System.IO;
using System.Linq;
using RpgLibrary.AttackData;

namespace SkeletonsAdventure.GameWorld
{
    public class GameManager
    {
        //SpriteFonts
        public static SpriteFont InfoFont { get; private set; }
        public static SpriteFont ToolTipFont { get; private set; }
        public static SpriteFont ControlFont { get; private set; } 
        public static SpriteFont StatusBarFont { get; private set; }

        //Strings
        public static string GamePath { get; private set; } 
        public static string SavePath { get; private set; }

        //Dictionaries
        private static Dictionary<string, Enemy> Enemies { get; set; } = [];
        private static Dictionary<string, GameItem> Items { get; set; } = [];
        private static Dictionary<string, Chest> Chests { get; set; } = [];
        public static Dictionary<string, Chest> ChestsClone => GetChestsClone();
        public static Dictionary<string, GameItem> ItemsClone => GetItemsClone();
        public static Dictionary<string, Enemy> EnemiesClone => GetEnemiesClone();

        //=====Textures=====
        //Entity Textures
        public static Texture2D SkeletonTexture { get; private set; }
        public static Texture2D SpiderTexture { get; private set; }

        //Attack Textures
        public static Texture2D SkeletonAttackTexture { get; private set; }
        public static Texture2D FireBallTexture { get; private set; }
        public static Texture2D FireBallTexture2 { get; private set; }
        public static Texture2D IcePillarTexture { get; private set; }

        //UI Textures
        public static Texture2D PopUpBoxTexture { get; private set; }
        public static Texture2D DefaultButtonTexture { get; private set; }
        public static Texture2D GameMenuTexture { get; set; }
        public static Texture2D BackpackBackground { get; set; }
        public static Texture2D StatusBarTexture { get; set; }
        public static Texture2D ButtonTexture { get; set; }
        //==========


        //Attacks
        public static AttackData BasicAttackData { get; set; }
        public static AttackData FireBallData { get; set; }
        public static AttackData IcePillarData { get; set; }

        //Miscellaneous Variables
        public static Game1 Game { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static ContentManager Content { get; private set; }
        public static List<int> PlayerLevelXPs { get; private set; } = [];

        public GameManager(Game1 game)
        {
            Game = game;
            Content = game.Content;
            GraphicsDevice = game.GraphicsDevice;

            GamePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName; //Project Directory
            SavePath = Path.GetFullPath(Path.Combine(GamePath, @"..\SaveFiles")); //Directory of the saved files

            CreatePlayerLevelXPs();

            LoadFonts();
            LoadTextures();
            LoadAttacks();

            CreateItems();
            CreateEnemies();
            CreateChests();
        }

        public static List<GameItem> LoadGameItemsFromItemData(List<ItemData> itemDatas)
        {
            List<GameItem> items = [];

            foreach (ItemData item in itemDatas)
                items.Add(LoadGameItemFromItemData(item));

            return items;
        }

        public static GameItem LoadGameItemFromItemData(ItemData itemData)
        {
            GameItem item = null;

            foreach (GameItem gameItem in Items.Values)
            {
                if (itemData.Name == gameItem.BaseItem.Name)
                {
                    item = gameItem.Clone();
                    item.Quantity = itemData.Quantity;
                    item.BaseItem.Quantity = itemData.Quantity;
                    item.Position = itemData.Position;
                }
            }

            return item;
        }

        private static Dictionary<string, GameItem> GetItemsClone()
        {
            Dictionary<string, GameItem> items = [];

            foreach (var item in Items)
                items.Add(item.Key, item.Value.Clone());

            return items;
        }

        private static Dictionary<string, Chest> GetChestsClone()
        {
            Dictionary<string, Chest> chests = [];

            foreach(var item in Chests)
                chests.Add(item.Key, item.Value.Clone());

            return chests;
        }


        private static Dictionary<string, Enemy> GetEnemiesClone()
        {
            Dictionary<string, Enemy> enemy = [];

            foreach (var item in Enemies)
                enemy.Add(item.Key, item.Value.Clone());

            return enemy;
        }

        public static void CreatePlayerLevelXPs() //TODO adjust the xp as needed
        {
            string levelsSavePath = Path.Combine(SavePath, "PlayerLevels.txt");

            if (File.Exists(levelsSavePath)) //If the file exists load the data
            {
                List<string> lines = [.. File.ReadAllLines(levelsSavePath)];

                foreach (var line in lines)
                {
                    string[] parts = line.Split(',');
                    if (int.TryParse(parts[1], out int xp))
                    {
                        PlayerLevelXPs.Add(xp);
                    }
                }
            }
            else
            {
                File.CreateText(levelsSavePath).Close(); //create the file if it doesn't exist
                string levels = string.Empty;

                for (int i = 0; i < 101; i++)
                {
                    if (i == 0)
                        PlayerLevelXPs.Add(0);
                    else
                        PlayerLevelXPs.Add((int)Math.Pow(i + 1, 2) * 20);

                    levels += $"{i},{PlayerLevelXPs[i]}" + Environment.NewLine;
                }

                File.WriteAllText(levelsSavePath, levels);
            }
        }

        public static int GetPlayerLevelAtXP(int XP)
        {
            int level = 0;

            foreach(var levelXP in PlayerLevelXPs)
                if(XP > levelXP)
                    level = PlayerLevelXPs.IndexOf(levelXP);

            return level;
        }

        public static int GetLevelXPAtLevel(int level)
        {
            return PlayerLevelXPs[level];
        }

        private static void LoadFonts()
        {
            InfoFont = Content.Load<SpriteFont>("Fonts/Font");
            ToolTipFont = Content.Load<SpriteFont>("Fonts/ItemToolTipFont");
            ControlFont = Content.Load<SpriteFont>("Fonts/ControlFont");
            StatusBarFont = Content.Load<SpriteFont>("Fonts/StatusBarFont");
        }

        private static void LoadTextures()
        {
            SkeletonTexture = Content.Load<Texture2D>(@"Player/SkeletonSpriteSheet");
            SkeletonAttackTexture = Content.Load<Texture2D>(@"Player/SkeletonAttackSprites");
            SpiderTexture = Content.Load<Texture2D>(@"EntitySprites/spider");
            FireBallTexture = Content.Load<Texture2D>(@"AttackSprites/FireBall_01");
            FireBallTexture2 = Content.Load<Texture2D>(@"AttackSprites/FireBallSpriteSheet");
            IcePillarTexture = Content.Load<Texture2D>(@"AttackSprites/IcePillar");

            PopUpBoxTexture = new(GraphicsDevice, 1, 1);
            PopUpBoxTexture.SetData([new Color(83, 105, 140, 230)]);

            DefaultButtonTexture = new(GraphicsDevice, 1, 1);
            DefaultButtonTexture.SetData([new Color(83, 105, 140, 230)]);

            GameMenuTexture = new(GraphicsDevice, 1, 1);
            GameMenuTexture.SetData([new Color(171, 144, 91, 250)]);

            BackpackBackground = Content.Load<Texture2D>(@"TiledFiles/BackpackBackground");

            StatusBarTexture = new(GraphicsDevice, 1, 1);
            StatusBarTexture.SetData([Color.White]);

            ButtonTexture = Content.Load<Texture2D>("Controls/Button");
        }

        private static void LoadAttacks()
        {
            BasicAttackData = Content.Load<AttackData>(@"AttackData/BasicAttack");
            FireBallData = Content.Load<AttackData>(@"AttackData/FireBall");
            IcePillarData = Content.Load<AttackData>(@"AttackData/IcePillar");
        }

        private static void CreateItems()
        {
            string[] folders = Directory.GetDirectories(@"Content/Items");
            string[] names;
            string filePath;

            ItemData itemData;
            BaseItem baseItem;
            Texture2D itemTexure;
            GameItem gameItem;
            Weapon weapon;
            Armor armor;
            Consumable consumable;

            foreach (string folder in folders)
            {
                //the name of the folder without extensions and the complete file path
                names = [.. Directory.GetFiles(folder).Select(fileName => Path.GetFileNameWithoutExtension(fileName))];

                foreach (string name in names)
                {
                    filePath = $@"..\{folder}\{name}"; //add the folder name to the path of the folder to get the file path without the extension
                    itemData = Content.Load<ItemData>(filePath);
                    baseItem = new(itemData.Clone());
                    itemTexure = Content.Load<Texture2D>(@$"{baseItem.TexturePath}");

                    //cast the itemData to the correct type
                    if (itemData is WeaponData weaponData)
                    {
                        weapon = new(weaponData.Clone());
                        baseItem = weapon;
                    }
                    else if (itemData is ArmorData armorData)
                    {
                        armor = new(armorData.Clone());
                        baseItem = armor;
                    }
                    else if (itemData is ConsumableData consumableData)
                    {
                        consumable = new(consumableData.Clone());
                        baseItem = consumable;
                    }

                    gameItem = new(baseItem, 1, itemTexure);

                    if (Items.ContainsKey(gameItem.Name) == false)
                        Items.Add(gameItem.Name, gameItem);
                }
            }
        }

        private static void CreateEnemies()
        {
            //set the quantiy of the stackable items
            GameItem Coins = ItemsClone["Coins"];
            Coins.Quantity = 10;

            //Create the loot list
            LootList loots = new();
            loots.Add(ItemsClone["Robes"]);
            loots.Add(ItemsClone["Bones"]);
            loots.Add(Coins.Clone());
            loots.Add(ItemsClone["Sword"]);

            //populate the loot list with the items the entity will be carrying
            List<ItemData> items = [];

            foreach (var loot in loots.Loots)
            {
                items.Add(loot.GetItemData());
            }

            //Create the entities from the data
            EntityData entityData = new(Content.Load<EntityData>(@"EntityData/SkeletonData"))
            {
                Items = items
            };

            Skeleton skeleton = new(entityData)
            {
                LootList = loots
            };

            EliteSkeleton eliteSkeleton = new(entityData)
            {
                LootList = loots
            };

            entityData = new(Content.Load<EntityData>(@"EntityData/SpiderData"))
            {
                Items = items
            };

            Spider spider = new(entityData)
            {
                LootList = loots
            };

            //Add the entities to the dictionary
            Enemies.Add("Skeleton", skeleton);
            Enemies.Add("Elite Skeleton", eliteSkeleton);
            Enemies.Add("Spider", spider);
        }

        private static void CreateChests() //TODO
        {
            GameManager.ItemsClone.TryGetValue("Coins", out GameItem Coins);
            Coins.Quantity = 10;

            LootList loots = new();
            loots.Add(ItemsClone["Robes"]);
            loots.Add(ItemsClone["Bones"]);
            loots.Add(Coins.Clone());
            loots.Add(ItemsClone["Sword"]);

            Chest BasicChest = new()
            {
                ID = 8,
                ChestType = ChestType.Basic,
                Loot = loots.Clone()
            };

            string name = nameof(BasicChest);
            if (Chests.ContainsKey(name) == false)
                Chests.Add(name, BasicChest);
        }
    }
}
