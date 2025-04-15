using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using System;
using System.IO;
using RpgLibrary.DataClasses;
using RpgLibrary.WorldClasses;
using SkeletonsAdventure.GameWorld;
using RpgLibrary.MenuClasses;

namespace SkeletonsAdventure.States
{
    public class ExitScreen : State
    {
        readonly Label title;
        readonly LinkLabel startLabel, menuLabel;

        public ExitScreen(Game1 game) : base(game)
        {
            title = new Label
            {
                Text = "The Adventures of The Skeleton",
                Color = Color.White
            };
            title.Position = new Vector2(Game1.ScreenWidth / 2 - (title.SpriteFont.MeasureString(title.Text)).X / 2, 30);
            ControlManager.Add(title);

            startLabel = new LinkLabel
            {
                Text = "Press ENTER to return to the game and save",
                Color = Color.White,
                TabStop = true,
                HasFocus = true
            };
            startLabel.Position = new Vector2(Game1.ScreenWidth / 2 - startLabel.SpriteFont.MeasureString(startLabel.Text).X / 2, 350);
            startLabel.Selected += new EventHandler(StartLabel_Selected);
            ControlManager.Add(startLabel);

            menuLabel = new LinkLabel
            {
                Text = "Press to save and retrun to menu screen",
                Color = Color.White,
                TabStop = true,
                HasFocus = true
            };
            menuLabel.Position = new Vector2(Game1.ScreenWidth / 2 - menuLabel.SpriteFont.MeasureString(menuLabel.Text).X / 2, 
                startLabel.Position.Y + startLabel.SpriteFont.MeasureString(startLabel.Text).Y + 20);
            menuLabel.Selected += MenuLabel_Selected;
            ControlManager.Add(menuLabel);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            GraphicsDevice.Clear(Color.MidnightBlue);

            spriteBatch.Begin();

            ControlManager.Draw(spriteBatch);

            spriteBatch.End();
        }

        public override void PostUpdate(GameTime gameTime)
        {
        }

        public override void Update(GameTime gameTime)
        {
            ControlManager.Update(gameTime, playerIndexInControl);
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

        private void SaveGame()
        {
            try
            {
                string savePath = GameManager.SavePath;

                if (Directory.Exists(savePath) == false)
                {
                    Directory.CreateDirectory(savePath);
                }

                XnaSerializer.Serialize<WorldData>(savePath + @"\World.xml", Game.GameScreen.World.GetWorldData());
                XnaSerializer.Serialize<TabbedMenuData>(savePath + @"\Settings.xml", Game.GameScreen.TabbedMenu.GetTabbedMenuData());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }
        }
    }
}
