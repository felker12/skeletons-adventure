﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SkeletonsAdventure.Controls
{
    public class Label : Control
    {
        public Label()
        {
            tabStop = false;
        }

        public override void Update(GameTime gameTime)
        {
        }
        public override void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(SpriteFont, Text, Position, Color);
        }
        public override void HandleInput(PlayerIndex playerIndex)
        {
        }
    }
}
