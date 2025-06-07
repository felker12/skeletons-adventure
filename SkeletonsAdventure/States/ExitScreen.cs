using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using RpgLibrary.DataClasses;
using RpgLibrary.MenuClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameMenu;
using SkeletonsAdventure.GameWorld;
using System;
using System.IO;

namespace SkeletonsAdventure.States
{
    internal class ExitScreen : State
    {
        private ExitScreenMenu ExitScreenMenu { get; set; }

        private readonly string savePath = GameManager.SavePath;

        public ExitScreen(Game1 game) : base(game)
        {
            ExitScreenMenu = new()
            {
                Position = new(0, 0),
                Visible = true,
                Width = Game1.ScreenWidth,
                Height = Game1.ScreenHeight,
            };
            ExitScreenMenu.TabBar.Width = Game1.ScreenWidth;
            ExitScreenMenu.SetBackgroundColor(Color.MidnightBlue);
            ExitScreenMenu.TabBar.SetAllTabsColors(Color.MidnightBlue);

            //Add event handlers to the controls in the Settings Menu
            ExitScreenMenu.StartLabel.Selected += StartLabel_Selected;
            ExitScreenMenu.MenuLabel.Selected += MenuLabel_Selected; 
            ExitScreenMenu.SaveGameButton.Click += SaveGameButton_Clicked;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            spriteBatch.Begin();
            ControlManager.Draw(spriteBatch);
            spriteBatch.End();

            ExitScreenMenu.Draw(spriteBatch);
        }

        public override void PostUpdate(GameTime gameTime){}

        private void SaveGame()
        {
            try
            {
                if (Directory.Exists(savePath) == false)
                    Directory.CreateDirectory(savePath);

                MenuManagerData GameScreenMenuData = new();
                foreach (BaseMenu baseMenu in Game.GameScreen.Menus)
                {
                    if (baseMenu is TabbedMenu tabbedMenu)
                        GameScreenMenuData.Menus.Add(tabbedMenu.GetTabbedMenuData());
                    else if (baseMenu is not null)
                        GameScreenMenuData.Menus.Add(baseMenu.GetMenuData());
                }

                //save the data
                XnaSerializer.Serialize<WorldData>(savePath + @"\World.xml", World.GetWorldData());
                XnaSerializer.Serialize<MenuManagerData>(savePath + @"\GameScreenMenuData.xml", GameScreenMenuData);
                XnaSerializer.Serialize<TabbedMenuData>(savePath + @"\ExitScreenData.xml", ExitScreenMenu.GetTabbedMenuData());
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
            ExitScreenMenu.SetMenuData(settingsMenu);
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndexInControl);
            ExitScreenMenu.Update(gameTime);
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
