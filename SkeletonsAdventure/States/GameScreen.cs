using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.GameWorld;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.Entities;
using System;
using System.Collections.Generic;
using RpgLibrary.ItemClasses;
using RpgLibrary.WorldClasses;

enum BoxSource { Game, Panel }

namespace SkeletonsAdventure.States
{
    public class GameScreen : State
    {
        private Viewport _gameWindow, _sidePanel;
        private InfoPanel _infoPanel;
        private Backpack _backpack;
        private MouseState _mouseState, _lastMouseState;
        private GameButton equip, unequip, pickUp, drop, consume;
        private GameItem itemUnderMouse = null;

        private BoxSource CurrentSource { get; set; }
        public Camera Camera { get; set; }
        public World World { get; private set; }
        public Player Player { get; set; }
        public GamePopUpBox PopUpBox { get; private set; }

        public GameScreen(Game1 game) : base(game)
        {
            Initialize();
        }

        public GameScreen(Game1 game, WorldData worldData) : base(game)
        {
            Initialize();
            World.LoadWorldDataIntoLevels(worldData);

        }

        public void Initialize()
        {
            int gameWindowWidth = Game1.ScreenWidth - 192,
                gameWindowHeight = Game1.ScreenHeight - 64;
            _gameWindow = new(0, 0, gameWindowWidth, gameWindowHeight);

            World = new(Content, GraphicsDevice, _gameWindow);
            Camera = World.CurrentLevel.Camera;
            Player = World.CurrentLevel.Player;

            _sidePanel = new(gameWindowWidth, 0, 200, Game1.ScreenHeight);

            TiledMap backsplash = Content.Load<TiledMap>(@"TiledFiles/SidePanel");
            _backpack = World.CurrentLevel.EntityManager.Player.Backpack;
            _infoPanel = new(_sidePanel, _backpack.Items, GraphicsDevice, backsplash);

            CreatePopUpBox();
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            GraphicsDevice.Viewport = _gameWindow;
            World.Draw(spriteBatch);

            //Draw the sidepanel
            GraphicsDevice.Viewport = _sidePanel;
            _infoPanel.Draw(spriteBatch);

            GraphicsDevice.Viewport = Game.GameViewport;

            //Draw the PopUpBox
            if (PopUpBox.Visible && CurrentSource == BoxSource.Game)
            {
                spriteBatch.Begin(
                    SpriteSortMode.Immediate,
                    BlendState.AlphaBlend,
                    SamplerState.PointClamp,
                    null,
                    null,
                    null,
                    Camera.Transformation);

                PopUpBox.Draw(spriteBatch);

                spriteBatch.End();
            }

            if (PopUpBox.Visible && CurrentSource == BoxSource.Panel)
            {
                spriteBatch.Begin();
                PopUpBox.Draw(spriteBatch);
                spriteBatch.End();
            }
        }

        public override void PostUpdate(GameTime gameTime) { }
        public override void Update(GameTime gameTime)
        {
            World.Update(gameTime);

            _infoPanel.Update(_backpack.Items);
            CheckUnderMouse(gameTime);
        }

        public void CheckUnderMouse(GameTime gameTime)
        {
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            Vector2 mousePos = new(_mouseState.X, _mouseState.Y), 
                tempV,
                pos = Vector2.Transform(mousePos, Matrix.Invert(Camera.Transformation)), //Mouse position in the world
                offset = new(_gameWindow.Width, 0); //Offset because these items are on the sidepanel viewport and their position is relavie to that viewport
            Rectangle transformedMouseRectangle = new((int)pos.X, (int)pos.Y, 1, 1), 
                mouseRec = new(_mouseState.X, _mouseState.Y, 1,1),
                tempR;
            
            itemUnderMouse = null;

            foreach (GameItem item in _infoPanel.Items)
            {
                //calculate the transformed position so we can find where the items are based on a world position
                tempV = Vector2.Transform(item.Position, Matrix.Invert(Camera.Transformation)); 
                tempV += offset; //offset the world position with the width of the game viewport
                tempR = new((int)tempV.X, (int)tempV.Y, GameItem.Width, GameItem.Height);

                Intersects(transformedMouseRectangle, tempR, item, BoxSource.Panel);

                if(PopUpBox.Visible)
                {
                    Vector2 tempPopUpPos = Vector2.Transform(PopUpBox.Position, Matrix.Invert(Camera.Transformation));
                    Rectangle rec = new((int)tempPopUpPos.X, (int)tempPopUpPos.Y, 1, 1);
                    if(rec.Intersects(tempR))
                        itemUnderMouse = item;
                }
                else if (transformedMouseRectangle.Intersects(tempR))
                    itemUnderMouse = item;
            }

            //TODO this variable might not be needed now. Could be useful in future if I want to be able to select from a list of items to pick up
            List<GameItem> itemsUnderMouse = []; 
            foreach (GameItem item in World.CurrentLevel.EntityManager.DroppedLootManager.Items)
            {
                Intersects(transformedMouseRectangle, item.ItemRectangle, item, BoxSource.Game);

                if (PopUpBox.Visible)
                {
                    Rectangle rec = new((int)PopUpBox.Position.X, (int)PopUpBox.Position.Y, 1, 1);
                    if (rec.Intersects(item.ItemRectangle))
                        itemUnderMouse = item;
                }
                else if (transformedMouseRectangle.Intersects(item.ItemRectangle))
                {
                    itemUnderMouse = item;
                    itemsUnderMouse.Add(item);
                }
            }

            //hide popupbox when no more items are under mouse
            if (itemUnderMouse == null)
                PopUpBox.Visible = false;

            if (_mouseState.LeftButton == ButtonState.Released && _lastMouseState.LeftButton == ButtonState.Pressed)
            {
                if (itemUnderMouse != null && transformedMouseRectangle.Intersects(itemUnderMouse.ItemRectangle))
                {
                    PickUp_Click(null, new EventArgs());
                }
            }

            if(PopUpBox.Visible)
            {
                foreach (Button button in PopUpBox.Buttons)
                    button.Visible = false;

                if (CurrentSource == BoxSource.Game)
                {
                    pickUp.Visible = true;

                    PopUpBox.Update(true, Camera.Transformation);

                    if (transformedMouseRectangle.Intersects(PopUpBox.Rectangle) == false)
                        PopUpBox.Visible = false;
                }
                else if (CurrentSource == BoxSource.Panel)
                {
                    if(itemUnderMouse != null)
                    {
                        if(itemUnderMouse.BaseItem.Stackable == false)
                        {
                            if (itemUnderMouse.BaseItem.Equipped == true)
                                unequip.Visible = true;
                            else if (itemUnderMouse.BaseItem.Equipped == false)
                                equip.Visible = true;
                        }
                        else if (itemUnderMouse.BaseItem.Stackable == true)
                        {
                            if (itemUnderMouse.BaseItem is Consumable)
                                consume.Visible = true;
                        }
                    }
                    drop.Visible = true;

                    PopUpBox.Update(false, Camera.Transformation);

                    if (mouseRec.Intersects(PopUpBox.Rectangle) == false)
                        PopUpBox.Visible = false;
                }
            }

            if (itemUnderMouse != null)
                Player.Info.Text += "\n" + itemUnderMouse.BaseItem.Name + " x " + itemUnderMouse.Quantity;
        }

        private bool Intersects(Rectangle mouseRec, Rectangle itemRec, GameItem item, BoxSource source)
        {
            bool intersects = false;
            if (mouseRec.Intersects(itemRec))
            {
                item.ToolTip.Visible = true;
                if (_mouseState.RightButton == ButtonState.Released && _lastMouseState.RightButton == ButtonState.Pressed)
                {
                    CurrentSource = source;
                    Vector2 mousePos = new(mouseRec.X, mouseRec.Y);

                    //if its in the panel find the correct postion on the screen 
                    if (source == BoxSource.Panel)
                        mousePos = Vector2.Transform(mousePos, Camera.Transformation);

                    PopUpBox.Position = mousePos - new Vector2(5,5);
                    PopUpBox.Visible = true;
                    intersects = true;
                }
            }
            else
                item.ToolTip.Visible = false;

            return intersects;
        }

        public void CreatePopUpBox()
        {
            Texture2D texture = GameManager.DefaultButtonTexture;
            SpriteFont font = GameManager.ToolTipFont;

            PopUpBox = new(Vector2.Zero, GameManager.GamePopUpBoxTexture, 100, 100)
            {
                Visible = false,
            };

            equip = new(texture, font);
            unequip = new(texture, font);
            pickUp = new(texture, font);
            consume = new(texture, font);
            drop = new(texture, font);

            equip.Click += Equip_Click;
            unequip.Click += Unequip_Click;
            pickUp.Click += PickUp_Click;
            consume.Click += Consume_Click;
            drop.Click += Drop_Click;

            PopUpBox.AddButton(equip, "Equip Item");
            PopUpBox.AddButton(unequip, "Unequip Item");
            PopUpBox.AddButton(pickUp, "Pick Up Item");
            PopUpBox.AddButton(consume, "Consume Item");
            PopUpBox.AddButton(drop, "Drop Item");
        }

        private void Consume_Click(object sender, EventArgs e)
        {
            if (itemUnderMouse != null && itemUnderMouse.BaseItem is Consumable)
                Player.ConsumeItem(itemUnderMouse);
        }

        private void Drop_Click(object sender, System.EventArgs e)
        {
            if (itemUnderMouse != null)
            {
                Player.EquippedItems.TryUnequipItem(itemUnderMouse);
                World.CurrentLevel.EntityManager.DroppedLootManager.Add(itemUnderMouse, Player.Position - new Vector2(60, 40));
                _backpack.RemoveItem(itemUnderMouse);
            }
        }

        private void PickUp_Click(object sender, System.EventArgs e)
        {
            if(itemUnderMouse != null && _backpack.AddItem(itemUnderMouse) == true)
                World.CurrentLevel.EntityManager.DroppedLootManager.ItemToRemove.Add(itemUnderMouse);
        }

        private void Unequip_Click(object sender, System.EventArgs e)
        {
            Player.EquippedItems.TryUnequipItem(itemUnderMouse);
        }

        private void Equip_Click(object sender, System.EventArgs e)
        {
            Player.EquippedItems.TryEquipItem(itemUnderMouse);
        }
    }
}
