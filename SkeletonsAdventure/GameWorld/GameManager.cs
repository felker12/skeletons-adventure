using Assimp;
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

namespace SkeletonsAdventure.GameWorld
{
    public class GameManager
    {
        public Game1 Game { get; private set; }
        public static ContentManager Content { get; private set; }
        public static SpriteFont InfoFont { get; private set; }
        public static SpriteFont ToolTipFont { get; private set; }
        public static Dictionary<string, Enemy> Enemies { get; private set; } = [];
        private static Dictionary<string, GameItem> Items { get; set; } = [];
        public static Dictionary<string, Chest> Chests { get; private set; } = [];
        public static Texture2D SkeletonTexture { get; private set; }
        public static Texture2D SkeletonAttackTexture { get; private set; }
        public static Texture2D SpiderTexture { get; private set; }

        public GameManager(Game1 game)
        {
            Game = game;
            Content = game.Content;
            InfoFont = Content.Load<SpriteFont>("Fonts/Font");
            LoadTextures ();
            CreateItems();
            CreateEnemies();
            CreateChests();
        }

        public static List<GameItem> LoadGameItemsFromItemData(List<ItemData> itemDatas)
        {
            List<GameItem> items = [];

            foreach (ItemData item in itemDatas)
            {
                items.Add(LoadGameItemFromItemData(item));
            }

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
            {
                items.Add(item.Key, item.Value.Clone());
            }

            return items;
        }

        public static void LoadTextures()
        {
            SkeletonTexture = Content.Load<Texture2D>(@"Player\SkeletonSpriteSheet");
            SkeletonAttackTexture = Content.Load<Texture2D>(@"Player\SkeletonAttackSprites");
            SpiderTexture = Content.Load<Texture2D>(@"EntitySprites/spider");
        }

        public static void CreateItems()
        {
            WeaponData _weaponData = Content.Load<WeaponData>(@"Items\Weapons\Sword");
            Weapon _sword = new(_weaponData.Clone());
            ItemData _coinsData = Content.Load<ItemData>(@"Items\Coins");
            BaseItem _coins = new(_coinsData.Clone());
            ArmorData _armorData = Content.Load<ArmorData>(@"Items\Armor\Robes");
            Armor _armor = new(_armorData.Clone());
            ConsumableData _consumableData = Content.Load<ConsumableData>(@"Items\Consumables\Bones");
            Consumable _bones = new(_consumableData.Clone());

            Texture2D texture = Content.Load<Texture2D>(@"TileSets/ProjectUtumno_full");
            ToolTipFont = Content.Load<SpriteFont>("Fonts/ItemToolTipFont");
            Rectangle source = new(1952, 1472, 32, 32);
            GameItem basicSword = new(_sword, 1, ToolTipFont, texture, source);

            source = new(1376, 1280, 32, 32);
            GameItem coins = new(_coins, 5, ToolTipFont, texture, source);

            source = new(832, 1216, 32, 32);
            GameItem basicRobe = new(_armor, 1, ToolTipFont, texture, source);

            texture = Content.Load<Texture2D>(@"Items/Bones");
            source = new(0, 0, 32, 32);
            GameItem bones = new(_bones, 1, ToolTipFont, texture, source);

            if (Items.ContainsKey(coins.BaseItem.Name) == false)
                Items.Add(coins.BaseItem.Name, coins);

            if (Items.ContainsKey(basicSword.BaseItem.Name) == false)
                Items.Add(basicSword.BaseItem.Name, basicSword);

            if (Items.ContainsKey(basicRobe.BaseItem.Name) == false)
                Items.Add(basicRobe.BaseItem.Name, basicRobe);

            if (Items.ContainsKey(bones.BaseItem.Name) == false)
                Items.Add(bones.BaseItem.Name, bones);
        }

        public static void CreateEnemies()
        {
            GameItem Coins = GetItemsClone()["Coins"];
            Coins.Quantity = 10;

            LootList loots = new();
            loots.Add(GetItemsClone()["Robes"]);
            loots.Add(GetItemsClone()["Bones"]);
            loots.Add(Coins.Clone());
            loots.Add(GetItemsClone()["Sword"]);

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
            GameManager.GetItemsClone().TryGetValue("Coins", out GameItem Coins);
            Coins.Quantity = 10;

            LootList loots = new();
            //loots.Add(basicSword.Clone());
            //loots.Add(basicRobe.Clone());
            loots.Add(Coins.Clone());
            //loots.Add(bones.Clone());


            Chest BasicChest = new(loots)
            {
                ID = 8
            };


            if (Chests.ContainsKey(BasicChest.GetType().Name) == false)
                Chests.Add(BasicChest.GetType().Name, BasicChest);
        }

    }
}
