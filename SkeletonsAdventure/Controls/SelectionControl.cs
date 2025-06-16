using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.GameWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Controls
{
    public class SelectionControl : Control
    {
        public event EventHandler Click;
        public Color HoverColor { get; set; } = Color.Gray;
        public bool Clicked { get; private set; } = false;

        public SelectionControl()
        {
            Initialize();
        }

        public SelectionControl(string text) : base(text)
        {
            Initialize();
        }

        public SelectionControl(SpriteFont font) : base(font)
        {
            Initialize();
        }

        private void Initialize()
        {
            TextColor = Color.Black;

            Click += SelectionControl_Click;
        }

        private void SelectionControl_Click(object sender, EventArgs e)
        {
            //TODO add logic here if needed
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (Visible is false)
                return;

            Color color = TextColor;

            if (_isHovering)
                color = HoverColor;

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (SpriteFont.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (SpriteFont.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(SpriteFont, Text, new Vector2(x, y), color);
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if (Visible is false)
                return;

            if (Clicked)
            {
                Click?.Invoke(this, new EventArgs());
                Clicked = false; //Reset the clicked state after handling the click event
            }
        }

        public override void Update(GameTime gameTime)
        {
            if (Visible is false)
                return;

            Width = (int)SpriteFont.MeasureString(Text).X;
            Height = (int)SpriteFont.MeasureString(Text).Y;

            IsMouseHovering();
            if (_isHovering)
                if (_currentMouse.LeftButton == ButtonState.Released
                    && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                }
        }
    }
}
