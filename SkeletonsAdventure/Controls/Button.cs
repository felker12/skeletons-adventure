using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkeletonsAdventure.GameWorld;
using System;

namespace SkeletonsAdventure.Controls
{
    public class Button(Texture2D texture, SpriteFont font) : Control
    {
        #region Fields

        protected MouseState _currentMouse;

        public SpriteFont Font { get; private set; } = font;

        protected bool _isHovering = false;

        protected MouseState _previousMouse;

        public Texture2D Texture { get; private set; } = texture;

        #endregion

        #region Properties

        public event EventHandler Click;

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

        public void Update(bool transformMouse, Matrix transformation)
        {
            IsMouseHovering(transformMouse, transformation);
            Update();
        }

        //This method wont work with the button being used in the ControlManager
        public void Update()
        {
            if (_isHovering)
            {
                if (_currentMouse.LeftButton == ButtonState.Released && _previousMouse.LeftButton == ButtonState.Pressed)
                {
                    Clicked = true;
                    System.Diagnostics.Debug.WriteLine("hovering");
                    //Click?.Invoke(this, new EventArgs());
                    //Clicked = false; //Reset the clicked state after handling the click event
                }
            }
            else
            {
                //Clicked = false;
            }
        }

        public void IsMouseHovering()
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            _isHovering = false;

            Rectangle mouseRectangle = new(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (mouseRectangle.Intersects(Rectangle))
            {
                _isHovering = true;
            }
        }

        public void IsMouseHovering(bool transformMouse, Matrix transformation)
        {
            _previousMouse = _currentMouse;
            _currentMouse = Mouse.GetState();
            _isHovering = false;

            Vector2 mousePos = new(_currentMouse.X, _currentMouse.Y);
            Rectangle mouseRectangle = new(_currentMouse.X, _currentMouse.Y, 1, 1);

            if (transformMouse == false)
            {
                if (mouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;
                }
            }
            else if (transformMouse)
            {
                Vector2 TransformedmousePos = Vector2.Transform(mousePos, Matrix.Invert(transformation)); //Mouse position in the world
                Rectangle TransformedMouseRectangle = new((int)TransformedmousePos.X, (int)TransformedmousePos.Y, 1, 1);
                if (TransformedMouseRectangle.Intersects(Rectangle))
                {
                    _isHovering = true;
                }
            }
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {
            if(Clicked)
            {
                System.Diagnostics.Debug.WriteLine("Clicked");
                Click?.Invoke(this, new EventArgs());
                Clicked = false; //Reset the clicked state after handling the click event
            }
        }

        #endregion
    }
}
