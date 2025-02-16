using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SkeletonsAdventure.Controls
{
    public  class GamePopUpBox(Vector2 pos, Texture2D texture, int width, int height) 
        : PopUpBox(pos, texture, width, height)
    {
        public void Update(GameTime gameTime, bool transformMouse, Matrix transformation)
        {
            Height = VisibleButtonsHeight() + (int)ButtonOffset.Y * 2;
            Width = LongestButtonTextLength() + (int)ButtonOffset.X * 2;
            Vector2 offset = ButtonOffset;
            if (transformMouse == false)
            {
                foreach (GameButton button in Buttons.Cast<GameButton>())
                {
                    if (button.Visible)
                    {
                        button.Update(false, transformation);
                        button.Position = Position + offset;
                        offset += new Vector2(0, button.Height);
                    }
                }
            }
            else if (transformMouse == true)
            {
                foreach (GameButton button in Buttons.Cast<GameButton>())
                {
                    if (button.Visible)
                    {
                        button.Update(true, transformation);
                        button.Position = Position + offset;
                        offset += new Vector2(0, button.Height);
                    }
                }
            }
        }
    }
}
