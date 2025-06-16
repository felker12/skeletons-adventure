using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using System.Collections.Generic;

namespace SkeletonsAdventure.GameUI
{
    internal class SelectionControlBox : ControlBox
    {
        public List<SelectionControl> SelectionControls { get; set; } = [];
        public SelectionControl ActiveSelectionControl { get; set; }

        public SelectionControlBox(Vector2 pos, Texture2D texture, int width, int height) : base(pos, texture, width, height)
        {
        }

        public SelectionControlBox() : base()
        {
            Texture = GameManager.CreateTextureFromColor(Color.LightGray);
            Position = new();
            Width = 600;
            Height = 400;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible is false)
                return;

            spriteBatch.Draw(Texture, Rectangle, Color);

            if (DrawOutline)
                spriteBatch.DrawRectangle(Rectangle, OutlineColor, 2, 0);

            foreach (SelectionControl selectionControl in SelectionControls)
            {
                if (selectionControl.Visible)
                    selectionControl.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible is false)
                return;

            Vector2 offset = ControlOffset;
            foreach (SelectionControl selectionControl in SelectionControls)
            {
                if (selectionControl.Visible)
                {
                    selectionControl.Update(gameTime);
                    selectionControl.Position = Position + offset;
                    offset += new Vector2(0, selectionControl.Height);
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (Visible is false)
                return;

            foreach (SelectionControl selectionControl in SelectionControls)
            {
                if (selectionControl.Visible)
                    selectionControl.HandleInput(playerIndex);
            }
        }

        public void AddSelectionControl(SelectionControl selectionControl)
        {
            if (selectionControl is null)
                return;

            selectionControl.Width = (int)selectionControl.SpriteFont.MeasureString(selectionControl.Text).X;
            selectionControl.Height = (int)selectionControl.SpriteFont.MeasureString(selectionControl.Text).Y;

            selectionControl.Click += SelectionControl_Click;

            SelectionControls.Add(selectionControl);

            if (SelectionControls.Count > 0 && ActiveSelectionControl is null)
            {
                ActiveSelectionControl = SelectionControls[0];
                ActiveSelectionControl.HasFocus = true;
            }
        }

        private void SelectionControl_Click(object sender, System.EventArgs e)
        {
            //TOOD add logic to only have 1 SelectionControl with focus at a time
            System.Diagnostics.Debug.WriteLine("Works");

            if(sender is SelectionControl control)
            {
                ChangeFocus(control);
            }
        }

        public void ChangeFocus(SelectionControl control)
        {
            foreach (SelectionControl selectionControl in SelectionControls)
            {
                selectionControl.HasFocus = false;

                if (selectionControl == control)
                {
                    selectionControl.HasFocus = true;
                }
            }
        }
    }
}
