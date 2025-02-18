using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SkeletonsAdventure.Controls
{
    public class Menu : GamePopUpBox
    {
        public Menu() : base()
        {
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }

        public void HandleInput(PlayerIndex playerIndex)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }
        public override void Update(bool transformMouse, Matrix transformation)
        {
            base.Update(transformMouse, transformation);
        }
    }
}
