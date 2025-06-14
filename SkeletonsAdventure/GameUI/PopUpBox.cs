using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Controls;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;

namespace SkeletonsAdventure.GameUI
{
    public class PopUpBox
    {
        public Vector2 Position { get; set; } = new();
        public Vector2 ButtonOffset { get; set; } = new(4, 4);
        public Texture2D Texture { get; set; }
        public int Width { get; set; } = 100;
        public int Height { get; set; } = 100;
        public Color Color { get; set; } = Color.White;
        public bool Visible { get; set; } = false;
        public List<GameButton> Buttons { get; set; } = [];
        public ControlManager ControlManager { get; set; } = new(GameManager.Arial12);
        public Rectangle Rectangle => new((int)Position.X, (int)Position.Y, Width, Height);

        public PopUpBox(Vector2 pos, Texture2D texture, int width, int height)
        {
            Position = pos;
            Texture = texture;
            Width = width;
            Height = height;
        }

        public PopUpBox()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(Texture, Rectangle, Color);

            foreach (GameButton button in Buttons)
            {
                if (button.Visible)
                    button.Draw(spriteBatch);
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Vector2 offset = ButtonOffset;
            foreach (GameButton button in Buttons)
            {
                if (button.Visible)
                {
                    button.Update(gameTime);
                    button.Position = Position + offset;
                    offset += new Vector2(0, button.Height);
                }
            }
        }

        public virtual void Update(GameTime gameTime, bool transformMouse, Matrix transformation)
        {
            if (VisibleButtonsCount() > 0)
            {
                Height = VisibleButtonsHeight() + (int)ButtonOffset.Y * 2;
                Width = LongestButtonTextLength() + (int)ButtonOffset.X * 2;
                Vector2 offset = ButtonOffset;

                foreach (GameButton button in Buttons)
                {
                    if (button.Visible)
                    {
                        button.TransformMouse = transformMouse;
                        button.Transformation = transformation;
                        button.Update(gameTime);
                        button.Position = Position + offset;
                        offset += new Vector2(0, button.Height);
                    }
                }
            }
            else
            {
                Height = 0;
                Width = 0;
            }
        }

        public void HandleInput(PlayerIndex playerIndex)
        {
            foreach (GameButton button in Buttons)
            {
                if (button.Visible)
                {
                    button.HandleInput(playerIndex);
                }
            }
        }


        public void AddButton(GameButton button, string buttonText)
        {
            button.Text = buttonText;
            button.Visible = false;
            button.Width = (int)button.SpriteFont.MeasureString(buttonText).X;
            button.Height = (int)button.SpriteFont.MeasureString(buttonText).Y;

            Buttons.Add(button);
        }

        public void AddButtons(Dictionary<string, GameButton> buttons)
        {
            foreach (KeyValuePair<string, GameButton> button in buttons)
            {
                AddButton(button.Value, button.Key);
            }
        }

        public int VisibleButtonsCount()
        {
            int count = 0;
            foreach (GameButton button in Buttons)
            {
                if (button.Visible == true)
                    count++;
            }
            return count;
        }

        public int VisibleButtonsHeight()
        {
            int heightCount = 0;

            foreach (GameButton button in Buttons)
            {
                if (button.Visible == true)
                    heightCount += button.Height;
            }

            return heightCount;
        }

        public int LongestButtonTextLength()
        {
            int length = 0;

            foreach (GameButton button in Buttons)
            {
                if (button.Visible == true)
                {
                    if (button.SpriteFont.MeasureString(button.Text).X > length)
                    {
                        length = (int)button.SpriteFont.MeasureString(button.Text).X;
                    }
                }
            }

            return length;
        }
    }
}
