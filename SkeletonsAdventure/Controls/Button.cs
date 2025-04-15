using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SkeletonsAdventure.Controls
{
    public class Button(Texture2D texture, SpriteFont font) : Control
    {
        #region Fields

        protected MouseState _currentMouse;

        public SpriteFont Font { get; private set; } = font;

        protected bool _isHovering;

        protected MouseState _previousMouse;

        public Texture2D Texture { get; private set; } = texture;

        #endregion

        #region Properties

        public event EventHandler Click;

        public bool Clicked { get; private set; }

        public Color PenColour { get; set; } = Color.Black;

        public int Width { get; set; } = texture.Width;
        public int Height { get; set; } = texture.Height;


        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)Position.X, (int)Position.Y, Width, Height);
            }
        }

        #endregion
        #region Methods

        public override void Draw(SpriteBatch spriteBatch)
        {
            var colour = Color.White;

            if (_isHovering)
                colour = Color.Gray;

            spriteBatch.Draw(Texture, Rectangle, colour);

            if (!string.IsNullOrEmpty(Text))
            {
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (Font.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (Font.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(Font, Text, new Vector2(x, y), PenColour);
            }
        }

        public override void Update(GameTime gameTime)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            var mouseRectangle = new Rectangle(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = false;

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;

                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Click?.Invoke(this, new EventArgs());
                }
            }
        }

        public void Update(bool transformMouse, Matrix transformation)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            Vector2 mousePos = new(_currentMouse.X, _currentMouse.Y);
            Vector2 TransformedmousePos = Vector2.Transform(mousePos, Matrix.Invert(transformation)); //Mouse position in the world
            Rectangle TransformedMouseRectangle = new((int)TransformedmousePos.X, (int)TransformedmousePos.Y, 1, 1);
            Rectangle mouseRectangle = new(_currentMouse.X, _currentMouse.Y, 1, 1);
            _isHovering = false;

            if (transformMouse == false)
            {
                if (mouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;

                    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }
            else if (transformMouse)
            {
                if (TransformedMouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;

                    if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                    {
                        Click?.Invoke(this, new EventArgs());
                    }
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
