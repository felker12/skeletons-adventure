using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameMenu
{
    public class SettingsMenu : TabbedMenu
    {
        public BaseMenu Settings { get; private set; }
        public BaseMenu SaveMenu { get; set; }
        public BaseMenu Menu3 { get; set; }
        public BaseMenu Menu4 { get; set; }

        public LinkLabel StartLabel { get; set; }
        public LinkLabel MenuLabel { get; set; }

        private Texture2D buttonTexture = GameManager.ButtonTexture;
        private SpriteFont buttonFont = GameManager.InfoFont;

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
            //create the child menus for the tabbed menu=======
            //Create the Save Menu
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
            //=================
            AddMenu(SaveMenu);

            //Create the Settings Menu
            Settings = new()
            {
                Visible = true,
                Title = "Settings",
            };
            Settings.SetBackgroundColor(Color.MidnightBlue);
            AddMenu(Settings);

            //Create the Menu
            Menu3 = new()
            {
                Visible = true,
                Title = "Test2",
            };
            Menu3.SetBackgroundColor(Color.MidnightBlue);
            AddMenu(Menu3);

            //Create the Menu
            Menu4 = new()
            {
                Visible = true,
                Title = "Test3",
            };
            Menu4.SetBackgroundColor(Color.MidnightBlue);
            AddMenu(Menu4);

            //Set the Active tab to the default active tab
            TabBar.SetActiveTab(SaveMenu);
            //=================================================
        }
    }
}
