using System;
using SkeletonsAdventure.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Data;
using System.IO;
using RpgLibrary.WorldClasses;

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
            WorldData worldData = new();

            try
            {
                string gamePath = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName; //Project Directory
                string savePath = Path.GetFullPath(Path.Combine(gamePath, @"..\SaveFiles")); //Directory of the saved files
                string path = string.Empty;

                if (Directory.Exists(savePath))
                {
                    path = savePath + @"\World.xml";
                    worldData = XnaSerializer.Deserialize<WorldData>(path);
                }
                else
                {
                    Directory.CreateDirectory(savePath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
                return;
            }

            Game.GameScreen = new(Game, worldData);
            StateManager.ChangeState(Game.GameScreen);
        }

        private void NewGameButton_Click(object sender, EventArgs e)
        {
            Game.GameScreen = new(Game);
            Game.GameScreen.World.FillPlayerBackback(); //TODO

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
