﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using RpgLibrary.GameObjectClasses;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.ItemLoot;
using SkeletonsAdventure.Quests;
using System;
using System.Collections.Generic;

namespace SkeletonsAdventure.GameObjects
{
    internal class Chest
    {
        public ChestType ChestType { get; set; } //TODO add different types of chests
        public LootList Loot { get; set; } = new();
        public Vector2 Position { get; set; } = new();
        public int ID { get; set; } = -1;
        public Rectangle DetectionArea { get; set; }
        public PopUpBox ChestMenu { get; set; } = new()
        {
            Visible = false,
            Texture = GameManager.PopUpBoxTexture,
        };
        public Label Info { get; set; } = new()
        {
            Text = "",
            Visible = false,
            SpriteFont = GameManager.Arial12
        };

        public Chest()
        {
        }

        public Chest(Chest chest)
        {
            Position = chest.Position;
            DetectionArea = chest.DetectionArea;
            ID = chest.ID;
            ChestType = chest.ChestType;
            Loot = chest.Loot.Clone();
            Info.Position = chest.Position;
        }

        public Chest(LootList loot)
        {
            Loot = loot;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.DrawRectangle(DetectionArea, Color.White, 1, 0); //TODO

            if (Info.Visible)
                Info.Draw(spriteBatch); 

            if (ChestMenu.Visible)
                ChestMenu.Draw(spriteBatch);
        }

        public void Update(GameTime gameTime)
        {
            if (Info.Visible)
                Info.Text = Loot.Count > 0 ? "Press R to Open" : "Chest Empty";

            ChestMenu.Update(true, World.Camera.Transformation);
        }

        public bool PlayerIntersects(Rectangle playerRec)
        {
            bool intersects = false;

            if (playerRec.Intersects(DetectionArea))
            {
                Info.Visible = true;
                intersects = true;
            }
            else
            {
                Info.Visible = false;
                ChestMenu.Visible = false;
            }

            return intersects;
        }

        public void ChestOpened()
        {
            if (ChestMenu.Visible == false && Info.Visible == true)
            {
                ChestMenu.Visible = true;
                ChestMenu.Buttons.Clear();

                Dictionary<string, Button> buttons = [];
                foreach (GameItem gameItem in Loot.Loots)
                {
                    Button btn = new(GameManager.DefaultButtonTexture, GameManager.Arial10);

                    btn.Click += (sender, e) =>
                    {
                        if (World.CurrentLevel.Player.Backpack.AddItem(gameItem))
                        {
                            btn.Visible = false;
                            Loot.Remove(gameItem);
                        }
                    };

                    buttons.Add(gameItem.Name, btn);
                }

                ChestMenu.AddButtons(buttons);

                foreach (Button button in ChestMenu.Buttons)
                    button.Visible = true;
            }
            else
                ChestMenu.Visible = false;

            ChestMenu.Position = Position;
        }

        public Chest Clone()
        {
            return new(this);
        }

        public ChestData GetChestData()
        {
            return new()
            {
                ItemDatas = Loot.GetLootListItemData(),
                ID = ID,
                ChestType = ChestType,
                Position = Position
            };
        }
    }
}
