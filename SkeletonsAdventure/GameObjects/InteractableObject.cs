using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SkeletonsAdventure.Entities;

namespace SkeletonsAdventure.GameObjects
{
    internal class InteractableObject : AnimatedSprite
    {
        public string TypeOfObject { get; set; } = string.Empty;

        public InteractableObject() : base() { }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);
        }
    }
}
