using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.GameUI
{
    internal class LinkLabelBox : ControlBox
    {
        public List<LinkLabel> LinkLabels { get; set; } = [];
        public LinkLabel ActiveLinkLabel { get; set; }

        public LinkLabelBox(Vector2 pos, Texture2D texture, int width, int height) : base(pos, texture, width, height)
        {
        }

        public LinkLabelBox() : base()
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

            foreach (LinkLabel linkLabel in LinkLabels)
            {
                if (linkLabel.Visible)
                    linkLabel.Draw(spriteBatch);
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible is false)
                return;

            Vector2 offset = ControlOffset;
            foreach (LinkLabel linkLabel in LinkLabels)
            {
                if (linkLabel.Visible)
                {
                    linkLabel.Update(gameTime);
                    linkLabel.Position = Position + offset;
                    offset += new Vector2(0, linkLabel.Height);
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (Visible is false)
                return;

            foreach (LinkLabel linkLabel in LinkLabels)
            {
                if (linkLabel.Visible)
                    linkLabel.HandleInput(playerIndex);
            }
        }

        public void AddLinkLabel(LinkLabel linkLabel)
        {
            if (linkLabel is null)
                return;

            linkLabel.Width = (int)linkLabel.SpriteFont.MeasureString(linkLabel.Text).X;
            linkLabel.Height = (int)linkLabel.SpriteFont.MeasureString(linkLabel.Text).Y;

            LinkLabels.Add(linkLabel);

            if(LinkLabels.Count > 0 && ActiveLinkLabel is null)
            {
                ActiveLinkLabel = LinkLabels[0];
                ActiveLinkLabel.HasFocus = true;
            }
        }

        public void ChangeFocus(LinkLabel lbl)
        {
            foreach(LinkLabel linkLabel in LinkLabels)
            {
                linkLabel.HasFocus = false;

                if (linkLabel == lbl)
                {
                    linkLabel.HasFocus = true;
                }
            }
        }

        public void AddLinkLabels(List<LinkLabel> labels)
        {
            foreach (LinkLabel label in labels)
                AddLinkLabel(label);
        }
    }
}
