using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using System;
using System.IO;
using RpgLibrary.DataClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.GameWorld;
using RpgLibrary.MenuClasses;
using SkeletonsAdventure.GameMenu;
using RpgLibrary.AttackData;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.Attacks;
using RpgLibrary.EntityClasses;

namespace SkeletonsAdventure.States
{
    public class ExitScreen : State
    {
        private SettingsMenu SettingsMenu { get; set; }

        private readonly string savePath = GameManager.SavePath;

        public ExitScreen(Game1 game) : base(game)
        {
            SettingsMenu = new()
            {
                Position = new(0, 0),
                Visible = true,
                Width = Game1.ScreenWidth,
                Height = Game1.ScreenHeight,
            };
            SettingsMenu.TabBar.Width = Game1.ScreenWidth;
            SettingsMenu.SetBackgroundColor(Color.MidnightBlue);
            SettingsMenu.TabBar.SetAllTabsColors(Color.MidnightBlue);

            //Add event handlers to the controls in the Settings Menu
            SettingsMenu.StartLabel.Selected += StartLabel_Selected;
            SettingsMenu.MenuLabel.Selected += MenuLabel_Selected; 
            SettingsMenu.SaveGameButton.Click += SaveGameButton_Clicked;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            spriteBatch.Begin();

            ControlManager.Draw(spriteBatch);

            spriteBatch.End();

            SettingsMenu.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime){}

        private void SaveGame()
        {
            try
            {
                if (Directory.Exists(savePath) == false)
                {
                    Directory.CreateDirectory(savePath);
                }

                MenuManagerData GameScreenMenuData = new();
                foreach (BaseMenu baseMenu in Game.GameScreen.Menus)
                {
                    if (baseMenu is TabbedMenu tabbedMenu)
                        GameScreenMenuData.Menus.Add(tabbedMenu.GetTabbedMenuData());
                    else if (baseMenu is not null)
                        GameScreenMenuData.Menus.Add(baseMenu.GetMenuData());
                }

                TabbedMenuData ExitScreenData = SettingsMenu.GetTabbedMenuData();

                //save the data
                XnaSerializer.Serialize<WorldData>(savePath + @"\World.xml", Game.GameScreen.World.GetWorldData());
                XnaSerializer.Serialize<MenuManagerData>(savePath + @"\GameScreenMenuData.xml", GameScreenMenuData);
                XnaSerializer.Serialize<TabbedMenuData>(savePath + @"\ExitScreenData.xml", ExitScreenData);
            }
            catch (Exception ex)
            {
                //TODO handle exception
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }
        }

        public void SetSettingsMenuData(TabbedMenuData settingsMenu)
        {
            SettingsMenu.SetMenuData(settingsMenu);
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, playerIndexInControl);
            SettingsMenu.Update(gameTime);
        }

        private void MenuLabel_Selected(object sender, EventArgs e)
        {
            SaveGame();
            StateManager.ChangeState(new MenuScreen(Game));
        }

        private void StartLabel_Selected(object sender, EventArgs e)
        {
            SaveGame();
            StateManager.ChangeState(Game.GameScreen);
        }

        private void SaveGameButton_Clicked(object sender, EventArgs e)
        {
            SaveGame();
        }
    }
}
