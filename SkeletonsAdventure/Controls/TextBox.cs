using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SkeletonsAdventure.Controls
{
    internal class TextBox : Control
    {

        public TextBox() : base()
        {

        }

        public TextBox(SpriteFont font) : base(font)
        {

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawRectangle(Rectangle, Color.White, 1, 0); 
            spriteBatch.DrawString(SpriteFont, Text, Position + new Vector2(5,5), TextColor);
        }

        public override void HandleInput(PlayerIndex playerIndex)
        {

        }

        public override void Update(GameTime gameTime)
        {

        }
    }
}
