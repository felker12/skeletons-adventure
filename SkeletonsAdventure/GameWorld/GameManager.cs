using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended.Tiled;
using RpgLibrary.AttackData;
using RpgLibrary.DataClasses;
using RpgLibrary.EntityClasses;
using RpgLibrary.GameObjectClasses;
using RpgLibrary.ItemClasses;
using RpgLibrary.QuestClasses;
using SkeletonsAdventure.Attacks;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameObjects;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.Quests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SkeletonsAdventure.GameWorld
{
    internal class GameManager
    {
        //SpriteFonts
        public static SpriteFont Arial10 { get; private set; }
        public static SpriteFont Arial12 { get; private set; }
        public static SpriteFont Arial14 { get; private set; }
        public static SpriteFont Arial20 { get; private set; }

        //Strings
        public static string GamePath { get; private set; }
        public static string SavePath { get; private set; }
        public static string ItemPath { get; private set; } 
        public static string EnemyPath {get; private set; }

        //Dictionaries
        private static Dictionary<string, Enemy> Enemies { get; set; } = [];
        public static Dictionary<string, Enemy> EnemiesClone => GetEnemiesClone();

        private static Dictionary<string, GameItem> Items { get; set; } = [];
        public static Dictionary<string, GameItem> ItemsClone => GetItemsClone();

        private static Dictionary<string, Chest> Chests { get; set; } = [];
        public static Dictionary<string, Chest> ChestsClone => GetChestsClone();

        private static Dictionary<string, EntityAttack> EntityAttacks { get; set; } = [];
        public static Dictionary<string, EntityAttack> EntityAttackClone => GetEntityAttacksClone();

        private static Dictionary<string, Quest> Quests { get; set; } = [];
        public static Dictionary<string, Quest> QuestsClone => GetQuestsClone();

        private static Dictionary<string, NPC> NPCs { get; set; } = [];
        public static Dictionary<string, NPC> NPCClone => GetNPCClone();

        //=====Textures=====
        //Entity Textures
        public static Texture2D SkeletonTexture { get; private set; }
        public static Texture2D SpiderTexture { get; private set; }

        //Attack Textures
        public static Texture2D AttackAreaTexture { get; private set; }
        public static Texture2D SkeletonAttackTexture { get; private set; }
        public static Texture2D FireBallTexture { get; private set; }
        public static Texture2D FireBallTexture2 { get; private set; }
        public static Texture2D IcePillarTexture { get; private set; }
        public static Texture2D IcePillarSpriteSheetTexture { get; private set; }
        public static Texture2D IceBulletTexture { get; private set; }

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
        public static AttackData IceBulletData { get; set; }

        //Miscellaneous Variables
        public static Game1 Game { get; private set; }
        public static GraphicsDevice GraphicsDevice { get; private set; }
        public static QuestManager QuestManager { get; set; } = new();
        public static ContentManager Content { get; private set; }
        public static List<int> PlayerLevelXPs { get; private set; } = [];

        public GameManager(Game1 game)
        {
            Game = game;
            Content = game.Content;
            GraphicsDevice = game.GraphicsDevice;

            SetPaths();
            CreatePlayerLevelXPs();

            LoadFonts();
            LoadTextures();
            LoadAttacks();

            CreateItems();
            CreateEnemies();
            //CreateEnemiesManually();

            CreateChests();
            CreateAttacks();
            CreateQuests();
            CreateNPCs();
        }

        private static void SetPaths()
        {
            GamePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName; //Project Directory
            SavePath = Path.GetFullPath(Path.Combine(GamePath, @"..\SaveFiles")); //Directory of the saved files

            //TODO these are temprorary paths, the files created here are not
            //saved in the content folder and will need to be moved to the content folder
            ItemPath = Path.Combine(SavePath, "Items"); //Directory of the items
            EnemyPath = Path.Combine(SavePath, "Enemies"); //Directory of the enemies

            if (Path.Exists(ItemPath) == false)
                Directory.CreateDirectory(ItemPath); //Create the directory if it doesn't exist
            if (Path.Exists(EnemyPath) == false)
                Directory.CreateDirectory(EnemyPath); //Create the directory if it doesn't exist
        }

        //Load data from saved files
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
                    //TODO
                    item.Quantity = itemData.Quantity;
                    item.BaseItem.Quantity = itemData.Quantity;
                    item.Position = itemData.Position;
                }
            }

            return item;
        }

        //Get a clone of the dictionaries
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

            foreach (var item in Chests)
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

        private static Dictionary<string, EntityAttack> GetEntityAttacksClone()
        {
            Dictionary<string, EntityAttack> attack = [];

            foreach (KeyValuePair<string, EntityAttack> item in EntityAttacks)
                attack.Add(item.Key, item.Value.Clone());

            return attack;
        }

        private static Dictionary<string, Quest> GetQuestsClone()
        {
            Dictionary<string, Quest> Quest = [];

            foreach (var quest in Quests)
                Quest.Add(quest.Key, quest.Value.Clone());

            return Quest;
        }

        private static Dictionary<string, NPC> GetNPCClone()
        {
            Dictionary<string, NPC> NPC = [];

            foreach (var npc in NPCs)
                NPC.Add(npc.Key, npc.Value.Clone());

            return NPC;
        }

        //Create the player levels and their XP values
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

        //Get the level the player is at given the XP
        public static int GetPlayerLevelAtXP(int XP)
        {
            int level = 0;

            foreach (var levelXP in PlayerLevelXPs)
                if (XP > levelXP)
                    level = PlayerLevelXPs.IndexOf(levelXP);

            return level;
        }

        //Get the XP needed for the level
        public static int GetLevelXPAtLevel(int level)
        {
            return PlayerLevelXPs[level];
        }

        //Load the data from the content folder
        private static void LoadFonts()
        {
            Arial10 = Content.Load<SpriteFont>("Fonts/Arial10");
            Arial12 = Content.Load<SpriteFont>("Fonts/Arial12");
            Arial14 = Content.Load<SpriteFont>("Fonts/Arial14");
            Arial20 = Content.Load<SpriteFont>("Fonts/Arial20");
        }

        private static void LoadTextures()
        {
            SkeletonTexture = Content.Load<Texture2D>(@"Player/SkeletonSpriteSheet");
            SkeletonAttackTexture = Content.Load<Texture2D>(@"Player/SkeletonAttackSprites");
            SpiderTexture = Content.Load<Texture2D>(@"EntitySprites/spider");
            FireBallTexture = Content.Load<Texture2D>(@"AttackSprites/FireBall_01");
            FireBallTexture2 = Content.Load<Texture2D>(@"AttackSprites/FireBallSpriteSheet");
            IcePillarTexture = Content.Load<Texture2D>(@"AttackSprites/IcePillar");
            IcePillarSpriteSheetTexture = Content.Load<Texture2D>(@"AttackSprites/IcePillarSpriteSheet");
            IceBulletTexture = Content.Load<Texture2D>(@"AttackSprites/IceBullet");

            AttackAreaTexture = new(GraphicsDevice, 1, 1);
            AttackAreaTexture.SetData([new Color(153, 29, 20, 250)]);

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
            IceBulletData = Content.Load<AttackData>(@"AttackData/IceBullet");
        }

        //Create the items from the content folder
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

        //Create the enemies from the content folder
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

            string EnemiesPath = Path.Combine(GamePath, "Content", "EntityData");
            string[] fileNames = Directory.GetFiles(EnemiesPath);

            foreach (string s in fileNames)
            {
                // Get just the filename without extension and path
                string fileName = Path.GetFileNameWithoutExtension(s);
                
                // Load using Content.Load with the correct content path format
                EntityData data = Content.Load<EntityData>($"EntityData/{fileName}");

                Type type = Type.GetType(data.type); //Get the type of the entity from the data

                //TODO
                //dynamic en = Activator.CreateInstance(type, data);
                //en.LootList = loots.Clone(); //Set the loot list for the entity

                Enemy en = (Enemy)Activator.CreateInstance(type, data);
                en.LootList = loots.Clone(); //Set the loot list for the entity

                Enemies.Add(en.GetType().FullName, en); //Add the entity to the dictionary of enemies
            }
        }

        //This is the old method for creating enemies. It is kept for reference and testing purposes
        private static void CreateEnemiesManually()
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

            //Create the entities from the data and add their items to their loot list
            EntityData entityData = new(Content.Load<EntityData>(@"EntityData/SkeletonData"))
            {
                //Items = items
            };

            Skeleton skeleton = new(entityData)
            {
                //LootList = loots
            };

            EliteSkeleton eliteSkeleton = new(entityData)
            {
                //LootList = loots
            };

            entityData = new(Content.Load<EntityData>(@"EntityData/SpiderData"))
            {
                //Items = items
            };

            Spider spider = new(entityData)
            {
                //LootList = loots
            };

            //Add the entities to the dictionary
            Enemies.Add(skeleton.GetType().FullName, skeleton);
            Enemies.Add(eliteSkeleton.GetType().FullName, eliteSkeleton);
            Enemies.Add(spider.GetType().FullName, spider);


            //TODO test this
            foreach (var enemy in Enemies)//shouldn't be needed now
            {
                //XnaSerializer.Serialize($@"{EnemyPath}\{enemy.Value.GetType().Name}.xml", enemy.Value.GetEntityData());
            }
        }

        //Create the chests from the content folder
        private static void CreateChests() //TODO
        {
            ItemsClone.TryGetValue("Coins", out GameItem Coins);
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

        private static void CreateAttacks()
        {
            //Create the attacks from the content folder
            EntityAttack attack = new(BasicAttackData, SkeletonAttackTexture, null);
            EntityAttacks.Add(attack.GetType().Name, attack);

            FireBall fireball = new(FireBallData, FireBallTexture2, null);
            EntityAttacks.Add(fireball.GetType().Name, fireball);

            IcePillar icePillar = new(IcePillarData, IcePillarSpriteSheetTexture, null);
            EntityAttacks.Add(icePillar.GetType().Name, icePillar);

            IceBullet iceBullet = new(IceBulletData, IceBulletTexture, null);
            EntityAttacks.Add(iceBullet.GetType().Name, iceBullet);
        }

        private static void CreateQuests() //TODO
        {
            BaseTask task = new()
            {
                RequiredAmount = 3,
                TaskToComplete = "Kill that thing"
            };
            BaseTask task2 = new()
            {
                RequiredAmount = 5,
                TaskToComplete = "Do that thing"
            };
            BaseTask task3 = new()
            {
                RequiredAmount = 5,
                TaskToComplete = "talk to that person"
            };
            SlayTask slayTask = new()
            {
                RequiredAmount = 10,
                TaskToComplete = "Slay Entity: Skeleton",
                MonsterToSlay = typeof(Skeleton).FullName,
            };

            List <BaseTask> Tasks = [task, task2, task3, slayTask];

            Requirements requirements = new()
            {
                Attack = 0,
                Defence = 0,
                Level = 0,
            };

            Quest quest = new()
            {
                Name = "Test Quest",
                Description = "This is a test quest to test the quest system.",
                Requirements = requirements,
                Tasks = Tasks,
            };

            Quest quest2 = quest.Clone();
            quest2.Name = "Test2";
            quest2.RequiredQuestNames.Add(quest.Name);

            Quest quest3 = new(quest.GetQuestData())
            {
                Name = "Test3",
            };

            QuestReward questReward = new()
            {
                Gold = 100,
                XP = 50,
                Items = [..ItemsClone.Values] // Convert the Dictionary to a List
            };

            Quest SlaySkeletons = new()
            {
                Name = "SlaySkeletons",
                Description = "Kill 10 skeletons",
                Requirements = requirements,
                Reward = questReward,
            };
            SlaySkeletons.Tasks.Add(slayTask);

            List<Quest> quests = [quest, quest2, quest3, SlaySkeletons];

            foreach(var q in quests)
            {
                Quests.Add(q.Name, q);
            }
        }

        private static void CreateNPCs() //TODO
        {

            NPCData data = new()
            {

            };


        }

        public static List<TiledMapTile> TileLocations(int id, TiledMapTile[] tiles)
        {
            List<TiledMapTile> mapTiles = [];

            foreach (var tile in tiles)
            {
                if (tile.GlobalIdentifier == id)
                    mapTiles.Add(tile);
            }
            return mapTiles;
        }
    }
}
