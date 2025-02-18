using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SkeletonsAdventure.Controls
{
    public class GameButton(Texture2D texture, SpriteFont font) : Button(texture, font)
    {
        new public event EventHandler Click;

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
    }
}
