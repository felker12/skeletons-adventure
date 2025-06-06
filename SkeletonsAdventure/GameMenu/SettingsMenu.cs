using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameMenu
{
    internal class SettingsMenu : TabbedMenu
    {
        public BaseMenu SaveMenu { get; set; }
        public BaseMenu Settings { get; private set; }
        public PlayerInfoMenu PlayerMenu { get; set; }
        public BaseMenu QuestMenu { get; set; }

        public LinkLabel StartLabel { get; set; }
        public LinkLabel MenuLabel { get; set; }

        private readonly Texture2D buttonTexture = GameManager.ButtonTexture;
        private readonly SpriteFont buttonFont = GameManager.Arial12;

        public Button SaveGameButton { get; set; }

        public SettingsMenu() : base()
        {
            Visible = false;
            Title = "SettingsMenu";

            CreateTabbedMenu();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        private void CreateTabbedMenu()
        {
            //create the child menus for the tabbed menu
            CreateSaveMenu();
            CreateSettingsMenu();
            CreatePlayerMenu();
            CreateQuestsMenu();

            TabBar.SetActiveTab(SaveMenu); //Set the active tab

            //Add the menus to the tab bar
            AddMenu(SaveMenu);
            AddMenu(Settings);
            AddMenu(PlayerMenu);
            AddMenu(QuestMenu);
        }

        private void CreateSaveMenu()
        {
            SaveMenu = new()
            {
                Visible = true,
                Title = "SaveMenu",
            };
            SaveMenu.SetBackgroundColor(Color.MidnightBlue);

            //Add controls to the Menu
            Label title = new()
            {
                Text = "The Adventures of The Skeleton",
                Color = Color.White
            };
            title.Position = new Vector2(Game1.ScreenWidth / 2 - (title.SpriteFont.MeasureString(title.Text)).X / 2, 30);
            ControlManager.Add(title);

            StartLabel = new()
            {
                Text = "Press ENTER to return to the game and save",
                TabStop = true,
                HasFocus = true
            };
            StartLabel.Position = new Vector2(Game1.ScreenWidth / 2 - StartLabel.SpriteFont.MeasureString(StartLabel.Text).X / 2, 350);
            SaveMenu.ControlManager.Add(StartLabel);

            MenuLabel = new()
            {
                Text = "Press to save and return to menu screen",
                TabStop = true,
                HasFocus = true
            };
            MenuLabel.Position = new Vector2(Game1.ScreenWidth / 2 - MenuLabel.SpriteFont.MeasureString(MenuLabel.Text).X / 2,
                StartLabel.Position.Y + StartLabel.SpriteFont.MeasureString(StartLabel.Text).Y + 20);
            SaveMenu.ControlManager.Add(MenuLabel);
        }

        private void CreateSettingsMenu()
        {
            Settings = new()
            {
                Visible = true,
                Title = "Settings",
            };
            Settings.SetBackgroundColor(Color.MidnightBlue);
        }

        private void CreatePlayerMenu()
        {
            PlayerMenu = new()
            {
                Visible = true,
                Title = "Test2",
            };
            PlayerMenu.SetBackgroundColor(Color.MidnightBlue);
        }

        private void CreateQuestsMenu()
        {
            QuestMenu = new()
            {
                Visible = true,
                Title = "Test3",
            };
            QuestMenu.SetBackgroundColor(Color.MidnightBlue);

            SaveGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Save Game",
            };
            QuestMenu.ControlManager.Add(SaveGameButton);
        }
    }
}
