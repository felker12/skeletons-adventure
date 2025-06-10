using System;
using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using RpgLibrary.WorldClasses;
using RpgLibrary.DataClasses;
using SkeletonsAdventure.GameWorld;
using RpgLibrary.MenuClasses;
using SkeletonsAdventure.GameMenu;
using Assimp.Configs;

namespace SkeletonsAdventure.States
{
    internal class MenuScreen : State
    {
        public MenuScreen(Game1 game) : base(game)
        {
            var buttonTexture = GameManager.ButtonTexture;
            var buttonFont = GameManager.Arial12;

            var newGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "New Game",
            };

            newGameButton.Click += NewGameButton_Click;

            var loadGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 250),
                Text = "Load Game",
            };

            loadGameButton.Click += LoadGameButton_Click;

            var quitGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 300),
                Text = "Quit Game",
            };

            quitGameButton.Click += QuitGameButton_Click;

            ControlManager.Add(newGameButton);
            ControlManager.Add(loadGameButton);
            ControlManager.Add(quitGameButton);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.CadetBlue);

            spriteBatch.Begin();
            ControlManager.Draw(spriteBatch);
            spriteBatch.End();
        }

        private void LoadGameButton_Click(object sender, EventArgs e)
        {
            WorldData worldData;
            MenuManagerData gameScreenMenuData;
            TabbedMenuData exitScreenData;

            try
            {
                string savePath = GameManager.SavePath;

                if (Directory.Exists(savePath) == false)
                    Directory.CreateDirectory(savePath);

                worldData = XnaSerializer.Deserialize<WorldData>(savePath + @"\World.xml");
                gameScreenMenuData = XnaSerializer.Deserialize<MenuManagerData>(savePath + @"\GameScreenMenuData.xml");
                exitScreenData = XnaSerializer.Deserialize<TabbedMenuData>(savePath + @"\ExitScreenData.xml");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);//TODO: Handle exception
                return;
            }

            //Update the game with the saved information
            Game.GameScreen = new(Game, worldData);
            UpdateMenusFromSave(gameScreenMenuData);
            Game.ExitScreen.SetExitScreenMenuData(exitScreenData);

            //return to the game screen
            StateManager.ChangeState(Game.GameScreen);
        }

        public void UpdateMenusFromSave(MenuManagerData menuManagerData)
        {
            foreach (MenuData menuData in menuManagerData.Menus)
            {
                foreach(BaseMenu gameMenu in Game.GameScreen.Menus)
                {
                    if(menuData.Title == gameMenu.Title)
                    {
                        if (menuData is TabbedMenuData tabbedMenuData && gameMenu is TabbedMenu tabbedMenu)
                        {
                            //update tabbed menus from loaded data
                            tabbedMenu.SetMenuData(tabbedMenuData);
                        }
                        else if (menuData is not null)
                        {
                            //update regular menus from loaded data
                            gameMenu.SetMenuData(menuData);
                        }
                    }
                }
            }
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Game.GameScreen = new(Game);
            World.FillPlayerBackback(); //TODO

            StateManager.ChangeState(Game.GameScreen);
        }

        public override void PostUpdate(GameTime gameTime)
        {
            //remove sprites if they're not needed
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, PlayerIndexInControl);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }
    }
}
