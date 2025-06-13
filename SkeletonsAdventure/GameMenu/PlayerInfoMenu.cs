using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.Entities;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;

namespace SkeletonsAdventure.GameMenu
{
    internal class PlayerInfoMenu : BaseMenu
    {
        public Player Player { get; private set; }

        // This menu is used to display player information such as stats, and such.
        private readonly Texture2D buttonTexture = GameManager.ButtonTexture;
        private readonly SpriteFont buttonFont = GameManager.Arial12;

        //Controls for displaying player information can be added here
        Label AttributePointsLbl, BaseAttackLbl, BaseDefenceLbl,
            BaseHealthLbl, BaseManaLbl, AttackLbl, DefenceLbl,
            HealthLbl, ManaLbl, AttackToAddLabel,
            DefenceToAddLabel, HealthToAddLabel, ManaToAddLabel;

        Button ApplyBtn, CancelBtn, IncreaseAttackBtn, IncreaseDefenceBtn, IncreaseHealthBtn, IncreaseManaBtn;

        private int currentAttributePoints,
            attackToAdd = 0, defenceToAdd = 0, healthToAdd = 0, manaToAdd = 0;

        public PlayerInfoMenu() : base()
        {
            ControlManager = new(GameManager.Arial14);
            CreateControls();
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
            currentAttributePoints = player.AttributePoints;

            UpdateLabelText(player);
            SetPositions(); 
            CheckEnoughAttributePoints();
        }

        public void ResetAttributePoints()
        {
            attackToAdd = 0;
            defenceToAdd = 0;
            healthToAdd = 0;
            manaToAdd = 0;
            SetToAddLabelsText();
        }

        private void UpdateLabelText(Player player)
        {
            //Update the labels with the player's information           
            AttributePointsLbl.Text = "Attribute Points: " + player.AttributePoints.ToString(); //TODO change this to currentAttributePoints
            BaseAttackLbl.Text = "Base Attack: " + player.baseAttack.ToString();
            BaseDefenceLbl.Text = "Base Defence: " + player.baseDefence.ToString();
            BaseHealthLbl.Text = "Base Health: " + player.baseHealth.ToString();
            BaseManaLbl.Text = "Base Mana: " + player.BaseMana.ToString();

            AttackLbl.Text = $"Total Attack: {player.Attack}, Base Attack: {player.baseAttack}, " +
                $"Attack From Level: {player.bonusAttackFromLevel}, " +
                $"Attack From Attributes: {player.bonusAttackFromAttributePoints}, " +
                $"Attack From Equipment: {player.EquippedItems.EquippedItemsAttackBonus()}";
            DefenceLbl.Text = $"Total Defence: {player.Defence}, Base Defence: {player.baseDefence}, " +
                $"Defence From Level: {player.bonusDefenceFromLevel}, " +
                $"Defence From Attributes: {player.bonusDefenceFromAttributePoints}, " +
                $"Defence From Equipment: {player.EquippedItems.EquippedItemsDefenceBonus()}";
            HealthLbl.Text = $"Total Health: {player.Health}, Base Health: {player.baseHealth}, " +
                $"Health From Level: {player.bonusHealthFromLevel}, " +
                $"Health From Attributes: {player.bonusHealthFromAttributePoints}, " +
                $"Health From Equipment: {player.EquippedItems.EquippedItemsHealthBonus()}";
            ManaLbl.Text = $"Mana: {player.Mana}, Base Mana: {player.BaseMana}, " +
                $"Mana From Level: {player.bonusManaFromLevel}, " +
                $"Mana From Attributes: {player.bonusManaFromAttributePoints}, " +
                $"Mana From Equipment: {player.EquippedItems.EquippedItemsManaBonus()}";

            SetToAddLabelsText();
        }

        private void SetToAddLabelsText()
        {
            AttackToAddLabel.Text = attackToAdd.ToString();
            DefenceToAddLabel.Text = defenceToAdd.ToString();
            HealthToAddLabel.Text = healthToAdd.ToString();
            ManaToAddLabel.Text = manaToAdd.ToString();
            AttributePointsLbl.Text = "Attribute Points: " + currentAttributePoints.ToString();
        }

        private void CheckEnoughAttributePoints()
        {
            if(currentAttributePoints == 0)
            {
                IncreaseAttackBtn.Enabled = false;
                IncreaseDefenceBtn.Enabled = false;
                IncreaseHealthBtn.Enabled = false;
                IncreaseManaBtn.Enabled = false;
            }
            else
            {
                IncreaseAttackBtn.Enabled = true;
                IncreaseDefenceBtn.Enabled = true;
                IncreaseHealthBtn.Enabled = true;
                IncreaseManaBtn.Enabled = true;
            }
        }

        private void SetPositions()
        {
            int labelSpace = 10;

            //get the longest label width to position the buttons correctly
            List<Label> labels = [BaseAttackLbl, BaseDefenceLbl, BaseHealthLbl, BaseManaLbl];
            int longestLbl = GetLongestLabel(labels);

            //Position the controls
            AttributePointsLbl.Position = new(Position.X + labelSpace, Position.Y + 15);
            BaseAttackLbl.Position = AttributePointsLbl.Position +
                new Vector2(0, AttributePointsLbl.SpriteFont.LineSpacing + labelSpace);
            BaseDefenceLbl.Position = BaseAttackLbl.Position +
                new Vector2(0, BaseAttackLbl.SpriteFont.LineSpacing + labelSpace);
            BaseHealthLbl.Position = BaseDefenceLbl.Position +
                new Vector2(0, BaseDefenceLbl.SpriteFont.LineSpacing + labelSpace);
            BaseManaLbl.Position = BaseHealthLbl.Position +
                new Vector2(0, BaseHealthLbl.SpriteFont.LineSpacing + labelSpace);

            ApplyBtn.Position = BaseManaLbl.Position +
                new Vector2(0, BaseManaLbl.SpriteFont.LineSpacing + 20);
            CancelBtn.Position = ApplyBtn.Position +
                new Vector2(CancelBtn.Width + labelSpace, 0);

            AttackLbl.Position = new Vector2(BaseManaLbl.Position.X,
                CancelBtn.Position.Y + CancelBtn.SpriteFont.LineSpacing + CancelBtn.Height + labelSpace);
            DefenceLbl.Position = AttackLbl.Position +
                new Vector2(0, AttackLbl.SpriteFont.LineSpacing + labelSpace);
            HealthLbl.Position = DefenceLbl.Position +
                new Vector2(0, DefenceLbl.SpriteFont.LineSpacing + labelSpace);
            ManaLbl.Position = HealthLbl.Position +
                new Vector2(0, HealthLbl.SpriteFont.LineSpacing + labelSpace);

            IncreaseAttackBtn.Position = new(labelSpace * 2 + longestLbl, BaseAttackLbl.Position.Y);
            IncreaseDefenceBtn.Position = new(labelSpace * 2 + longestLbl, BaseDefenceLbl.Position.Y);
            IncreaseHealthBtn.Position = new(labelSpace * 2 + longestLbl, BaseHealthLbl.Position.Y);
            IncreaseManaBtn.Position = new(labelSpace * 2 + longestLbl, BaseManaLbl.Position.Y);

            AttackToAddLabel.Position = IncreaseAttackBtn.Position +
                new Vector2(IncreaseAttackBtn.Width + labelSpace, 0);
            DefenceToAddLabel.Position = IncreaseDefenceBtn.Position +
                new Vector2(IncreaseDefenceBtn.Width + labelSpace, 0);
            HealthToAddLabel.Position = IncreaseHealthBtn.Position +
                new Vector2(IncreaseHealthBtn.Width + labelSpace, 0);
            ManaToAddLabel.Position = IncreaseManaBtn.Position +
                new Vector2(IncreaseManaBtn.Width + labelSpace, 0);
        }

        private static int GetLongestLabel(List<Label> labels)
        {
            int longest = 0;

            foreach(Label lbl in labels)
            {
                if(lbl.SpriteFont.MeasureString(lbl.Text).X > longest)
                {
                    longest = (int)lbl.SpriteFont.MeasureString(lbl.Text).X;
                }
            }

            return longest;
        }

        public void CreateControls()
        {
            AttributePointsLbl = new(); 
            BaseAttackLbl = new(); 
            BaseDefenceLbl = new();
            BaseHealthLbl = new(); 
            BaseManaLbl = new(); 
            AttackLbl = new(); 
            DefenceLbl = new();
            HealthLbl = new(); 
            ManaLbl = new(); 
            AttackToAddLabel = new();
            DefenceToAddLabel = new(); 
            HealthToAddLabel = new(); 
            ManaToAddLabel = new();

            //Initialize the buttons
            ApplyBtn = new(buttonTexture, buttonFont)
            {
                Text = "Apply",
                Width = 80,
            };
            CancelBtn = new(buttonTexture, buttonFont) 
            { 
                Text = "Cancel",
                Width = 80,
            };
            IncreaseAttackBtn = new(buttonTexture, buttonFont)
            {
                Text = " +1 ",
            };
            IncreaseAttackBtn.Height = (int)IncreaseAttackBtn.SpriteFont.MeasureString(IncreaseAttackBtn.Text).Y;
            IncreaseAttackBtn.Width = (int)IncreaseAttackBtn.SpriteFont.MeasureString(IncreaseAttackBtn.Text).X;

            IncreaseDefenceBtn = new(buttonTexture, buttonFont)
            {
                Text = " +1 ",
            };
            IncreaseDefenceBtn.Height = (int)IncreaseDefenceBtn.SpriteFont.MeasureString(IncreaseDefenceBtn.Text).Y;
            IncreaseDefenceBtn.Width = (int)IncreaseDefenceBtn.SpriteFont.MeasureString(IncreaseDefenceBtn.Text).X;

            IncreaseHealthBtn = new(buttonTexture, buttonFont)
            {
                Text = " +1 ",
            };
            IncreaseHealthBtn.Height = (int)IncreaseHealthBtn.SpriteFont.MeasureString(IncreaseHealthBtn.Text).Y;
            IncreaseHealthBtn.Width = (int)IncreaseHealthBtn.SpriteFont.MeasureString(IncreaseHealthBtn.Text).X;

            IncreaseManaBtn = new(buttonTexture, buttonFont)
            {
                Text = " +1 ",
            };
            IncreaseManaBtn.Height = (int)IncreaseManaBtn.SpriteFont.MeasureString(IncreaseManaBtn.Text).Y;
            IncreaseManaBtn.Width = (int)IncreaseManaBtn.SpriteFont.MeasureString(IncreaseManaBtn.Text).X;

            //Add the event handlers
            ApplyBtn.Click += ApplyBtn_Click;
            CancelBtn.Click += CancelBtn_Click;
            IncreaseAttackBtn.Click += IncreaseAttackBtn_Click;
            IncreaseDefenceBtn.Click += IncreaseDefenceBtn_Click;
            IncreaseHealthBtn.Click += IncreaseHealthBtn_Click;
            IncreaseManaBtn.Click += IncreaseManaBtn_Click;

            //Add the labels to the control manager
            ControlManager.Add(AttributePointsLbl);
            ControlManager.Add(BaseAttackLbl);
            ControlManager.Add(BaseDefenceLbl);
            ControlManager.Add(BaseHealthLbl);
            ControlManager.Add(BaseManaLbl);
            ControlManager.Add(AttackLbl);
            ControlManager.Add(DefenceLbl);
            ControlManager.Add(HealthLbl);
            ControlManager.Add(ManaLbl);
            ControlManager.Add(AttackToAddLabel);
            ControlManager.Add(DefenceToAddLabel);
            ControlManager.Add(HealthToAddLabel);
            ControlManager.Add(ManaToAddLabel);

            //Add the buttons to the control manager
            ControlManager.Add(ApplyBtn);
            ControlManager.Add(CancelBtn);
            ControlManager.Add(IncreaseAttackBtn);
            ControlManager.Add(IncreaseDefenceBtn);
            ControlManager.Add(IncreaseHealthBtn);
            ControlManager.Add(IncreaseManaBtn);
        }

        private void CancelBtn_Click(object sender, EventArgs e)
        {
            ResetAttributePoints();
            currentAttributePoints = Player.AttributePoints;
            CheckEnoughAttributePoints();
            UpdateLabelText(Player);
        }

        private void ApplyBtn_Click(object sender, EventArgs e)
        {
            Player.ApplyAttributePoints(attackToAdd, defenceToAdd, healthToAdd, manaToAdd);
            currentAttributePoints = Player.AttributePoints;
            UpdateLabelText(Player); 
            ResetAttributePoints();
        }

        private void IncreaseManaBtn_Click(object sender, EventArgs e)
        {
            if (currentAttributePoints > 0 is false)
                return;

            manaToAdd++;
            currentAttributePoints--;
            SetToAddLabelsText();
            CheckEnoughAttributePoints();
        }

        private void IncreaseHealthBtn_Click(object sender, EventArgs e)
        {
            if (currentAttributePoints > 0 is false)
                return;

            healthToAdd++;
            currentAttributePoints--;
            SetToAddLabelsText(); 
            CheckEnoughAttributePoints();
        }

        private void IncreaseDefenceBtn_Click(object sender, EventArgs e)
        {
            if (currentAttributePoints > 0 is false)
                return;

            defenceToAdd++;
            currentAttributePoints--;
            SetToAddLabelsText();
            CheckEnoughAttributePoints();
        }
        private void IncreaseAttackBtn_Click(object sender, EventArgs e)
        {
            if (currentAttributePoints > 0 is false)
                return;

            attackToAdd++;
            currentAttributePoints--;
            SetToAddLabelsText();
            CheckEnoughAttributePoints();
        }
    }
}
