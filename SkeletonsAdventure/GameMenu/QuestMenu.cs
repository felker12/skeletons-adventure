using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using SkeletonsAdventure.Controls;
using System;
using System.Collections.Generic;
using SkeletonsAdventure.Quests;
using SkeletonsAdventure.GameUI;

namespace SkeletonsAdventure.GameMenu
{
    internal class QuestMenu : BaseMenu
    {
        //TODO this menu is used to display the player's quests
        public Player Player { get; set; } = World.Player;

        private Label QuestsLbl;

        private ToggleButton CompletedQuestsToggle, StartedQuestsToggle;

        private TextBox QuestsTxtBox;
        private ButtonBox QuestButtonBox;
        private LinkLabelBox QuestLinkLabelBox;
        private SelectionControlBox QuestSelectionControlBox;

        public QuestMenu() : base()
        {
            Initialize();
        }

        public QuestMenu(int width, int height) : base(width, height)
        {
            Initialize();
        }

        private void Initialize()
        {
            CreateControls();
            SetPositions();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime); 
            QuestButtonBox.Update(gameTime);
            QuestLinkLabelBox.Update(gameTime);
            QuestSelectionControlBox.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            spriteBatch.Begin();
            QuestButtonBox.Draw(spriteBatch);
            QuestLinkLabelBox.Draw(spriteBatch);
            QuestSelectionControlBox.Draw(spriteBatch);
            spriteBatch.End();
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            QuestSelectionControlBox.HandleInput(playerIndex);
        }

        public void UpdateWithPlayer(Player player)
        {
            Player = player;
        }

        public override void MenuOpened()
        {
            UpdateWithPlayer(World.Player); 
            SetPositions(); 
            LoadQuests();
        }

        private void CreateControls()
        {
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

            //Create the ButtonBox
            QuestButtonBox = new()
            {
                Visible = true,
                Texture = GameManager.TextBoxTexture,
                DrawOutline = true,
            };

            QuestLinkLabelBox = new()
            {
                Visible = true,
                Texture = GameManager.TextBoxTexture,
                DrawOutline = true,
            };


            QuestSelectionControlBox = new()
            {
                Visible = true,
                Texture = GameManager.TextBoxTexture,
                DrawOutline = true,
            };
        }

        private void SetPositions()
        {
            int labelSpace = 10;

            QuestsLbl.Position = new(Position.X + 15, Position.Y + 15);

            CompletedQuestsToggle.Position = new(QuestsLbl.Position.X, QuestsLbl.Position.Y + QuestsLbl.SpriteFont.LineSpacing + labelSpace);
            StartedQuestsToggle.Position = new(CompletedQuestsToggle.Position.X, CompletedQuestsToggle.Position.Y + CompletedQuestsToggle.Height + labelSpace);

            QuestsTxtBox.Position = new(CompletedQuestsToggle.Position.X + CompletedQuestsToggle.Width + labelSpace, QuestsLbl.Position.Y);
            QuestsTxtBox.Width = Width - (int)QuestsTxtBox.Position.X - labelSpace*3;
            QuestsTxtBox.Height = 120;

            QuestButtonBox.Position = new(QuestsTxtBox.Position.X, QuestsTxtBox.Position.Y + QuestsTxtBox.Height + labelSpace);
            QuestButtonBox.Width = QuestsTxtBox.Width;
            QuestButtonBox.Height = (int)(120);

            QuestLinkLabelBox.Position = new(QuestsTxtBox.Position.X, QuestButtonBox.Position.Y + QuestButtonBox.Height + labelSpace);
            QuestLinkLabelBox.Width = QuestButtonBox.Width;
            QuestLinkLabelBox.Height = (int)(120);

            QuestSelectionControlBox.Position = new(QuestLinkLabelBox.Position.X, QuestLinkLabelBox.Position.Y + QuestLinkLabelBox.Height + labelSpace);
            QuestSelectionControlBox.Width = QuestLinkLabelBox.Width;
            QuestSelectionControlBox.Height = (int)(120);
        }

        private void LoadQuests()
        {
            List<Quest> quests = [.. GameManager.QuestsClone.Values];
            List<string> questStrings = quests.ConvertAll(q => $"{q.Name}");

            QuestsTxtBox.Text = string.Join(Environment.NewLine, questStrings);

            QuestButtonBox.Buttons.Clear();
            QuestLinkLabelBox.LinkLabels.Clear();
            QuestLinkLabelBox.ActiveLinkLabel = null;

            Button btn;
            LinkLabel lbl;
            SelectionControl control;
            foreach (Quest quest in quests)
            {
                btn = new();
                QuestButtonBox.AddButton(btn, quest.Name);
                btn.Visible = true;
                btn.Texture = GameManager.TextBoxTexture;
                btn.BackgroundColor = GameManager.TextBoxColor;

                lbl = new(quest.Name)
                {
                    TextColor = Color.Black,
                };

                QuestLinkLabelBox.AddLinkLabel(lbl);


                control = new(quest.Name)
                {
                    TextColor = Color.Black,
                };

                QuestSelectionControlBox.AddSelectionControl(control);
            }
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
