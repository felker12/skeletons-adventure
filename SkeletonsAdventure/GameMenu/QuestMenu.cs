using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameMenu
{
    internal class QuestMenu : BaseMenu
    {
        //TODO this menu is used to display the player's quests
        public Player Player { get; set; } = World.Player;

        private Label QuestsLbl;

        private ToggleButton CompletedQuestsToggle, StartedQuestsToggle;

        private TextBox QuestsTxtBox;

        public QuestMenu() : base()
        {
            CreateControls(); 
            SetPositions();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void UpdateWithPlayer(Player player)
        {
            Player = player;
        }

        public override void MenuOpened()
        {
            UpdateWithPlayer(World.Player); 
            SetPositions();
        }

        private void CreateControls()
        {
            // Here you can create controls to display the player's quests
            // For example, you could create a list or a grid to show the quests
            // and their statuses (e.g., completed, in progress, not started)


            //Create the labels
            QuestsLbl = new(GameManager.Arial16)
            {
                Text = "Quests",
            };

            //Create the TextBox
            QuestsTxtBox = new()
            {
                Text = "Test",
            };

            //Create the Toggles for the quests
            CompletedQuestsToggle = new()
            {
                Text = "Hide Completed",
                ToggledText = "Show completed",
            };
            CompletedQuestsToggle.Height = (int)CompletedQuestsToggle.SpriteFont.MeasureString(CompletedQuestsToggle.Text).Y + 4;
            CompletedQuestsToggle.Width = (int)CompletedQuestsToggle.SpriteFont.MeasureString(CompletedQuestsToggle.Text).X + 10;

            StartedQuestsToggle = new()
            {
                Text = " Hide Started",
                ToggledText = "Show Started",
                Height = CompletedQuestsToggle.Height,
                Width = CompletedQuestsToggle.Width
            };

            //add the event handlers for the toggles
            CompletedQuestsToggle.Click += CompletedQuestsToggle_Click;
            StartedQuestsToggle.Click += StartedQuestsToggle_Click;

            //Add the controls to the control manager
            ControlManager.Add(QuestsLbl);

            ControlManager.Add(CompletedQuestsToggle);
            ControlManager.Add(StartedQuestsToggle);

            ControlManager.Add(QuestsTxtBox);
        }
        private void SetPositions()
        {
            int labelSpace = 10;

            QuestsLbl.Position = new(Position.X + 15, Position.Y + 15);

            CompletedQuestsToggle.Position = new(QuestsLbl.Position.X, QuestsLbl.Position.Y + QuestsLbl.SpriteFont.LineSpacing + labelSpace);
            StartedQuestsToggle.Position = new(CompletedQuestsToggle.Position.X, CompletedQuestsToggle.Position.Y + CompletedQuestsToggle.Height + labelSpace);

            QuestsTxtBox.Position = new(CompletedQuestsToggle.Position.X + CompletedQuestsToggle.Width + labelSpace, QuestsLbl.Position.Y);
            QuestsTxtBox.Width = Width - (int)QuestsTxtBox.Position.X - labelSpace;
            QuestsTxtBox.Height = 300; //TODO
            QuestsTxtBox.Text = Width.ToString(); //TODO
        }

        private void StartedQuestsToggle_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void CompletedQuestsToggle_Click(object sender, EventArgs e)
        {
            //TODO
        }

    }
}
