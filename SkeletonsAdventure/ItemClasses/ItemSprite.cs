using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using RpgLibrary.ItemClasses;
using SkeletonsAdventure.Entities;

namespace SkeletonsAdventure.ItemClasses
{
    public class ItemSprite(BaseItem item, Sprite sprite)
    {
        public BaseItem Item { get; } = item;
        public Sprite Sprite { get; set; } = sprite;

        public virtual void Update(GameTime gameTime)
        {
            Sprite.Update(gameTime);
        }
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            Sprite.Draw(spriteBatch);
        }
    }
}
