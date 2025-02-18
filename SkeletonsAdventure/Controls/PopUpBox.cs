using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace SkeletonsAdventure.Controls
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
        public List<Button> Buttons { get; protected set; } = [];
        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }
        Rectangle SourceRectangle //TODO
        {
            get
            {
                return new Rectangle(0, 0, Width, Height);
            }
        }

        public PopUpBox(Vector2 pos, Texture2D texture, int width, int height)
        {
            Position = pos;
            Texture = texture;
            Width = width;
            Height = height;
        }

        public  PopUpBox()
        {
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            //spriteBatch.Draw(Texture, Rectangle, SourceRectangle, Color);
            spriteBatch.Draw(Texture, Rectangle, Color);

            foreach (Button button in Buttons)
            {
                if (button.Visible)
                {
                    button.Draw(spriteBatch);
                }
            }
        }

        public virtual void Update(GameTime gameTime)
        {
            Vector2 offset = ButtonOffset;
            foreach (Button button in Buttons)
            {
                if (button.Visible)
                {
                    button.Update(gameTime);
                    button.Position = Position + offset;
                    offset += new Vector2(0, button.Height);
                }
            }
        }

        public void AddButton(Button button, string buttonText)
        {
            button.Text = buttonText;
            button.Visible = false;
            button.Width = (int)button.Font.MeasureString(buttonText).X;
            button.Height = (int)button.Font.MeasureString(buttonText).Y;

            Buttons.Add(button);
        }

        public void AddButtons(Dictionary<string, Button> buttons)
        {
            foreach (KeyValuePair<string, Button> button in buttons)
            {
                AddButton(button.Value, button.Key);
            }
        }

        public int VisibleButtonsCount()
        {
            int count = 0;
            foreach (Button button in Buttons)
            {
                if (button.Visible == true)
                    count++;
            }
            return count;
        }

        public int VisibleButtonsHeight()
        {
            int heightCount = 0;

            foreach (Button button in Buttons)
            {
                if (button.Visible == true)
                    heightCount += button.Height;
            }

            return heightCount;
        }

        public int LongestButtonTextLength()
        {
            int length = 0;

            foreach (Button button in Buttons)
            {
                if (button.Visible == true)
                {
                    if (button.Font.MeasureString(button.Text).X > length)
                    {
                        length = (int)button.Font.MeasureString(button.Text).X;
                    }
                }
            }

            return length;
        }
    }
}
