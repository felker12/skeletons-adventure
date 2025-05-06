using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.GameWorld;
using MonoGame.Extended.Tiled;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.ItemClasses;
using SkeletonsAdventure.Entities;
using System;
using RpgLibrary.ItemClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.Engines;
using SkeletonsAdventure.GameMenu;
using System.Collections.Generic;
using RpgLibrary.MenuClasses;

namespace SkeletonsAdventure.States
{
    enum BoxSource { Game, Panel }
    public class GameScreen : State
    {
        private Viewport _sidePanel;
        private Backpack _backpack;
        private MouseState _mouseState, _lastMouseState;
        private Button equip, unequip, pickUp, drop, consume;
        private GameItem itemUnderMouse = null;

        private BoxSource CurrentSource { get; set; }
        public Camera Camera { get; set; }
        public World World { get; private set; }
        public Player Player { get; set; }
        public List<BaseMenu> Menus { get; set; } = [];
        public PopUpBox PopUpBox { get; private set; }
        public TabbedMenu TabbedMenu { get; set; }
        public BaseMenu SettingsMenu { get; private set; }
        public InfoPanel InfoPanel { get; set; }
        private static int InfoPanelWidth { get; set; }

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
            TiledMap backsplash = Content.Load<TiledMap>(@"TiledFiles/SidePanel");
            InfoPanelWidth = backsplash.WidthInPixels;

            World = new(Content, GraphicsDevice);
            Camera = World.CurrentLevel.Camera;
            Player = World.CurrentLevel.Player;
            _backpack = World.CurrentLevel.EntityManager.Player.Backpack;

            _sidePanel = new(Game1.ScreenWidth - InfoPanelWidth, 0, InfoPanelWidth, Game1.ScreenHeight);
            InfoPanel = new(_sidePanel, _backpack.Items, GraphicsDevice, backsplash);

            CreatePopUpBox();
            CreateTabbedMenu();

            Menus = [TabbedMenu, InfoPanel];
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.DarkCyan);

            World.Draw(spriteBatch);

            //Draw the sidepanel
            GraphicsDevice.Viewport = _sidePanel;
            InfoPanel.Draw(spriteBatch);

            GraphicsDevice.Viewport = Game1.GameViewport;

            //Draw the PopUpBox //TODO fix this so the if statement isn't needed
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

            TabbedMenu.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime) { }
        public override void Update(GameTime gameTime)
        {
            World.Update(gameTime);
            InfoPanel.Update(_backpack.Items);

            CheckUnderMouse();
            HandleInput();

            //TODO
            if (InfoPanel.Visible)
            {
                //Game1.ScreenWidth = 1280 + InfoPanelWidth;
                //Camera.Width = Game1.ScreenWidth;
            }
            else
            {
                //Game1.ScreenWidth = 1280;
                //Camera.Width = Game1.ScreenWidth;
            }

            //Game1.Graphics.PreferredBackBufferWidth = Game1.ScreenWidth;
            //Game1.Graphics.ApplyChanges();

            TabbedMenu.Update(gameTime);
            //SettingsMenu.Update(gameTime);
        }

        private void HandleInput()
        {
            if (InputHandler.KeyReleased(Keys.B))
            {
                if (InfoPanel.Visible == true)
                    InfoPanel.Visible = false;
                else if (InfoPanel.Visible == false)
                    InfoPanel.Visible = true;
            }

            if (InputHandler.KeyReleased(Keys.I))
            {
                TabbedMenu.ToggleVisibility();

                //System.Diagnostics.Debug.WriteLine(TabbedMenu.GetTabbedMenuData().ToString());//TODO
                foreach(MenuData menuDatas in TabbedMenu.GetTabbedMenuData().MenuDatas)
                {
                    System.Diagnostics.Debug.WriteLine(menuDatas.ToString());
                }

                System.Diagnostics.Debug.WriteLine(TabbedMenu.GetTabbedMenuData().MenuDatas.ToString());//TODO
            }
        }

        private void CheckUnderMouse()
        {
            _lastMouseState = _mouseState;
            _mouseState = Mouse.GetState();
            Vector2 tempV,
                //Mouse position in the world and their position is relative to that viewport
                pos = Vector2.Transform(new(_mouseState.X, _mouseState.Y), Matrix.Invert(Camera.Transformation));
            Rectangle transformedMouseRectangle = new((int)pos.X, (int)pos.Y, 1, 1),
                mouseRec = new(_mouseState.X, _mouseState.Y, 1, 1),
                tempR;

            itemUnderMouse = null;

            if (InfoPanel.Visible)
            {
                foreach (GameItem item in InfoPanel.Items)
                {
                    //calculate the transformed position so we can find where the items are based on a world position
                    tempV = Vector2.Transform(item.Position, Matrix.Invert(Camera.Transformation));
                    tempV += new Vector2(InfoPanel.Position.X, 0); //offset the world position with the width of the game viewport
                    tempR = new((int)tempV.X, (int)tempV.Y, GameItem.Width, GameItem.Height);

                    Intersects(transformedMouseRectangle, tempR, item, BoxSource.Panel);

                    if (PopUpBox.Visible)
                    {
                        Vector2 tempPopUpPos = Vector2.Transform(PopUpBox.Position, Matrix.Invert(Camera.Transformation));
                        Rectangle rec = new((int)tempPopUpPos.X, (int)tempPopUpPos.Y, 1, 1);
                        if (rec.Intersects(tempR))
                            itemUnderMouse = item;
                    }
                    else if (transformedMouseRectangle.Intersects(tempR))
                        itemUnderMouse = item;
                }
            }

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

            if (PopUpBox.Visible)
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
                    if (itemUnderMouse != null)
                    {
                        if (itemUnderMouse.BaseItem.Stackable == false)
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

                    PopUpBox.Position = mousePos;
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

            PopUpBox = new(Vector2.Zero, GameManager.PopUpBoxTexture, 100, 100)
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

        private void CreateTabbedMenu()
        {
            //Create the container menu=======================
            //Texture2D texture = new(GraphicsDevice, 1, 1);
            //texture.SetData([new Color(171, 144, 91, 250)]);
            

            Texture2D test = new(GraphicsDevice, 1, 1);
            test.SetData([Color.White]);

            TabbedMenu = new()
            {
                Visible = true,
                Title = "TabbedMenu",
            };
            TabbedMenu.SetBackgroundColor(new Color(171, 144, 91, 250));
            //=================================================

            //create the child menus for the tabbed menu=======
            Texture2D texture = new(GraphicsDevice, 1, 1);
            //texture.SetData([new Color(171,144,91,250)]);
            texture.SetData([new Color(3, 23, 64, 250)]);

            SettingsMenu = new()
            {
                Visible = true,
                Position = new(800, 300),
                Title = "Settings",
            };
            SettingsMenu.SetBackgroundColor(new Color(3, 23, 64, 250));
            TabbedMenu.AddMenu(SettingsMenu);

            texture = new(GraphicsDevice, 1, 1);
            texture.SetData([Color.Wheat]);
            BaseMenu testMenu = new()
            {
                Visible = true,
                Position = new(800, 300),
                Title = "Test",
            };
            testMenu.SetBackgroundColor(Color.Wheat);
            TabbedMenu.AddMenu(testMenu);

            texture = new(GraphicsDevice, 1, 1);
            texture.SetData([Color.Ivory]);
            BaseMenu testMenu2 = new()
            {
                Visible = true,
                Position = new(800, 300),
                Title = "Test2",
            };
            testMenu2.SetBackgroundColor(Color.Ivory);
            TabbedMenu.AddMenu(testMenu2);

            TabbedMenu.TabBar.SetActiveTab(SettingsMenu);
            //=================================================
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
            if (itemUnderMouse != null && _backpack.AddItem(itemUnderMouse) == true)
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
