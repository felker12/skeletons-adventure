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
        protected bool _isHovering = false;
        protected MouseState _previousMouse;
        #endregion
        #region Properties
        public event EventHandler Click;
        public SpriteFont Font { get; private set; } = font;
        public Texture2D Texture { get; private set; } = texture;
        public bool Clicked { get; private set; } = false;
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

            if (Enabled is false)
                colour = Color.DarkGray;

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
            IsMouseHovering();
            Update();
        }

        //This method wont work with the button being used in the ControlManager
        protected void Update()
        {
            if (_isHovering)
                if (_currentMouse.LeftButton == ButtonState.Released 
                    && _previousMouse.LeftButton == ButtonState.Pressed)
                    Clicked = true;
        }

        public void IsMouseHovering()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();

            Rectangle mouseRectangle = new(_currentMouse.X, _currentMouse.Y, 1, 1);

            _isHovering = mouseRectangle.Intersects(Rectangle);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if(Clicked)
            {
                Click?.Invoke(this, new EventArgs());
                Clicked = false; //Reset the clicked state after handling the click event
            }
        }
        #endregion
    }
}
