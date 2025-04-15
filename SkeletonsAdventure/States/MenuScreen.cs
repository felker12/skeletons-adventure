using System;
using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.IO;
using RpgLibrary.WorldClasses;
using RpgLibrary.DataClasses;
using SkeletonsAdventure.GameWorld;
using RpgLibrary.MenuClasses;

namespace SkeletonsAdventure.States
{
    public class MenuScreen : State
    {
        public MenuScreen(Game1 game) : base(game)
        {
            var buttonTexture = Content.Load<Texture2D>("Controls/Button");
            var buttonFont = Content.Load<SpriteFont>("Fonts/Font");

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
            TabbedMenuData tabbedMenuData;

            try
            {
                string savePath = GameManager.SavePath;

                if (Directory.Exists(savePath) == false)
                {
                    Directory.CreateDirectory(savePath);
                }

                worldData = XnaSerializer.Deserialize<WorldData>(savePath + @"\World.xml");
                tabbedMenuData = XnaSerializer.Deserialize<TabbedMenuData>(savePath + @"\Settings.xml");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);//TODO: Handle exception
                return;
            }

            Game.GameScreen = new(Game, worldData)
            {
                //TabbedMenu = new(tabbedMenuData)
            };
            Game.GameScreen.TabbedMenu.SetTabbedMenuData(tabbedMenuData);

            StateManager.ChangeState(Game.GameScreen);
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
            ControlManager.Update(gameTime, playerIndexInControl);
        }

        private void QuitGameButton_Click(object sender, EventArgs e)
        {
            Game.Exit();
        }
    }
}
