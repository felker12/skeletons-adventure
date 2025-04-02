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

namespace SkeletonsAdventure.GameWorld
{
    public class GameManager
    {
        public static Game1 Game { get; private set; }
        public static ContentManager Content { get; private set; }
        public static SpriteFont InfoFont { get; private set; }
        public static SpriteFont ToolTipFont { get; private set; }

        private static Dictionary<string, Enemy> Enemies { get; set; } = [];
        private static Dictionary<string, GameItem> Items { get; set; } = [];
        private static Dictionary<string, Chest> Chests { get; set; } = [];
        public static Dictionary<string, Chest> ChestsClone => GetChestsClone();
        public static Dictionary<string, GameItem> ItemsClone => GetItemsClone();

        public static Texture2D SkeletonTexture { get; private set; }
        public static Texture2D SkeletonAttackTexture { get; private set; }
        public static Texture2D SpiderTexture { get; private set; }
        public static Texture2D GamePopUpBoxTexture { get; private set; }
        public static Texture2D DefaultButtonTexture { get; private set; }

        private static GraphicsDevice GraphicsDevice { get; set; }

        public GameManager(Game1 game)
        {
            Game = game;
            Content = game.Content;

            InfoFont = Content.Load<SpriteFont>("Fonts/Font");
            ToolTipFont = Content.Load<SpriteFont>("Fonts/ItemToolTipFont");

            GraphicsDevice = game.GraphicsDevice;

            LoadTextures ();
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

        public static Dictionary<string, GameItem> GetItemsClone()
        {
            Dictionary<string, GameItem> items = [];

            foreach (var item in Items)
                items.Add(item.Key, item.Value.Clone());

            return items;
        }

        public static Dictionary<string, Chest> GetChestsClone()
        {
            Dictionary<string, Chest> chests = [];

            foreach(var item in Chests)
                chests.Add(item.Key, item.Value.Clone());

            return chests;
        }


        public static Dictionary<string, Enemy> GetEnemiesClone()
        {
            Dictionary<string, Enemy> enemy = [];

            foreach (var item in Enemies)
                enemy.Add(item.Key, item.Value.Clone());

            return enemy;
        }

        public static void LoadTextures()
        {
            SkeletonTexture = Content.Load<Texture2D>(@"Player\SkeletonSpriteSheet");
            SkeletonAttackTexture = Content.Load<Texture2D>(@"Player\SkeletonAttackSprites");
            SpiderTexture = Content.Load<Texture2D>(@"EntitySprites\spider");

            GamePopUpBoxTexture = new(GraphicsDevice, 1, 1);
            GamePopUpBoxTexture.SetData([new Color(83, 105, 140, 230)]);

            DefaultButtonTexture = new(GraphicsDevice, 1, 1);
            DefaultButtonTexture.SetData([new Color(83, 105, 140, 230)]);
        }

        public static void CreateItems()
        {
            string[] folders = Directory.GetDirectories(@"Content\Items");
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

        public static void CreateEnemies()
        {
            GameItem Coins = ItemsClone["Coins"];
            Coins.Quantity = 10;

            LootList loots = new();
            loots.Add(ItemsClone["Robes"]);
            loots.Add(ItemsClone["Bones"]);
            loots.Add(Coins.Clone());
            loots.Add(ItemsClone["Sword"]);

            List<ItemData> items = [];

            foreach (var loot in loots.Loots)
            {
                items.Add(loot.GetItemData());
            }

            EntityData entityData = new(null, 20, 2, 9, 0, 5, null, null, 20, false, null)
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

            entityData = new(null, 15, 2, 7, 0, 4, new Vector2(0, 0), new Vector2(0, 0), 15, false, new TimeSpan())
            {
                Items = items
            };

            Spider spider = new(entityData)
            {
                LootList = loots
            };

            Enemies.Add("Skeleton", skeleton);
            Enemies.Add("Elite Skeleton", eliteSkeleton);
            Enemies.Add("Spider", spider);
        }

        public static void CreateChests() //TODO
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
