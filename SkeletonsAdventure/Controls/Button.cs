using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.GameWorld;
using System;

namespace SkeletonsAdventure.Controls
{
    public class Button : Control
    {
        #region Fields
        protected MouseState _currentMouse;
        protected bool _isHovering = false;
        protected MouseState _previousMouse;
        #endregion
        #region Properties
        public event EventHandler Click;
        public Texture2D Texture { get; private set; } = GameManager.ButtonTexture;
        public bool Clicked { get; private set; } = false;
        //public Color PenColor { get; set; } = Color.Black;
        #endregion
        #region Constructors
        public Button()
        {
            Initialize();
        }

        public Button(SpriteFont font) : base(font)
        {
            Initialize();
        }

        public Button(Texture2D texture)
        {
            Texture = texture;
            Initialize();
        }

        public Button(Texture2D texture, SpriteFont font) : base(font)
        {
            Texture = texture;
            Initialize();
        }

        private void Initialize()
        {
            Width = Texture.Width;
            Height = Texture.Height;
            TextColor = Color.Black;
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
                var x = (Rectangle.X + (Rectangle.Width / 2)) - (SpriteFont.MeasureString(Text).X / 2);
                var y = (Rectangle.Y + (Rectangle.Height / 2)) - (SpriteFont.MeasureString(Text).Y / 2);

                spriteBatch.DrawString(SpriteFont, Text, new Vector2(x, y), TextColor);
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
