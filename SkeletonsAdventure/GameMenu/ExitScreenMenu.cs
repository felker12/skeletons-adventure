using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;

namespace SkeletonsAdventure.GameMenu
{
    internal class ExitScreenMenu : TabbedMenu
    {
        public BaseMenu SaveMenu { get; set; }
        public BaseMenu Settings { get; private set; }
        public PlayerInfoMenu PlayerMenu { get; set; }
        public QuestMenu QuestMenu { get; set; }
        public LinkLabel ReturnToGameLabel { get; set; }
        public LinkLabel ReturnToMenuLabel { get; set; }

        private readonly Texture2D buttonTexture = GameManager.ButtonTexture;
        private readonly SpriteFont buttonFont = GameManager.Arial12;

        public Button SaveGameButton { get; set; }

        public ExitScreenMenu() : base()
        {
            Visible = false;
            Title = "ExitScreenMenu";

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
            //Create the child menus for the tabbed menu
            CreateSaveMenu();
            CreateSettingsMenu();
            CreatePlayerMenu();
            CreateQuestsMenu();

            //Add the menus to the tab bar
            AddMenu(SaveMenu);
            AddMenu(Settings);
            AddMenu(PlayerMenu);
            AddMenu(QuestMenu);

            TabBar.SetActiveTab(SaveMenu); //Set the active tab

            CreateTabOpenedLogic(); //Needs to be the active tab before this is called so it can update the menus correctly
        }

        private void CreateTabOpenedLogic()
        {
            //Keep the player data displayed up to date any time the tab is opened
            TabBar.TabClicked += (sender, e) =>
            {
                if (TabBar.ActiveMenu == PlayerMenu)
                {
                    PlayerMenu.UpdateWithPlayer(World.Player);
                }
                else if (TabBar.ActiveMenu == QuestMenu)
                {
                    //TODO update quest menu with quest data from player
                    QuestMenu.SetPlayer(World.Player);
                }
            };
        }

        private void CreateSaveMenu()
        {
            SaveMenu = new()
            {
                Visible = true,
                Title = "Save",
            };
            SaveMenu.SetBackgroundColor(Color.MidnightBlue);

            //Add controls to the Menu
            ReturnToGameLabel = new()
            {
                Text = "Press ENTER to return to the game",
                TabStop = true,
                HasFocus = true
            };
            ReturnToGameLabel.Position = new Vector2(Game1.ScreenWidth / 2 - ReturnToGameLabel.SpriteFont.MeasureString(ReturnToGameLabel.Text).X / 2, 350);

            ReturnToMenuLabel = new()
            {
                Text = "Press to save and return to menu screen",
                TabStop = true,
            };
            ReturnToMenuLabel.Position = new Vector2(Game1.ScreenWidth / 2 - ReturnToMenuLabel.SpriteFont.MeasureString(ReturnToMenuLabel.Text).X / 2,
                ReturnToGameLabel.Position.Y + ReturnToGameLabel.SpriteFont.MeasureString(ReturnToGameLabel.Text).Y + 20);

            SaveGameButton = new Button(buttonTexture, buttonFont)
            {
                Position = new Vector2(300, 200),
                Text = "Save Game",
            };

            SaveMenu.ControlManager.Add(ReturnToMenuLabel);
            SaveMenu.ControlManager.Add(ReturnToGameLabel);//Add this label after return to menu label so if enter is pressed the event for this will be triggered instead of the 1 for return to menu
            SaveMenu.ControlManager.Add(SaveGameButton);
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
            PlayerMenu = new PlayerInfoMenu()
            {
                Visible = true,
                Title = "Player",
            };
            PlayerMenu.SetBackgroundColor(Color.MidnightBlue);
        }

        private void CreateQuestsMenu()
        {
            QuestMenu = new()
            {
                Visible = true,
                Title = "Quests",
            };
            QuestMenu.SetBackgroundColor(Color.MidnightBlue);
        }
    }
}
