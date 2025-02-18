using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;

namespace SkeletonsAdventure.Controls
{
    public  class GamePopUpBox : PopUpBox
    {
        public GamePopUpBox(Vector2 pos, Texture2D texture, int width, int height) 
            : base(pos, texture, width, height) { }

        public GamePopUpBox() { }
        public virtual void Update(bool transformMouse, Matrix transformation)
        {
            if(VisibleButtonsCount() > 0)
            {
                Height = VisibleButtonsHeight() + (int)ButtonOffset.Y * 2;
                Width = LongestButtonTextLength() + (int)ButtonOffset.X * 2; Vector2 offset = ButtonOffset;

                foreach (GameButton button in Buttons.Cast<GameButton>())
                {
                    if (button.Visible)
                    {
                        button.Update(transformMouse, transformation);
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
    }
}
